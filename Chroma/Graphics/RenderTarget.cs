using Chroma.SDL2;

namespace Chroma.Graphics
{
    public class RenderTarget
    {
        internal SDL_gpu.GPU_Target_PTR Handle { get; }

        public Texture Texture { get; }

        public RenderTarget(ushort width, ushort height)
        {
            Texture = new Texture(width, height, PixelFormat.RGB)
            {
                Origin = Vector2.Zero
            };

            Texture.SetBlendingMode(BlendingPreset.Multiply);

            Handle = SDL_gpu.GPU_LoadTarget(Texture.ImageHandle);
        }
    }
}
