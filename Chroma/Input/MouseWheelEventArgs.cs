using System.Numerics;
using Chroma.Natives.SDL;

namespace Chroma.Input
{
    public class MouseWheelEventArgs : System.EventArgs
    {
        public Vector2 Motion { get; }
        public bool DirectionFlipped { get; }

        internal MouseWheelEventArgs(Vector2 motion, uint direction)
        {
            Motion = motion;
            DirectionFlipped = direction == (uint)SDL2.SDL_MouseWheelDirection.SDL_MOUSEWHEEL_FLIPPED;
        }
    }
}