using System;
using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public class RenderTarget
    {
        internal IntPtr Handle { get; }
        public Texture Texture { get; }

        public RenderTarget(ushort width, ushort height) : this(new Texture(width, height)) 
        { }

        public RenderTarget(Texture texture)
        {
            Texture = texture;
            Handle = SDL_gpu.GPU_LoadTarget(Texture.ImageHandle);
        }
    }
}
