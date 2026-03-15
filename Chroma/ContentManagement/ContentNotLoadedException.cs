namespace Chroma.ContentManagement;

public sealed class ContentNotLoadedException(string message) 
    : ContentException(message);