using System;

namespace Chroma.Input
{
    [Flags]
    public enum KeyModifiers
    {
        None = 0x0000,
        LeftShift = 0x0001,
        RightShift = 0x0002,
        LeftControl = 0x0040,
        RightControl = 0x0080,
        LeftAlt = 0x0100,
        RightAlt = 0x0200,
        LeftSuper = 0x0400,
        RightSuper = 0x0800,
        NumLock = 0x1000,
        CapsLock = 0x2000,
        Mode = 0x4000
    }
}
