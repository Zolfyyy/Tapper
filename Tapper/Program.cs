using System.Collections.Immutable;
using System.CommandLine;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Tapper.Core.Services;
using Tapper.Core.Services.Extensions;

namespace Tapper;

internal readonly record struct Operation
{
    public string Name { get; init; }
    public string Description { get; init; }
    public Func<string, string, bool> Func { get; init; }
}

internal class Program
{
    private static readonly ImmutableArray<Operation> Operations = [
        new() {
            Name = "equals",
            Description = "",
            Func = static (a, b) => a.Equals(b)
        },
        new() {
            Name = "contains",
            Description = "",
            Func = static (a, b) => a.Contains(b)
        },
        new() {
            Name = "regex",
            Description = "",
            Func = static (a, b) => Regex.IsMatch(a, b)
        },
        new() {
            Name = "startswith",
            Description = "",
            Func = static (a, b) => a.StartsWith(b)
        },
        new() {
            Name = "endswith",
            Description = "",
            Func = static (a, b) => a.EndsWith(b)
        },
        new() {
            Name = "process",
            Description = "",
            Func = static (a, b) => {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = b,
                    Arguments = a,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process == null)
                    return false;

                process.WaitForExit();

                return process.ExitCode == 0;
            }
        }
    ];

    private static bool TryGetTargetPid(string criteria, string operationName, out int pid)
    {
        var operation = Operations.FirstOrDefault(o => o.Name == operationName);
        if (operation == default)
        {
            pid = 0;
            return false;
        }

        var ppt = Process.GetProcesses()
            .FirstOrDefault(p => operation.Func(p.ProcessName, criteria));
        pid = ppt?.Id ?? 0;
        return ppt != null;
    }

    private static async Task<int> Main(string[] args)
    {
        var provider = new ServiceCollection()
            .AddInputService()
            .BuildServiceProvider();

        var inputService = provider.GetRequiredService<IInputService>();

        var root = new RootCommand("A command line tool for advancing presentation slides");

        var targetOption = new Option<string>("--target", "-t")
        {
            Description = "The name or substring of the process to search for",
            Required = true
        };

        var operationOption = new Option<string>("--operation", "-o")
        {
            Description = "The operation to perform when searching for the process",
            Required = true,
            DefaultValueFactory = (r) => Operations.First().Name,
        }.AcceptOnlyFromAmong([.. Operations.Select(o => o.Name)]);

        var queryCommand = new Command("query", "Performs a query using the specified operation and target, returning the process id.")
        {
            targetOption,
            operationOption
        };

        queryCommand.SetAction(p =>
        {
            var criteria = p.GetRequiredValue(targetOption);
            var operationName = p.GetRequiredValue(operationOption);

            if (!TryGetTargetPid(criteria, operationName, out var pid))
            {
                Console.WriteLine($"The process could not be found");
                return 1;
            }

            Console.WriteLine($"{pid}");
            return 0;
        });

        var nextCommand = new Command("next", "Sends a page down tap event to the process")
        {
            targetOption,
            operationOption
        };

        nextCommand.SetAction(p =>
        {
            var criteria = p.GetRequiredValue(targetOption);
            var operationName = p.GetRequiredValue(operationOption);

            if (!TryGetTargetPid(criteria, operationName, out var pid))
            {
                Console.WriteLine($"The process could not be found");
                return 1;
            }

            inputService.SendButtonTap(pid, InputKey.PageDown);
            return 0;
        });

        root.Add(nextCommand);

        var previousCommand = new Command("prev", "Sends a page up tap event to the process")
        {
            targetOption,
            operationOption
        };

        previousCommand.SetAction(p =>
        {
            var target = p.GetRequiredValue(targetOption);
            var operationName = p.GetRequiredValue(operationOption);

            if (!TryGetTargetPid(target, operationName, out var pid))
            {
                Console.WriteLine($"The process could not be found");
                return 1;
            }

            inputService.SendButtonTap(pid, InputKey.PageUp);
            return 0;
        });

        root.Add(previousCommand);

        var result = root.Parse(args);
        return await result.InvokeAsync();
    }
}
