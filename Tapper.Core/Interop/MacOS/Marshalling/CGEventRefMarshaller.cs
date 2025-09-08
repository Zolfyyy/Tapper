using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Versioning;

namespace Tapper.Core.Interop.MacOS.Marshalling;

[SupportedOSPlatform("macos")]
[CustomMarshaller(typeof(CoreGraphics.CGEventRef), MarshalMode.Default, typeof(CGEventRefMarshaller))]
internal static class CGEventRefMarshaller
{
    public static nint ConvertToUnmanaged(CoreGraphics.CGEventRef managed) => managed.Value;
    public static CoreGraphics.CGEventRef ConvertToManaged(nint native) => new() { Value = native };
}