using Chroma.SDL2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chroma.Graphics
{
    public class GraphicsManager
    {
        private static GraphicsManager _instance;
        public static readonly GraphicsManager Instance = new Lazy<GraphicsManager>(() => _instance ??= new GraphicsManager()).Value;

        private bool _vSyncEnabled;

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

        private GraphicsManager()
        {
            if (SDL.SDL_WasInit(SDL.SDL_INIT_EVERYTHING) == 0)
                SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);

            Console.WriteLine(" => GraphicsManager initializing...");
            Console.WriteLine("    Registered renderers:");

            foreach (var s in GetRendererNames())
                Console.WriteLine($"      {s}");

            Console.WriteLine("    Available displays:");

            foreach (var d in FetchDisplayInfo())
                Console.WriteLine($"      {d.Index}: {d.Dimensions.Width}x{d.Dimensions.Height}@{d.RefreshRate}Hz");
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
                Console.WriteLine($"Failed to retrieve display {index} info: {SDL.SDL_GetError()}");
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
                Console.WriteLine($"Failed to retrieve desktop display {index} info: {SDL.SDL_GetError()}");
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
