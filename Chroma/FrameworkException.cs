using System;
using System.Text;
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

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(Message);
            
            if(!string.IsNullOrEmpty(SdlError))
                sb.AppendLine($"SDL: {SdlError}");
            
            sb.AppendLine(StackTrace);

            return sb.ToString();
        }
    }
}