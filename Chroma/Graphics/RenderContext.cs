using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Chroma.Graphics.Batching;
using Chroma.Graphics.TextRendering;
using Chroma.Graphics.TextRendering.Bitmap;
using Chroma.Natives.SDL;
using Chroma.Windowing;

namespace Chroma.Graphics
{
    public class RenderContext
    {
        private Rectangle _scissor = System.Drawing.Rectangle.Empty;

        internal List<BatchInfo> BatchBuffer { get; }

        internal Window Owner { get; }
        internal IntPtr CurrentRenderTarget => TargetStack.Peek();

        internal Stack<IntPtr> TargetStack { get; }

        public bool RenderingToWindow
            => CurrentRenderTarget == Owner.RenderTargetHandle;

        public Rectangle Scissor
        {
            get => _scissor;
            set
            {
                _scissor = value;

                if (_scissor == System.Drawing.Rectangle.Empty)
                {
                    SDL_gpu.GPU_UnsetClip(CurrentRenderTarget);
                }
                else
                {
                    SDL_gpu.GPU_SetClip(
                        CurrentRenderTarget,
                        (short)_scissor.X,
                        (short)_scissor.Y,
                        (ushort)_scissor.Width,
                        (ushort)_scissor.Height
                    );
                }
            }
        }

        internal RenderContext(Window owner)
        {
            Owner = owner;

            TargetStack = new Stack<IntPtr>();
            TargetStack.Push(owner.RenderTargetHandle);

            BatchBuffer = new List<BatchInfo>();
        }

