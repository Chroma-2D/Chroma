using Chroma.SDL2;

namespace Chroma.Graphics
{
    public enum TextureWrappingMode
    {
        None = SDL_gpu.GPU_WrapEnum.GPU_WRAP_NONE,
        Mirror = SDL_gpu.GPU_WrapEnum.GPU_WRAP_MIRRORED,
        Repeat = SDL_gpu.GPU_WrapEnum.GPU_WRAP_REPEAT
    }
}
