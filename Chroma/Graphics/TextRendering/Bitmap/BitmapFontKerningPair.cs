namespace Chroma.Graphics.TextRendering.Bitmap;

internal struct BitmapFontKerningPair
{
    public char First { get; internal set; }
    public char Second { get; internal set; }
    public int Amount { get; internal set; }
}