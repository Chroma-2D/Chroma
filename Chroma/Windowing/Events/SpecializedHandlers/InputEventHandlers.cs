using Chroma.Input;
using Chroma.Input.EventArgs;
using Chroma.SDL2;

namespace Chroma.Windowing.Events.SpecializedHandlers
{
    internal class InputEventHandlers
    {
        private EventDispatcher Dispatcher { get; }

        internal InputEventHandlers(EventDispatcher dispatcher)
        {
            Dispatcher = dispatcher;

            Dispatcher.RegisterGenericEventHandler(SDL.SDL_EventType.SDL_MOUSEMOTION, MouseMoved);
            Dispatcher.RegisterGenericEventHandler(SDL.SDL_EventType.SDL_MOUSEWHEEL, WheelMoved);
            Dispatcher.RegisterGenericEventHandler(SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN, MousePressed);
            Dispatcher.RegisterGenericEventHandler(SDL.SDL_EventType.SDL_MOUSEBUTTONUP, MouseReleased);
            Dispatcher.RegisterGenericEventHandler(SDL.SDL_EventType.SDL_KEYUP, KeyUp);
            Dispatcher.RegisterGenericEventHandler(SDL.SDL_EventType.SDL_KEYDOWN, KeyDown);
        }

        private void KeyUp(OpenGlWindow owner, SDL.SDL_Event ev)
        {
            Keyboard.OnKeyUp(new KeyEventArgs((Scancode)ev.key.keysym.scancode, false, ev.key.repeat != 0));
        }

        private void KeyDown(OpenGlWindow owner, SDL.SDL_Event ev)
        {
            Keyboard.OnKeyDown(new KeyEventArgs((Scancode)ev.key.keysym.scancode, true, ev.key.repeat != 0));
        }

        private void MouseMoved(OpenGlWindow owner, SDL.SDL_Event ev)
        {
            if (ev.motion.which == SDL.SDL_TOUCH_MOUSEID)
                return;

            Mouse.OnMoved(
                new MouseMoveEventArgs(
                    new Vector2(ev.motion.x, ev.motion.y),
                    new Vector2(ev.motion.xrel, ev.motion.yrel),
                    new MouseButtonState(ev.motion.state)
                )
            );
        }

        private void WheelMoved(OpenGlWindow owner, SDL.SDL_Event ev)
        {
            if (ev.wheel.which == SDL.SDL_TOUCH_MOUSEID)
                return;

            Mouse.OnWheelMoved(
                new MouseWheelEventArgs(
                    new Vector2(ev.wheel.x, ev.wheel.y),
                    ev.wheel.direction
                )
            );
        }

        private void MousePressed(OpenGlWindow owner, SDL.SDL_Event ev)
        {
            if (ev.button.which == SDL.SDL_TOUCH_MOUSEID)
                return;

            Mouse.OnPressed(
                new MouseButtonEventArgs(
                    new Vector2(ev.button.x, ev.button.y),
                    ev.button.state,
                    ev.button.button,
                    ev.button.clicks
                )
            );
        }

        private void MouseReleased(OpenGlWindow owner, SDL.SDL_Event ev)
        {
            if (ev.button.which == SDL.SDL_TOUCH_MOUSEID)
                return;

            Mouse.OnReleased(
                new MouseButtonEventArgs(
                    new Vector2(ev.button.x, ev.button.y),
                    ev.button.state,
                    ev.button.button,
                    ev.button.clicks
                )
            );
        }
    }
}
