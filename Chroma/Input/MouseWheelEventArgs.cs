namespace Chroma.Input;

using System.Numerics;
using Chroma.Natives.Bindings.SDL;

public sealed class MouseWheelEventArgs
{
    public Vector2 Motion { get; }
    public bool DirectionFlipped { get; }

    private MouseWheelEventArgs(Vector2 motion, bool directionFlipped)
    {
        Motion = motion;
        DirectionFlipped = DirectionFlipped = directionFlipped;
    }
        
    internal MouseWheelEventArgs(Vector2 motion, uint direction)
        : this(motion, direction == (uint)SDL2.SDL_MouseWheelDirection.SDL_MOUSEWHEEL_FLIPPED)
    {
    }

    public static MouseWheelEventArgs With(Vector2 motion, bool directionFlipped) => new(
        motion,
        directionFlipped
    );

    public MouseWheelEventArgs WithMotion(Vector2 motion) => With(
        motion,
        DirectionFlipped
    );

    public MouseWheelEventArgs WithDirectionFlip(bool directionFlipped) => new(
        Motion,
        directionFlipped
    );
}