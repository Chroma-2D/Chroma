using System;
using System.Numerics;

namespace Chroma.Windowing
{
    public sealed class WindowMoveEventArgs
    {
        public Vector2 Position { get; }

        internal WindowMoveEventArgs(Vector2 position)
        {
            Position = position;
        }
    }
}