using Chroma.SDL2;
using Chroma.Windowing.EventArgs;

namespace Chroma.Windowing.EventHandling.Specialized
{
    internal class FrameworkEventHandlers
    {
        private EventDispatcher Dispatcher { get; }

        internal FrameworkEventHandlers(EventDispatcher dispatcher)
        {
            Dispatcher = dispatcher;

            Dispatcher.RegisterEventHandler(SDL.SDL_EventType.SDL_QUIT, QuitRequested);
        }

        private void QuitRequested(Window owner, SDL.SDL_Event ev)
            => owner.OnQuitRequested(new CancelEventArgs());
    }
}
