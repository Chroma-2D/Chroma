namespace Chroma;

using System;

public class FrameworkException : Exception
{
    public FrameworkException(string message) 
        : base(message)
    {

    }
}