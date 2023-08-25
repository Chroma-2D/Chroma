using System;

namespace Chroma.Graphics.Batching
{
    internal struct BatchInfo
    {
        internal Action<RenderContext> DrawAction;
        internal int Depth;
    }
}