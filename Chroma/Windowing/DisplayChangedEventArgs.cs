namespace Chroma.Windowing;

public sealed class DisplayChangedEventArgs
{
    public int Index { get; }

    internal DisplayChangedEventArgs(int index)
    {
        Index = index;
    }
}