using Chroma.Windowing;
using Chroma.Graphics;

namespace Chroma
{
    public class Game
    {
        protected OpenGlWindow Window { get; }
        protected GraphicsManager Graphics => GraphicsManager.Instance;

        public bool Running { get; private set; }

        public Game()
        {
            Window = new OpenGlWindow(this, 640, 480)
            {
                Draw = Draw,
                Update = Update
            };
        }

        public void Run()
        {
            Window.Run();
        }

        protected virtual void Draw(RenderContext context)
        {
        }

        protected virtual void Update(float delta)
        {
        }
    }
}
