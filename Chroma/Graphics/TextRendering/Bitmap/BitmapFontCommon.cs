namespace Chroma.Graphics.TextRendering.Bitmap
{
    public class BitmapFontCommon
    {
        public int LineHeight { get; internal set; }
        public int BaseLine { get; internal set; }
        public int ScaleW { get; internal set; }
        public int ScaleH { get; internal set; }
        public int PageCount { get; internal set; }
        public bool IsPacked { get; internal set; }

        public BitmapFontChannelMode AlphaMode { get; internal set; }
        public BitmapFontChannelMode RedMode { get; internal set; }
        public BitmapFontChannelMode GreenMode { get; internal set; }
        public BitmapFontChannelMode BlueMode { get; internal set; }
    }
}