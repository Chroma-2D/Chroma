using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public class RenderTarget
    {
        internal SDL_gpu.GPU_Target_PTR Handle { get; }

        public Texture Texture { get; }

        public RenderTarget(ushort width, ushort height)
        {
            Texture = new Texture(width, height);
            Handle = SDL_gpu.GPU_LoadTarget(Texture.ImageHandle);
        }
    }
}
