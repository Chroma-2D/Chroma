using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.GL;
using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public class GraphicsManager
    {
        private static VerticalSyncMode _verticalSyncMode;

        private static bool _enableMultiSampling = true;
        private static int _multiSamplingPrecision = 4;

        private Game Game { get; }

        private static Log Log { get; } = LogManager.GetForCurrentAssembly();

        public static bool ViewportAutoResize { get; set; } = true;
        public static bool LimitFramerate { get; set; } = true;
        public static bool AutoClear { get; set; } = true;
        public static Color AutoClearColor { get; set; } = Color.Transparent;

        public static bool MultiSamplingEnabled
        {
            get => _enableMultiSampling;
            set
            {
                _enableMultiSampling = value;
                var result = SDL2.SDL_GL_SetAttribute(SDL2.SDL_GLattr.SDL_GL_MULTISAMPLEBUFFERS,
                    _enableMultiSampling ? 1 : 0);

                if (result < 0)
                    Log.Warning(
                        $"Failed to set SDL_GL_MULTISAMPLEBUFFERS to '{_enableMultiSampling}: {SDL2.SDL_GetError()}");
            }
        }

        public static int MultiSamplingPrecision
        {
            get => _multiSamplingPrecision;
            set
            {
                if (value > MaximumMultiSamplingPrecision)
                {
                    Log.Warning(
                        $"Maximum supported multisampling precision is {MaximumMultiSamplingPrecision}. " +
                        $"Amount of {value} was requested."
                    );

                    _multiSamplingPrecision = MaximumMultiSamplingPrecision;
                }
                else
                {
                    _multiSamplingPrecision = value;
                }

                var result =
                    SDL2.SDL_GL_SetAttribute(SDL2.SDL_GLattr.SDL_GL_MULTISAMPLESAMPLES, (int)_multiSamplingPrecision);

                if (result < 0)
                    Log.Warning(
                        $"Failed to set SDL_GL_MULTISAMPLESAMPLES to '{_multiSamplingPrecision}': {SDL2.SDL_GetError()}");
            }
        }

        public static int MaximumMultiSamplingPrecision { get; private set; }

        public float LineThickness
        {
            get => SDL_gpu.GPU_GetLineThickness();
            set => SDL_gpu.GPU_SetLineThickness(value);
        }

        public VerticalSyncMode VerticalSyncMode
        {
            get => _verticalSyncMode;
            set
            {
                _verticalSyncMode = value;

                var result = SDL2.SDL_GL_SetSwapInterval((int)_verticalSyncMode);

                if (result < 0)
                {
                    _verticalSyncMode = VerticalSyncMode.Retrace;
                    SDL2.SDL_GL_SetSwapInterval(1);

                    Log.Warning(
                        $"Failed to set the requested display synchronization mode: {SDL2.SDL_GetError()}. " +
                        "Defaulting to vertical retrace."
                    );
                }
            }
        }

        public bool IsAdaptiveVSyncSupported { get; private set; }

        public float ScreenGamma
        {
            get => SDL2.SDL_GetWindowBrightness(Game.Window.Handle);
            set => SDL2.SDL_SetWindowBrightness(Game.Window.Handle, value);
        }

        public bool IsDefaultShaderActive
            => SDL_gpu.GPU_IsDefaultShaderProgram(SDL_gpu.GPU_GetCurrentShaderProgram());

        static GraphicsManager()
        {
            ProbeGlLimits();
        }

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

            CheckGlExtensionAvailability();

            LineThickness = 1;
            VerticalSyncMode = VerticalSyncMode.Retrace;
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

        internal static List<SDL_gpu.GPU_RendererID> GetRegisteredRenderers()
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

        internal static SDL_gpu.GPU_RendererID GetBestRenderer()
            => GetRegisteredRenderers().OrderByDescending(x => x.major_version).First();

        private void CheckGlExtensionAvailability()
        {
            var glxResult = SDL2.SDL_GL_ExtensionSupported("GLX_EXT_swap_control_tear");
            var wglResult = SDL2.SDL_GL_ExtensionSupported("WGL_EXT_swap_control_tear");

            IsAdaptiveVSyncSupported = (glxResult == SDL2.SDL_bool.SDL_TRUE || wglResult == SDL2.SDL_bool.SDL_TRUE);
        }

        private static void ProbeGlLimits()
        {
            var prevSamples = MultiSamplingPrecision;
            MultiSamplingPrecision = 0;

            SDL2.SDL_CreateWindowAndRenderer(
                0, 
                0,
                SDL2.SDL_WindowFlags.SDL_WINDOW_HIDDEN |
                SDL2.SDL_WindowFlags.SDL_WINDOW_OPENGL,
                out var window,
                out var renderer
            );

            Gl.GlGetIntegerV(Gl.GL_MAX_SAMPLES, out var maxSamples);
            MaximumMultiSamplingPrecision = maxSamples;
            
            SDL2.SDL_DestroyRenderer(renderer);
            SDL2.SDL_DestroyWindow(window);
            
            MultiSamplingPrecision = prevSamples;
        }
    }
}