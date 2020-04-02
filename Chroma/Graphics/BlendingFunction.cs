using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public enum BlendingFunction
    {
        Zero = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_ZERO,
        One = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_ONE,
        SourceColor = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_SRC_COLOR,
        DestinationColor = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_DST_COLOR,
        InverseSourceColor = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_ONE_MINUS_SRC,
        InverseDestinationColor = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_ONE_MINUS_DST,
        SourceAlpha = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_SRC_ALPHA,
        DestinationAlpha = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_DST_ALPHA,
        InverseSourceAlpha = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_ONE_MINUS_SRC_ALPHA,
        InverseDestinationAlpha = SDL_gpu.GPU_BlendFuncEnum.GPU_FUNC_ONE_MINUS_DST_ALPHA
    }
}
