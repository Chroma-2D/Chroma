using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.Bindings.GL;
using Chroma.Natives.Bindings.SDL;
using Chroma.Windowing;

namespace Chroma.Graphics
{
    public sealed class GraphicsManager
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();
        private readonly GameStartupOptions _startupOptions;

        private Stack<SDL_gpu.GPU_RendererID> _rendererIdStack;

        private VerticalSyncMode _verticalSyncMode;

        internal bool AnyRenderersAvailable => _rendererIdStack != null && _rendererIdStack.Any();

        public bool ViewportAutoResize { get; set; } = true;
        public bool LimitFramerate { get; set; } = true;

        public int MaximumMSAA { get; private set; }

        public Size DrawableSize
        {
            get
            {
                SDL2.SDL_GL_GetDrawableSize(
                    Window.Instance.Handle, 
                    out var w, 
                    out var h
                );

                return new Size(w, h);
            }
        }

        public VerticalSyncMode VerticalSyncMode
        {
            get => _verticalSyncMode;
            set
            {
                if (SDL2.SDL_GL_SetSwapInterval((int)value) < 0)
                {
                    _log.Warning(
                        $"Failed to set the requested display synchronization mode: {SDL2.SDL_GetError()}. " +
                        "Attempting to default to vertical retrace."
                    );

                    if (SDL2.SDL_GL_SetSwapInterval(1) < 0)
                    {
                        _log.Error($"Failed to set the fallback vertical retrace synchronization mode: {SDL2.SDL_GetError()}.");
                        return;
                    }
                    
                    _verticalSyncMode = VerticalSyncMode.Retrace;
                    return;
                }
                
                _verticalSyncMode = value;
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

        public List<string> GlExtensions { get; } = new();

        public bool IsValidOpenGlContextPresent
            => SDL2.SDL_GL_GetCurrentContext() != IntPtr.Zero;

        internal GraphicsManager(GameStartupOptions startupOptions)
        {
            _startupOptions = startupOptions;
        }

        public IEnumerable<string> GetRendererNames()
            => GetRegisteredRenderers().Select(x => $"{x.name} ({x.major_version}.{x.minor_version})");

        public IList<Display> GetDisplayList()
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

        internal bool QueryOpenGl()
        {
            bool QueryProperties()
            {
                if (!IsValidOpenGlContextPresent)
                {
                    _log.Error("No valid OpenGL context was present at query time.");
                    return false;
                }

                if (!EnumerateGlExtensions())
                {
                    _log.Error("OpenGL extension enumeration has failed.");
                    return false;
                }

                Gl.GetIntegerV(Gl.GL_MAX_SAMPLES, out var maxMsaa);
                MaximumMSAA = maxMsaa;

                IsAdaptiveVSyncSupported = GlExtensions.Intersect(new[]
                {
                    "GLX_EXT_swap_control_tear",
                    "WGL_EXT_swap_control_tear"
                }).Any();

                return true;
            }

            _rendererIdStack ??= new(GetRegisteredRenderers().OrderBy(x => x.major_version));

            while (AnyRenderersAvailable)
            {
                var rendererId = _rendererIdStack.Peek();

                _log.Info(
                    $"Querying OpenGL details for OpenGL {rendererId.major_version}.{rendererId.minor_version}...");

                if (ProbeGlLimits(rendererId, QueryProperties))
                    return true;

                _rendererIdStack.Pop();
            }

            return false;
        }

        internal IntPtr InitializeRenderer(Window window, out IntPtr windowHandle)
        {
            if (!AnyRenderersAvailable)
            {
                throw new GraphicsException(
                    "No available OpenGL renderers have been successfully queried."
                );
            }

            SDL_gpu.GPU_SetRequiredFeatures(
                SDL_gpu.GPU_FeatureEnum.GPU_FEATURE_BASIC_SHADERS
                | SDL_gpu.GPU_FeatureEnum.GPU_FEATURE_RENDER_TARGETS
                | SDL_gpu.GPU_FeatureEnum.GPU_FEATURE_BLEND_EQUATIONS
            );

            var msaaSamples = _startupOptions.MsaaSamples;
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

                if (SDL2.SDL_GL_SetAttribute(SDL2.SDL_GLattr.SDL_GL_MULTISAMPLEBUFFERS, 1) < 0)
                {
                    _log.Error($"Failed to enable MSAA buffer: {SDL2.SDL_GetError()}");
                }

                if (SDL2.SDL_GL_SetAttribute(SDL2.SDL_GLattr.SDL_GL_MULTISAMPLESAMPLES, msaaSamples) < 0)
                {
                    _log.Error($"Failed to set MSAA samples to {msaaSamples}: {SDL2.SDL_GetError()}");
                }
            }

            if (SDL2.SDL_GL_SetAttribute(SDL2.SDL_GLattr.SDL_GL_ACCELERATED_VISUAL, 1) < 0)
            {
                _log.Error($"Failed to force hardware-accelerated visuals: {SDL2.SDL_GetError()} " +
                           "Performance might be degraded.");
            }

            var rendererId = _rendererIdStack.Peek();
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
                var err = SDL_gpu.GPU_PopErrorCode();
                while (err.error != SDL_gpu.GPU_ErrorEnum.GPU_ERROR_NONE)
                {
                    err = SDL_gpu.GPU_PopErrorCode();
                }

                _rendererIdStack.Pop();
                windowHandle = IntPtr.Zero;

                return IntPtr.Zero;
            }

            SDL_gpu.GPU_SetDefaultAnchor(0, 0);

            _log.Info($"{OpenGlVendorString} {OpenGlRendererString} v{OpenGlVersionString}");
            _log.Info(" Available displays:");

            var displays = GetDisplayList();
            for (var i = 0; i < displays.Count; i++)
            {
                var d = displays[i];
                _log.Info($"  Display {d.Index} ({d.Name}) [{d.Bounds.Width}x{d.Bounds.Height}], mode {d.DesktopMode}");
            }

            windowHandle = SDL2.SDL_GL_GetCurrentWindow();
            return renderTargetHandle;
        }

