using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public class GraphicsManager
    {
        private static DisplaySynchronization _displaySynchronization;

        private Game Game { get; }

        private static Log Log { get; } = LogManager.GetForCurrentAssembly();

        public static bool ViewportAutoResize { get; set; } = true;
        public static bool LimitFramerate { get; set; } = true;
        public static bool AutoClear { get; set; } = true;
        public static Color AutoClearColor { get; set; } = Color.Transparent;

        public static float LineThickness
        {
            get => SDL_gpu.GPU_GetLineThickness();
            set => SDL_gpu.GPU_SetLineThickness(value);
        }

        public static DisplaySynchronization DisplaySynchronization
        {
            get => _displaySynchronization;
            set
            {
                _displaySynchronization = value;
                
                var result = SDL2.SDL_GL_SetSwapInterval((int)_displaySynchronization);
                
                if (result < 0)
                {
                    _displaySynchronization = DisplaySynchronization.VerticalRetrace;
                    SDL2.SDL_GL_SetSwapInterval(1);
                    
                    Log.Warning(
                        $"Failed to set the requested display synchronization mode: {SDL2.SDL_GetError()}. Defaulting to vertical retrace.");
                }
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

        public Size GetNativeResolution(int index)
        {
            var display = FetchDesktopDisplayInfo(index);
            return new Size(display.Width, display.Height);
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