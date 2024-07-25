namespace Chroma.Graphics.Batching;

using System;

internal struct BatchInfo
{
    internal Action<RenderContext> DrawAction;
    internal int Depth;
}