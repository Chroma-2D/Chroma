using System;

namespace Chroma.Natives
{
    public class NativeLibraryException : Exception
    {
        public NativeLibraryException(string message) : base(message) { }
    }
}
