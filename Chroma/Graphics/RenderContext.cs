using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Chroma.Graphics.Batching;
using Chroma.Graphics.TextRendering;
using Chroma.Natives.SDL;
using Chroma.Windowing;

namespace Chroma.Graphics
{
    public class RenderContext
    {
        internal List<BatchInfo> BatchBuffer { get; }

        internal Window Owner { get; }
        internal IntPtr CurrentRenderTarget => TargetStack.Peek();

        internal Stack<IntPtr> TargetStack { get; }

        public bool RenderingToWindow
            => CurrentRenderTarget == Owner.RenderTargetHandle;

        internal RenderContext(Window owner)
        {
            Owner = owner;

            TargetStack = new Stack<IntPtr>();
            TargetStack.Push(owner.RenderTargetHandle);

            BatchBuffer = new List<BatchInfo>();
        }

        public void Clear(Color color)
            => SDL_gpu.GPU_ClearRGBA(CurrentRenderTarget, color.R, color.G, color.B, color.A);

        public void Arc(ShapeMode mode, float x, float y, float radius, float startAngle, float endAngle, Color color)
        {
            if (mode == ShapeMode.Stroke)
            {
                SDL_gpu.GPU_Arc(
                    CurrentRenderTarget,
                    x,
                    y,
                    radius,
                    startAngle,
                    endAngle,
                    Color.ToSdlColor(color)
                );
            }
            else if (mode == ShapeMode.Fill)
            {
                SDL_gpu.GPU_ArcFilled(
                    CurrentRenderTarget,
                    x,
                    y,
                    radius,
                    startAngle,
                    endAngle,
                    Color.ToSdlColor(color)
                );
            }
        }

        public void Arc(ShapeMode mode, Vector2 position, float radius, float startAngle, float endAngle, Color color)
            => Arc(mode, position.X, position.Y, radius, startAngle, endAngle, color);

        public void Circle(ShapeMode mode, float x, float y, float radius, Color color)
        {
            if (mode == ShapeMode.Stroke)
            {
                SDL_gpu.GPU_Circle(
                    CurrentRenderTarget,
                    x,
                    y,
                    radius,
                    Color.ToSdlColor(color)
                );
            }
            else if (mode == ShapeMode.Fill)
            {
                SDL_gpu.GPU_CircleFilled(
                    CurrentRenderTarget,
                    x,
                    y,
                    radius,
                    Color.ToSdlColor(color)
                );
            }
        }

        public void Circle(ShapeMode mode, Vector2 position, float radius, Color color)
            => Circle(mode, position.X, position.Y, radius, color);

        public void Ellipse(ShapeMode mode, float x, float y, float hRadius, float vRadius, float rotation, Color color)
        {
            if (mode == ShapeMode.Stroke)
            {
                SDL_gpu.GPU_Ellipse(
                    CurrentRenderTarget,
                    x,
                    y,
                    hRadius,
                    vRadius,
                    rotation,
                    Color.ToSdlColor(color)
                );
            }
            else if (mode == ShapeMode.Fill)
            {
                SDL_gpu.GPU_EllipseFilled(
                    CurrentRenderTarget,
                    x,
                    y,
                    hRadius,
                    vRadius,
                    rotation,
                    Color.ToSdlColor(color)
                );
            }
        }

        public void Ellipse(ShapeMode mode, Vector2 position, Vector2 radii, float rotation, Color color)
            => Ellipse(mode, position.X, position.Y, radii.X, radii.Y, rotation, color);

        public void Line(float x1, float y1, float x2, float y2, Color color)
            => SDL_gpu.GPU_Line(CurrentRenderTarget, x1, y1, x2, y2, Color.ToSdlColor(color));

        public void Line(Vector2 start, Vector2 end, Color color)
            => Line(start.X, start.Y, end.X, end.Y, color);
        
        public void Pixel(float x, float y, Color color)
            => SDL_gpu.GPU_Pixel(CurrentRenderTarget, x, y, Color.ToSdlColor(color));

        public void Pixel(Vector2 position, Color color)
            => Pixel(position.X, position.Y, color);
        
        public Color GetPixel(short x, short y)
            => Color.FromSdlColor(SDL_gpu.GPU_GetPixel(CurrentRenderTarget, x, y));

        public Color GetPixel(Vector2 position)
            => GetPixel((short)position.X, (short)position.Y);

        public void Polygon(ShapeMode mode, List<Point> vertices, Color color)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException(nameof(vertices), "Vertex list cannot be null.");
            }

