using System.Numerics;
using Chroma;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace WindowOperations
{
    public class GameCore : Game
    {
        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                $"Use <F1> to toggle window resizable status ({Window.Properties.CanResize}).\n" +
                "Use <F2> to switch into exclusive fullscreen mode with native resolution\n" +
                "Use <F3> to switch into borderless fullscreen mode with native resolution\n" +
                "Use <F4> to switch into 1024x600 windowed mode.\n\n" +
                $"Current viewport resolution: {Window.Properties.Width}x{Window.Properties.Height}",
                new Vector2(8)
            );
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.F1)
            {
                Window.Properties.CanResize = !Window.Properties.CanResize;
            }
            else if(e.KeyCode == KeyCode.F2)
            {
                Window.GoFullscreen(true, true);
            }
            else if (e.KeyCode == KeyCode.F3)
            {
                Window.GoFullscreen(false, true); // alternatively don't pass any parameters
                                                  // because they're optional for borderless native fullscreen
            }
            else if (e.KeyCode == KeyCode.F4)
            {
                Window.GoWindowed(1024, 600);
            }
        }
    }
}