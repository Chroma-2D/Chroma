using System;
using System.Numerics;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SDL;

namespace Chroma.Windowing
{
    public class WindowProperties
    {
        private WindowState _state;
        private int _width;
        private int _height;
        private Vector2 _position;
        private string _title;

        internal Window Owner { get; }
        private Log Log => LogManager.GetForCurrentAssembly();

        public bool ViewportAutoResize { get; set; } = true;

        public int Height
        {
            get
            {
                if (Owner.Handle == IntPtr.Zero)
                    return _height;

                SDL2.SDL_GetWindowSize(Owner.Handle, out _, out var h);
                return h;
            }

            set
            {
                _height = value;

                if (Owner.Handle != IntPtr.Zero)
                {
                    if (ViewportAutoResize)
                        SDL_gpu.GPU_SetWindowResolution((ushort)Width, (ushort)_height);
                    else
                        SDL2.SDL_SetWindowSize(Owner.Handle, (ushort)Width, (ushort)_height);
                }
            }
        }

        public int Width
        {
            get
            {
                if (Owner.Handle == IntPtr.Zero)
                    return _width;

                SDL2.SDL_GetWindowSize(Owner.Handle, out var w, out _);
                return w;
            }

            set
            {
                _width = value;

                if (Owner.Handle != IntPtr.Zero)
                {
                    if (ViewportAutoResize)
                        SDL_gpu.GPU_SetWindowResolution((ushort)_width, (ushort)Height);
                    else
                        SDL2.SDL_SetWindowSize(Owner.Handle, (ushort)_width, (ushort)Height);
                }
            }
        }

        public Vector2 Position
        {
            get
            {
                if (Owner.Handle == IntPtr.Zero)
                    return _position;

                SDL2.SDL_GetWindowPosition(Owner.Handle, out var x, out var y);
                return new Vector2(x, y);
            }

            set
            {
                _position = value;

                if (Owner.Handle != IntPtr.Zero)
                    SDL2.SDL_SetWindowPosition(Owner.Handle, (int)_position.X, (int)_position.Y);
            }
        }

        public string Title
        {
            get
            {
                if (Owner.Handle == IntPtr.Zero)
                    return _title;

                return SDL2.SDL_GetWindowTitle(Owner.Handle);
            }
            set
            {
                _title = value;

                if (Owner.Handle != IntPtr.Zero)
                    SDL2.SDL_SetWindowTitle(Owner.Handle, _title);
            }
        }

        public WindowState State
        {
            get => _state;

            set
            {
                _state = value;

                if (Owner.Handle != IntPtr.Zero)
                {
                    switch (value)
                    {
                        case WindowState.Maximized:
                            var flags = (SDL2.SDL_WindowFlags)SDL2.SDL_GetWindowFlags(Owner.Handle);

                            if (!flags.HasFlag(SDL2.SDL_WindowFlags.SDL_WINDOW_RESIZABLE))
                            {
                                Log.Warning("Refusing to maximize a non-resizable window.");
                                return;
                            }

                            SDL2.SDL_MaximizeWindow(Owner.Handle);
                            break;

                        case WindowState.Minimized:
                            SDL2.SDL_MinimizeWindow(Owner.Handle);
                            break;

                        case WindowState.Normal:
                            SDL2.SDL_RestoreWindow(Owner.Handle);
                            break;
                    }
                }
            }
        }

        public bool IsFullScreen
        {
            get
            {
                var flags = (SDL2.SDL_WindowFlags)SDL2.SDL_GetWindowFlags(Owner.Handle);

                return flags.HasFlag(SDL2.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN)
                    || flags.HasFlag(SDL2.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);
            }
        }
        
        public Vector2 Center => new Vector2(Width / 2, Height / 2);

        internal WindowProperties(Window owner)
        {
            Owner = owner;
            Owner.StateChanged += Owner_StateChanged;

            Width = 800;
            Height = 600;
            Position = new Vector2(SDL2.SDL_WINDOWPOS_CENTERED, SDL2.SDL_WINDOWPOS_CENTERED);
            Title = "Chroma Engine";
        }

        private void Owner_StateChanged(object sender, EventArgs.WindowStateEventArgs e)
            => _state = e.State;
    }
}
