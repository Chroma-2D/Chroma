using System;
using System.IO;
using Chroma.SDL2;

namespace Chroma.Graphics
{
    public class Texture : IDisposable
    {
        internal SDL_gpu.GPU_Image_PTR ImageHandle { get; set; }

        public bool Disposed { get; private set; }

        public float Width
        {
            get
            {
                EnsureNotDisposed();
                return ImageHandle.Value.w;
            }
        }

        public float Height
        {
            get
            {
                EnsureNotDisposed();
                return ImageHandle.Value.h;
            }
        }

        public Vector2 Anchor
        {
            get
            {
                EnsureNotDisposed();

                return new Vector2(
                    ImageHandle.Value.anchor_x,
                    ImageHandle.Value.anchor_y
                );
            }

            set
            {
                EnsureNotDisposed();

                SDL_gpu.GPU_SetAnchor(
                    ImageHandle,
                    value.X,
                    value.Y
                );
            }
        }

        public TextureWrappingMode HorizontalWrappingMode
        {
            get
            {
                EnsureNotDisposed();
                return (TextureWrappingMode)ImageHandle.Value.wrap_mode_x;
            }

            set
            {
                EnsureNotDisposed();

                SDL_gpu.GPU_SetWrapMode(
                    ImageHandle,
                    (SDL_gpu.GPU_WrapEnum)value,
                    ImageHandle.Value.wrap_mode_y
                );
            }
        }

        public TextureWrappingMode VerticalWrappingMode
        {
            get
            {
                EnsureNotDisposed();
                return (TextureWrappingMode)ImageHandle.Value.wrap_mode_y;
            }

            set
            {
                EnsureNotDisposed();
                
                SDL_gpu.GPU_SetWrapMode(
                    ImageHandle,
                    ImageHandle.Value.wrap_mode_x,
                    (SDL_gpu.GPU_WrapEnum)value
                );
            }
        }

        public bool UseBlending
        {
            get
            {
                EnsureNotDisposed();
                return ImageHandle.Value.use_blending != 0;
            }

            set
            {
                EnsureNotDisposed();
                SDL_gpu.GPU_SetBlending(ImageHandle, value ? (byte)1 : (byte)0);
            }
        }

        public BlendingEquation ColorBlendingEquation
        {
            get
            {
                EnsureNotDisposed();
                return (BlendingEquation)ImageHandle.Value.blend_mode.color_equation;
            }
            
            set
            {
                EnsureNotDisposed();
                
                SDL_gpu.GPU_SetBlendEquation(
                    ImageHandle,
                    (SDL_gpu.GPU_BlendEqEnum)value,
                    ImageHandle.Value.blend_mode.alpha_equation
                );
            }
        }

        public BlendingFunction SourceColorBlendingFunction
        {
            get
            {
                EnsureNotDisposed();
                return (BlendingFunction)ImageHandle.Value.blend_mode.source_color;
            }
            
            set
            {
                EnsureNotDisposed();
                
                SDL_gpu.GPU_SetBlendFunction(
                    ImageHandle,
                    (SDL_gpu.GPU_BlendFuncEnum)value,
                    ImageHandle.Value.blend_mode.dest_color,
                    ImageHandle.Value.blend_mode.source_alpha,
                    ImageHandle.Value.blend_mode.dest_alpha
                );
            }
        }

        public BlendingFunction DestinationColorBlendingFunction
        {
            get
            {
                EnsureNotDisposed();
                return (BlendingFunction)ImageHandle.Value.blend_mode.dest_color;
            }
            
            set
            {
                EnsureNotDisposed();
                
                SDL_gpu.GPU_SetBlendFunction(
                    ImageHandle,
                    ImageHandle.Value.blend_mode.source_color,
                    (SDL_gpu.GPU_BlendFuncEnum)value,
                    ImageHandle.Value.blend_mode.source_alpha,
                    ImageHandle.Value.blend_mode.dest_alpha
                );
            }
        }

        public BlendingEquation AlphaBlendingEquation
        {
            get
            {
                EnsureNotDisposed();
                return (BlendingEquation)ImageHandle.Value.blend_mode.alpha_equation;
            }
            
            set
            {
                EnsureNotDisposed();
                
                SDL_gpu.GPU_SetBlendEquation(
                    ImageHandle,
                    ImageHandle.Value.blend_mode.color_equation,
                    (SDL_gpu.GPU_BlendEqEnum)value
                );
            }
        }

        public BlendingFunction SourceAlphaBlendingFunction
        {
            get
            {
                EnsureNotDisposed();
                return (BlendingFunction)ImageHandle.Value.blend_mode.source_color;
            }
            
            set
            {
                EnsureNotDisposed();
                
                SDL_gpu.GPU_SetBlendFunction(
                    ImageHandle,
                    ImageHandle.Value.blend_mode.source_color,
                    ImageHandle.Value.blend_mode.dest_color,
                    (SDL_gpu.GPU_BlendFuncEnum)value,
                    ImageHandle.Value.blend_mode.dest_alpha
                );
            }
        }

        public BlendingFunction DestinationAlphaBlendingFunction
        {
            get
            {
                EnsureNotDisposed();
                return (BlendingFunction)ImageHandle.Value.blend_mode.dest_color;
            }

            set
            {
                EnsureNotDisposed();

                SDL_gpu.GPU_SetBlendFunction(
                    ImageHandle,
                    ImageHandle.Value.blend_mode.source_color,
                    ImageHandle.Value.blend_mode.dest_color,
                    ImageHandle.Value.blend_mode.source_alpha,
                    (SDL_gpu.GPU_BlendFuncEnum)value
                );
            }
        }

        public int BytesPerPixel
        {
            get
            {
                EnsureNotDisposed();
                return ImageHandle.Value.bytes_per_pixel;
            }
        }

        public TextureFilteringMode FilteringMode
        {
            get
            {
                EnsureNotDisposed();
                return (TextureFilteringMode)ImageHandle.Value.filter_mode;
            }

            set
            {
                EnsureNotDisposed();
                SDL_gpu.GPU_SetImageFilter(ImageHandle, (SDL_gpu.GPU_FilterEnum)value);
            }
        }

        public TextureSnappingMode SnappingMode
        {
            get
            {
                EnsureNotDisposed();
                return (TextureSnappingMode)ImageHandle.Value.snap_mode;
            }

            set
            {
                EnsureNotDisposed();
                SDL_gpu.GPU_SetSnapMode(ImageHandle, (SDL_gpu.GPU_SnapEnum)value);
            }
        }

        public Color ColorMask
        {
            get
            {
                EnsureNotDisposed();
                return ImageHandle.Value.color;
            }

            set
            {
                EnsureNotDisposed();
                SDL_gpu.GPU_SetColor(ImageHandle, value);
            }
        }
        
        public Texture(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The provided file path does not exist.", filePath);

            ImageHandle = SDL_gpu.GPU_LoadImage(filePath);            
        }

        public void SetBlendingMode(BlendingPreset preset)
        {
            EnsureNotDisposed();

            SDL_gpu.GPU_SetBlendMode(
                ImageHandle,
                (SDL_gpu.GPU_BlendPresetEnum)preset
            );
        }

        public void GenerateMipMaps()
        {
            EnsureNotDisposed();
            SDL_gpu.GPU_GenerateMipmaps(ImageHandle);
        }

        private void EnsureNotDisposed()
        {
            if (Disposed)
                throw new InvalidOperationException("This texture has already been disposed.");
        }

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