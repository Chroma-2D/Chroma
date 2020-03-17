using Chroma.SDL2;

namespace Chroma.Windowing
{
    public class WindowProperties
    {
        private Size _size;
        private Vector2 _position;
        private string _title;

        internal OpenGlWindow Owner { get; }

        public Size Size
        {
            get => _size;
            set
            {
                _size = value;
                SDL.SDL_SetWindowSize(Owner.Handle, (ushort)_size.Width, (ushort)_size.Height);
            }
        }

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                SDL.SDL_SetWindowPosition(Owner.Handle, (int)_position.X, (int)_position.Y);
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                SDL.SDL_SetWindowTitle(Owner.Handle, _title);
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

        internal WindowProperties(OpenGlWindow owner)
        {
            _size = new Size(800, 600);
            _position = new Vector2(SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED);
            _title = "Chroma Engine";

            Owner = owner;
            Owner.SizeChanged += Owner_SizeChanged;
            Owner.Moved += Owner_Moved;
        }

        private void Owner_Moved(object sender, EventArgs.WindowMoveEventArgs e)
            => _position = new Vector2(e.Position.X, e.Position.Y);

        private void Owner_SizeChanged(object sender, EventArgs.WindowResizeEventArgs e)
            => _size = new Size(e.Size.Width, e.Size.Height);
    }
}
