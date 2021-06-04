using System;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SDL;

namespace Chroma
{
    public static partial class Extensions
    {
        internal static void WarningWhenFails(this Log log, string msg, Func<int> func)
        {
            if (func() < 0)
                log.Warning($"{msg}: {SDL2.SDL_GetError()}");
        }
    }
}