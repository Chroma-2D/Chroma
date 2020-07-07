using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace TextInput
{
    public class GameCore : Game
    {
        private TrueTypeFont _font;
        
        private Terminal _terminal;
        private VGA _vga;

        public GameCore()
        {
            Content = new FileSystemContentProvider(this, Path.Combine(LocationOnDisk, "../../../../_common"));
        }

        protected override void LoadContent()
        {
            _font = Content.Load<TrueTypeFont>("Fonts/dos8x14.ttf", 16);

            _vga = new VGA(Window, _font)
            {
                CursorEnabled = true
            };
            
            _terminal = new Terminal(_vga);

        }

        protected override void Update(float delta)
        {
            _vga.Update(delta);
            
            
        }

        protected override void Draw(RenderContext context)
        {
            _vga.Draw(context);
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.Return)
            {
                _terminal.PutChar('\n');
            }
            else if (e.KeyCode == KeyCode.Backspace)
            {
                _terminal.PutChar('\b');
            }
        }

        protected override void TextInput(TextInputEventArgs e)
        {
            // _terminal.TextInput(e);
            
            _terminal.Write(e.Text);
        }
    }
}