using System;

namespace Chroma.NALO
{
    internal class NativeLibraryException : Exception
    {
        public NativeLibraryException(string message) : base(message) { }
    }
}
