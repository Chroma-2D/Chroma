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
        private VGA _vga;

        private string _input;
        private int _currentRow;
        private int _currentCol;

        private float _cursorTimer;
        private bool _drawCursor;

        public GameCore()
        {
            Content = new FileSystemContentProvider(this, Path.Combine(LocationOnDisk, "../../../../_common"));
        }

        protected override void LoadContent()
        {
            _font = Content.Load<TrueTypeFont>("Fonts/dos8x14.ttf", 12);
            
            InitializeVGA();
        }

        protected override void Update(float delta)
        {
            _cursorTimer += 2000 * delta;

            if (_cursorTimer > 1000)
            {
                _drawCursor = !_drawCursor;
                _cursorTimer = 0;
            }
        }

        protected override void Draw(RenderContext context)
        {
            _vga.Render(context);

            if (_drawCursor)
            {
                context.Rectangle(ShapeMode.Fill, new Vector2(_currentCol * _vga.ColSize, _currentRow * _vga.RowSize),
                    _vga.ColSize, _vga.RowSize, Color.White);
            }
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.F1)
            {
                _font.Size--;
                InitializeVGA();
            }
            else if (e.KeyCode == KeyCode.F2)
            {
                _font.Size++;
                InitializeVGA();
            }
            
            if (e.KeyCode == KeyCode.Backspace)
            {
                if (_input.Length == 0)
                    return;

                if (_currentCol == 0 && _currentRow > 0)
                {
                    _currentRow--;
                    _currentCol = _vga.MaxCols - 1;
                }
                else
                {
                    _currentCol--;
                }

                _vga.SetCharAt(_currentCol, _currentRow, ' ');
                _input = _input.Substring(0, _input.Length - 1);
            }
            else if (e.KeyCode == KeyCode.Return)
            {
                _currentRow++;
                _currentCol = 0;

                WriteLine(_input ?? string.Empty);
                _input = string.Empty;
                
                Write("/root # ");
            }
        }

        protected override void TextInput(TextInputEventArgs e)
        {
            if (_currentCol + 1 > _vga.MaxCols)
            {
                _currentCol = 0;
                _currentRow++;
            }
            else
            {
                _input += e.Text;

                _vga.SetCharAt(_currentCol++, _currentRow, e.Text[0]);
            }
        }

        private void WriteLine(string line)
        {
            Write(line);

            _currentRow++;
            _currentCol = 0;
        }

        private void Write(string line)
        {
            _vga.WriteStringTo(_currentCol, _currentRow, line);
            _currentCol += line.Length;

            if (_currentCol >= _vga.MaxCols)
            {
                var off = _currentCol - _vga.MaxCols - 1;
                _currentCol = off;
            }
        }

        private void InitializeVGA()
        {
            _currentCol = 0;
            _currentRow = 0;
            
            _vga = new VGA(Window, _font);            
            Write("/root # ");
        }
    }
}