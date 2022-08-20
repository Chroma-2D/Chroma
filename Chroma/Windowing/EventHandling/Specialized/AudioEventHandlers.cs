using Chroma.Natives.Bindings.SDL;

namespace Chroma.Windowing.EventHandling.Specialized
{
    internal class AudioEventHandlers
    {
        private EventDispatcher Dispatcher { get; }

        internal AudioEventHandlers(EventDispatcher dispatcher)
        {
            Dispatcher = dispatcher;

            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_AUDIODEVICEADDED, AudioDeviceAdded);
            Dispatcher.RegisterEventHandler(SDL2.SDL_EventType.SDL_AUDIODEVICEREMOVED, AudioDeviceRemoved);
        }

        private void AudioDeviceAdded(Window owner, SDL2.SDL_Event ev)
            => owner.Game.Audio.OnDeviceAdded(ev.adevice.which, ev.adevice.iscapture != 0);

        private void AudioDeviceRemoved(Window owner, SDL2.SDL_Event ev)
            => owner.Game.Audio.OnDeviceRemoved(ev.adevice.which, ev.adevice.iscapture != 0);
    }
}