using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Versioning;
using Tapper.Core.Interop.MacOS.Marshalling;

namespace Tapper.Core.Interop.MacOS;

// https://developer.apple.com/documentation/coregraphics/

[SupportedOSPlatform("macos")]
internal static partial class CoreGraphics
{
    public enum CGEventTapLocation : uint
    {
        HID = 0,
        Session = 1,
        AnnotatedSession = 2
    }

    public enum CGEventTapPlacement : uint
    {
        HeadInsert = 0,
        TailAppend = 1
    }

    public enum CGEventTapOptions : uint
    {
        Default = 0,
        ListenOnly = 1
    }

    public enum CGKeyCode : ushort
    {
        kVK_PageUp = 0x74,
        kVK_PageDown = 0x79
    }

    public delegate nint CGEventCallback(nint proxy, uint type, nint @event, nint refcon);

    [NativeMarshalling(typeof(CGEventRefMarshaller))]
    public record class CGEventRef : CoreFoundation.CFTypeRef
    {
        public static readonly CGEventRef Zero = new() { Value = 0 };

        public static explicit operator nint(CGEventRef value) => value.Value;
        public static explicit operator CGEventRef(nint value) => new() { Value = value };
    }

    [LibraryImport(ModulePaths.CoreGraphics)]
    public static partial CoreFoundation.CFTypeRefHandle<CGEventRef> CGEventCreateKeyboardEvent(
        CGEventRef source,
        CGKeyCode virtualKey,
        [MarshalAs(UnmanagedType.I1)] bool keyDown);

    public static CoreFoundation.CFTypeRefHandle<CGEventRef> CGEventCreateKeyboardEvent(
        CGKeyCode virtualKey,
        [MarshalAs(UnmanagedType.I1)] bool keyDown) => CGEventCreateKeyboardEvent(CGEventRef.Zero, virtualKey, keyDown);

    [LibraryImport(ModulePaths.CoreGraphics)]
    public static partial void CGEventPostToPid(int pid, CGEventRef @event);
}
