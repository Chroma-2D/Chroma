using System;
using System.Numerics;

namespace Chroma.Windowing
{
    public class WindowMoveEventArgs : EventArgs
    {
        public Vector2 Position { get; }

        internal WindowMoveEventArgs(Vector2 position)
        {
            Position = position;
        }
    }
}