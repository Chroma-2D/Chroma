using System;

namespace Chroma.Natives
{
    internal class NativeLibraryException : Exception
    {
        public NativeLibraryException(string message) : base(message) { }
    }
}
