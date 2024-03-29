﻿using System.Numerics;
using Chroma.Natives.Bindings.SDL;

namespace Chroma.Input
{
    public sealed class MouseWheelEventArgs
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