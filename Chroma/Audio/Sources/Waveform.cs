﻿namespace Chroma.Audio.Sources;

using System;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.Bindings.SDL;
using Chroma.Natives.Ports.NMIX;

public class Waveform : AudioSource
{
    private static readonly Log _log = LogManager.GetForCurrentAssembly();
    private SDL2_nmix.NMIX_SourceCallback? _internalCallback; // Needs to be a class field to avoid GC collection.

    public AudioStreamDelegate SampleGenerator { get; set; }

    public ChannelMode ChannelMode { get; }
    public int Frequency { get; }

    public Waveform(AudioFormat format, AudioStreamDelegate sampleGenerator, ChannelMode channelMode = ChannelMode.Stereo, int frequency = 44100)
    {
        _internalCallback = AudioCallback;
        SampleGenerator = sampleGenerator;

        ChannelMode = channelMode;
        Frequency = frequency;

        Handle = SDL2_nmix.NMIX_NewSource(
            format.SdlFormat,
            (byte)ChannelMode,
            Frequency,
            _internalCallback,
            IntPtr.Zero
        );

        if (Handle == IntPtr.Zero)
        {
            _log.Error($"Failed to create a new audio source: {SDL2.SDL_GetError()}");
            _internalCallback = null;
        }
    }

    private void AudioCallback(IntPtr userData, IntPtr samples, int bufferSize)
    {
        unsafe
        {
            var span = new Span<byte>(
                samples.ToPointer(),
                bufferSize
            );

            SampleGenerator(span, Format);
        }
    }
}