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
        internal IntPtr CurrentRenderTarget { get; private set; }
        internal IntPtr OriginalRenderTarget { get; }

        public bool RenderingToWindow
            => CurrentRenderTarget == OriginalRenderTarget;

        public float LineThickness
        {
            get => SDL_gpu.GPU_GetLineThickness();
            set => SDL_gpu.GPU_SetLineThickness(value);
        }

        internal RenderContext(Window owner)
        {
            Owner = owner;

            CurrentRenderTarget = owner.RenderTargetHandle;
            OriginalRenderTarget = owner.RenderTargetHandle;

            BatchBuffer = new List<BatchInfo>();
            LineThickness = 1;

            SDL_gpu.GPU_SetDefaultAnchor(0, 0);
        }

        public void DeactivateShader()
        {
            SDL_gpu.GPU_ActivateShaderProgram(0, IntPtr.Zero);
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

        public void DrawTexture(Texture texture, Vector2 position, Vector2 scale, Vector2 origin, float rotation, Rectangle sourceRectangle)
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

            CurrentRenderTarget = target.Handle;

            drawingLogic?.Invoke();

            CurrentRenderTarget = OriginalRenderTarget;
        }

        public void DrawString(ImageFont font, string text, Vector2 position, Func<char, int, Vector2, GlyphTransformData> perCharTransform = null)
        {
            var x = position.X;
            var y = position.Y;

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if (c == '\n')
                {
                    x = position.X;
                    y += font.Height + font.LineMargin;

                    continue;
                }

                if (!font.HasGlyph(c))
                    continue;

                var rect = font.GlyphRectangles[c];

                var pos = new Vector2(x, y);
                var transform = new GlyphTransformData(pos);

                if (perCharTransform != null)
                    transform = perCharTransform(c, i, pos);

                SDL_gpu.GPU_SetColor(font.Texture.ImageHandle, transform.Color);
                SDL_gpu.GPU_BlitTransform(
                    font.Texture.ImageHandle,
                    ref rect,
                    CurrentRenderTarget,
                    transform.Position.X,
                    transform.Position.Y,
                    transform.Rotation,
                    transform.Scale.X,
                    transform.Scale.Y
                );
                SDL_gpu.GPU_SetColor(font.Texture.ImageHandle, Color.White);

                x += rect.w + font.CharSpacing;
            }
        }

        public void DrawString(TrueTypeFont font, string text, Vector2 position, Func<char, int, Vector2, TrueTypeGlyph, GlyphTransformData> perCharTransform = null)
        {
            var x = position.X;
            var y = position.Y;

            var maxBearing = 0;

            foreach (var c in text)
            {
                if (!font.HasGlyph(c))
                    continue;

                var info = font.RenderInfo[c];

                if (info.Bearing.Y > maxBearing)
                    maxBearing = (int)info.Bearing.Y;
            }

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
                    w = info.Size.X,
                    h = info.Size.Y
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
                var yPos = y - info.Bearing.Y + maxBearing;

                var transform = new GlyphTransformData(new Vector2(xPos, yPos));

                if (perCharTransform != null)
                    transform = perCharTransform(c, i, new Vector2(xPos, yPos), info);

                SDL_gpu.GPU_SetColor(font.Atlas.ImageHandle, transform.Color);
                SDL_gpu.GPU_BlitTransform(
                    font.Atlas.ImageHandle,
                    ref srcRect,
                    CurrentRenderTarget,
                    transform.Position.X,
                    transform.Position.Y,
                    transform.Rotation,
                    transform.Scale.X,
                    transform.Scale.Y
                );
                SDL_gpu.GPU_SetColor(font.Atlas.ImageHandle, Color.White);

                x += info.Advance.X;
            }
        }

        public void DrawBatch(DrawOrder order = DrawOrder.BackToFront, bool discard = true)
        {
            BatchBuffer.Sort(
                (a, b) =>
                {
                    if (order == DrawOrder.BackToFront)
                        return a.Depth.CompareTo(b.Depth);
                    else return b.Depth.CompareTo(a.Depth);
                }
            );

            for (var i = 0; i < BatchBuffer.Count; i++)
                BatchBuffer[i].DrawAction.Invoke();

            if (discard)
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