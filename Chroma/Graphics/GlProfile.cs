using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public enum GlProfile
    {
        Core = SDL2.SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE,
        Compatibility = SDL2.SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_COMPATIBILITY,
        ES = SDL2.SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_ES
    }
}