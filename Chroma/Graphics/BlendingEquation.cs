namespace Chroma.Graphics;

using Chroma.Natives.Bindings.SDL;

public enum BlendingEquation
{
    Add = SDL_gpu.GPU_BlendEqEnum.GPU_EQ_ADD,
    Subtract = SDL_gpu.GPU_BlendEqEnum.GPU_EQ_SUBTRACT,
    ReverseSubtract = SDL_gpu.GPU_BlendEqEnum.GPU_EQ_REVERSE_SUBTRACT
}