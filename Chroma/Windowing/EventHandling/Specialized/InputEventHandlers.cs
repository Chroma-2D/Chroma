using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Chroma.Hardware;
using Chroma.Input;
using Chroma.Input.EventArgs;
using Chroma.Input.Internal;
using Chroma.Natives.SDL;

namespace Chroma.Windowing.EventHandling.Specialized
{
    internal class InputEventHandlers
    {
        private EventDispatcher Dispatcher { get; }

        internal InputEventHandlers(EventDispatcher dispatcher)
        {
            Dispatcher = dispatcher;

            Dispatcher.Discard(
                new[]
                {
                    SDL2.SDL_EventType.SDL_JOYAXISMOTION,
                    SDL2.SDL_EventType.SDL_JOYDEVICEADDED,
                    SDL2.SDL_EventType.SDL_JOYDEVICEREMOVED,
                    SDL2.SDL_EventType.SDL_JOYBUTTONUP,
                    SDL2.SDL_EventType.SDL_JOYBUTTONDOWN,
                    SDL2.SDL_EventType.SDL_JOYHATMOTION,
                    SDL2.SDL_EventType.SDL_JOYBALLMOTION,
                    SDL2.SDL_EventType.SDL_KEYMAPCHANGED,
                    SDL2.SDL_EventType.SDL_TEXTEDITING
                }
            );

            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_CONTROLLERDEVICEADDED, ControllerConnected);
            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_CONTROLLERDEVICEREMOVED, ControllerDisconnected);
            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_CONTROLLERBUTTONDOWN, ControllerButtonPressed);
            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_CONTROLLERBUTTONUP, ControllerButtonReleased);
            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_CONTROLLERAXISMOTION, ControllerAxisMoved);

            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_KEYUP, KeyReleased);
            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_KEYDOWN, KeyPressed);
            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_TEXTINPUT, TextInput);

            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_MOUSEMOTION, MouseMoved);
            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_MOUSEWHEEL, WheelMoved);
            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_MOUSEBUTTONDOWN, MousePressed);
            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_MOUSEBUTTONUP, MouseReleased);
        }

        private void ControllerAxisMoved(Window owner, SDL2.SDL_Event ev)
        {
            var instance = SDL2.SDL_GameControllerFromInstanceID(ev.caxis.which);
            var controller = ControllerRegistry.Instance.GetControllerInfoByPointer(instance);

            var axis = (ControllerAxis)ev.caxis.axis;

            owner.Game.OnControllerAxisMoved(
                new ControllerAxisEventArgs(
                    controller,
                    axis,
                    ev.caxis.axisValue
                )
            );
        }

        private void ControllerButtonPressed(Window owner, SDL2.SDL_Event ev)
        {
            var instance = SDL2.SDL_GameControllerFromInstanceID(ev.cbutton.which);
            var controller = ControllerRegistry.Instance.GetControllerInfoByPointer(instance);

            var button = (ControllerButton)ev.cbutton.button;

            owner.Game.OnControllerButtonPressed(
                new ControllerButtonEventArgs(
                    controller,
                    button
                )
            );
        }

        private void ControllerButtonReleased(Window owner, SDL2.SDL_Event ev)
        {
            var instance = SDL2.SDL_GameControllerFromInstanceID(ev.cbutton.which);
            var controller = ControllerRegistry.Instance.GetControllerInfoByPointer(instance);

            var button = (ControllerButton)ev.cbutton.button;

            owner.Game.OnControllerButtonReleased(
                new ControllerButtonEventArgs(
                    controller,
                    button
                )
            );
        }

        private void ControllerConnected(Window owner, SDL2.SDL_Event ev)
        {
            var instance = SDL2.SDL_GameControllerOpen(ev.cdevice.which);
            var joyInstance = SDL2.SDL_GameControllerGetJoystick(instance);
            var instanceId = SDL2.SDL_JoystickInstanceID(joyInstance);

            var playerIndex = ControllerRegistry.Instance.GetFirstFreePlayerSlot();
            SDL2.SDL_GameControllerSetPlayerIndex(instance, playerIndex);

            var name = SDL2.SDL_GameControllerName(instance);
            var productInfo = new ProductInfo(
                SDL2.SDL_GameControllerGetVendor(instance),
                SDL2.SDL_GameControllerGetProduct(instance)
            );

            var controllerInfo = new ControllerInfo(instance, instanceId, playerIndex, name, productInfo);
            ControllerRegistry.Instance.Register(instance, controllerInfo);

            owner.Game.OnControllerConnected(
                new ControllerEventArgs(controllerInfo)
            );
        }

        private void ControllerDisconnected(Window owner, SDL2.SDL_Event ev)
        {
            var instance = SDL2.SDL_GameControllerFromInstanceID(ev.cdevice.which);
            var controllerInfo = ControllerRegistry.Instance.GetControllerInfoByPointer(instance);

            ControllerRegistry.Instance.Unregister(instance);

            owner.Game.OnControllerDisconnected(
                new ControllerEventArgs(controllerInfo)
            );
        }

        private void KeyReleased(Window owner, SDL2.SDL_Event ev)
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

        private void KeyPressed(Window owner, SDL2.SDL_Event ev)
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

        private void TextInput(Window owner, SDL2.SDL_Event ev)
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

        private void MouseMoved(Window owner, SDL2.SDL_Event ev)
        {
            if (ev.motion.which == SDL2.SDL_TOUCH_MOUSEID)
                return;

            owner.Game.OnMouseMoved(
                new MouseMoveEventArgs(
                    new Vector2(ev.motion.x, ev.motion.y),
                    new Vector2(ev.motion.xrel, ev.motion.yrel),
                    new MouseButtonState(ev.motion.state)
                )
            );
        }

        private void WheelMoved(Window owner, SDL2.SDL_Event ev)
        {
            if (ev.wheel.which == SDL2.SDL_TOUCH_MOUSEID)
                return;

            owner.Game.OnWheelMoved(
                new MouseWheelEventArgs(
                    new Vector2(ev.wheel.x, ev.wheel.y),
                    ev.wheel.direction
                )
            );
        }

        private void MousePressed(Window owner, SDL2.SDL_Event ev)
        {
            if (ev.button.which == SDL2.SDL_TOUCH_MOUSEID)
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

        private void MouseReleased(Window owner, SDL2.SDL_Event ev)
        {
            if (ev.button.which == SDL2.SDL_TOUCH_MOUSEID)
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