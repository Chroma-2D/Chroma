using Chroma.Natives.SDL;

namespace Chroma.Windowing
{
    public enum WindowHitTestResult
    {
        Normal = SDL2.SDL_HitTestResult.SDL_HITTEST_NORMAL,
        Draggable = SDL2.SDL_HitTestResult.SDL_HITTEST_DRAGGABLE,
        ResizeLeft = SDL2.SDL_HitTestResult.SDL_HITTEST_RESIZE_LEFT,
        ResizeTopLeft = SDL2.SDL_HitTestResult.SDL_HITTEST_RESIZE_TOPLEFT,
        ResizeTop = SDL2.SDL_HitTestResult.SDL_HITTEST_RESIZE_TOP,
        ResizeTopRight = SDL2.SDL_HitTestResult.SDL_HITTEST_RESIZE_TOPRIGHT,
        ResizeRight = SDL2.SDL_HitTestResult.SDL_HITTEST_RESIZE_RIGHT,
        ResizeBottomRight = SDL2.SDL_HitTestResult.SDL_HITTEST_RESIZE_BOTTOMRIGHT,
        ResizeBottom = SDL2.SDL_HitTestResult.SDL_HITTEST_RESIZE_BOTTOM,
        ResizeBottomLeft = SDL2.SDL_HitTestResult.SDL_HITTEST_RESIZE_BOTTOMLEFT
    }
}