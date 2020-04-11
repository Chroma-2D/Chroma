using System.Numerics;

namespace Chroma.Windowing.EventArgs
{
    public class WindowMoveEventArgs : System.EventArgs
    {
        public Vector2 Position { get; }

        internal WindowMoveEventArgs(Vector2 position)
        {
            Position = position;
        }
    }
}
