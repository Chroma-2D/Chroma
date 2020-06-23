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
            Content = new FileSystemContentProvider(this, "../../../../_common");
        }

        protected override void LoadContent()
        {
            _cursor = Content.Load<Cursor>("Cursors/cursor.png");
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                "Press <space> to toggle between default and custom cursor.\n" +
                "Click <LMB> to toggle the cursor's visibility.",
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
        }

        protected override void MousePressed(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
                Cursor.IsVisible = !Cursor.IsVisible;
        }
    }
}