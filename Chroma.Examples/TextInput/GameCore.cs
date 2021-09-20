using System;
using System.Drawing;
using System.IO;
using Chroma;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering.TrueType;
using Chroma.Input;
using Color = Chroma.Graphics.Color;

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

        public GameCore() : base(new(false, false))
        {
        }

        protected override IContentProvider InitializeContentPipeline()
        {
            return new FileSystemContentProvider(
                Path.Combine(AppContext.BaseDirectory, "../../../../_common")
            );
        }

        protected override void LoadContent()
        {
            using var fs = new FileStream(
                Path.Combine(AppContext.BaseDirectory, "../../../../_common/Fonts/dos8x14.ttf"),
                FileMode.Open
            );

            _font = new TrueTypeFont(fs, 16);
            _font.PreferAutoHinter = true;

            Window.SizeChanged += (_, _) => { InitializeDisplay(); };

            InitializeDisplay();
        }

        private void InitializeDisplay()
        {
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
                        _terminal.WriteLine($" ECHO -> {input}");
                    }

                    _terminal.Write("root # ");
                }
            };
            _terminal.WriteLine(
                "Welcome to the faux terminal shell!\nInput text to get it echoed back. Hit <F1> for a surprise.\n");
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
            if (e.KeyCode == KeyCode.Return || e.KeyCode == KeyCode.NumEnter)
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
                _font.IsKerningEnabled = !_font.IsKerningEnabled;
            }
            else if (e.KeyCode == KeyCode.F3)
            {
                _font.PreferAutoHinter = !_font.PreferAutoHinter;
            }
            else if (e.KeyCode == KeyCode.F4)
            {
                _font.HintingMode = HintingMode.Light;
            }
            else if (e.KeyCode == KeyCode.F5)
            {
                _font.HintingMode = HintingMode.Monochrome;
            }
            else if (e.KeyCode == KeyCode.F6)
            {
                Window.Mode.SetBorderlessFullScreen();
            }
            else if (e.KeyCode == KeyCode.F7)
            {
                Window.Mode.SetWindowed(new Size(800, 600), true);
            }
            else if (e.KeyCode == KeyCode.F8)
            {
                Window.CanResize = !Window.CanResize;
            }
        }

        protected override void TextInput(TextInputEventArgs e)
        {
            _terminal.TextInput(e);
        }
    }
}