using System;
using System.Collections.Generic;
using System.Linq;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public class GraphicsManager
    {
        private bool _vSyncEnabled;

        private Game Game { get; }

        private Log Log { get; } = LogManager.GetForCurrentAssembly();

        public bool LimitFramerate { get; set; } = true;
        public bool AutoClear { get; set; } = true;
        public Color AutoClearColor { get; set; } = Color.Transparent;

        public bool VSyncEnabled
        {
            get => _vSyncEnabled;
            set
            {
                _vSyncEnabled = value;
                SDL2.SDL_GL_SetSwapInterval(value ? 1 : 0);
            }
        }

        public float ScreenGamma
        {
            get => SDL2.SDL_GetWindowBrightness(Game.Window.Handle);
            set => SDL2.SDL_SetWindowBrightness(Game.Window.Handle, value);
        }

        public bool IsDefaultShaderActive
            => SDL_gpu.GPU_IsDefaultShaderProgram(SDL_gpu.GPU_GetCurrentShaderProgram());

        internal GraphicsManager(Game game)
        {
            Game = game;
            var renderers = GetRendererNames();

            Log.Info("GraphicsManager initializing...");
            Log.Info(" Registered renderers:");

            foreach (var s in renderers)
                Log.Info($"  {s}");

            Log.Info(" Available displays:");

            foreach (var d in FetchDisplayInfo())
                Log.Info($"  {d.Index}: {d.Width}x{d.Height}@{d.RefreshRate}Hz");

            VSyncEnabled = true;
        }

        public List<string> GetRendererNames()
            => GetRegisteredRenderers().Select(x => $"{x.name} ({x.major_version}.{x.minor_version})").ToList();

        public List<Display> FetchDisplayInfo()
        {
            var displays = new List<Display>();
            var count = SDL2.SDL_GetNumVideoDisplays();

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
            var count = SDL2.SDL_GetNumVideoDisplays();

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
            if (SDL2.SDL_GetCurrentDisplayMode(index, out var mode) == 0)
            {
                return new Display(index, mode.refresh_rate, (ushort)mode.w, (ushort)mode.h)
                {
                    UnderlyingDisplayMode = mode
                };
            }

            Log.Error($"Failed to retrieve display {index} info: {SDL2.SDL_GetError()}");
            return null;
        }

        public Display FetchDesktopDisplayInfo(int index)
        {
            if (SDL2.SDL_GetDesktopDisplayMode(index, out var mode) == 0)
            {
                return new Display(index, mode.refresh_rate, (ushort)mode.w, (ushort)mode.h)
                {
                    UnderlyingDisplayMode = mode
                };
            }

            Log.Error($"Failed to retrieve desktop display {index} info: {SDL2.SDL_GetError()}");
            return null;
        }

        internal List<SDL_gpu.GPU_RendererID> GetRegisteredRenderers()
        {
            var renderers = SDL_gpu.GPU_GetNumRegisteredRenderers();
            var registeredRenderers = new SDL_gpu.GPU_RendererID[renderers];
            SDL_gpu.GPU_GetRegisteredRendererList(registeredRenderers);

            if (registeredRenderers.Length == 0)
            {
                Log.Error("Your computer does not support any rendering APIs that Chroma supports.");
                throw new NotSupportedException("None of Chroma's Rendering APIs are supported on this computer. " +
                                                "Make sure you have support for at least OpenGL 3.");
            }

            return registeredRenderers.ToList();
        }

        internal SDL_gpu.GPU_RendererID GetBestRenderer()
            => GetRegisteredRenderers().OrderByDescending(x => x.major_version).First();
    }
}