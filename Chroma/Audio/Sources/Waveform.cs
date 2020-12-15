using System;
using Chroma.Natives.SDL;

namespace Chroma.Audio.Sources
{
    public class Waveform : AudioSource
    {
        private readonly AudioStreamDelegate _chunkGenerator;
        private SDL2_nmix.NMIX_SourceCallback _internalCallback; // Needs to be a class field to avoid GC collection.
        
        private AudioFormat Format { get; }
        public ChannelMode ChannelMode { get; }
        public int Frequency { get; }
        
        public Waveform(AudioFormat format, AudioStreamDelegate chunkGenerator, ChannelMode channelMode = ChannelMode.Stereo, int frequency = 44100)
        {
            _internalCallback = AudioCallback;
            _chunkGenerator = chunkGenerator;
            
            Format = format;
            ChannelMode = channelMode;
            Frequency = frequency;
            
            Handle = SDL2_nmix.NMIX_NewSource(
                Format.SdlFormat,
                (byte)ChannelMode,
                Frequency,
                _internalCallback,
                IntPtr.Zero
            );
        }
        
        private void AudioCallback(IntPtr userData, IntPtr samples, int bufferSize)
        {
            unsafe
            {
                var span = new Span<byte>(
                    samples.ToPointer(), 
                    bufferSize
                );
                
                _chunkGenerator.Invoke(span, Format);
            }
        }
    }
}