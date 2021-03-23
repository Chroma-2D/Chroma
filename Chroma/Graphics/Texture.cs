using System;
using System.Drawing;
using System.IO;
using System.Numerics;
using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;
using Chroma.Natives.SDL;
using Chroma.Threading;
using Chroma.STB.Image;

namespace Chroma.Graphics
{
    public class Texture : DisposableResource
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        internal IntPtr ImageHandle { get; private set; }
        internal unsafe SDL_gpu.GPU_Image* Image => (SDL_gpu.GPU_Image*)ImageHandle.ToPointer();

        private byte[] _pixelData;

        public PixelFormat Format { get; private set; }

        public int Width
        {
            get
            {
                EnsureNotDisposed();

                unsafe
                {
                    return Image->texture_w;
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
                    return Image->texture_h;
                }
            }
        }

        public Vector2 AbsoluteCenter
        {
            get
            {
                EnsureNotDisposed();
                return new Vector2(Width / 2f, Height / 2f);
            }
        }

        public Vector2 Center
        {
            get
            {
                EnsureNotDisposed();

                if (VirtualResolution.HasValue && IsVirtualized)
                {
                    return new Vector2(
                        VirtualResolution.Value.Width / 2f,
                        VirtualResolution.Value.Height / 2f
                    );
                }
                else return AbsoluteCenter;
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
        }

        public int BytesPerPixel { get; private set; }

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

        private Size? _virtualResolution;

        public Size? VirtualResolution
        {
            get
            {
                EnsureNotDisposed();

                if (!_virtualResolution.HasValue)
                {
                    _virtualResolution = new Size(Width, Height);
                }

                return _virtualResolution;
            }

            set
            {
                EnsureNotDisposed();

                if (value == null)
                {
                    _virtualResolution = new Size(Width, Height);
                    SDL_gpu.GPU_UnsetImageVirtualResolution(ImageHandle);
                }
                else
                {
                    _virtualResolution = value;
                    SDL_gpu.GPU_SetImageVirtualResolution(
                        ImageHandle,
                        (ushort)_virtualResolution.Value.Width,
                        (ushort)_virtualResolution.Value.Height
                    );
                }
            }
        }

        public bool IsVirtualized
        {
            get
            {
                EnsureNotDisposed();

                return VirtualResolution.HasValue
                       && (VirtualResolution.Value.Height != Height
                           || VirtualResolution.Value.Width != Width);
            }
        }

        public int Stride => Width * BytesPerPixel;

        public Color this[int x, int y]
        {
            get => GetPixel(x, y);
            set => SetPixel(x, y, value);
        }

        public Texture(Stream stream)
        {
            EnsureOnMainThread();

            var result = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

            if (result == null)
            {
                throw new FormatException("Image format not supported.");
            }
            
            CreateEmpty(
                result.Width,
                result.Height,
                PixelFormat.RGBA,
                true
            );

            result.Data.CopyTo(_pixelData, 0);
            Flush();

            SnappingMode = TextureSnappingMode.None;
        }

        public Texture(string filePath)
            : this(new FileStream(filePath, FileMode.Open))
        {
        }

        public Texture(Texture other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), "The other texture is null.");
            
            if (other.Disposed)
                throw new InvalidOperationException("The source texture has been disposed.");
            
            EnsureOnMainThread();

            CreateEmpty(
                other.Width,
                other.Height,
                other.Format,
                true
            );

            CopyDataFrom(other);
            Flush();

