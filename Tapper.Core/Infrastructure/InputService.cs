using System.Runtime.Versioning;
using Tapper.Core.Interop.MacOS;
using Tapper.Core.Services;

namespace Tapper.Core.Infrastructure;

internal sealed class InputService : IInputService
{
    [SupportedOSPlatform("macos")]
    private static CoreGraphics.CGKeyCode TranslateToCGKeyCode(InputKey key)
    {
        return key switch
        {
            InputKey.PageUp => CoreGraphics.CGKeyCode.kVK_PageUp,
            InputKey.PageDown => CoreGraphics.CGKeyCode.kVK_PageDown,
            _ => throw new ArgumentOutOfRangeException(nameof(key))
        };
    }

    public void SendButtonTap(int pid, InputKey key)
    {
        if (OperatingSystem.IsMacOS())
        {
            var actualKey = TranslateToCGKeyCode(key);
            var event1 = CoreGraphics.CGEventCreateKeyboardEvent(actualKey, true);
            CoreGraphics.CGEventPostToPid(pid, event1);
            var event2 = CoreGraphics.CGEventCreateKeyboardEvent(actualKey, false);
            CoreGraphics.CGEventPostToPid(pid, event2);
        }
    }
}