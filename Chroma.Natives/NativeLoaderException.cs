using System;

namespace Chroma.Natives
{
    internal class NativeLoaderException : Exception
    {
        public NativeLoaderException(string message) : base(message) { }
    }
}
