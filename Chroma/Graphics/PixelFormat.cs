using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public enum PixelFormat
    {
        Alpha = SDL_gpu.GPU_FormatEnum.GPU_FORMAT_ALPHA,
        Luminance = SDL_gpu.GPU_FormatEnum.GPU_FORMAT_LUMINANCE,
        AlphaLuminance = SDL_gpu.GPU_FormatEnum.GPU_FORMAT_LUMINANCE_ALPHA,
        RG = SDL_gpu.GPU_FormatEnum.GPU_FORMAT_RG,
        RGB = SDL_gpu.GPU_FormatEnum.GPU_FORMAT_RGB,
        RGBA = SDL_gpu.GPU_FormatEnum.GPU_FORMAT_RGBA,
        YUV420P = SDL_gpu.GPU_FormatEnum.GPU_FORMAT_YCbCr420P,
        YUV442 = SDL_gpu.GPU_FormatEnum.GPU_FORMAT_YCbCr422
    }
}
