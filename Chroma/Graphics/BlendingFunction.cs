namespace Chroma.Graphics;

using Chroma.Natives.Bindings.SDL;

public enum BlendingFunction
{
    Zero = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_ZERO,
    One = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_ONE,
    SourceColor = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_SRC_COLOR,
    SourceAlpha = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_SRC_ALPHA,
    DestinationAlpha = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_DST_ALPHA,
    DestinationColor = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_DST_COLOR,
    OneMinusSourceColor = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_ONE_MINUS_SRC,
    OneMinusDestinationColor = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_ONE_MINUS_DST,
    OneMinusSourceAlpha = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_ONE_MINUS_SRC_ALPHA,
    OneMinusDestinationAlpha = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_ONE_MINUS_DST_ALPHA
}