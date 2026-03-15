namespace Chroma.Graphics;

using System.Drawing;
using System.Numerics;
using Chroma.Graphics.TextRendering;

public static class Extensions
{
    extension(RenderContext context)
    {
        public void Clear(Color color
        ) => context.Clear(color.R, color.G, color.B, color.A);

        public void Clear(byte r,
            byte g,
            byte b
        ) => context.Clear(r, g, b, 255);

        public void Arc(ShapeMode mode,
            Vector2 position,
            float radius,
            float startAngle,
            float endAngle,
            Color color
        ) => context.Arc(mode, position.X, position.Y, radius, startAngle, endAngle, color);

        public void Circle(ShapeMode mode,
            Vector2 position,
            float radius,
            Color color
        ) => context.Circle(mode, position.X, position.Y, radius, color);

        public void Ellipse(ShapeMode mode,
            Vector2 position,
            Vector2 radii,
            float rotation,
            Color color
        ) => context.Ellipse(mode, position.X, position.Y, radii.X, radii.Y, rotation, color);

        public void Line(Vector2 start,
            Vector2 end,
            Color color
        ) => context.Line(start.X, start.Y, end.X, end.Y, color);

        public void Pixel(Vector2 position,
            Color color
        ) => context.Pixel(position.X, position.Y, color);

        public Color GetPixel(Vector2 position
        ) => context.GetPixel((short)position.X, (short)position.Y);

        public void Triangle(ShapeMode mode,
            Vector2 a,
            Vector2 b,
            Vector2 c,
            Color color
        ) => context.Triangle(mode, a.X, a.Y, b.X, b.Y, c.X, c.Y, color);

        public void Rectangle(ShapeMode mode,
            Vector2 position,
            float width,
            float height,
            Color color
        ) => context.Rectangle(mode, position.X, position.Y, width, height, color);

        public void Rectangle(ShapeMode mode,
            Vector2 position,
            Size size,
            Color color
        ) => context.Rectangle(mode, position, size.Width, size.Height, color);

        public void Rectangle(ShapeMode mode,
            Rectangle rectangle,
            Color color
        ) => context.Rectangle(mode, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, color);

        public void Rectangle(ShapeMode mode,
            RectangleF rectangle,
            Color color
        ) => context.Rectangle(mode, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, color);

        public void DrawTexture(Texture texture,
            Vector2 position,
            Vector2 scale,
            Vector2 origin,
            float rotation
        ) => context.DrawTexture(texture, position, scale, origin, rotation, null);

        public void DrawTexture(Texture texture,
            Vector2 position,
            Vector2 scale,
            Vector2 origin
        ) => context.DrawTexture(texture, position, scale, origin, 0, null);

        public void DrawTexture(Texture texture,
            Vector2 position,
            Vector2 scale
        ) => context.DrawTexture(texture, position, scale, Vector2.Zero, 0, null);

        public void DrawTexture(Texture texture,
            Vector2 position
        ) => context.DrawTexture(texture, position, Vector2.One, Vector2.Zero, 0, null);

        public void DrawString(IFontProvider font,
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

        public void DrawString(IFontProvider font,
            string text,
            Vector2 position,
            GlyphTransform? perCharTransform = null
        ) => context.DrawString(font, text, position.X, position.Y, perCharTransform);

        public void DrawString(IFontProvider font,
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

        public void DrawString(string text,
            float x,
            float y,
            GlyphTransform? perCharTransform = null
        ) => context.DrawString(EmbeddedAssets.DefaultFont, text, x, y, perCharTransform);

        public void DrawString(string text,
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

        public void DrawString(string text,
            Vector2 position,
            GlyphTransform? perCharTransform = null
        ) => context.DrawString(EmbeddedAssets.DefaultFont, text, position.X, position.Y, perCharTransform);

        public void DrawString(string text,
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
}