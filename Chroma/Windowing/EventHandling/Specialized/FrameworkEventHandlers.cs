using Chroma.Natives.SDL;

namespace Chroma.Windowing.EventHandling.Specialized
{
    internal class FrameworkEventHandlers
    {
        private EventDispatcher Dispatcher { get; }

        internal FrameworkEventHandlers(EventDispatcher dispatcher)
        {
            Dispatcher = dispatcher;

            Dispatcher.Discard(
                SDL2.SDL_EventType.SDL_CLIPBOARDUPDATE
            );
            
            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_QUIT, QuitRequested);
        }

        private void QuitRequested(Window owner, SDL2.SDL_Event ev)
            => owner.OnQuitRequested(new CancelEventArgs());
    }
}