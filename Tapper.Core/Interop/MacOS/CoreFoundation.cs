using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Versioning;
using Tapper.Core.Interop.MacOS.Marshalling;

namespace Tapper.Core.Interop.MacOS;

// https://developer.apple.com/documentation/corefoundation/

[SupportedOSPlatform("macos")]
internal static partial class CoreFoundation
{
    [NativeMarshalling(typeof(CFTypeRefMarshaller))]
    public record class CFTypeRef
    {
        public nint Value { get; set; }

        public static explicit operator CFTypeRef(nint value) => new() { Value = value };
    }

    public class CFTypeRefHandle : SafeHandle
    {
        public CFTypeRefHandle() : base(0, ownsHandle: true)
        {
        }

        protected override bool ReleaseHandle() => CFRelease((CFTypeRef)handle) == kern_return_t.KERN_SUCCESS;
        public override bool IsInvalid => handle == 0;

        public static implicit operator CFTypeRef(CFTypeRefHandle handle) => new() { Value = handle.DangerousGetHandle() };
    }

    public class CFTypeRefHandle<TTypeRef> : CFTypeRefHandle where TTypeRef : CFTypeRef, new()
    {
        public CFTypeRefHandle() : base()
        {
        }

        protected override bool ReleaseHandle() => CFRelease((CFTypeRef)handle) == kern_return_t.KERN_SUCCESS;
        public override bool IsInvalid => handle == 0;

        public static implicit operator TTypeRef(CFTypeRefHandle<TTypeRef> handle) => new() { Value = handle.DangerousGetHandle() };
    }

    [LibraryImport(ModulePaths.CoreFoundation)]
    public static partial CFTypeRef CFRetain(CFTypeRef str);

    [LibraryImport(ModulePaths.CoreFoundation)]
    public static partial kern_return_t CFRelease(CFTypeRef str);
}
