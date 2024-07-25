namespace Chroma.Graphics;

using Chroma.Natives.Bindings.SDL;

public enum BlendingPreset
{
    Normal = SDL_gpu.GPU_BlendPresetEnum.GPU_BLEND_NORMAL,
    NormalAddAlpha = SDL_gpu.GPU_BlendPresetEnum.GPU_BLEND_NORMAL_ADD_ALPHA,
    NormalKeepAlpha = SDL_gpu.GPU_BlendPresetEnum.GPU_BLEND_NORMAL_KEEP_ALPHA,
    Premultiplied = SDL_gpu.GPU_BlendPresetEnum.GPU_BLEND_PREMULTIPLIED_ALPHA,
    Multiply = SDL_gpu.GPU_BlendPresetEnum.GPU_BLEND_MULTIPLY,
    Add = SDL_gpu.GPU_BlendPresetEnum.GPU_BLEND_ADD,
    Subtract = SDL_gpu.GPU_BlendPresetEnum.GPU_BLEND_SUBTRACT,
    ModAlpha = SDL_gpu.GPU_BlendPresetEnum.GPU_BLEND_MOD_ALPHA,
    SetAlpha = SDL_gpu.GPU_BlendPresetEnum.GPU_BLEND_SET_ALPHA,
    Set = SDL_gpu.GPU_BlendPresetEnum.GPU_BLEND_SET
}