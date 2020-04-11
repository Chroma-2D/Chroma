using System.Numerics;
using Chroma.Natives.SDL;
using Chroma.Windowing.EventArgs;

namespace Chroma.Windowing.EventHandling.Specialized
{
    internal class WindowEventHandlers
    {
        private EventDispatcher Dispatcher { get; }

        internal WindowEventHandlers(EventDispatcher dispatcher)
        {
            Dispatcher = dispatcher;

            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_SHOWN, Shown);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_HIDDEN, Hidden);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_EXPOSED, Invalidated);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_MOVED, Moved);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED, Resized);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED, SizeChanged);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_MINIMIZED, Minimized);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_MAXIMIZED, Maximized);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_RESTORED, Restored);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_ENTER, MouseEntered);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE, MouseLeft);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED, Focused);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST, Unfocused);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE, Closed);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_TAKE_FOCUS, FocusOffered);
        }

        private void Minimized(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnStateChanged(new WindowStateEventArgs(WindowState.Minimized));

        private void Maximized(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnStateChanged(new WindowStateEventArgs(WindowState.Maximized));

        private void Restored(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnStateChanged(new WindowStateEventArgs(WindowState.Normal));

        private void Closed(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnClosed();

        private void Hidden(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnHidden();

        private void Shown(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnShown();

        private void Invalidated(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnInvalidated();

        private void MouseEntered(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnMouseEntered();

        private void MouseLeft(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnMouseLeft();

        private void FocusOffered(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnFocusOffered();

        private void Focused(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnFocused();

        private void Unfocused(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnUnfocused();

        private void Moved(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnMoved(new WindowMoveEventArgs(new Vector2(ev.data1, ev.data2)));

        private void SizeChanged(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnSizeChanged(new WindowSizeEventArgs(ev.data1, ev.data2));

        private void Resized(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnResized(new WindowSizeEventArgs(ev.data1, ev.data2));
    }
}
