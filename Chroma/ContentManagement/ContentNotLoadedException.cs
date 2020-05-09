using System;

namespace Chroma.ContentManagement
{
    public class ContentNotLoadedException : Exception
    {
        public ContentNotLoadedException(string message) 
            : base(message) { }
    }
}