        public void WithCamera(Camera camera, Action drawingLogic)
        {
            if (camera == null)
                throw new ArgumentNullException(nameof(camera), "Camera cannot be null.");

            SDL_gpu.GPU_SetCamera(CurrentRenderTarget, ref camera.GpuCamera);

            drawingLogic?.Invoke();

            SDL_gpu.GPU_SetCamera(CurrentRenderTarget, IntPtr.Zero);
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
                    Color.ToSdlColor(color)
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
                    Color.ToSdlColor(color)
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
                    Color.ToSdlColor(color)
                );
            }
            else if (mode == ShapeMode.Fill)
            {
                SDL_gpu.GPU_CircleFilled(
                    CurrentRenderTarget,
                    position.X,
                    position.Y,
                    radius,
                    Color.ToSdlColor(color)
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
                    Color.ToSdlColor(color)
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
                    Color.ToSdlColor(color)
                );
            }
        }

        public void Line(Vector2 start, Vector2 end, Color color)
            => SDL_gpu.GPU_Line(CurrentRenderTarget, start.X, start.Y, end.X, end.Y, Color.ToSdlColor(color));

        public void Pixel(Vector2 position, Color color)
            => SDL_gpu.GPU_Pixel(CurrentRenderTarget, position.X, position.Y, Color.ToSdlColor(color));

        public Color GetPixel(Vector2 position)
            => Color.FromSdlColor(SDL_gpu.GPU_GetPixel(CurrentRenderTarget, (short)position.X, (short)position.Y));

        public void Polygon(ShapeMode mode, List<Point> vertices, Color color)
        {
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
                    Color.ToSdlColor(color)
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
                    Color.ToSdlColor(color)
                );
            }
        }

        public void Rectangle(ShapeMode mode, Vector2 position, Size size, Color color)
            => Rectangle(mode, position, size.Width, size.Height, color);

        public void Rectangle(ShapeMode mode, Rectangle rectangle, Color color)
            => Rectangle(
                mode,
                new Vector2(rectangle.X, rectangle.Y),
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
                throw new ArgumentNullException(nameof(target),
                    "You can't just draw an image to a null render target...");

            TargetStack.Push(target.TargetHandle);
            drawingLogic?.Invoke();
            TargetStack.Pop();
        }

        public void DrawString(BitmapFont font, string text, Vector2 position,
            BitmapGlyphTransform glyphTransform = null)
        {
            var x = position.X;
            var y = position.Y;

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if (c == '\n')
                {
                    x = position.X;
                    y += font.Common.LineHeight + font.Info.Spacing.Y;

                    continue;
                }

                if (!font.HasGlyph(c))
                    continue;

                var glyph = font.Glyphs[c];

                var rect = new SDL_gpu.GPU_Rect
                {
                    x = glyph.BitmapX,
                    y = glyph.BitmapY,
                    w = glyph.Width,
                    h = glyph.Height
                };

                var pageTexture = font.Pages[glyph.Page].Texture;

                var kerningAmount = 0f;
                if (i != 0 && font.UseKerning)
                {
                    var kerning = font.GetKerning(text[i - 1], text[i]);

                    if (kerning != null)
                        kerningAmount = kerning.Value.Amount;
                }

                var pos = new Vector2(
                    x + glyph.OffsetX + kerningAmount,
                    y + glyph.OffsetY
                );

                var transform = new GlyphTransformData(pos);

                if (glyphTransform != null)
                    transform = glyphTransform(c, i, pos, glyph);

                SDL_gpu.GPU_SetColor(pageTexture.ImageHandle, Color.ToSdlColor(transform.Color));
                SDL_gpu.GPU_BlitTransformX(
                    pageTexture.ImageHandle,
                    ref rect,
                    CurrentRenderTarget,
                    transform.Position.X,
                    transform.Position.Y,
                    transform.Origin.X,
                    transform.Origin.Y,
                    transform.Rotation,
                    transform.Scale.X,
                    transform.Scale.Y
                );
                SDL_gpu.GPU_SetColor(pageTexture.ImageHandle, Color.ToSdlColor(Color.White));

                x += glyph.HorizontalAdvance;
            }
        }

        public void DrawString(BitmapFont font, string text, Vector2 position, Color color)
            => DrawString(font, text, position, (_, _, p, _) => new GlyphTransformData(p) {Color = color});

        public void DrawString(string text, Vector2 position,
            TrueTypeFontGlyphTransform perCharTransform = null)
            => DrawString(EmbeddedAssets.DefaultFont, text, position, perCharTransform);

        public void DrawString(string text, Vector2 position, Color color)
            => DrawString(EmbeddedAssets.DefaultFont, text, position,
                (_, _, p, _) => new GlyphTransformData(p) {Color = color});

        public void DrawString(TrueTypeFont font, string text, Vector2 position,
            TrueTypeFontGlyphTransform glyphTransform = null)
        {
            var x = position.X;
            var y = position.Y;

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if (c == '\n')
                {
                    x = position.X;
                    y += font.ScaledLineSpacing;

                    continue;
                }

                if (!font.HasGlyph(c))
                    continue;

                var info = font.RenderInfo[c];

                var srcRect = new SDL_gpu.GPU_Rect
                {
                    x = info.Position.X,
                    y = info.Position.Y,
                    w = info.BitmapSize.X,
                    h = info.BitmapSize.Y
                };

                // info.Size.X / 2 and info.Size.Y / 2
                // to compensate for blitting anchor.
                // for some reason settings the blitting anchor to [0, 0]
                // makes the entire text blurry at time of blitting
                //
                // 12 apr 2020: fixed by setting Texture snapping mode to
                // TextureSnappingMode.None by default, along with
                // defaulting the anchor to 0.
                var xPos = x + info.Bearing.X;
                var yPos = y - info.Bearing.Y + font.MaxBearing;

                if (font.UseKerning && i != 0)
                {
                    var kerning = font.GetKerning(text[i - 1], text[i]);
                    xPos += kerning.X;
                }

                var pos = new Vector2(xPos, yPos);
                var transform = new GlyphTransformData(pos);

                if (glyphTransform != null)
                    transform = glyphTransform(c, i, pos, info);

                SDL_gpu.GPU_SetColor(font.Atlas.ImageHandle, Color.ToSdlColor(transform.Color));
                SDL_gpu.GPU_BlitTransformX(
                    font.Atlas.ImageHandle,
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
                SDL_gpu.GPU_SetColor(font.Atlas.ImageHandle, Color.ToSdlColor(Color.White));

                x += info.Advance.X;
            }
        }

        public void DrawString(TrueTypeFont font, string text, Vector2 position, Color color)
            => DrawString(font, text, position, (_, _, p, _) => new GlyphTransformData(p) {Color = color});

        public void DrawBatch(DrawOrder order = DrawOrder.BackToFront, bool discardBatchAfterUse = true)
        {
            BatchBuffer.Sort(
                (a, b) => order == DrawOrder.BackToFront
                    ? a.Depth.CompareTo(b.Depth)
                    : b.Depth.CompareTo(a.Depth)
            );

            for (var i = 0; i < BatchBuffer.Count; i++)
                BatchBuffer[i].DrawAction.Invoke();

            if (discardBatchAfterUse)
                BatchBuffer.Clear();
        }

        public void Batch(Action drawAction, int depth)
        {
            if (drawAction == null)
                return;

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