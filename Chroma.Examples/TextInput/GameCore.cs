using System.IO;
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

        private bool _colorCycle;

        private int _colorOffset;
        private float _cycleTimer;

        private Color[] _colors = new[]
        {
            Color.Red,
            Color.Orange,
            Color.Yellow,
            Color.Lime,
            Color.DodgerBlue,
            Color.Indigo,
            Color.Violet
        };

        private Terminal _terminal;
        private VGA _vga;

        public GameCore()
        {
            Content = new FileSystemContentProvider(this, Path.Combine(LocationOnDisk, "../../../../_common"));
        }

        protected override void LoadContent()
        {
            _font = Content.Load<TrueTypeFont>("Fonts/m9.ttf", 14);
            _font.ForceAutoHinting = true;

            _vga = new VGA(Window, _font)
            {
                CursorEnabled = true
            };

            _terminal = new Terminal(_vga)
            {
                InputAccepted = (input) =>
                {
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        _terminal.WriteLine($"    ECHO: {input}");
                    }

                    _terminal.Write("root # ");
                }
            };
            _terminal.Write("root # ");
        }

        protected override void Update(float delta)
        {
            _terminal.Update(delta);
            
            if (_colorCycle)
            {
                _cycleTimer += 25 * delta;
                if (_cycleTimer > 5)
                {
                    for (var y = 0; y < _vga.TotalRows; y++)
                        _vga.SetLineToColor(_colors[(y + _colorOffset) % _colors.Length], y);

                    _colorOffset++;
                    _cycleTimer = 0;
                }
            }
        }

        protected override void Draw(RenderContext context)
        {
            _terminal.Draw(context);
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.Return)
            {
                _terminal.PutChar('\n');
                _terminal.AcceptInput();
            }
            else if (e.KeyCode == KeyCode.Backspace)
            {
                _terminal.PutChar('\b');
            }
            else if (e.KeyCode == KeyCode.F1)
            {
                _colorCycle = !_colorCycle;

                if (!_colorCycle)
                {
                    _vga.Clear(false, true);
                }
            }
            else if (e.KeyCode == KeyCode.F2)
            {
                _font.UseKerning = !_font.UseKerning;
            }
            else if (e.KeyCode == KeyCode.F3)
            {
                _font.ForceAutoHinting = !_font.ForceAutoHinting;
            }
        }

        protected override void TextInput(TextInputEventArgs e)
        {
            _terminal.TextInput(e);
        }
    }
}