namespace Chroma.NALO;

using System;

internal class NativeExtractorException : Exception
{
    public NativeExtractorException(string message)
        : base(message) { }

    public NativeExtractorException(string message, Exception innerException)
        : base(message, innerException) { }
}