using System.Drawing;
using System.Linq;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Natives.Bindings.SDL;

namespace Chroma.Windowing
{
    public class WindowMode
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();
        
        private readonly Window _owner;
        
        internal WindowMode(Window owner)
        {
            _owner = owner;
        }
        
        public bool SetExclusiveFullScreen(DisplayMode displayMode)
        {
            if (SDL2.SDL_SetWindowFullscreen(_owner.Handle, (uint)SDL2.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN) < 0)
            {
                _log.Error($"Failed to switch into exclusive full-screen mode: {SDL2.SDL_GetError()}");
                return false;
            }

            var sdlDisplayMode = displayMode.ToSdlDisplayMode();
           
            if (SDL2.SDL_SetWindowDisplayMode(_owner.Handle, ref sdlDisplayMode) < 0)
            {
                _log.Error($"Failed to set the desired display mode: {SDL2.SDL_GetError()}");
                return false;
            }
            
            SDL_gpu.GPU_SetWindowResolution(
                (ushort)displayMode.Width, 
                (ushort)displayMode.Height
            );

            _owner.Size = new Size(
                displayMode.Width,
                displayMode.Height
            );

            return true;
        }
        
        public bool SetExclusiveFullScreen(int width, int height, int refreshRate)
        {
            var displayMode = _owner.CurrentDisplay.QuerySupportedDisplayModes()
                .FirstOrDefault(x =>
                    x.Width == width
                    && x.Height == height
                    && x.RefreshRate == refreshRate
                );

            if (displayMode == null)
            {
                _log.Warning($"Exact match for {width}x{height}@{refreshRate} not found. Trying nearest best match.");
                displayMode = _owner.CurrentDisplay.GetClosestSupportedMode(width, height, refreshRate);
            }

            return SetExclusiveFullScreen(displayMode);
        }

        public bool SetBorderlessFullScreen()
        {
            if (SDL2.SDL_SetWindowFullscreen(_owner.Handle, (uint)SDL2.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP) < 0)
            {
                _log.Error("Failed to switch into borderless full-screen mode.");
                return false;
            }
            
            _owner.Size = new Size(
                _owner.CurrentDisplay.DesktopMode.Width,
                _owner.CurrentDisplay.DesktopMode.Height
            );

            return true;
        }

        public bool SetWindowed(ushort width, ushort height, bool centerOnScreen = false)
        {
            if (SDL2.SDL_SetWindowFullscreen(_owner.Handle, 0) < 0)
            {
                _log.Error($"Failed to switch into windowed mode: {SDL2.SDL_GetError()}");
                return false;
            }

            _owner.Size = new Size(width, height);

            if (centerOnScreen)
                _owner.CenterOnScreen();

            return true;
        }

        public bool SetWindowed(Size size, bool centerOnScreen = false)
            => SetWindowed((ushort)size.Width, (ushort)size.Height, centerOnScreen);
    }
}