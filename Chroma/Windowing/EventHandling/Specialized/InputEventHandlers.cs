using Chroma.Input;
using Chroma.Input.EventArgs;
using Chroma.SDL2;
using System;
using System.Runtime.InteropServices;

namespace Chroma.Windowing.EventHandling.Specialized
{
    internal class InputEventHandlers
    {
        private EventDispatcher Dispatcher { get; }

        internal InputEventHandlers(EventDispatcher dispatcher)
        {
            Dispatcher = dispatcher;

            Dispatcher.RegisterEventHandler(SDL.SDL_EventType.SDL_CONTROLLERDEVICEADDED, ControllerConnected);
            Dispatcher.RegisterEventHandler(SDL.SDL_EventType.SDL_CONTROLLERDEVICEREMOVED, ControllerDisconnected);
            Dispatcher.RegisterEventHandler(SDL.SDL_EventType.SDL_CONTROLLERAXISMOTION, ControllerAxisMotion);

            Dispatcher.RegisterEventHandler(SDL.SDL_EventType.SDL_KEYUP, KeyReleased);
            Dispatcher.RegisterEventHandler(SDL.SDL_EventType.SDL_KEYDOWN, KeyPressed);
            Dispatcher.RegisterEventHandler(SDL.SDL_EventType.SDL_TEXTINPUT, TextInput);
            Dispatcher.RegisterEventHandler(SDL.SDL_EventType.SDL_KEYMAPCHANGED, KeyMapChanged);
            Dispatcher.RegisterEventHandler(SDL.SDL_EventType.SDL_TEXTEDITING, TextEdition);

            Dispatcher.RegisterEventHandler(SDL.SDL_EventType.SDL_MOUSEMOTION, MouseMoved);
            Dispatcher.RegisterEventHandler(SDL.SDL_EventType.SDL_MOUSEWHEEL, WheelMoved);
            Dispatcher.RegisterEventHandler(SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN, MousePressed);
            Dispatcher.RegisterEventHandler(SDL.SDL_EventType.SDL_MOUSEBUTTONUP, MouseReleased);
        }

        private void ControllerConnected(Window owner, SDL.SDL_Event ev)
        {
            Controller.SetDeadZone(ev.cdevice.which, 3500);
            SDL.SDL_GameControllerOpen(ev.cdevice.which);
        }

        private void ControllerDisconnected(Window owner, SDL.SDL_Event ev)
        {
            SDL.SDL_GameControllerClose(ev.user.data2);
        }

        private void ControllerAxisMotion(Window owner, SDL.SDL_Event ev)
        {
        }

        private void KeyReleased(Window owner, SDL.SDL_Event ev)
        {
            Keyboard.OnKeyReleased(
                owner.Game,
                new KeyEventArgs(
                    (ScanCode)ev.key.keysym.scancode,
                    (KeyCode)ev.key.keysym.sym,
                    (KeyModifiers)ev.key.keysym.mod,
                    ev.key.repeat != 0
                )
            );
        }

        private void KeyPressed(Window owner, SDL.SDL_Event ev)
        {
            Keyboard.OnKeyPressed(
                owner.Game,
                new KeyEventArgs(
                    (ScanCode)ev.key.keysym.scancode,
                    (KeyCode)ev.key.keysym.sym,
                    (KeyModifiers)ev.key.keysym.mod,
                    ev.key.repeat != 0
                )
            );
        }

        private void TextEdition(Window owner, SDL.SDL_Event ev)
        {
            // TODO someday?
            // Not handled for no: no practical use found.
        }

        private void KeyMapChanged(Window owner, SDL.SDL_Event ev)
        {
            // TODO someday?
            // Not handled for now: no practical use found.
        }

        private void TextInput(Window owner, SDL.SDL_Event ev)
        {
            string textInput;
            unsafe
            {
                textInput = Marshal.PtrToStringUTF8(
                    new IntPtr(ev.text.text)
                );
            }

            owner.Game.OnTextInput(new TextInputEventArgs(textInput));
        }

        private void MouseMoved(Window owner, SDL.SDL_Event ev)
        {
            if (ev.motion.which == SDL.SDL_TOUCH_MOUSEID)
                return;

            owner.Game.OnMouseMoved(
                new MouseMoveEventArgs(
                    new Vector2(ev.motion.x, ev.motion.y),
                    new Vector2(ev.motion.xrel, ev.motion.yrel),
                    new MouseButtonState(ev.motion.state)
                )
            );
        }

        private void WheelMoved(Window owner, SDL.SDL_Event ev)
        {
            if (ev.wheel.which == SDL.SDL_TOUCH_MOUSEID)
                return;

            owner.Game.OnWheelMoved(
                new MouseWheelEventArgs(
                    new Vector2(ev.wheel.x, ev.wheel.y),
                    ev.wheel.direction
                )
            );
        }

        private void MousePressed(Window owner, SDL.SDL_Event ev)
        {
            if (ev.button.which == SDL.SDL_TOUCH_MOUSEID)
                return;

            owner.Game.OnMousePressed(
                new MouseButtonEventArgs(
                    new Vector2(
                        ev.button.x,
                        ev.button.y
                    ),
                    ev.button.state,
                    ev.button.button,
                    ev.button.clicks
                )
            );
        }

        private void MouseReleased(Window owner, SDL.SDL_Event ev)
        {
            if (ev.button.which == SDL.SDL_TOUCH_MOUSEID)
                return;

            owner.Game.OnMouseReleased(
                new MouseButtonEventArgs(
                    new Vector2(
                        ev.button.x,
                        ev.button.y
                    ),
                    ev.button.state,
                    ev.button.button,
                    ev.button.clicks
                )
            );
        }
    }
}
