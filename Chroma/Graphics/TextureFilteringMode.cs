﻿namespace Chroma.Graphics;

using Chroma.Natives.Bindings.SDL;

public enum TextureFilteringMode
{
    NearestNeighbor = SDL_gpu.GPU_FilterEnum.GPU_FILTER_NEAREST,
    Linear = SDL_gpu.GPU_FilterEnum.GPU_FILTER_LINEAR,
    LinearMipmapped = SDL_gpu.GPU_FilterEnum.GPU_FILTER_LINEAR_MIPMAP
}