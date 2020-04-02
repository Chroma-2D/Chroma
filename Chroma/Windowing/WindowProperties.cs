using Chroma.Diagnostics;
using Chroma.Natives.SDL;

namespace Chroma.Windowing
{
    public class WindowProperties
    {
        private WindowState _state;
        internal Window Owner { get; }

        public bool ViewportAutoResize { get; set; } = true;

        public float Height
        {
            get
            {
                SDL2.SDL_GetWindowSize(Owner.Handle, out int _, out int h);
                return h;
            }

            set
            {
                if (ViewportAutoResize)
                    SDL_gpu.GPU_SetWindowResolution((ushort)Width, (ushort)value);
                else
                    SDL2.SDL_SetWindowSize(Owner.Handle, (ushort)Width, (ushort)value);
            }
        }

        public float Width
        {
            get
            {
                SDL2.SDL_GetWindowSize(Owner.Handle, out int w, out int _);
                return w;
            }

            set
            {
                if (ViewportAutoResize)
                    SDL_gpu.GPU_SetWindowResolution((ushort)value, (ushort)Height);
                else
                    SDL2.SDL_SetWindowSize(Owner.Handle, (ushort)value, (ushort)Height);
            }
        }

        public Vector2 Position
        {
            get
            {
                SDL2.SDL_GetWindowPosition(Owner.Handle, out int w, out int h);
                return new Vector2(w, h);
            }

            set => SDL2.SDL_SetWindowPosition(Owner.Handle, (int)value.X, (int)value.Y);
        }

        public string Title
        {
            get => SDL2.SDL_GetWindowTitle(Owner.Handle);
            set => SDL2.SDL_SetWindowTitle(Owner.Handle, value);
        }

        public WindowState State
        {
            get => _state;

            set
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

                _state = value;
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
