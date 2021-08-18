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
        private readonly Log _log = LogManager.GetForCurrentAssembly();
        private VerticalSyncMode _verticalSyncMode;
        private Game _game;

        public bool ViewportAutoResize { get; set; } = true;
        public bool LimitFramerate { get; set; } = true;

        public int MaximumMSAA { get; private set; }

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

        public string OpenGlVendorString =>
            Marshal.PtrToStringAnsi(
                Gl.GetString(Gl.GL_VENDOR)
            );

        public string OpenGlVersionString =>
            Marshal.PtrToStringAnsi(
                Gl.GetString(Gl.GL_VERSION)
            );

        public string OpenGlRendererString =>
            Marshal.PtrToStringAnsi(
                Gl.GetString(Gl.GL_RENDERER)
            );

        public bool IsAdaptiveVSyncSupported { get; private set; }

        public float ScreenGamma
        {
            get => SDL2.SDL_GetWindowBrightness(_game.Window.Handle);
            set => SDL2.SDL_SetWindowBrightness(_game.Window.Handle, value);
        }

        public List<string> GlExtensions { get; } = new();

        internal GraphicsManager(Game game)
        {
            _game = game;

            ProbeGlLimits(
                () =>
                {
                    EnumerateGlExtensions();

                    Gl.GetIntegerV(Gl.GL_MAX_SAMPLES, out var maxMsaa);
                    MaximumMSAA = maxMsaa;
                }
            );
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

        internal IntPtr InitializeRenderer(Window window, out IntPtr windowHandle)
        {           
            SDL_gpu.GPU_SetRequiredFeatures(
                SDL_gpu.GPU_FeatureEnum.GPU_FEATURE_BASIC_SHADERS
                | SDL_gpu.GPU_FeatureEnum.GPU_FEATURE_RENDER_TARGETS
                | SDL_gpu.GPU_FeatureEnum.GPU_FEATURE_BLEND_EQUATIONS
            );

            var msaaSamples = _game.StartupOptions.MsaaSamples;
            if (msaaSamples > 0)
            {
                if (msaaSamples > MaximumMSAA)
                {
                    _log.Warning(
                        $"Requested {msaaSamples} MSAA samples, however " +
                        $"your driver supports a maximum of {MaximumMSAA}, so you'll get that many."
                    );

                    msaaSamples = MaximumMSAA;
                }
                
                SDL2.SDL_GL_SetAttribute(SDL2.SDL_GLattr.SDL_GL_MULTISAMPLEBUFFERS, 1);
                SDL2.SDL_GL_SetAttribute(SDL2.SDL_GLattr.SDL_GL_MULTISAMPLESAMPLES, msaaSamples);
            }

            var rendererId = FindBestRenderer();
            var renderTargetHandle = SDL_gpu.GPU_InitRendererByID(
                rendererId,
                (ushort)window.Size.Width,
                (ushort)window.Size.Height,
                SDL2.SDL_WindowFlags.SDL_WINDOW_OPENGL
                | SDL2.SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI
                | SDL2.SDL_WindowFlags.SDL_WINDOW_SHOWN
            );

            if (renderTargetHandle == IntPtr.Zero)
            {
                if (renderTargetHandle == IntPtr.Zero)
                    throw new FrameworkException("Failed to initialize the renderer.", true);
            }
            
            SDL_gpu.GPU_SetDefaultAnchor(0, 0);
            
            _log.Info($"{OpenGlVendorString} {OpenGlRendererString} v{OpenGlVersionString}");

            _log.Info(" Available displays:");
            foreach (var d in GetDisplayList())
                _log.Info($"  Display {d.Index} ({d.Name}) [{d.Bounds.Width}x{d.Bounds.Height}], mode {d.DesktopMode}");

            windowHandle = SDL2.SDL_GL_GetCurrentWindow();
            return renderTargetHandle;
        }

        private SDL_gpu.GPU_RendererID FindBestRenderer()
            => GetRegisteredRenderers()
                .OrderByDescending(x => x.major_version)
                .First();

        private List<SDL_gpu.GPU_RendererID> GetRegisteredRenderers()
        {
            var renderers = SDL_gpu.GPU_GetNumRegisteredRenderers();
            var registeredRenderers = new SDL_gpu.GPU_RendererID[renderers];
            SDL_gpu.GPU_GetRegisteredRendererList(registeredRenderers);

            if (registeredRenderers.Length == 0)
            {
                throw new NotSupportedException("None of Chroma's Rendering APIs are supported on this computer. " +
                                                "Make sure you have support for at least OpenGL 3.");
            }

            return registeredRenderers.ToList();
        }

        private void EnumerateGlExtensions()
        {
            Gl.GetIntegerV(Gl.GL_NUM_EXTENSIONS, out var numExtensions);

            if (numExtensions > 0)
            {
                for (var i = 0; i < numExtensions; i++)
                {
                    var strPtr = Gl.GetStringI(Gl.GL_EXTENSIONS, (uint)i);
                    var str = Marshal.PtrToStringAnsi(strPtr);

                    GlExtensions.Add(str);
                }
            }
            else
            {
                _log.Info("Couldn't retrieve OpenGL extension list.");
            }

            IsAdaptiveVSyncSupported = GlExtensions.Intersect(new[]
            {
                "GLX_EXT_swap_control_tear",
                "WGL_EXT_swap_control_tear"
            }).Any();
        }

        private void ProbeGlLimits(Action probeLogic)
        {
            var gpuRendererId = FindBestRenderer();

            _log.WarningWhenFails(
                $"Failed to set OpenGL major version for probe",
                () => SDL2.SDL_GL_SetAttribute(
                    SDL2.SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION,
                    gpuRendererId.major_version
                )
            );

            _log.WarningWhenFails(
                $"Failed to set OpenGL minor version for probe",
                () => SDL2.SDL_GL_SetAttribute(
                    SDL2.SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION,
                    gpuRendererId.minor_version
                )
            );

            _log.WarningWhenFails(
                $"Failed to set OpenGL core profile for probe",
                () => SDL2.SDL_GL_SetAttribute(
                    SDL2.SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK,
                    SDL2.SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE
                )
            );

            SDL2.SDL_CreateWindowAndRenderer(
                0, 0,
                SDL2.SDL_WindowFlags.SDL_WINDOW_OPENGL
                | SDL2.SDL_WindowFlags.SDL_WINDOW_BORDERLESS,
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

            probeLogic();

            if (destroyContextAfter)
                SDL2.SDL_GL_DeleteContext(context);

            SDL2.SDL_DestroyRenderer(renderer);
            SDL2.SDL_DestroyWindow(window);
        }
    }
}