using System;

namespace Chroma.Audio
{
    public class AudioDeviceEventArgs : EventArgs
    {
        public AudioDevice Device { get; }

        internal AudioDeviceEventArgs(AudioDevice device)
        {
            Device = device;
        }
    }
}