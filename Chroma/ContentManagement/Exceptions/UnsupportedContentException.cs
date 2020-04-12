using System;

namespace Chroma.ContentManagement.Exceptions
{
    public class UnsupportedContentException : Exception
    {
        public string TargetPath { get; }

        public UnsupportedContentException(string message, string targetPath) : base(message)
        {
            TargetPath = targetPath;
        }
    }
}
