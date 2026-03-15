namespace Chroma.ContentManagement;

public sealed class UnsupportedContentException(string message, string targetPath)
    : ContentException(message)
{
    public string TargetPath { get; } = targetPath;
}