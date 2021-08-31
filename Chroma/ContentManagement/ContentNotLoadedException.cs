namespace Chroma.ContentManagement
{
    public class ContentNotLoadedException : ContentException
    {
        public ContentNotLoadedException(string message)
            : base(message)
        {
        }
    }
}