        private static IEnumerable<SDL_gpu.GPU_RendererID> GetRegisteredRenderers()
        {
            var renderers = SDL_gpu.GPU_GetNumRegisteredRenderers();
            var registeredRenderers = new SDL_gpu.GPU_RendererID[renderers];
            SDL_gpu.GPU_GetRegisteredRendererList(registeredRenderers);

            if (registeredRenderers.Length == 0)
            {
                throw new GraphicsException(
                    $"No renderers have been found: {SDL_gpu.GPU_PopErrorCode().details}" +
                    "Make sure SDL_gpu was built with at least 1 renderer available."
                );
            }

            return registeredRenderers.ToList();
        }

        private bool EnumerateGlExtensions()
        {
            Gl.GetIntegerV(Gl.GL_NUM_EXTENSIONS, out var numExtensions);

            if (numExtensions <= 0)
                return false;

            for (var i = 0; i < numExtensions; i++)
            {
                var strPtr = Gl.GetStringI(Gl.GL_EXTENSIONS, (uint)i);
                var str = Marshal.PtrToStringAnsi(strPtr);

                GlExtensions.Add(str);
            }

            return true;
        }

        private bool ProbeGlLimits(SDL_gpu.GPU_RendererID rendererId, Func<bool> probeLogic)
        {
            if (SDL2.SDL_GL_SetAttribute(
                SDL2.SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, rendererId.major_version) < 0)
            {
                _log.Error($"Failed to set probe-time OpenGL major version attribute: {SDL2.SDL_GetError()}");
                return false;
            }

            if (SDL2.SDL_GL_SetAttribute(
                SDL2.SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, rendererId.minor_version) < 0)
            {
                _log.Error($"Failed to set probe-time OpenGL minor version attribute: {SDL2.SDL_GetError()}");
                return false;
            }

            if (SDL2.SDL_GL_SetAttribute(
                SDL2.SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, SDL2.SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE) < 0)
            {
                _log.Error($"Failed to set probe-time OpenGL core profile preference: {SDL2.SDL_GetError()}");
                return false;
            }

            var window = SDL2.SDL_CreateWindow(
                "Chroma Probe Window",
                0, 0, 0, 0,
                SDL2.SDL_WindowFlags.SDL_WINDOW_OPENGL
                | SDL2.SDL_WindowFlags.SDL_WINDOW_BORDERLESS
                | SDL2.SDL_WindowFlags.SDL_WINDOW_HIDDEN
            );

            var context = SDL2.SDL_GL_GetCurrentContext();
            var destroyContextAfter = false;

            if (context == IntPtr.Zero)
            {
                destroyContextAfter = true;
                context = SDL2.SDL_GL_CreateContext(window);

                if (context == IntPtr.Zero)
                {
                    _log.Error($"Failed to create probe-time OpenGL context: {SDL2.SDL_GetError()}");
                    return false;
                }

                if (SDL2.SDL_GL_MakeCurrent(window, context) < 0)
                {
                    _log.Error($"Failed to set probe-time OpenGL context: {SDL2.SDL_GetError()}");
                    return false;
                }
            }

            var probeResult = probeLogic();

            SDL2.SDL_DestroyWindow(window);

            if (destroyContextAfter)
                SDL2.SDL_GL_DeleteContext(context);

            return probeResult;
        }
    }
}