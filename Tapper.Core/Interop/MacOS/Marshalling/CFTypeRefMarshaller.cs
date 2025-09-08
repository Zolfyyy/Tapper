using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Versioning;

namespace Tapper.Core.Interop.MacOS.Marshalling;

[SupportedOSPlatform("macos")]
[CustomMarshaller(typeof(CoreFoundation.CFTypeRef), MarshalMode.Default, typeof(CFTypeRefMarshaller))]
internal static class CFTypeRefMarshaller
{
    public static nint ConvertToUnmanaged(CoreFoundation.CFTypeRef managed) => managed.Value;
    public static CoreFoundation.CFTypeRef ConvertToManaged(nint native) => new() { Value = native };
}
