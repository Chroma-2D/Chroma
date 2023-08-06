namespace Chroma.ContentManagement
{
    public sealed class UnsupportedContentException : ContentException
    {
        public string TargetPath { get; }

        public UnsupportedContentException(string message, string targetPath) 
            : base(message)
        {
            TargetPath = targetPath;
        }
    }
}