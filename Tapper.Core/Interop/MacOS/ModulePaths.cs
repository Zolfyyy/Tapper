using System.Runtime.Versioning;

namespace Tapper.Core.Interop.MacOS;

[SupportedOSPlatform("macos")]
internal static class ModulePaths
{
    public const string CoreFoundation = "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";
    public const string CoreGraphics = "/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics";
}