            var floatArray = new float[vertices.Count * 2];

            for (var i = 0; i < vertices.Count; i++)
            {
                floatArray[i * 2] = vertices[i].X;
                floatArray[i * 2 + 1] = vertices[i].Y;
            }

            if (mode == ShapeMode.Stroke)
            {
                SDL_gpu.GPU_Polygon(
                    CurrentRenderTarget,
                    (uint)vertices.Count,
                    floatArray,
                    Color.ToSdlColor(color)
                );
            }
            else if (mode == ShapeMode.Fill)
            {
                SDL_gpu.GPU_PolygonFilled(
                    CurrentRenderTarget,
                    (uint)vertices.Count,
                    floatArray,
                    Color.ToSdlColor(color)
                );
            }
        }

        public void Polyline(List<Point> vertices, Color color, bool closeLoop)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException(nameof(vertices), "Vertex list cannot be null.");
            }

            for (var i = 0; i < vertices.Count; i++)
            {
                if (i + 1 >= vertices.Count)
                    break;

                Line(
                    new Vector2(vertices[i].X, vertices[i].Y),
                    new Vector2(vertices[i + 1].X, vertices[i + 1].Y),
                    color
                );
            }

            if (closeLoop)
            {
                Line(
                    new Vector2(vertices[0].X, vertices[0].Y),
                    new Vector2(vertices[vertices.Count - 1].X, vertices[vertices.Count - 1].Y),
                    color
                );
            }
        }

        public void Rectangle(ShapeMode mode, float x, float y, float width, float height, Color color)
        {
            if (mode == ShapeMode.Stroke)
            {
                SDL_gpu.GPU_Rectangle(
                    CurrentRenderTarget,
                    x,
                    y,
                    x + width,
                    y + height,
                    Color.ToSdlColor(color)
                );
            }
            else if (mode == ShapeMode.Fill)
            {
                SDL_gpu.GPU_RectangleFilled(
                    CurrentRenderTarget,
                    x,
                    y,
                    x + width,
                    y + height,
                    Color.ToSdlColor(color)
                );
            }
        }

        public void Rectangle(ShapeMode mode, Vector2 position, float width, float height, Color color)
            => Rectangle(mode, position.X, position.Y, width, height, color);

        public void Rectangle(ShapeMode mode, Vector2 position, Size size, Color color)
            => Rectangle(mode, position, size.Width, size.Height, color);

        public void Rectangle(ShapeMode mode, Rectangle rectangle, Color color)
            => Rectangle(
                mode,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                color
            );

        public void Rectangle(ShapeMode mode, RectangleF rectangle, Color color)
            => Rectangle(
                mode,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                color
            );

        public void Triangle(ShapeMode mode, Vector2 a, Vector2 b, Vector2 c, Color color)
        {
            if (mode == ShapeMode.Stroke)
            {
                SDL_gpu.GPU_Tri(
                    CurrentRenderTarget,
                    a.X, a.Y,
                    b.X, b.Y,
                    c.X, c.Y,
                    Color.ToSdlColor(color)
                );
            }
            else if (mode == ShapeMode.Fill)
            {
                SDL_gpu.GPU_TriFilled(
                    CurrentRenderTarget,
                    a.X, a.Y,
                    b.X, b.Y,
                    c.X, c.Y,
                    Color.ToSdlColor(color)
                );
            }
        }

        public void RenderArbitraryGeometry(Texture texture, VertexFormat format, ushort vertexCount,
            float[] vertexData, ushort[] indices)
        {
            if (texture == null)
            {
                throw new ArgumentNullException(nameof(texture), "Texture cannot be null.");
            }

            SDL_gpu.GPU_TriangleBatch(
                texture.ImageHandle,
                CurrentRenderTarget,
                vertexCount,
                vertexData,
                (ushort)indices.Length,
                indices,
                (SDL_gpu.GPU_BatchFlagEnum)format
            );
        }

        public void DrawTexture(Texture texture, Vector2 position, Vector2 scale, Vector2 origin, float rotation)
        {
            if (texture == null)
            {
                throw new ArgumentNullException(nameof(texture), "Texture cannot be null.");
            }

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

        public void DrawTexture(Texture texture, Vector2 position, Vector2 scale, Vector2 origin, float rotation,
            Rectangle sourceRectangle)
        {
            var rect = new SDL_gpu.GPU_Rect
            {
                x = sourceRectangle.X,
                y = sourceRectangle.Y,
                w = sourceRectangle.Width,
                h = sourceRectangle.Height
            };

            SDL_gpu.GPU_BlitTransformX(
                texture.ImageHandle,
                ref rect,
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
            {
                throw new ArgumentNullException(nameof(target), "Render target you're drawing to cannot be null.");
            }

            TargetStack.Push(target.TargetHandle);
            drawingLogic?.Invoke();
            TargetStack.Pop();
        }

        public void WithCamera(Camera camera, Action drawingLogic)
        {
            if (camera == null)
            {
                throw new ArgumentNullException(nameof(camera), "Cannot assign a null camera to a render target.");
            }

            SDL_gpu.GPU_SetCamera(CurrentRenderTarget, ref camera.GpuCamera);
            drawingLogic?.Invoke();
            SDL_gpu.GPU_SetCamera(CurrentRenderTarget, IntPtr.Zero);
        }

        public void DrawString(IFontProvider font, string text, Vector2 position, GlyphTransform glyphTransform = null)
        {
            if (font == null)
            {
                throw new ArgumentNullException(nameof(font), "Font cannot be null.");
            }

            text = text ?? string.Empty;

            var x = position.X;
            var y = position.Y;

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if (c == '\n')
                {
                    x = position.X;
                    y += font.LineSpacing;

                    continue;
                }

                if (!font.HasGlyph(c))
                    continue;

                var bounds = font.GetGlyphBounds(c);
                var offsets = font.GetRenderOffsets(c);
                var advance = font.GetHorizontalAdvance(c);
                var texture = font.GetTexture(c);

                var srcRect = new SDL_gpu.GPU_Rect
                {
                    x = bounds.X,
                    y = bounds.Y,
                    w = bounds.Width,
                    h = bounds.Height
                };

                // info.Size.X / 2 and info.Size.Y / 2
                // to compensate for blitting anchor.
                // for some reason settings the blitting anchor to [0, 0]
                // makes the entire text blurry at time of blitting
                //
                // 12 apr 2020: fixed by setting Texture snapping mode to
                // TextureSnappingMode.None by default, along with
                // defaulting the anchor to 0.
                var xPos = x + offsets.X;
                var yPos = y + offsets.Y;

                if (font.IsKerningEnabled && i != 0)
                {
                    var kerning = font.GetKerning(text[i - 1], text[i]);
                    xPos += kerning;
                }

                var pos = new Vector2(xPos, yPos);
                var transform = new GlyphTransformData(pos);

                if (glyphTransform != null)
                    transform = glyphTransform(c, i, pos);

                SDL_gpu.GPU_SetColor(texture.ImageHandle, Color.ToSdlColor(transform.Color));
                SDL_gpu.GPU_BlitTransformX(
                    texture.ImageHandle,
                    ref srcRect,
                    CurrentRenderTarget,
                    transform.Position.X,
                    transform.Position.Y,
                    transform.Origin.X,
                    transform.Origin.Y,
                    transform.Rotation,
                    transform.Scale.X,
                    transform.Scale.Y
                );
                SDL_gpu.GPU_SetColor(texture.ImageHandle, Color.ToSdlColor(Color.White));

                x += advance;
            }
        }

        public void DrawString(string text, Vector2 position, GlyphTransform perCharTransform = null)
            => DrawString(EmbeddedAssets.DefaultFont, text, position, perCharTransform);

        public void DrawString(IFontProvider font, string text, Vector2 position, Color color)
            => DrawString(font, text, position, (_, _, p) => new GlyphTransformData(p) {Color = color});

        public void DrawString(string text, Vector2 position, Color color)
            => DrawString(
                EmbeddedAssets.DefaultFont,
                text,
                position,
                (_, _, p) => new GlyphTransformData(p) {Color = color}
            );

        public void DrawBatch(DrawOrder order = DrawOrder.BackToFront, bool discardBatchAfterUse = true)
        {
            BatchBuffer.Sort(
                (a, b) => order == DrawOrder.BackToFront
                    ? a.Depth.CompareTo(b.Depth)
                    : b.Depth.CompareTo(a.Depth)
            );

            for (var i = 0; i < BatchBuffer.Count; i++)
                BatchBuffer[i].DrawAction?.Invoke();

            if (discardBatchAfterUse)
                BatchBuffer.Clear();
        }

        public void Batch(Action drawAction, int depth)
        {
            BatchBuffer.Add(
                new BatchInfo
                {
                    DrawAction = drawAction,
                    Depth = depth
                }
            );
        }
    }
}