using Chroma.SDL2;

namespace Chroma.Windowing
{
    public class WindowProperties
    {
        private WindowState _state;
        internal Window Owner { get; }

        public bool ViewportAutoResize { get; set; } = true;

        public Size Size
        {
            get
            {
                SDL.SDL_GetWindowSize(Owner.Handle, out int w, out int h);
                return new Size(w, h);
            }

            set
            {
                if (ViewportAutoResize)
                    SDL_gpu.GPU_SetWindowResolution((ushort)value.Width, (ushort)value.Height);
                else
                    SDL.SDL_SetWindowSize(Owner.Handle, (ushort)value.Width, (ushort)value.Height);
            }
        }

        public Vector2 Position
        {
            get
            {
                SDL.SDL_GetWindowPosition(Owner.Handle, out int w, out int h);
                return new Vector2(w, h);
            }

            set => SDL.SDL_SetWindowPosition(Owner.Handle, (int)value.X, (int)value.Y);
        }

        public string Title
        {
            get => SDL.SDL_GetWindowTitle(Owner.Handle);
            set => SDL.SDL_SetWindowTitle(Owner.Handle, value);
        }

        public WindowState State
        {
            get => _state;

            set
            {
                _state = value;

                switch (_state)
                {
                    case WindowState.Maximized:
                        SDL.SDL_MaximizeWindow(Owner.Handle);
                        break;

                    case WindowState.Minimized:
                        SDL.SDL_MinimizeWindow(Owner.Handle);
                        break;

                    case WindowState.Normal:
                        SDL.SDL_RestoreWindow(Owner.Handle);
                        break;
                }
            }
        }

        public bool IsFullScreen
        {
            get
            {
                var flags = (SDL.SDL_WindowFlags)SDL.SDL_GetWindowFlags(Owner.Handle);

                return flags.HasFlag(SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN)
                    || flags.HasFlag(SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);
            }
        }

        internal WindowProperties(Window owner)
        {
            Owner = owner;
            Owner.StateChanged += Owner_StateChanged;

            Size = new Size(800, 600);
            Position = new Vector2(SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED);
            Title = "Chroma Engine";
        }

        private void Owner_StateChanged(object sender, EventArgs.WindowStateEventArgs e)
            => _state = e.State;
    }
}
