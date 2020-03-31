using System;
using Chroma.SDL2;

namespace Chroma.Graphics
{
    public class Font
    {
        private FontHintingMode _hintingMode;
        
        internal IntPtr Handle { get; private set; }

        public int Height { get; }

        public FontHintingMode HintingMode
        {
            get => _hintingMode;
            set
            {
                _hintingMode = value;
                SDL_ttf.TTF_SetFontHinting(Handle, (int)_hintingMode);
            }
        }

        public bool IsMonoSpace 
            => SDL_ttf.TTF_FontFaceIsFixedWidth(Handle) != 0;
        
        public Font(string filePath, int pixelHeight)
        {
            Height = pixelHeight;
            Handle = SDL_ttf.TTF_OpenFont(filePath, pixelHeight);
        }

        public Font(string filePath, int pixelHeight, long faceIndex)
        {
            Height = pixelHeight;
            Handle = SDL_ttf.TTF_OpenFontIndex(filePath, pixelHeight, faceIndex);
        }

        public Vector2 Measure(string text)
        {
            SDL_ttf.TTF_SizeUTF8(Handle, text, out int width, out int height);
            return new Vector2(width, height);
        }

        internal SDL_gpu.GPU_Image_PTR RenderSolid(string text, Color foreground)
        {
            var sdlSurface = SDL_ttf.TTF_RenderUTF8_Solid(Handle, text, foreground);
            return SDL_gpu.GPU_CopyImageFromSurface(sdlSurface);
        }

        internal SDL_gpu.GPU_Image_PTR RenderShaded(string text, Color foreground, Color background)
        {
            var sdlSurface = SDL_ttf.TTF_RenderUTF8_Shaded(Handle, text, foreground, background);
            return SDL_gpu.GPU_CopyImageFromSurface(sdlSurface);
        }

        internal IntPtr RenderBlended(string text, Color foreground, out SDL_gpu.GPU_Image_PTR image)
        {
            var sdlSurface = SDL_ttf.TTF_RenderUNICODE_Blended(Handle, text, foreground);
            image = SDL_gpu.GPU_CopyImageFromSurface(sdlSurface);

            return sdlSurface;
        }
    }
}