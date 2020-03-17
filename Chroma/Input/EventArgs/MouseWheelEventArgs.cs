using Chroma.SDL2;

namespace Chroma.Input.EventArgs
{
    public class MouseWheelEventArgs : System.EventArgs
    {
        public Vector2 Motion { get; }
        public bool DirectionFlipped { get; }

        internal MouseWheelEventArgs(Vector2 motion, uint direction)
        {
            Motion = motion;
            DirectionFlipped = direction == (uint)SDL.SDL_MouseWheelDirection.SDL_MOUSEWHEEL_FLIPPED;
        }
    }
}
