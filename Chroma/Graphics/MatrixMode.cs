namespace Chroma.Graphics;

using Chroma.Natives.Bindings.SDL;

public enum MatrixMode
{
    View = SDL_gpu.GPU_MatrixModeEnum.GPU_VIEW,
    Model = SDL_gpu.GPU_MatrixModeEnum.GPU_MODEL,
    Projection = SDL_gpu.GPU_MatrixModeEnum.GPU_PROJECTION
}