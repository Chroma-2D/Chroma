using System;
using System.Collections.Generic;
using System.Linq;
using Chroma.Graphics.TextRendering;
using Chroma.Natives.SDL;
//using Chroma.Graphics.Text;
//using Chroma.Graphics.Text.BMFont;
using Chroma.Windowing;

namespace Chroma.Graphics
{
    public class RenderContext
    {
        internal Window Owner { get; }
        internal SDL_gpu.GPU_Target_PTR CurrentRenderTarget { get; private set; }
        internal SDL_gpu.GPU_Target_PTR OriginalRenderTarget { get; }

        internal RenderContext(Window owner)
        {
            Owner = owner;

            CurrentRenderTarget = owner.RenderTargetPointer;
            OriginalRenderTarget = owner.RenderTargetPointer;

            LineThickness = 1;
        }

        public float LineThickness
        {
            get => SDL_gpu.GPU_GetLineThickness();
            set => SDL_gpu.GPU_SetLineThickness(value);
        }

        public void Clear(Color color)
            => SDL_gpu.GPU_ClearRGBA(CurrentRenderTarget, color.R, color.G, color.B, color.A);

        public void Arc(ShapeMode mode, Vector2 position, float radius, float startAngle, float endAngle, Color color)
        {
            if (mode == ShapeMode.Stroke)
            {
                SDL_gpu.GPU_Arc(
                    CurrentRenderTarget,
                    position.X,
                    position.Y,
                    radius,
                    startAngle,
                    endAngle,
                    color
                );
            }
            else if (mode == ShapeMode.Fill)
            {
                SDL_gpu.GPU_ArcFilled(
                    CurrentRenderTarget,
                    position.X,
                    position.Y,
                    radius,
                    startAngle,
                    endAngle,
                    color
                );
            }
        }

        public void Circle(ShapeMode mode, Vector2 position, float radius, Color color)
        {
            if (mode == ShapeMode.Stroke)
            {
                SDL_gpu.GPU_Circle(
                    CurrentRenderTarget,
                    position.X,
                    position.Y,
                    radius,
                    color
                );
            }
            else if (mode == ShapeMode.Fill)
            {
                SDL_gpu.GPU_CircleFilled(
                    CurrentRenderTarget,
                    position.X,
                    position.Y,
                    radius,
                    color
                );
            }
        }

        public void Ellipse(ShapeMode mode, Vector2 position, Vector2 radii, float rotation, Color color)
        {
            if (mode == ShapeMode.Stroke)
            {
                SDL_gpu.GPU_Ellipse(
                    CurrentRenderTarget,
                    position.X,
                    position.Y,
                    radii.X,
                    radii.Y,
                    rotation,
                    color
                );
            }
            else if (mode == ShapeMode.Fill)
            {
                SDL_gpu.GPU_EllipseFilled(
                    CurrentRenderTarget,
                    position.X,
                    position.Y,
                    radii.X,
                    radii.Y,
                    rotation,
                    color
                );
            }
        }

        public void Line(Vector2 start, Vector2 end, Color color)
        {
            SDL_gpu.GPU_Line(CurrentRenderTarget, start.X, start.Y, end.X, end.Y, color);
        }

        public void Pixel(Vector2 position, Color color)
        {
            SDL_gpu.GPU_Pixel(CurrentRenderTarget, position.X, position.Y, color);
        }

        public Color GetPixel(Vector2 position)
            => SDL_gpu.GPU_GetPixel(CurrentRenderTarget, (short)position.X, (short)position.Y);

        public void Polygon(ShapeMode mode, List<Vertex> vertices, Color color)
        {
            var floatVertexList = new List<float>();

            foreach (var v in vertices)
                floatVertexList.AddRange(v.ToGpuVertexArray());

            if (mode == ShapeMode.Stroke)
            {
                SDL_gpu.GPU_Polygon(
                    CurrentRenderTarget,
                    (uint)vertices.Count,
                    floatVertexList.ToArray(),
                    color
                );
            }
            else if (mode == ShapeMode.Fill)
            {
                SDL_gpu.GPU_PolygonFilled(
                    CurrentRenderTarget,
                    (uint)vertices.Count,
                    floatVertexList.ToArray(),
                    color
                );
            }
        }

        public void Rectangle(ShapeMode mode, Vector2 position, float width, float height, Color color)
        {
            if (mode == ShapeMode.Stroke)
            {
                SDL_gpu.GPU_Rectangle(
                    CurrentRenderTarget,
                    position.X,
                    position.Y,
                    position.X + width,
                    position.Y + height,
                    color
                );
            }
            else if (mode == ShapeMode.Fill)
            {
                SDL_gpu.GPU_RectangleFilled(
                    CurrentRenderTarget,
                    position.X,
                    position.Y,
                    position.X + width,
                    position.Y + height,
                    color
                );
            }
        }

        public void Triangle(ShapeMode mode, Vector2 a, Vector2 b, Vector2 c, Color color)
        {
            if (mode == ShapeMode.Stroke)
            {
                SDL_gpu.GPU_Tri(CurrentRenderTarget, a.X, a.Y, b.X, b.Y, c.X, c.Y, color);
            }
            else if (mode == ShapeMode.Fill)
            {
                SDL_gpu.GPU_TriFilled(CurrentRenderTarget, a.X, a.Y, b.X, b.Y, c.X, c.Y, color);
            }
        }

        public void DrawTexture(Texture texture, Vector2 position, Vector2 scale, Vector2 origin, float rotation)
        {
            SDL_gpu.GPU_BlitTransformX(
                texture.ImageHandle,
                IntPtr.Zero,
                CurrentRenderTarget,
                position.X,
                position.Y,
                origin.X,
                origin.Y,
                rotation,
                scale.X,
                scale.Y
            );
        }

        public void RenderTo(RenderTarget target, Action drawingLogic)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target),
                    "You can't just draw an image to a null render target...");

            CurrentRenderTarget = target.Handle;

            drawingLogic?.Invoke();

            CurrentRenderTarget = OriginalRenderTarget;
        }

        public void DrawString(TrueTypeFont font, string text, Vector2 position)
        {
            var x = position.X;
            var y = position.Y;
            var prevChar = (char)0;

            var maxBearing = 0;

            foreach (var c in text)
            {
                if (!font.HasGlyph(c))
                    continue;

                var info = font.RenderInfo[c];

                if (info.Bearing.Y > maxBearing)
                    maxBearing = (int)info.Bearing.Y;
            }

            foreach (var c in text)
            {
                if (c == '\n')
                {
                    x = position.X;
                    y += font.LineHeight;
                }

                if (!font.HasGlyph(c))
                    continue;

                var info = font.RenderInfo[c];

                var srcRect = new SDL_gpu.GPU_Rect
                {
                    x = info.Position.X,
                    y = info.Position.Y,
                    w = info.Size.X,
                    h = info.Size.Y
                };

                var kerning = Vector2.Zero;

                if (prevChar != 0)
                    kerning = font.GetKerning(prevChar, c);

                // info.Size.X / 2 and info.Size.Y / 2
                // to compensate for blitting anchor
                SDL_gpu.GPU_Blit(
                    font.Atlas.ImageHandle,
                    ref srcRect,
                    CurrentRenderTarget,
                    x + info.BitmapCoordinates.X + (info.Size.X / 2),
                    y - info.BitmapCoordinates.Y + (info.Size.Y / 2) + maxBearing
                );

                x += info.Advance;
                prevChar = c;
            }
        }
    }
}