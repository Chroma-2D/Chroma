using System;

namespace Chroma
{
    public class FrameworkException : Exception
    {
        public FrameworkException(string message) 
            : base(message)
        {

        }
    }
}