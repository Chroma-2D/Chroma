using System;
using System.IO;
using System.Runtime.InteropServices;
using Chroma.Diagnostics;
using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public class Texture : IDisposable
    {
        internal SDL_gpu.GPU_Image_PTR ImageHandle { get; private set; }
        internal unsafe SDL2.SDL_Surface* Surface { get; private set; }

        public bool Disposed { get; private set; }

        public float Width
        {
            get
            {
                EnsureNotDisposed();

                unsafe
                {
                    return Surface->w;
                }
            }
        }

        public float Height
        {
            get
            {
                EnsureNotDisposed();

                unsafe
                {
                    return Surface->h;
                }
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

        public uint BytesPerPixel
        {
            get
            {
                EnsureNotDisposed();
                return (uint)ImageHandle.Value.bytes_per_pixel;
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

        public Color this[int x, int y]
        {
            get => GetPixel(x, y);
            set => SetPixel(x, y, value);
        }

        public Texture(Stream stream)
        {
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            IntPtr surfaceHandle;
            unsafe
            {
                fixed (byte* bp = &bytes[0])
                {
                    var rwops = SDL2.SDL_RWFromMem(new IntPtr(bp), bytes.Length);
                    surfaceHandle = SDL_image.IMG_Load_RW(rwops, 1);

                    Surface = (SDL2.SDL_Surface*)surfaceHandle.ToPointer();
                }
            }

            ImageHandle = SDL_gpu.GPU_CopyImageFromSurface(surfaceHandle);
        }

        public Texture(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The provided file path does not exist.", filePath);

            var surfaceHandle = SDL_image.IMG_Load(filePath);

            unsafe
            {
                Surface = (SDL2.SDL_Surface*)surfaceHandle.ToPointer();
                var fmt = ((SDL2.SDL_PixelFormat*)Surface->format.ToPointer());

                var standardPixelFormat = new SDL2.SDL_PixelFormat
                {
                    format = SDL2.SDL_PIXELFORMAT_RGBA8888,
                    palette = IntPtr.Zero,
                    Rmask = 0x000000FF,
                    Gmask = 0x0000FF00,
                    Bmask = 0x00FF0000,
                    Amask = 0xFF000000,
                    BitsPerPixel = 32,
                    BytesPerPixel = 4
                };

                if (fmt->BytesPerPixel != 4 || fmt->format != SDL2.SDL_PIXELFORMAT_RGBA8888)
                {
                    var rgbaSurface = SDL2.SDL_ConvertSurface(
                        surfaceHandle,
                        new IntPtr(&standardPixelFormat),
                        0
                    );

                    SDL2.SDL_FreeSurface(surfaceHandle);
                    surfaceHandle = rgbaSurface;

                    Surface = (SDL2.SDL_Surface*)surfaceHandle.ToPointer();
                }
            }

            ImageHandle = SDL_gpu.GPU_CopyImageFromSurface(surfaceHandle);
        }

        public Texture(Texture other)
        {
            unsafe
            {
                var handle = SDL2.SDL_CreateRGBSurfaceFrom(
                    other.Surface->pixels,
                    other.Surface->w,
                    other.Surface->h,
                    32,
                    other.Surface->pitch,
                    0x000000FF,
                    0x0000FF00,
                    0x00FF0000,
                    0xFF000000
                );

                other.Surface = (SDL2.SDL_Surface*)handle.ToPointer();
                other.ImageHandle = SDL_gpu.GPU_CopyImageFromSurface(handle);
            }
        }

        public Texture(ushort width, ushort height)
        {
            unsafe
            {
                var handle = SDL2.SDL_CreateRGBSurface(
                    0,
                    width,
                    height,
                    32,
                    0x000000FF,
                    0x0000FF00,
                    0x00FF0000,
                    0xFF000000
                );

                Surface = (SDL2.SDL_Surface*)handle.ToPointer();
                ImageHandle = SDL_gpu.GPU_CopyImageFromSurface(handle);
            }
        }

        internal Texture(SDL_gpu.GPU_Image_PTR imageHandle)
        {
            ImageHandle = imageHandle;
        }

        public void SetPixel(int x, int y, Color color)
        {
            EnsureNotDisposed();

            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                Log.Warning($"Tried to set a texture pixel on out-of-bounds coordinates ({x},{y})");
                return;
            }

            unsafe
            {
                uint* pixel = (uint*)((byte*)Surface->pixels +
                                      (y * Surface->pitch) +
                                      (x * sizeof(uint)));

                *pixel = color.PackedValue;
            }
        }

        public Color GetPixel(int x, int y)
        {
            EnsureNotDisposed();

            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                Log.Warning($"Tried to retrieve a texture pixel on out-of-bounds coordinates ({x},{y})");
                return Color.Black;
            }

            unsafe
            {
                uint* pixel = (uint*)((byte*)Surface->pixels +
                                      (y * Surface->pitch) +
                                      (x * sizeof(uint)));

                return new Color(*pixel);
            }
        }

        public void Flush()
        {
            EnsureNotDisposed();

            unsafe
            {
                var imgRect = new SDL_gpu.GPU_Rect { x = 0, y = 0, w = Width, h = Height };
                var surfRect = new SDL_gpu.GPU_Rect { x = 0, y = 0, w = Surface->w, h = Surface->h };

                SDL_gpu.GPU_UpdateImage(ImageHandle, imgRect, new IntPtr(Surface), surfRect);
            }
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

                unsafe
                {
                    SDL2.SDL_FreeSurface(new IntPtr(Surface));
                }
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