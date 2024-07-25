namespace Chroma.Graphics;

using Chroma.Natives.Bindings.SDL;

public enum TextureWrappingMode
{
    None = SDL_gpu.GPU_WrapEnum.GPU_WRAP_NONE,
    Mirror = SDL_gpu.GPU_WrapEnum.GPU_WRAP_MIRRORED,
    Repeat = SDL_gpu.GPU_WrapEnum.GPU_WRAP_REPEAT
}