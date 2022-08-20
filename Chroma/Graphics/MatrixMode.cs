using Chroma.Natives.Bindings.SDL;

namespace Chroma.Graphics
{
    public enum MatrixMode
    {
        View = SDL_gpu.GPU_MatrixModeEnum.GPU_VIEW,
        Model = SDL_gpu.GPU_MatrixModeEnum.GPU_MODEL,
        Projection = SDL_gpu.GPU_MatrixModeEnum.GPU_PROJECTION
    }
}