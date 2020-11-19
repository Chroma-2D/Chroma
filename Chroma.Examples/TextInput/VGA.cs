using System.Numerics;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Windowing;

namespace TextInput
{
    public class VGA
    {
        private readonly TrueTypeFont _ttf;
        private Color[] _fgColorBuffer;
        private char[] _charBuffer;

        private int _cx;
        private int _cy;

        private float _cursorTimer;
        private bool _drawCursor;

        public int TotalCols { get; }
        public int TotalRows { get; }

        public int RowSize { get; }
        public int ColSize { get; }

        public int CursorX
        {
            get => _cx;
            set
            {
                if (value >= TotalCols)
                    _cx = TotalCols - 1;
                else if (value < 0)
                    _cx = 0;
                else _cx = value;
            }
        }

        public int CursorY
        {
            get => _cy;
            set
            {
                if (value >= TotalRows)
                    _cy = TotalRows - 1;
                else if (value < 0)
                    _cy = 0;
                else _cy = value;
            }
        }

        public bool CursorEnabled { get; set; }

        public VGA(Window window, TrueTypeFont ttf)
        {
            _ttf = ttf;

            ColSize = _ttf.Measure("A").Width;
            RowSize = _ttf.Size;

            TotalCols = window.Size.Width / ColSize;
            TotalRows = window.Size.Height / RowSize;

            Reset();
        }

        public void Reset()
        {
            _cx = 0;
            _cy = 0;

            _fgColorBuffer = new Color[TotalCols * TotalRows];
            _charBuffer = new char[TotalCols * TotalRows];

            Clear(true, true);
        }

        public void Clear(bool chars, bool colors)
        {
            for (var y = 0; y < TotalRows; y++)
            {
                for (var x = 0; x < TotalCols; x++)
                {
                    if (chars)
                        _charBuffer[y * TotalCols + x] = ' ';

                    if (colors)
                        _fgColorBuffer[y * TotalCols + x] = Color.White;
                }
            }
        }

        public void SetCharAt(int x, int y, char c)
            => _charBuffer[y * TotalCols + x] = c;

        public void SetColorAt(int x, int y, Color c)
            => _fgColorBuffer[y * TotalCols + x] = c;

        public void WriteStringTo(int x, int y, string str)
        {
            var tx = x;
            var ty = y;

            for (var i = 0; i < str.Length; i++)
            {
                SetCharAt(tx, ty, str[i]);

                tx++;

                if (tx >= TotalCols)
                {
                    tx = 0;
                    ty++;

                    if (ty >= TotalRows)
                        ty = 0;
                }
            }
        }

        public void ScrollUp()
        {
            for (var y = 1; y < TotalRows; y++)
            {
                for (var x = 0; x < TotalCols; x++)
                {
                    _charBuffer[(y - 1) * TotalCols + x] = _charBuffer[y * TotalCols + x];
                }
            }

            for (var x = 0; x < TotalCols; x++)
            {
                _charBuffer[(TotalRows - 1) * TotalCols + x] = ' ';
                _fgColorBuffer[(TotalRows - 1) * TotalCols + x] = Color.White;
            }
        }

        public void SetLineToColor(Color color, int y)
        {
            if (y < 0 || y >= TotalRows)
                return;

            for (var x = 0; x < TotalCols; x++)
                _fgColorBuffer[y * TotalCols + x] = color;
        }

        public void Update(float delta)
        {
            _cursorTimer += 2000 * delta;

            if (_cursorTimer > 1000)
            {
                _drawCursor = !_drawCursor;
                _cursorTimer = 0;
            }
        }

        public void Draw(RenderContext context)
        {
            for (var y = 0; y < TotalRows; y++)
            {
                var start = y * TotalCols;
                var end = start + TotalCols;

                var str = new string(_charBuffer[start..end]);

                context.DrawString(
                    _ttf,
                    str,
                    new Vector2(0, y * _ttf.Size),
                    (c, i, p, g) =>
                    {
                        return new GlyphTransformData
                        {
                            Color = _fgColorBuffer[y * TotalCols + i]
                        };
                    }
                );
            }

            if (CursorEnabled && _drawCursor)
            {
                context.Rectangle(
                    ShapeMode.Stroke,
                    new Vector2(
                        _cx * ColSize + 4,
                        _cy * RowSize - 1
                    ),
                    ColSize,
                    RowSize - 2,
                    _fgColorBuffer[_cy * TotalCols + _cx]
                );
            }
        }
    }
}