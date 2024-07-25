namespace Chroma.Windowing;

using System.Drawing;

public sealed class WindowSizeEventArgs
{
    public Size Size { get; }

    internal WindowSizeEventArgs(Size size)
    {
        Size = size;
    }
}