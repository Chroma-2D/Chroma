using System.Numerics;

namespace Chroma.Graphics.TextRendering.Bitmap
{
    public class BitmapFontInfo
    {
        public string FaceName { get; internal set; }
        public int Size { get; internal set; }
        public bool IsBold { get; internal set; }
        public bool IsItalic { get; internal set; }
        public string CharSet { get; internal set; }
        public bool IsUnicode { get; internal set; }
        public int ScalePercent { get; internal set; }
        public bool IsSmooth { get; internal set; }
        public bool IsSuperSampled { get; internal set; }
        public Vector4 Padding { get; internal set; }
        public Vector2 Spacing { get; internal set; }
        public int OutlineThickness { get; internal set; }
    }
}