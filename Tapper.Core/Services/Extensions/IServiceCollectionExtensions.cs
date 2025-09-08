using Microsoft.Extensions.DependencyInjection;
using Tapper.Core.Infrastructure;

namespace Tapper.Core.Services.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInputService(this IServiceCollection services)
    {
        services.AddSingleton<IInputService, InputService>();
        return services;
    }
}