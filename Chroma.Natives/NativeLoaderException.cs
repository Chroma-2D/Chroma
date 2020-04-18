using System;

namespace Chroma.Natives
{
    public class NativeLoaderException : Exception
    {
        public NativeLoaderException(string message) : base(message) { }
    }
}
