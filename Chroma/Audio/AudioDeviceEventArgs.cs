namespace Chroma.Audio;

public sealed class AudioDeviceEventArgs
{
    public AudioDevice Device { get; }

    internal AudioDeviceEventArgs(AudioDevice device)
    {
        Device = device;
    }
}