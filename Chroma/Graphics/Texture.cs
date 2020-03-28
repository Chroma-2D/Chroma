using System;
using Chroma.SDL2;

namespace Chroma.Graphics
{
    public class Texture : IDisposable
    {
        internal SDL_gpu.GPU_Image_PTR ImageHandle { get; set; }

        public bool Disposed { get; private set; }

        public Size Size
        {
            get => new Size(
                ImageHandle.Value.w,
                ImageHandle.Value.h
            );
        }

        public Vector2 Anchor
        {
            get => new Vector2(
                ImageHandle.Value.anchor_x,
                ImageHandle.Value.anchor_y
            );

            set => SDL_gpu.GPU_SetAnchor(
                ImageHandle,
                value.X,
                value.Y
            );
        }

        public TextureWrappingMode HorizontalWrappingMode
        {
            get => (TextureWrappingMode)ImageHandle.Value.wrap_mode_x;
            set => SDL_gpu.GPU_SetWrapMode(
                ImageHandle,
                (SDL_gpu.GPU_WrapEnum)value,
                ImageHandle.Value.wrap_mode_y
            );
        }

        public TextureWrappingMode VerticalWrappingMode
        {
            get => (TextureWrappingMode)ImageHandle.Value.wrap_mode_y;
            set => SDL_gpu.GPU_SetWrapMode(
                ImageHandle,
                ImageHandle.Value.wrap_mode_x,
                (SDL_gpu.GPU_WrapEnum)value
            );
        }

        public bool UseBlending
        {
            get => ImageHandle.Value.use_blending != 0;
            set => SDL_gpu.GPU_SetBlending(ImageHandle, value ? (byte)1 : (byte)0);
        }

        public BlendingEquation ColorBlendingEquation
        {
            get => (BlendingEquation)ImageHandle.Value.blend_mode.color_equation;
            set => SDL_gpu.GPU_SetBlendEquation(
                ImageHandle,
                (SDL_gpu.GPU_BlendEqEnum)value,
                ImageHandle.Value.blend_mode.alpha_equation
            );
        }

        public BlendingFunction SourceColorBlendingFunction
        {
            get => (BlendingFunction)ImageHandle.Value.blend_mode.source_color;
            set => SDL_gpu.GPU_SetBlendFunction(
                ImageHandle,
                (SDL_gpu.GPU_BlendFuncEnum)value,
                ImageHandle.Value.blend_mode.dest_color,
                ImageHandle.Value.blend_mode.source_alpha,
                ImageHandle.Value.blend_mode.dest_alpha
            );
        }

        public BlendingFunction DestinationColorBlendingFunction
        {
            get => (BlendingFunction)ImageHandle.Value.blend_mode.dest_color;
            set => SDL_gpu.GPU_SetBlendFunction(
                ImageHandle,
                ImageHandle.Value.blend_mode.source_color,
                (SDL_gpu.GPU_BlendFuncEnum)value,
                ImageHandle.Value.blend_mode.source_alpha,
                ImageHandle.Value.blend_mode.dest_alpha
            );
        }

        public BlendingEquation AlphaBlendingEquation
        {
            get => (BlendingEquation)ImageHandle.Value.blend_mode.alpha_equation;
            set => SDL_gpu.GPU_SetBlendEquation(
                ImageHandle,
                ImageHandle.Value.blend_mode.color_equation,
                (SDL_gpu.GPU_BlendEqEnum)value
            );
        }

        public BlendingFunction SourceAlphaBlendingFunction
        {
            get => (BlendingFunction)ImageHandle.Value.blend_mode.source_color;
            set => SDL_gpu.GPU_SetBlendFunction(
                ImageHandle,
                ImageHandle.Value.blend_mode.source_color,
                ImageHandle.Value.blend_mode.dest_color,
                (SDL_gpu.GPU_BlendFuncEnum)value,
                ImageHandle.Value.blend_mode.dest_alpha
            );
        }

        public BlendingFunction DestinationAlphaBlendingFunction
        {
            get => (BlendingFunction)ImageHandle.Value.blend_mode.dest_color;
            set => SDL_gpu.GPU_SetBlendFunction(
                ImageHandle,
                ImageHandle.Value.blend_mode.source_color,
                ImageHandle.Value.blend_mode.dest_color,
                ImageHandle.Value.blend_mode.source_alpha,
                (SDL_gpu.GPU_BlendFuncEnum)value
            );
        }

        public int BytesPerPixel => ImageHandle.Value.bytes_per_pixel;

        public TextureFilteringMode FilteringMode
        {
            get => (TextureFilteringMode)ImageHandle.Value.filter_mode;
            set => SDL_gpu.GPU_SetImageFilter(ImageHandle, (SDL_gpu.GPU_FilterEnum)value);
        }

        public Texture(string fileName)
        {
            ImageHandle = SDL_gpu.GPU_LoadImage(fileName);
        }
            
        public void SetBlendingMode(BlendingPreset preset)
        {
            SDL_gpu.GPU_SetBlendMode(
                ImageHandle, 
                (SDL_gpu.GPU_BlendPresetEnum)preset
            );
        }

        public void GenerateMipMaps()
            => SDL_gpu.GPU_GenerateMipmaps(ImageHandle);

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    // No unmanaged resources to free.
                }

                SDL_gpu.GPU_FreeImage(ImageHandle);
                Disposed = true;
            }
        }

        ~Texture() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
