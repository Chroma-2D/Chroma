using System;

namespace Chroma.Diagnostics
{
    [Flags]
    public enum Verbosity
    {
        Info = 0x01,
        Warning = 0x02,
        Error = 0x04,
        Debug = 0x08,

        All = Info | Warning | Error | Debug,
        Standard = Info | Warning | Error
    }
}
