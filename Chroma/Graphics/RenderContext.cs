using Chroma.SDL2;
using Chroma.Windowing;
using System.Collections.Generic;

namespace Chroma.Graphics
{
    public class RenderContext
    {
        internal OpenGlWindow Owner { get; }
        internal SDL_gpu.GPU_Target_PTR CurrentRenderTarget { get; }

        internal RenderContext(OpenGlWindow owner)
        {
            Owner = owner;
            CurrentRenderTarget = owner.RenderTargetPointer;
            
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
            => SDL_gpu.GPU_Line(CurrentRenderTarget, start.X, start.Y, end.X, end.Y, color);

        public void Pixel(Vector2 position, Color color)
            => SDL_gpu.GPU_Pixel(CurrentRenderTarget, position.X, position.Y, color);

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

        public void Rectangle(ShapeMode mode, Vector2 position, Size size, Color color)
        {
            if (mode == ShapeMode.Stroke)
            {
                SDL_gpu.GPU_Rectangle(
                    CurrentRenderTarget,
                    position.X,
                    position.Y,
                    position.X + size.Width,
                    position.Y + size.Height,
                    color
                );
            }
            else if (mode == ShapeMode.Fill)
            {
                SDL_gpu.GPU_RectangleFilled(
                    CurrentRenderTarget,
                    position.X,
                    position.Y,
                    position.X + size.Width,
                    position.Y + size.Height,
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
    }
}
