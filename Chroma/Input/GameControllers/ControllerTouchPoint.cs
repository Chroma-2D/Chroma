namespace Chroma.Input.GameControllers;

using System.Numerics;

public struct ControllerTouchPoint
{
    internal float X;
    internal float Y;

    public bool Touching { get; internal set; }
    public Vector2 Position => new(X, Y);
}