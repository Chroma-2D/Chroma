namespace Chroma.NALO;

using System;

internal class NativeLibraryException : Exception
{
    public NativeLibraryException(string message) : base(message) { }
}