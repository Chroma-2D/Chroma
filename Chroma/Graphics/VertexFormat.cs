using System;
using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    [Flags]
    public enum VertexFormat : uint
    {
        XY = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_XY,
        XYZ = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_XYZ,
        ST = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_ST,
        RGB = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_RGB,
        RGBA = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_RGBA,
        RGB8 = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_RGB8,
        RGBA8 = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_RGBA8,
        
        XYST = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_XY_ST,
        XYZST = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_XYZ_ST,
        XYRGB = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_XY_RGB,
        XYZRGB = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_XYZ_RGB,
        XYRGBA = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_XY_RGBA,
        XYZRGBA = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_XYZ_RGBA,
        XYSTRGBA = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_XY_ST_RGBA,
        XYZSTRGBA = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_XYZ_ST_RGBA,
        XYRGB8 = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_XY_RGB8,
        XYZRGB8 = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_XYZ_RGB8,
        XYRGBA8 = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_XY_RGBA8,
        XYZRGBA8 = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_XYZ_RGBA8,
        XYSTRGBA8 = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_XY_ST_RGBA8,
        XYZSTRGBA8 = SDL_gpu.GPU_BatchFlagEnum.GPU_BATCH_XYZ_ST_RGBA8
    }
}