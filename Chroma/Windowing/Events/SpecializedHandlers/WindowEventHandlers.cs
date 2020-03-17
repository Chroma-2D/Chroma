using Chroma.SDL2;

namespace Chroma.Windowing.Events.SpecializedHandlers
{
    internal class WindowEventHandlers
    {
        private EventDispatcher Dispatcher { get; }

        internal WindowEventHandlers(EventDispatcher dispatcher)
        {
            Dispatcher = dispatcher;


            Dispatcher.RegisterWindowEventHandler(SDL.SDL_WindowEventID.SDL_WINDOWEVENT_ENTER, MouseEntered);
            Dispatcher.RegisterWindowEventHandler(SDL.SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE, MouseLeft);
            Dispatcher.RegisterWindowEventHandler(SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED, Focused);
            Dispatcher.RegisterWindowEventHandler(SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST, Unfocused);
            Dispatcher.RegisterWindowEventHandler(SDL.SDL_WindowEventID.SDL_WINDOWEVENT_EXPOSED, Exposed);
            Dispatcher.RegisterWindowEventHandler(SDL.SDL_WindowEventID.SDL_WINDOWEVENT_MOVED, Moved);
            Dispatcher.RegisterWindowEventHandler(SDL.SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED, SizeChanged);
            Dispatcher.RegisterWindowEventHandler(SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED, Resized);
            Dispatcher.RegisterWindowEventHandler(SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE, Closed);
        }

        private void MouseEntered(OpenGlWindow owner, SDL.SDL_WindowEvent ev)
            => owner.OnMouseEntered();

        private void MouseLeft(OpenGlWindow owner, SDL.SDL_WindowEvent ev)
            => owner.OnMouseLeft();

        private void Focused(OpenGlWindow owner, SDL.SDL_WindowEvent ev)
            => owner.OnFocused();

        private void Unfocused(OpenGlWindow owner, SDL.SDL_WindowEvent ev)
            => owner.OnUnfocused();

        private void Exposed(OpenGlWindow owner, SDL.SDL_WindowEvent ev)
            => owner.OnExposed();

        private void Moved(OpenGlWindow owner, SDL.SDL_WindowEvent ev)
            => owner.OnMoved(new EventArgs.WindowMoveEventArgs(new Vector2(ev.data1, ev.data2)));

        private void SizeChanged(OpenGlWindow owner, SDL.SDL_WindowEvent ev)
            => owner.OnSizeChanged(new EventArgs.WindowResizeEventArgs(new Size(ev.data1, ev.data2)));

        private void Resized(OpenGlWindow owner, SDL.SDL_WindowEvent ev)
            => owner.OnResized(new EventArgs.WindowResizeEventArgs(new Size(ev.data1, ev.data2)));

        private void Closed(OpenGlWindow owner, SDL.SDL_WindowEvent ev)
            => owner.OnClosed();
    }
}
