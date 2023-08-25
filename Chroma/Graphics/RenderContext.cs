using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Chroma.Graphics.Batching;
using Chroma.Graphics.TextRendering;
using Chroma.Natives.Bindings.SDL;
using Chroma.Windowing;

namespace Chroma.Graphics
{
    public sealed class RenderContext
    {
        public delegate void RenderTargetDrawDelegate(RenderContext context, RenderTarget target);
        public delegate void CameraDrawDelegate(RenderContext context, Camera camera);
        
        private readonly GlyphTransformData _transformData = new();
        
        internal Window Owner { get; }
        internal IntPtr CurrentRenderTarget => TargetStack.Peek();

        internal Stack<IntPtr> TargetStack { get; } = new();
        internal List<BatchInfo> BatchBuffer { get; } = new();

        public bool RenderingToWindow
            => CurrentRenderTarget == Owner.RenderTargetHandle;

        internal RenderContext(Window owner)
        {
            Owner = owner;
            TargetStack.Push(owner.RenderTargetHandle);
        }

        public void Clear(byte r, byte g, byte b, byte a)
            => SDL_gpu.GPU_ClearRGBA(CurrentRenderTarget, r, g, b, a);

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

        public void Line(float x1, float y1, float x2, float y2, Color color)
            => SDL_gpu.GPU_Line(CurrentRenderTarget, x1, y1, x2, y2, Color.ToSdlColor(color));

        public void Pixel(float x, float y, Color color)
            => SDL_gpu.GPU_Pixel(CurrentRenderTarget, x, y, Color.ToSdlColor(color));

        public Color GetPixel(short x, short y)
            => Color.FromSdlColor(SDL_gpu.GPU_GetPixel(CurrentRenderTarget, x, y));

        public void Polygon(ShapeMode mode, List<Vector2> vertices, Color color)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException(
                    nameof(vertices), 
                    "Vertex list cannot be null."
                );
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

        public void Polyline(List<Vector2> vertices, Color color, bool closeLoop)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException(
                    nameof(vertices), 
                    "Vertex list cannot be null."
                );
            }

            for (var i = 0; i < vertices.Count; i++)
            {
                if (i + 1 >= vertices.Count)
                    break;

                Line(
                    vertices[i].X,
                    vertices[i].Y,
                    vertices[i + 1].X,
                    vertices[i + 1].Y,
                    color
                );
            }

