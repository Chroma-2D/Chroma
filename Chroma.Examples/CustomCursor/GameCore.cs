using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace CustomCursor
{
    public class GameCore : Game
    {
        private Cursor _cursor;

        public GameCore()
        {
            // in normal use cases this is completely unnecessary
            // but there is more than one example project
            // so sharing between them is necessary to keep the
            // source tree clean
            Content = new FileSystemContentProvider(this, Path.Combine(LocationOnDisk, "../../../../_common"));
        }

        protected override void LoadContent()
        {
            _cursor = Content.Load<Cursor>("Cursors/cursor.png");
        }

        protected override void Draw(RenderContext context)
        {
            context.Clear(Color.CornflowerBlue);
            
            context.DrawString(
                "Press <space> to toggle between default and custom cursor.\n" +
                "Click <LMB> to toggle the cursor's visibility.\n" +
                "Press <F1> to toggle window resizable/non-resizable.\n\n" +
                $"Window Size: {Window.Properties.Width}x{Window.Properties.Height}",
                new Vector2(8, 8)
            );
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.Space)
            {
                if (!_cursor.IsCurrent)
                    _cursor.SetCurrent();
                else Cursor.Reset();
            }
            else if (e.KeyCode == KeyCode.F1)
            {
                Window.Properties.CanResize = !Window.Properties.CanResize;
            }
        }

        protected override void MousePressed(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
                Cursor.IsVisible = !Cursor.IsVisible;
        }
    }
}