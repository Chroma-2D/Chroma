using Chroma.Input;
using Chroma.SDL2;
using Chroma.Windowing.EventArgs;

namespace Chroma.Windowing.Events.SpecializedHandlers
{
    internal class FrameworkEventHandlers
    {
        private EventDispatcher Dispatcher { get; }

        internal FrameworkEventHandlers(EventDispatcher dispatcher)
        {
            Dispatcher = dispatcher;

            Dispatcher.RegisterGenericEventHandler(SDL.SDL_EventType.SDL_QUIT, QuitRequested);
        }

        private void QuitRequested(OpenGlWindow owner, SDL.SDL_Event ev)
            => owner.OnQuitRequested(new CancelEventArgs());
    }
}