            if (closeLoop)
            {
                Line(
                    vertices[0].X,
                    vertices[0].Y,
                    vertices[^1].X,
                    vertices[^1].Y,
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

        public void Triangle(ShapeMode mode, float x1, float y1, float x2, float y2, float x3, float y3, Color color)
        {
            if (mode == ShapeMode.Stroke)
            {
                SDL_gpu.GPU_Tri(
                    CurrentRenderTarget,
                    x1, y1,
                    x2, y2,
                    x3, y3,
                    Color.ToSdlColor(color)
                );
            }
            else if (mode == ShapeMode.Fill)
            {
                SDL_gpu.GPU_TriFilled(
                    CurrentRenderTarget,
                    x1, y1,
                    x2, y2,
                    x3, y3,
                    Color.ToSdlColor(color)
                );
            }
        }

        public void RenderArbitraryGeometry(Texture texture, VertexFormat format, ushort vertexCount,
            float[] vertexData, ushort[] indices)
        {
            if (texture == null)
            {
                throw new ArgumentNullException(
                    nameof(texture), 
                    "Texture cannot be null."
                );
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

        public void DrawTexture(Texture texture, Vector2 position, Vector2 scale, Vector2 origin, float rotation,
            Rectangle? sourceRectangle)
        {
            if (texture == null)
            {
                throw new ArgumentNullException(nameof(texture), "Texture cannot be null.");
            }
            
            unsafe
            {
                var rect = (SDL_gpu.GPU_Rect*)0;

                if (sourceRectangle.HasValue)
                {
                    var r = new SDL_gpu.GPU_Rect
                    {
                        x = sourceRectangle.Value.X,
                        y = sourceRectangle.Value.Y,
                        w = sourceRectangle.Value.Width,
                        h = sourceRectangle.Value.Height
                    };

                    rect = &r;
                }

                SDL_gpu.GPU_BlitTransformX(
                    texture.ImageHandle,
                    rect,
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
        }

        [Obsolete("This call is obsolete and will be removed in version 0.61. Use the alternate overload instead.")]
        public void RenderTo(RenderTarget target, Action drawingLogic)
        {           
            if (target == null)
            {
                throw new ArgumentNullException(
                    nameof(target), 
                    "Render target cannot be null."
                );
            }
            
            if (drawingLogic == null)
                return;

            TargetStack.Push(target.TargetHandle);
            {
                drawingLogic.Invoke();
            }
            TargetStack.Pop();
        }
        
        public void RenderTo(RenderTarget target, RenderTargetDrawDelegate drawingLogic)
        {           
            if (target == null)
            {
                throw new ArgumentNullException(
                    nameof(target), 
                    "Render target cannot be null."
                );
            }
            
            if (drawingLogic == null)
                return;

            TargetStack.Push(target.TargetHandle);
            {
                drawingLogic.Invoke(this, target);
            }
            TargetStack.Pop();
        }

        [Obsolete("This call is obsolete and will be removed in version 0.61. Use the alternate overload instead.")]
        public void WithCamera(Camera camera, Action drawingLogic)
        {
            if (camera == null)
            {
                throw new ArgumentNullException(
                    nameof(camera), 
                    "Camera cannot be null"
                );
            }

            if (drawingLogic == null)
                return;

            SDL_gpu.GPU_SetCamera(CurrentRenderTarget, ref camera.GpuCamera);
            {
                drawingLogic.Invoke();
            }
            SDL_gpu.GPU_SetCamera(CurrentRenderTarget, IntPtr.Zero);
        }
        
        public void WithCamera(Camera camera, CameraDrawDelegate drawingLogic)
        {
            if (camera == null)
            {
                throw new ArgumentNullException(
                    nameof(camera), 
                    "Camera cannot be null"
                );
            }

            if (drawingLogic == null)
                return;

            SDL_gpu.GPU_SetCamera(CurrentRenderTarget, ref camera.GpuCamera);
            {
                drawingLogic.Invoke(this, camera);
            }
            SDL_gpu.GPU_SetCamera(CurrentRenderTarget, IntPtr.Zero);
        }
        
        public void DrawString(IFontProvider font, string text, float x, float y, GlyphTransform glyphTransform = null)
        {
            if (font == null)
            {
                throw new ArgumentNullException(nameof(font), "Font cannot be null.");
            }

            text ??= string.Empty;

            var tx = x;
            var ty = y;

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if (c == '\n')
                {
                    tx = x;
                    ty += font.LineSpacing;

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
                var xPos = tx + offsets.X;
                var yPos = ty + offsets.Y;

                if (font.IsKerningEnabled && i != 0)
                {
                    var kerning = font.GetKerning(text[i - 1], text[i]);
                    xPos += kerning;
                }

                var pos = new Vector2(xPos, yPos);
                
                _transformData.Clear(pos, new GlyphRenderMetrics(bounds, offsets, advance));
                
                if (glyphTransform != null)
                    glyphTransform(_transformData, c, i, pos);

                SDL_gpu.GPU_SetColor(texture.ImageHandle, Color.ToSdlColor(_transformData.Color));
                SDL_gpu.GPU_BlitTransformX(
                    texture.ImageHandle,
                    ref srcRect,
                    CurrentRenderTarget,
                    _transformData.Position.X,
                    _transformData.Position.Y,
                    _transformData.Origin.X,
                    _transformData.Origin.Y,
                    _transformData.Rotation,
                    _transformData.Scale.X,
                    _transformData.Scale.Y
                );
                SDL_gpu.GPU_SetColor(texture.ImageHandle, Color.ToSdlColor(Color.White));

                tx += advance;
            }
        }

        public void DrawBatch(DrawOrder order = DrawOrder.BackToFront, bool discardBatchAfterUse = true)
        {
            BatchBuffer.Sort(
                (a, b) => order == DrawOrder.BackToFront
                    ? a.Depth.CompareTo(b.Depth)
                    : b.Depth.CompareTo(a.Depth)
            );

            for (var i = 0; i < BatchBuffer.Count; i++)
                BatchBuffer[i].DrawAction?.Invoke(this);

            if (discardBatchAfterUse)
                BatchBuffer.Clear();
        }
        
        [Obsolete("This call is obsolete and will be removed in version 0.61. Use the alternate overload instead.")]
        public void Batch(Action drawAction, int depth)
        {
            if (drawAction == null)
                return;

            BatchBuffer.Add(
                new BatchInfo
                {
                    DrawAction = (c) => drawAction(),
                    Depth = depth
                }
            );
        }

        public void Batch(Action<RenderContext> drawAction, int depth)
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