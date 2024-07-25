namespace Chroma.NALO.Compression;

using System;

internal class BZip2Exception : NativeExtractorException
{
    public BZip2Exception(string message)
        : base(message)
    {
    }

    public BZip2Exception(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}