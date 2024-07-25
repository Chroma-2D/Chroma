namespace Chroma.Graphics;

using System.Drawing;
using System.Numerics;
using Chroma.Graphics.TextRendering;

public static class Extensions
{
    public static void Clear(
        this RenderContext context,
        Color color
    ) => context.Clear(color.R, color.G, color.B, color.A);

    public static void Clear(
        this RenderContext context,
        byte r,
        byte g,
        byte b
    ) => context.Clear(r, g, b, 255);
        
    public static void Arc(
        this RenderContext context,
        ShapeMode mode,
        Vector2 position,
        float radius,
        float startAngle,
        float endAngle,
        Color color
    ) => context.Arc(mode, position.X, position.Y, radius, startAngle, endAngle, color);

    public static void Circle(
        this RenderContext context,
        ShapeMode mode,
        Vector2 position,
        float radius,
        Color color
    ) => context.Circle(mode, position.X, position.Y, radius, color);

    public static void Ellipse(
        this RenderContext context,
        ShapeMode mode,
        Vector2 position,
        Vector2 radii,
        float rotation,
        Color color
    ) => context.Ellipse(mode, position.X, position.Y, radii.X, radii.Y, rotation, color);

    public static void Line(
        this RenderContext context,
        Vector2 start,
        Vector2 end,
        Color color
    ) => context.Line(start.X, start.Y, end.X, end.Y, color);

    public static void Pixel(
        this RenderContext context,
        Vector2 position,
        Color color
    ) => context.Pixel(position.X, position.Y, color);

    public static Color GetPixel(
        this RenderContext context,
        Vector2 position
    ) => context.GetPixel((short)position.X, (short)position.Y);

    public static void Triangle(
        this RenderContext context,
        ShapeMode mode,
        Vector2 a,
        Vector2 b,
        Vector2 c,
        Color color
    ) => context.Triangle(mode, a.X, a.Y, b.X, b.Y, c.X, c.Y, color);
        
    public static void Rectangle(
        this RenderContext context,
        ShapeMode mode,
        Vector2 position,
        float width,
        float height,
        Color color
    ) => context.Rectangle(mode, position.X, position.Y, width, height, color);

    public static void Rectangle(
        this RenderContext context,
        ShapeMode mode,
        Vector2 position,
        Size size,
        Color color
    ) => context.Rectangle(mode, position, size.Width, size.Height, color);
        
    public static void Rectangle(
        this RenderContext context,
        ShapeMode mode,
        Rectangle rectangle,
        Color color
    ) => context.Rectangle(mode, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, color);

    public static void Rectangle(
        this RenderContext context,
        ShapeMode mode,
        RectangleF rectangle,
        Color color
    ) => context.Rectangle(mode, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, color);

    public static void DrawTexture(
        this RenderContext context,
        Texture texture,
        Vector2 position,
        Vector2 scale,
        Vector2 origin,
        float rotation
    ) => context.DrawTexture(texture, position, scale, origin, rotation, null);
        
    public static void DrawTexture(
        this RenderContext context,
        Texture texture,
        Vector2 position,
        Vector2 scale,
        Vector2 origin
    ) => context.DrawTexture(texture, position, scale, origin, 0, null);
        
    public static void DrawTexture(
        this RenderContext context,
        Texture texture,
        Vector2 position,
        Vector2 scale
    ) => context.DrawTexture(texture, position, scale, Vector2.Zero, 0, null);
        
    public static void DrawTexture(
        this RenderContext context,
        Texture texture,
        Vector2 position
    ) => context.DrawTexture(texture, position, Vector2.One, Vector2.Zero, 0, null);

    public static void DrawString(
        this RenderContext context,
        IFontProvider font,
        string text,
        float x,
        float y,
        Color color
    ) => context.DrawString(font, text, x, y, 
        (d, _, _, p) =>
        {
            d.Position = p;
            d.Color = color;
        }
    );
        
    public static void DrawString(
        this RenderContext context,
        IFontProvider font,
        string text,
        Vector2 position,
        GlyphTransform perCharTransform = null
    ) => context.DrawString(font, text, position.X, position.Y, perCharTransform);
        
    public static void DrawString(
        this RenderContext context,
        IFontProvider font,
        string text,
        Vector2 position,
        Color color
    ) => context.DrawString(font, text, position.X, position.Y, 
        (d, _, _, p) =>
        {
            d.Position = p;
            d.Color = color;
        }
    );

    public static void DrawString(
        this RenderContext context,
        string text,
        float x,
        float y,
        GlyphTransform perCharTransform = null
    ) => context.DrawString(EmbeddedAssets.DefaultFont, text, x, y, perCharTransform);
        
    public static void DrawString(
        this RenderContext context,
        string text,
        float x,
        float y,
        Color color
    ) => context.DrawString(
        EmbeddedAssets.DefaultFont, 
        text, 
        x, 
        y,            
        (d, _, _, p) =>
        {
            d.Position = p;
            d.Color = color;
        }
    );

        
    public static void DrawString(
        this RenderContext context,
        string text,
        Vector2 position,
        GlyphTransform perCharTransform = null
    ) => context.DrawString(EmbeddedAssets.DefaultFont, text, position.X, position.Y, perCharTransform);
        
    public static void DrawString(
        this RenderContext context,
        string text,
        Vector2 position,
        Color color
    ) => context.DrawString(
        EmbeddedAssets.DefaultFont, 
        text, 
        position.X, 
        position.Y,
        (d, _, _, p) =>
        {
            d.Position = p;
            d.Color = color;
        }
    );
}