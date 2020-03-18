using Chroma.SDL2;
using System;
using System.Collections.Generic;

namespace Chroma.Windowing.EventHandling
{
    public sealed class EventDispatcher
    {
        private Window Owner { get; }

        internal delegate void SdlEventHandler(Window window, SDL.SDL_Event ev);
        internal delegate void WindowEventHandler(Window window, SDL.SDL_WindowEvent ev);

        internal Dictionary<SDL.SDL_EventType, SdlEventHandler> SdlEventHandlers { get; }
        internal Dictionary<SDL.SDL_WindowEventID, WindowEventHandler> WindowEventHandlers { get; }

        internal EventDispatcher(Window owner)
        {
            Owner = owner;

            SdlEventHandlers = new Dictionary<SDL.SDL_EventType, SdlEventHandler>();
            WindowEventHandlers = new Dictionary<SDL.SDL_WindowEventID, WindowEventHandler>();
        }

        internal void Dispatch(SDL.SDL_Event ev)
        {
            if (ev.type == SDL.SDL_EventType.SDL_WINDOWEVENT)
            {
                DispatchWindowEvent(ev.window);
                return;
            }

            DispatchEvent(ev);
        }

        internal void DispatchWindowEvent(SDL.SDL_WindowEvent ev)
        {
            if (WindowEventHandlers.ContainsKey(ev.windowEvent))
            {
                WindowEventHandlers[ev.windowEvent]?.Invoke(Owner, ev);
            }
            else
            {
                Console.WriteLine($"Unsupported window event: {ev.windowEvent}.");
            }
        }

        internal void DispatchEvent(SDL.SDL_Event ev)
        {
            if (SdlEventHandlers.ContainsKey(ev.type))
            {
                SdlEventHandlers[ev.type]?.Invoke(Owner, ev);
            }
            else
            {
                Console.WriteLine($"Unsupported generic event: {ev.type}.");
            }
        }

        internal void RegisterWindowEventHandler(SDL.SDL_WindowEventID eventId, WindowEventHandler handler)
        {
            if (WindowEventHandlers.ContainsKey(eventId))
            {
                Console.WriteLine($"{eventId} handler is getting redefined.");
                WindowEventHandlers[eventId] = handler;
            }
            else
            {
                WindowEventHandlers.Add(eventId, handler);
            }
        }

        internal void RegisterEventHandler(SDL.SDL_EventType type, SdlEventHandler handler)
        {
            if (SdlEventHandlers.ContainsKey(type))
            {
                Console.WriteLine($"{type} handler is getting redefined.");
                SdlEventHandlers[type] = handler;
            }
            else
            {
                SdlEventHandlers.Add(type, handler);
            }
        }
    }
}
