using System;

namespace Chroma.Graphics.Batching
{
    internal struct BatchInfo
    {
        internal Action DrawAction;
        internal int Depth;
    }
}
