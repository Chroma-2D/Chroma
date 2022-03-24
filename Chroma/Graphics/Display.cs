using System;
using System.Collections.Generic;
using System.Drawing;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.Bindings.SDL;

namespace Chroma.Graphics
{
    public class Display
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();

        public static readonly Display Invalid = new(-1);

        public int Index { get; }
        public bool IsValid => Index >= 0;

        public string Name
        {
            get
            {
                EnsureValid();
                var name = SDL2.SDL_GetDisplayName(Index);

                if (name == null)
                {
                    _log.Error($"Failed to retrieve display {Index} name: {SDL2.SDL_GetError()}");
                    return string.Empty;
                }
                
                return name;
            }
        }

        public Rectangle Bounds
        {
            get
            {
                EnsureValid();

                if (SDL2.SDL_GetDisplayBounds(Index, out var rect) < 0)
                {
                    _log.Error($"Failed to retrieve display {Index} bounds: {SDL2.SDL_GetError()}");
                    return Rectangle.Empty;
                }
                
                return new Rectangle(
                    rect.x,
                    rect.y,
                    rect.w,
                    rect.h
                );
            }
        }

        public Rectangle DesktopBounds
        {
            get
            {
                EnsureValid();

                if (SDL2.SDL_GetDisplayUsableBounds(Index, out var rect) < 0)
                {
                    _log.Error($"Failed to retrieve display {Index} desktop bounds: {SDL2.SDL_GetError()}");
                    return Rectangle.Empty;
                }
                
                return new Rectangle(
                    rect.x,
                    rect.y,
                    rect.w,
                    rect.h
                );
            }
        }

        public DisplayDpi DPI
        {
            get
            {
                EnsureValid();

                if (SDL2.SDL_GetDisplayDPI(Index, out var d, out var h, out var v) < 0)
                {
                    _log.Error($"Failed to retrieve display {Index} DPI info: {SDL2.SDL_GetError()}");
                    return DisplayDpi.None;
                }

                return new DisplayDpi(d, h, v);
            }
        }

        public DisplayMode DesktopMode
        {
            get
            {
                EnsureValid();

                if (SDL2.SDL_GetDesktopDisplayMode(Index, out var mode) < 0)
                {
                    _log.Error($"Failed to retrieve desktop display mode: {SDL2.SDL_GetError()}");
                    return DisplayMode.Invalid;
                }

                return new DisplayMode(mode.w, mode.h, mode.refresh_rate);
            }
        }

        public DisplayOrientation Orientation
        {
            get
            {
                EnsureValid();
                return (DisplayOrientation)SDL2.SDL_GetDisplayOrientation(Index);
            }
        }

        internal Display(int index)
        {
            Index = index;
        }

        public List<DisplayMode> QuerySupportedDisplayModes()
        {
            var ret = new List<DisplayMode>();
            var displayModeCount = SDL2.SDL_GetNumDisplayModes(Index);

            for (var i = 0; i < displayModeCount; i++)
            {
                if (SDL2.SDL_GetDisplayMode(Index, i, out var mode) < 0)
                {
                    _log.Error(
                        $"Failed to retrieve display mode for display {Index}, mode index {i}: {SDL2.SDL_GetError()}"
                    );
                    continue;
                }
                
                ret.Add(new DisplayMode(mode.w, mode.h, mode.refresh_rate));
            }

            return ret;
        }

        public DisplayMode GetClosestSupportedMode(int width, int height, int refreshRate = 0)
        {
            var mode = new SDL2.SDL_DisplayMode
            {
                w = width,
                h = height,
                refresh_rate = refreshRate
            };

            if (SDL2.SDL_GetClosestDisplayMode(Index, ref mode, out var closest) == IntPtr.Zero)
            {
                _log.Error($"Failed to retrieve a closest display mode for {width}x{height}");
                return DisplayMode.Invalid;
            }

            return new DisplayMode(closest.w, closest.h, closest.refresh_rate);
        }

        private void EnsureValid()
        {
            if (!IsValid)
                throw new InvalidOperationException("This operation requires a valid display.");
        }
    }
}