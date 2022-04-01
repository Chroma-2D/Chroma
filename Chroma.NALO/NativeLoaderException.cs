using System;

namespace Chroma.NALO
{
    public class NativeLoaderException : Exception
    {
        public NativeLoaderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}