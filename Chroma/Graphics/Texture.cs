using System;
using System.IO;
using System.Numerics;
using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;
using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public class Texture : DisposableResource
    {
        private Log Log => LogManager.GetForCurrentAssembly();
        internal IntPtr ImageHandle { get; private set; }

        internal unsafe SDL_gpu.GPU_Image* Image => (SDL_gpu.GPU_Image*)ImageHandle.ToPointer();
        internal unsafe SDL2.SDL_Surface* Surface { get; private set; }

        public int Width
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

        public int Height
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

                unsafe
                {
                    return new Vector2(
                        Image->anchor_x,
                        Image->anchor_y
                    );
                }
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

                unsafe
                {
                    return (TextureWrappingMode)Image->wrap_mode_x;
                }
            }

            set
            {
                EnsureNotDisposed();

                unsafe
                {
                    SDL_gpu.GPU_SetWrapMode(
                        ImageHandle,
                        (SDL_gpu.GPU_WrapEnum)value,
                        Image->wrap_mode_y
                    );
                }
            }
        }

        public TextureWrappingMode VerticalWrappingMode
        {
            get
            {
                EnsureNotDisposed();

                unsafe
                {
                    return (TextureWrappingMode)Image->wrap_mode_y;
                }
            }

            set
            {
                EnsureNotDisposed();

                unsafe
                {
                    SDL_gpu.GPU_SetWrapMode(
                        ImageHandle,
                        Image->wrap_mode_x,
                        (SDL_gpu.GPU_WrapEnum)value
                    );
                }
            }
        }

        public bool UseBlending
        {
            get
            {
                EnsureNotDisposed();

                unsafe
                {
                    return Image->use_blending;
                }
            }

            set
            {
                EnsureNotDisposed();
                SDL_gpu.GPU_SetBlending(ImageHandle, value);
            }
        }

        public BlendingEquation ColorBlendingEquation
        {
            get
            {
                EnsureNotDisposed();

                unsafe
                {
                    return (BlendingEquation)Image->blend_mode.color_equation;
                }
            }

            set
            {
                EnsureNotDisposed();

                unsafe
                {
                    SDL_gpu.GPU_SetBlendEquation(
                        ImageHandle,
                        (SDL_gpu.GPU_BlendEqEnum)value,
                        Image->blend_mode.alpha_equation
                    );
                }
            }
        }

        public BlendingFunction SourceColorBlendingFunction
        {
            get
            {
                EnsureNotDisposed();

                unsafe
                {
                    return (BlendingFunction)Image->blend_mode.source_color;
                }
            }

            set
            {
                EnsureNotDisposed();

                unsafe
                {
                    SDL_gpu.GPU_SetBlendFunction(
                        ImageHandle,
                        (SDL_gpu.GPU_BlendFuncEnum)value,
                        Image->blend_mode.dest_color,
                        Image->blend_mode.source_alpha,
                        Image->blend_mode.dest_alpha
                    );
                }
            }
        }

        public BlendingFunction DestinationColorBlendingFunction
        {
            get
            {
                EnsureNotDisposed();

                unsafe
                {
                    return (BlendingFunction)Image->blend_mode.dest_color;
                }
            }

            set
            {
                EnsureNotDisposed();

                unsafe
                {
                    SDL_gpu.GPU_SetBlendFunction(
                        ImageHandle,
                        Image->blend_mode.source_color,
                        (SDL_gpu.GPU_BlendFuncEnum)value,
                        Image->blend_mode.source_alpha,
                        Image->blend_mode.dest_alpha
                    );
                }
            }
        }

        public BlendingEquation AlphaBlendingEquation
        {
            get
            {
                EnsureNotDisposed();

                unsafe
                {
                    return (BlendingEquation)Image->blend_mode.alpha_equation;
                }
            }

            set
            {
                EnsureNotDisposed();

                unsafe
                {
                    SDL_gpu.GPU_SetBlendEquation(
                        ImageHandle,
                        Image->blend_mode.color_equation,
                        (SDL_gpu.GPU_BlendEqEnum)value
                    );
                }
            }
        }

        public BlendingFunction SourceAlphaBlendingFunction
        {
            get
            {
                EnsureNotDisposed();

                unsafe
                {
                    return (BlendingFunction)Image->blend_mode.source_color;
                }
            }

            set
            {
                EnsureNotDisposed();

                unsafe
                {
                    SDL_gpu.GPU_SetBlendFunction(
                        ImageHandle,
                        Image->blend_mode.source_color,
                        Image->blend_mode.dest_color,
                        (SDL_gpu.GPU_BlendFuncEnum)value,
                        Image->blend_mode.dest_alpha
                    );
                }
            }
        }

        public BlendingFunction DestinationAlphaBlendingFunction
        {
            get
            {
                EnsureNotDisposed();

                unsafe
                {
                    return (BlendingFunction)Image->blend_mode.dest_color;
                }
            }

            set
            {
                EnsureNotDisposed();

                unsafe
                {
                    SDL_gpu.GPU_SetBlendFunction(
                        ImageHandle,
                        Image->blend_mode.source_color,
                        Image->blend_mode.dest_color,
                        Image->blend_mode.source_alpha,
                        (SDL_gpu.GPU_BlendFuncEnum)value
                    );
                }
            }
        }

        public uint BytesPerPixel
        {
            get
            {
                EnsureNotDisposed();

                unsafe
                {
                    return (uint)Image->bytes_per_pixel;
                }
            }
        }

        public TextureFilteringMode FilteringMode
        {
            get
            {
                EnsureNotDisposed();

                unsafe
                {
                    return (TextureFilteringMode)Image->filter_mode;
                }
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

                unsafe
                {
                    return (TextureSnappingMode)Image->snap_mode;
                }
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

                unsafe
                {
                    return Color.FromSdlColor(Image->color);
                }
            }

            set
            {
                EnsureNotDisposed();
                SDL_gpu.GPU_SetColor(ImageHandle, Color.ToSdlColor(value));
            }
        }

        private Vector2? _virtualResolution;
        public Vector2? VirtualResolution
        {
            get
            {
                EnsureNotDisposed();

                if (!_virtualResolution.HasValue)
                {
                    _virtualResolution = new Vector2(Width, Height);
                }

                return _virtualResolution;
            }

            set
            {
                EnsureNotDisposed();

                unsafe
                {
                    if (value == null)
                    {
                        _virtualResolution = new Vector2(Width, Height);
                        SDL_gpu.GPU_UnsetImageVirtualResolution(ImageHandle);
                    }
                    else
                    {
                        _virtualResolution = value;
                        SDL_gpu.GPU_SetImageVirtualResolution(ImageHandle, (ushort)_virtualResolution.Value.X, (ushort)_virtualResolution.Value.Y);
                    }
                }
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
            SnappingMode = TextureSnappingMode.None;
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

            SnappingMode = TextureSnappingMode.None;
        }

        public Texture(Texture other)
        {
            if (other.Disposed)
                throw new InvalidOperationException("The source texture has been disposed.");

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
            SnappingMode = TextureSnappingMode.None;
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
            SnappingMode = TextureSnappingMode.None;
        }

        internal Texture(IntPtr imageHandle)
        {
            ImageHandle = imageHandle;
            SnappingMode = TextureSnappingMode.None;
        }

        public void SetPixelData(Color[] colors)
        {
            EnsureNotDisposed();

            unsafe
            {
                for (var i = 0; i < colors.Length; i++)
                {
                    var color = colors[i];

                    uint* pixel = (uint*)Surface->pixels;
                    *(pixel + i) = color.PackedValue;
                }
            }
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

                SDL_gpu.GPU_UpdateImage(ImageHandle, ref imgRect, new IntPtr(Surface), ref surfRect);
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

        protected override void FreeNativeResources()
        {
            SDL_gpu.GPU_FreeImage(ImageHandle);

            unsafe
            {
                SDL2.SDL_FreeSurface(new IntPtr(Surface));
            }
        }
    }
}