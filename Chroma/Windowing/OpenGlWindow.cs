using Chroma.Graphics;
using Chroma.SDL2;

namespace Chroma.Windowing
{
    public class OpenGlWindow : WindowBase
    {
        internal delegate void DrawDelegate(RenderContext context);
        internal delegate void UpdateDelegate(float delta);

        internal DrawDelegate Draw { get; set; }
        internal UpdateDelegate Update { get; set; }

        internal OpenGlWindow(Game game, ushort width, ushort height)
          : base(
                game, 
                new Vector2(
                    SDL.SDL_WINDOWPOS_CENTERED, 
                    SDL.SDL_WINDOWPOS_CENTERED
                ), 
                width, 
                height, 
                false, 
                SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL
            )
        { }

        internal override void OnSdlEvent(SDL.SDL_Event ev)
        {
            base.OnSdlEvent(ev);
        }

        protected override void OnDraw()
        {
            Draw?.Invoke(RenderContext);
        }

        protected override void OnUpdate(float delta)
        {
            Update?.Invoke(delta);
        }
    }
}
