using System.Collections.Generic;
using System.Linq;
using Chroma.Diagnostics;
using Chroma.SDL2;

namespace Chroma.Graphics
{
    public class GraphicsManager
    {
        private bool _vSyncEnabled;

        private Game Game { get; }
        
        public bool VSyncEnabled
        {
            get => _vSyncEnabled;
            set
            {
                _vSyncEnabled = value;

                if (!value)
                {
                    SDL.SDL_GL_SetSwapInterval(0);
                }
                else
                {
                    SDL.SDL_GL_SetSwapInterval(1);
                }
            }
        }

        public float Gamma
        {
            get => SDL.SDL_GetWindowBrightness(Game.Window.Handle);
            set => SDL.SDL_SetWindowBrightness(Game.Window.Handle, value);
        }

        internal GraphicsManager(Game game)
        {
            if (SDL.SDL_WasInit(SDL.SDL_INIT_EVERYTHING) == 0)
                SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);

            Game = game;

            Log.Info("GraphicsManager initializing...");
            Log.Info(" Registered renderers:");

            foreach (var s in GetRendererNames())
                Log.Info($"  {s}");

            Log.Info(" Available displays:");

            foreach (var d in FetchDisplayInfo())
                Log.Info($"  {d.Index}: {d.Width}x{d.Height}@{d.RefreshRate}Hz");

            VSyncEnabled = true;
        }

        public List<string> GetRendererNames()
            => GetRegisteredRenderers().Select(x => $"{x.name.Value} ({x.major_version}.{x.minor_version})").ToList();

        public List<Display> FetchDisplayInfo()
        {
            var displays = new List<Display>();
            var count = SDL.SDL_GetNumVideoDisplays();

            for (var i = 0; i < count; i++)
            {
                var display = FetchDisplayInfo(i);

                if (display != null)
                    displays.Add(display);
            }

            return displays;
        }

        public List<Display> FetchDesktopDisplayInfo()
        {
            var displays = new List<Display>();
            var count = SDL.SDL_GetNumVideoDisplays();

            for (var i = 0; i < count; i++)
            {
                var display = FetchDesktopDisplayInfo(i);

                if (display != null)
                    displays.Add(display);
            }

            return displays;
        }

        public Display FetchDisplayInfo(int index)
        {
            if (SDL.SDL_GetCurrentDisplayMode(index, out SDL.SDL_DisplayMode mode) == 0)
            {
                return new Display(index, mode.refresh_rate, (ushort)mode.w, (ushort)mode.h)
                {
                    UnderlyingDisplayMode = mode
                };
            }
            else
            {
                Log.Error($"Failed to retrieve display {index} info: {SDL.SDL_GetError()}");
                return null;
            }
        }

        public Display FetchDesktopDisplayInfo(int index)
        {
            if (SDL.SDL_GetDesktopDisplayMode(index, out SDL.SDL_DisplayMode mode) == 0)
            {
                return new Display(index, mode.refresh_rate, (ushort)mode.w, (ushort)mode.h)
                {
                    UnderlyingDisplayMode = mode
                };
            }
            else
            {
                Log.Error($"Failed to retrieve desktop display {index} info: {SDL.SDL_GetError()}");
                return null;
            }
        }

        internal List<SDL_gpu.GPU_RendererID> GetRegisteredRenderers()
        {
            var registeredRenderers = new SDL_gpu.GPU_RendererID[SDL_gpu.GPU_GetNumRegisteredRenderers()];
            SDL_gpu.GPU_GetRegisteredRendererList(registeredRenderers);

            return registeredRenderers.ToList();
        }

        internal SDL_gpu.GPU_RendererID GetBestRenderer()
            => GetRegisteredRenderers().OrderByDescending(x => x.major_version).First();
    }
}
