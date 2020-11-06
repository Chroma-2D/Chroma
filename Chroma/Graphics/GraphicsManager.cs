using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.GL;
using Chroma.Natives.SDL;
using Chroma.Windowing;

namespace Chroma.Graphics
{
    public class GraphicsManager
    {
        private static VerticalSyncMode _verticalSyncMode;
        private static int _multiSamplingPrecision;

        private static readonly Log _log = LogManager.GetForCurrentAssembly();

        private Game Game { get; }

        public static bool ViewportAutoResize { get; set; } = true;
        public static bool LimitFramerate { get; set; } = true;
        public static bool AutoClear { get; set; } = true;
        public static Color AutoClearColor { get; set; } = Color.Transparent;

        public static int MaximumMultiSamplingPrecision { get; private set; }

        public static int MultiSamplingPrecision
        {
            get => _multiSamplingPrecision;
            set
            {
                if (value > MaximumMultiSamplingPrecision)
                {
                    _log.Warning(
                        $"Maximum supported multisampling precision is {MaximumMultiSamplingPrecision}. " +
                        $"Amount of {value} was requested. Setting maximum instead."
                    );

                    _multiSamplingPrecision = MaximumMultiSamplingPrecision;
                }
                else
                {
                    _multiSamplingPrecision = value;
                }

                SDL2.SDL_GL_SetAttribute(
                    SDL2.SDL_GLattr.SDL_GL_MULTISAMPLESAMPLES,
                    _multiSamplingPrecision
                );
            }
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

                    _log.Warning(
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

        public List<string> GlExtensions { get; } = new List<string>();

        static GraphicsManager()
        {
            _log.Info("Initializing...");
            _log.Info("Probing OpenGL limits...");

            ProbeGlLimits(
                preProbe: () => { MultiSamplingPrecision = 0; },
                probe: () =>
                {
                    Gl.GetIntegerV(Gl.GL_MAX_SAMPLES, out var maxSamples);
                    MaximumMultiSamplingPrecision = maxSamples;
                }, () => { }
            );
        }

        internal GraphicsManager(Game game)
        {
            Game = game;

            var renderers = GetRendererNames();

            _log.Info(" Registered renderers:");
            foreach (var s in renderers)
                _log.Info($"  {s}");

            _log.Info(" Available displays:");

            foreach (var d in GetDisplayList())
                _log.Info($"  Display {d.Index} ({d.Name}) [{d.Bounds.Width}x{d.Bounds.Height}], mode {d.DesktopMode}");

            CheckGlExtensionAvailability();
            VerticalSyncMode = VerticalSyncMode.Retrace;
        }

        public List<string> GetRendererNames()
            => GetRegisteredRenderers().Select(x => $"{x.name} ({x.major_version}.{x.minor_version})").ToList();

        public List<Display> GetDisplayList()
        {
            var displays = new List<Display>();
            var count = SDL2.SDL_GetNumVideoDisplays();

            for (var i = 0; i < count; i++)
            {
                var display = FetchDisplay(i);

                if (display != null)
                    displays.Add(display);
            }

            return displays;
        }

        public Display FetchDisplay(int index)
        {
            if (SDL2.SDL_GetCurrentDisplayMode(index, out _) == 0)
                return new Display(index);

            _log.Error($"Failed to retrieve display {index} info: {SDL2.SDL_GetError()}");
            return null;
        }

        internal static List<SDL_gpu.GPU_RendererID> GetRegisteredRenderers()
        {
            var renderers = SDL_gpu.GPU_GetNumRegisteredRenderers();
            var registeredRenderers = new SDL_gpu.GPU_RendererID[renderers];
            SDL_gpu.GPU_GetRegisteredRendererList(registeredRenderers);

            if (registeredRenderers.Length == 0)
            {
                _log.Error("Your computer does not support any rendering APIs that Chroma supports.");
                throw new NotSupportedException("None of Chroma's Rendering APIs are supported on this computer. " +
                                                "Make sure you have support for at least OpenGL 3.");
            }

            return registeredRenderers.ToList();
        }

        internal static SDL_gpu.GPU_RendererID GetBestRenderer()
        {
            var renderer = GetRegisteredRenderers().OrderByDescending(x => x.major_version).First();
            _log.Info($"Selecting highest available renderer version: {renderer.name}");
            return renderer;
        }

        internal static IntPtr InitializeRenderer(Window window, SDL_gpu.GPU_RendererID id)
        {
            var renderTargetHandle = SDL_gpu.GPU_InitRenderer(
                id.renderer,
                (ushort)window.Size.Width,
                (ushort)window.Size.Height,
                0
            );

            if (renderTargetHandle == IntPtr.Zero)
                throw new FrameworkException("Failed to initialize the renderer.", true);

            // unsafe
            // {
            //     var rptr = SDL_gpu.GPU_GetRenderer(id);
            //     SDL_gpu.GPU_Renderer* r = (SDL_gpu.GPU_Renderer*)rptr.ToPointer();
            // }

            return renderTargetHandle;
        }

        private void CheckGlExtensionAvailability()
        {
            var glxResult = SDL2.SDL_GL_ExtensionSupported("GLX_EXT_swap_control_tear");
            var wglResult = SDL2.SDL_GL_ExtensionSupported("WGL_EXT_swap_control_tear");

            IsAdaptiveVSyncSupported = (glxResult == SDL2.SDL_bool.SDL_TRUE || wglResult == SDL2.SDL_bool.SDL_TRUE);

            Gl.GetIntegerV(Gl.GL_NUM_EXTENSIONS, out var numExtensions);

            if (numExtensions > 0)
            {
                for (var i = 0; i < numExtensions; i++)
                {
                    var strPtr = Gl.GetStringI(Gl.GL_EXTENSIONS, (uint)i);
                    var str = Marshal.PtrToStringAuto(strPtr);

                    GlExtensions.Add(str);
                }
            }
            else
            {
                _log.Info("Couldn't retrieve OpenGL extension list.");
            }
        }

        private static void ProbeGlLimits(Action preProbe, Action probe, Action postProbe)
        {
            preProbe();

            SDL2.SDL_CreateWindowAndRenderer(
                0, 0,
                SDL2.SDL_WindowFlags.SDL_WINDOW_OPENGL |
                SDL2.SDL_WindowFlags.SDL_WINDOW_BORDERLESS,
                out var window,
                out var renderer
            );

            var context = SDL2.SDL_GL_GetCurrentContext();
            var destroyContextAfter = false;

            if (context == IntPtr.Zero) // can and will happen on windows
            {
                destroyContextAfter = true;
                context = SDL2.SDL_GL_CreateContext(window);
                SDL2.SDL_GL_MakeCurrent(window, context);
            }

            probe();

            if (destroyContextAfter)
                SDL2.SDL_GL_DeleteContext(context);

            SDL2.SDL_DestroyRenderer(renderer);
            SDL2.SDL_DestroyWindow(window);

            postProbe();
        }
    }
}