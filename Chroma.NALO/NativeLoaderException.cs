namespace Chroma.NALO;

using System;

public class NativeLoaderException : Exception
{
    public NativeLoaderException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}