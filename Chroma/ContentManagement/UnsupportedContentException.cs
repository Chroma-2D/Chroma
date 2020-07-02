using System;

namespace Chroma.ContentManagement
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