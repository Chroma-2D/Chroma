﻿using Chroma.Natives.Bindings.SDL;

namespace Chroma.Graphics
{
    public enum PixelFormat
    {
        RGB = SDL_gpu.GPU_FormatEnum.GPU_FORMAT_RGB,
        BGR = SDL_gpu.GPU_FormatEnum.GPU_FORMAT_BGR,
        BGRA = SDL_gpu.GPU_FormatEnum.GPU_FORMAT_BGRA,
        RGBA = SDL_gpu.GPU_FormatEnum.GPU_FORMAT_RGBA,
        ABGR = SDL_gpu.GPU_FormatEnum.GPU_FORMAT_ABGR
    }
}