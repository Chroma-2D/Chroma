namespace Chroma.Windowing;

using System.Numerics;

public sealed class WindowMoveEventArgs
{
    public Vector2 Position { get; }

    internal WindowMoveEventArgs(Vector2 position)
    {
        Position = position;
    }
}