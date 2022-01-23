using System.Drawing;
using System.Numerics;
using Chroma.Natives.SDL;

namespace Chroma.Windowing.EventHandling.Specialized
{
    internal class WindowEventHandlers
    {
        private EventDispatcher Dispatcher { get; }

        internal WindowEventHandlers(EventDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            
            Dispatcher.Discard(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_HIT_TEST);
            Dispatcher.Discard(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_ICCPROF_CHANGED);

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
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_TAKE_FOCUS, FocusOffered);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED, Focused);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST, Unfocused);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE, Closed);
            Dispatcher.RegisterWindowEventHandler(SDL2.SDL_WindowEventID.SDL_WINDOWEVENT_DISPLAY_CHANGED, DisplayChanged);

            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_DROPBEGIN, DropStarted);
            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_DROPCOMPLETE, DropFinished);
            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_DROPFILE, FileDropped);
            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_DROPTEXT, TextDropped);
        }

        private void Shown(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnShown();
        
        private void Hidden(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnHidden();
        
        private void Invalidated(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnInvalidated();
        
        private void Moved(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnMoved(new WindowMoveEventArgs(new Vector2(ev.data1, ev.data2)));
        
        private void Resized(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnResized(new WindowSizeEventArgs(new Size(ev.data1, ev.data2)));

        private void SizeChanged(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnSizeChanged(new WindowSizeEventArgs(new Size(ev.data1, ev.data2)));
        
        private void Minimized(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnStateChanged(new WindowStateEventArgs(WindowState.Minimized));

        private void Maximized(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnStateChanged(new WindowStateEventArgs(WindowState.Maximized));

        private void Restored(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnStateChanged(new WindowStateEventArgs(WindowState.Normal));
        
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

        private void Closed(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnClosed();

        private void DisplayChanged(Window owner, SDL2.SDL_WindowEvent ev)
            => owner.OnDisplayChanged(new DisplayChangedEventArgs(ev.data1));

        private void DropStarted(Window owner, SDL2.SDL_Event ev)
            => owner.DragDropManager.BeginDrop();

        private void DropFinished(Window owner, SDL2.SDL_Event ev)
            => owner.DragDropManager.FinishDrop();

        // ev.drop.file on events below is freed in DragDropManager after buffering the data
        private void FileDropped(Window owner, SDL2.SDL_Event ev)
            => owner.DragDropManager.OnFileDropped(ev.drop.file);

        private void TextDropped(Window owner, SDL2.SDL_Event ev)
            => owner.DragDropManager.OnTextDropped(ev.drop.file);
    }
}