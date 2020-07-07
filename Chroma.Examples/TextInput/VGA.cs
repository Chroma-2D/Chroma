using System.Numerics;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Windowing;

namespace TextInput
{
    public class VGA
    {
        private readonly TrueTypeFont _ttf;
        private readonly Color[] _fgColorBuffer;
        private readonly char[] _charBuffer;

        public int MaxCols { get; }
        public int MaxRows { get; }

        public int RowSize { get; }
        public int ColSize { get; }

        public VGA(Window window, TrueTypeFont ttf)
        {
            _ttf = ttf;

            ColSize = _ttf.Measure("A").Width;
            RowSize = _ttf.Size;

            MaxCols = window.Size.Width / ColSize;
            MaxRows = window.Size.Height / RowSize;

            _fgColorBuffer = new Color[MaxCols * MaxRows];
            _charBuffer = new char[MaxCols * MaxRows];

            for (var y = 0; y < MaxRows; y++)
            {
                for (var x = 0; x < MaxCols; x++)
                {
                    _fgColorBuffer[y * MaxCols + x] = Color.White;
                    _charBuffer[y * MaxCols + x] = ' ';
                }
            }
        }

        public void SetCharAt(int x, int y, char c)
            => _charBuffer[y * MaxCols + x] = c;

        public void SetColorAt(int x, int y, Color c)
            => _fgColorBuffer[y * MaxCols + x] = c;

        public void WriteStringTo(int x, int y, string str)
        {
            var tx = x;
            var ty = y;

            for (var i = 0; i < str.Length; i++)
            {
                SetCharAt(tx, ty, str[i]);

                tx++;

                if (tx >= MaxCols)
                {
                    tx = 0;
                    ty++;

                    if (ty >= MaxRows)
                        ty = 0;
                }
            }
        }

        public void Render(RenderContext context)
        {
            for (var y = 0; y < MaxRows; y++)
            {
                var start = y * MaxCols;
                var end = start + MaxCols;

                var str = new string(_charBuffer[start..end]);

                context.DrawString(
                    _ttf,
                    str,
                    new Vector2(0, y * _ttf.Size),
                    (c, i, p, g) => new GlyphTransformData(p) {Color = _fgColorBuffer[y * MaxCols + i]}
                );
            }
        }
    }
}