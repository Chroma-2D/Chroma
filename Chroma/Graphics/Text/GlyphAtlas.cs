using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Chroma.SDL2;

namespace Chroma.Graphics.Text
{
    public class GlyphAtlas
    {
        private const int MaxAtlasSize = 1024;

        private Font Font { get; }

        internal IntPtr AtlasSurfaceHandle { get; }
        internal Dictionary<ushort, GlyphInfo> GlyphMetadata { get; }

        public Texture Texture { get; private set; }

        public GlyphAtlas(Font font)
        {
            Font = font;

            var height = SDL_ttf.TTF_FontHeight(font.Handle);
            AtlasSurfaceHandle = SDL.SDL_CreateRGBSurfaceWithFormat(
                0,
                MaxAtlasSize,
                MaxAtlasSize,
                32,
                SDL.SDL_PIXELFORMAT_RGBA8888
            );
            GlyphMetadata = new Dictionary<ushort, GlyphInfo>();

            var currentAtlasX = 0;
            var currentAtlasY = 0;
            for (ushort i = 1; i < ushort.MaxValue; i++)
            {
                var glyphIndex = SDL_ttf.TTF_GlyphIsProvided(Font.Handle, i);
                if (glyphIndex != 0)
                {
                    var glyphSurfaceHandle = SDL_ttf.TTF_RenderGlyph_Blended(Font.Handle, i, Color.White);

                    if (glyphSurfaceHandle == IntPtr.Zero)
                    {
                        Console.WriteLine(SDL.SDL_GetError());
                        continue;
                    }

                    var surface = Marshal.PtrToStructure<SDL.SDL_Surface>(glyphSurfaceHandle);

                    var srcRect = new SDL.SDL_Rect
                    {
                        x = 0,
                        y = 0,
                        w = surface.w,
                        h = surface.h
                    };

                    var destRect = new SDL.SDL_Rect
                    {
                        x = currentAtlasX,
                        y = currentAtlasY,
                        w = surface.w,
                        h = surface.h
                    };

                    SDL.SDL_BlitSurface(
                        glyphSurfaceHandle,
                        ref srcRect,
                        AtlasSurfaceHandle,
                        ref destRect
                    );
                    SDL.SDL_FreeSurface(glyphSurfaceHandle);
                    SDL_ttf.TTF_GlyphMetrics(Font.Handle, i, out int minX, out int maxX, out int minY, out int maxY, out int advance);

                    GlyphMetadata.Add(i, new GlyphInfo(minX, maxX, minY, maxY, advance, surface.w, surface.h, new Vector2(currentAtlasX, currentAtlasY)));

                    currentAtlasX += surface.w;
                    if(currentAtlasX >= MaxAtlasSize)
                    {
                        currentAtlasX = 0;
                        currentAtlasY += height;
                    }
                }
            }

            Texture = new Texture(SDL_gpu.GPU_CopyImageFromSurface(AtlasSurfaceHandle))
            {
                Anchor = Vector2.Zero
            };
        }
    }
}
