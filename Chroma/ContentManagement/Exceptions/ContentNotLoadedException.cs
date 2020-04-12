using System;

namespace Chroma.ContentManagement.Exceptions
{
    public class ContentNotLoadedException : Exception
    {
        public ContentNotLoadedException(string message) 
            : base(message) { }
    }
}
