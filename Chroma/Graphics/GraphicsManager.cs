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

        private GraphicsManager()
        {
            if (SDL.SDL_WasInit(SDL.SDL_INIT_EVERYTHING) == 0)
                SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);

            Console.WriteLine(" => GraphicsManager initializing. Registered renderers: ");

            foreach(var s in GetRendererNames())
                Console.WriteLine($"    {s}");
        }

        public List<string> GetRendererNames()
            => GetRegisteredRenderers().Select(x => $"{x.name.Value} ({x.major_version}.{x.minor_version})").ToList();

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
