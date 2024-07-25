namespace Chroma.Natives.Bindings.SDL;

using System;
using System.Runtime.InteropServices;

internal struct AnsiString
{
    public IntPtr Pointer;

    public string Value 
        => Marshal.PtrToStringAnsi(Pointer);

    public override string ToString()
        => Value;
}