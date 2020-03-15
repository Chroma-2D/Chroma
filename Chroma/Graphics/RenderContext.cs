using Chroma.SDL2;
using Chroma.Windowing;

namespace Chroma.Graphics
{
    public class RenderContext
    {
        internal WindowBase Owner { get; }
        internal SDL_gpu.GPU_Target_PTR CurrentRenderTarget { get; }

        internal RenderContext(WindowBase owner)
        {
            Owner = owner;
            CurrentRenderTarget = owner.RenderTargetPointer;
        }

        public float LineThickness
        {
            get => SDL_gpu.GPU_GetLineThickness();
            set => SDL_gpu.GPU_SetLineThickness(value);
        }

        public void Clear(Color color)
            => SDL_gpu.GPU_ClearRGBA(CurrentRenderTarget, color.R, color.G, color.B, color.A);

        public void Rectangle(RectangleMode mode, Vector2 position, Size size, Color color)
        {
            if (mode == RectangleMode.Stroke)
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
            else if (mode == RectangleMode.Fill)
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
    }
}
