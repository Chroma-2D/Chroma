namespace Chroma.Graphics;

using Chroma.Natives.Bindings.SDL;

public enum ImageFileFormat
{
    BMP = SDL_gpu.GPU_FileFormatEnum.GPU_FILE_BMP,
    PNG = SDL_gpu.GPU_FileFormatEnum.GPU_FILE_PNG,
    TGA = SDL_gpu.GPU_FileFormatEnum.GPU_FILE_TGA
}