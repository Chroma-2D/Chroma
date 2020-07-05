using System;
using Chroma.Natives.SDL;

namespace Chroma
{
    public class FrameworkException : Exception
    {
        public string SdlError { get; }

        public FrameworkException(string message, bool getSdlError = false) : base(message)
        {
            if (getSdlError)
                SdlError = SDL2.SDL_GetError();
        }
    }
}