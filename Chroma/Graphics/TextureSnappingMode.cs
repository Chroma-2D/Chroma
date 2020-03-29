using Chroma.SDL2;

namespace Chroma.Graphics
{
    public enum TextureSnappingMode
    {
        None = SDL_gpu.GPU_SnapEnum.GPU_SNAP_NONE,
        Position = SDL_gpu.GPU_SnapEnum.GPU_SNAP_POSITION,
        Dimensions = SDL_gpu.GPU_SnapEnum.GPU_SNAP_DIMENSIONS,
        Both = SDL_gpu.GPU_SnapEnum.GPU_SNAP_POSITION_AND_DIMENSIONS
    }
}