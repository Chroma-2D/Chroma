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

        public OpenGlWindow(Game game, ushort width, ushort height) : 
            base(game, width, height, SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL)
        {
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
