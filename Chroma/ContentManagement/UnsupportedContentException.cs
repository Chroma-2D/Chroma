namespace Chroma.ContentManagement
{
    public class UnsupportedContentException : ContentException
    {
        public string TargetPath { get; }

        public UnsupportedContentException(string message, string targetPath) 
            : base(message)
        {
            TargetPath = targetPath;
        }
    }
}