            SnappingMode = TextureSnappingMode.None;
        }

        public Texture(int width, int height, PixelFormat pixelFormat = PixelFormat.RGBA)
        {
            if (width < 0)
                throw new ArgumentOutOfRangeException(nameof(width), "Width cannot be negative.");

            if (height < 0)
                throw new ArgumentOutOfRangeException(nameof(height), "Height cannot be negative.");
            
            EnsureOnMainThread();

            CreateEmpty(
                (ushort)width,
                (ushort)height,
                pixelFormat,
                true
            );
            Flush();

            SnappingMode = TextureSnappingMode.None;
        }

        public Texture(int width, int height, byte[] data, PixelFormat pixelFormat = PixelFormat.RGBA)
        {
            if (width < 0)
                throw new ArgumentOutOfRangeException(nameof(width), "Width cannot be negative.");

            if (height < 0)
                throw new ArgumentOutOfRangeException(nameof(height), "Height cannot be negative.");

            if (data == null)
                throw new ArgumentNullException(nameof(data), "Pixel data is null.");
            
            EnsureOnMainThread();
            
            CreateEmpty(
                (ushort)width,
                (ushort)height,
                pixelFormat,
                true
            );

            data.CopyTo(_pixelData, 0);
            Flush();
            
            SnappingMode = TextureSnappingMode.None;
        }

        internal Texture(IntPtr gpuImageHandle)
        {
            EnsureOnMainThread();

            if (gpuImageHandle == IntPtr.Zero)
                throw new ArgumentException("Invalid image handle.", nameof(gpuImageHandle));

            ImageHandle = gpuImageHandle;

            InitializeWithSurface(
                SDL_gpu.GPU_CopySurfaceFromImage(gpuImageHandle)
            );

            SnappingMode = TextureSnappingMode.None;
        }

        public void SetBlendingEquations(BlendingEquation colorBlend, BlendingEquation alphaBlend)
        {
            EnsureNotDisposed();

            SDL_gpu.GPU_SetBlendEquation(
                ImageHandle,
                (SDL_gpu.GPU_BlendEqEnum)colorBlend,
                (SDL_gpu.GPU_BlendEqEnum)alphaBlend
            );
        }

        public void SetBlendingFunctions(BlendingFunction sourceColorBlend, BlendingFunction sourceAlphaBlend,
            BlendingFunction destinationColorBlend, BlendingFunction destinationAlphaBlend)
        {
            EnsureNotDisposed();

            SDL_gpu.GPU_SetBlendFunction(
                ImageHandle,
                (SDL_gpu.GPU_BlendFuncEnum)sourceColorBlend,
                (SDL_gpu.GPU_BlendFuncEnum)destinationColorBlend,
                (SDL_gpu.GPU_BlendFuncEnum)sourceAlphaBlend,
                (SDL_gpu.GPU_BlendFuncEnum)destinationAlphaBlend
            );
        }

        public void SetPixelData(Color[] colors)
        {
            EnsureNotDisposed();

            var pixelCount = Width * Height;

            if (colors.Length != Width * Height)
                throw new InvalidOperationException("The pixel array must be the same size as texture's.");

            for (var i = 0; i < pixelCount; i++)
                WritePixel(i * BytesPerPixel, colors[i]);
        }

        public void SetPixelData(byte[] data)
        {
            EnsureNotDisposed();

            if (data.Length != _pixelData.Length)
                throw new InvalidOperationException("The byte array must be the same size as texture's.");

            data.CopyTo(_pixelData, 0);
        }

        public void SetPixel(int x, int y, Color color)
        {
            EnsureNotDisposed();

            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                _log.Warning($"Tried to set a texture pixel on out-of-bounds coordinates ({x},{y})");
                return;
            }

            var i = y * Stride + (x * BytesPerPixel);
            WritePixel(i, color);
        }

        public Color GetPixel(int x, int y)
        {
            EnsureNotDisposed();

            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                _log.Warning($"Tried to retrieve a texture pixel on out-of-bounds coordinates ({x},{y})");
                return Color.Black;
            }

            var i = y * Stride + (x * BytesPerPixel);
            return ReadPixel(i);
        }

        public void Flush()
        {
            EnsureOnMainThread();
            EnsureNotDisposed();

            if (_pixelData.Length < Width * Height * BytesPerPixel)
            {
                _log.Error("Cannot flush. Pixel data size mismatch.");
                return;
            }

            var imgRect = new SDL_gpu.GPU_Rect
            {
                x = 0,
                y = 0,
                w = Width,
                h = Height
            };

            SDL_gpu.GPU_UpdateImageBytes(
                ImageHandle,
                ref imgRect,
                _pixelData,
                Stride
            );
        }

        public void SaveToFile(string filePath, ImageFileFormat format)
        {
            EnsureNotDisposed();

            if (!SDL_gpu.GPU_SaveImage(ImageHandle, filePath, (SDL_gpu.GPU_FileFormatEnum)format))
            {
                _log.Error($"Saving texture to file failed: {SDL2.SDL_GetError()}");
            }
        }

        public void SaveToArray(byte[] buffer, ImageFileFormat format)
        {
            EnsureNotDisposed();

            unsafe
            {
                fixed (byte* ptr = &buffer[0])
                {
                    var rwops = SDL2.SDL_RWFromMem(new IntPtr(ptr), buffer.Length);
                    if (!SDL_gpu.GPU_SaveImage_RW(ImageHandle, rwops, true, (SDL_gpu.GPU_FileFormatEnum)format))
                    {
                        _log.Error($"Writing texture to memory failed: {SDL2.SDL_GetError()}");
                    }
                }
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

        internal IntPtr AsSdlSurface()
            => SDL_gpu.GPU_CopySurfaceFromImage(ImageHandle);

        private unsafe void InitializeWithSurface(IntPtr imageSurfaceHandle)
        {
            IntPtr rgbaSurfaceHandle;
            SDL2.SDL_Surface* rgbaSurface;

            rgbaSurfaceHandle = SDL2.SDL_ConvertSurfaceFormat(
                imageSurfaceHandle,
                SDL2.SDL_PIXELFORMAT_ABGR8888, // endianness? pixel order still confuses me sometimes
                0
            );

            SDL2.SDL_FreeSurface(imageSurfaceHandle);
            rgbaSurface = (SDL2.SDL_Surface*)rgbaSurfaceHandle.ToPointer();

            CreateEmpty(
                rgbaSurface->w,
                rgbaSurface->h,
                PixelFormat.RGBA,
                ImageHandle == IntPtr.Zero
            );

            var pixels = (byte*)rgbaSurface->pixels.ToPointer();
            var format = (SDL2.SDL_PixelFormat*)rgbaSurface->format.ToPointer();
            var dataLength = rgbaSurface->w * rgbaSurface->h * format->BytesPerPixel;

            for (var i = 0; i < dataLength; i++)
                _pixelData[i] = pixels[i];

            SDL2.SDL_FreeSurface(rgbaSurfaceHandle);
        }

        private void CreateEmpty(int width, int height, PixelFormat format, bool allocateImage)
        {
            var pixelCount = width * height;
            var bytesPerPixel = 0;

            switch (format)
            {
                case PixelFormat.RGB:
                case PixelFormat.BGR:
                    bytesPerPixel = 3;
                    break;

                case PixelFormat.RGBA:
                case PixelFormat.BGRA:
                case PixelFormat.ABGR:
                    bytesPerPixel = 4;
                    break;
            }

            Format = format;
            BytesPerPixel = bytesPerPixel;

            _pixelData = new byte[pixelCount * bytesPerPixel];

            if (allocateImage)
            {
                ImageHandle = SDL_gpu.GPU_CreateImage(
                    (ushort)width,
                    (ushort)height,
                    (SDL_gpu.GPU_FormatEnum)format
                );
            }
        }

        private Color ReadPixel(int i)
        {
            var c = new Color {A = 255};

            switch (Format)
            {
                case PixelFormat.BGR:
                    c.B = _pixelData[i + 0];
                    c.G = _pixelData[i + 1];
                    c.R = _pixelData[i + 2];
                    break;

                case PixelFormat.RGB:
                    c.R = _pixelData[i + 0];
                    c.G = _pixelData[i + 1];
                    c.B = _pixelData[i + 2];
                    break;

                case PixelFormat.ABGR:
                    c.A = _pixelData[i + 0];
                    c.B = _pixelData[i + 1];
                    c.G = _pixelData[i + 2];
                    c.R = _pixelData[i + 3];
                    break;

                case PixelFormat.BGRA:
                    c.B = _pixelData[i + 0];
                    c.G = _pixelData[i + 1];
                    c.R = _pixelData[i + 2];
                    c.A = _pixelData[i + 3];
                    break;

                case PixelFormat.RGBA:
                    c.R = _pixelData[i + 3];
                    c.G = _pixelData[i + 2];
                    c.B = _pixelData[i + 1];
                    c.A = _pixelData[i + 0];
                    break;

                default: throw new InvalidOperationException("Unsupported pixel format.");
            }

            return c;
        }

        private void WritePixel(int i, Color c)
        {
            switch (Format)
            {
                case PixelFormat.BGR:
                    _pixelData[i + 0] = c.B;
                    _pixelData[i + 1] = c.G;
                    _pixelData[i + 2] = c.R;
                    break;

                case PixelFormat.RGB:
                    _pixelData[i + 0] = c.R;
                    _pixelData[i + 1] = c.G;
                    _pixelData[i + 2] = c.B;
                    break;

                case PixelFormat.ABGR:
                    _pixelData[i + 0] = c.A;
                    _pixelData[i + 1] = c.B;
                    _pixelData[i + 2] = c.G;
                    _pixelData[i + 3] = c.R;
                    break;

                case PixelFormat.BGRA:
                    _pixelData[i + 0] = c.B;
                    _pixelData[i + 1] = c.G;
                    _pixelData[i + 2] = c.R;
                    _pixelData[i + 3] = c.A;
                    break;

                case PixelFormat.RGBA:
                    _pixelData[i + 0] = c.R;
                    _pixelData[i + 1] = c.G;
                    _pixelData[i + 2] = c.B;
                    _pixelData[i + 3] = c.A;
                    break;

                default: throw new InvalidOperationException("Unsupported pixel format.");
            }
        }

        private void CopyDataFrom(Texture other)
            => other._pixelData.CopyTo(_pixelData, 0);

        protected void EnsureOnMainThread()
        {
            if (!Dispatcher.IsMainThread)
                throw new InvalidOperationException(
                    "This operation is not thread-safe and must be scheduled to run on main thread.");
        }

        protected override void FreeNativeResources()
            => SDL_gpu.GPU_FreeImage(ImageHandle);
    }
}