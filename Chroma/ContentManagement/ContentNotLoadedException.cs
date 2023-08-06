namespace Chroma.ContentManagement
{
    public sealed class ContentNotLoadedException : ContentException
    {
        public ContentNotLoadedException(string message)
            : base(message)
        {
        }
    }
}