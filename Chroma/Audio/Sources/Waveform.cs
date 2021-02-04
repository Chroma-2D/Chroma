using System;
using Chroma.Natives.SDL;

namespace Chroma.Audio.Sources
{
    public class Waveform : AudioSource
    {
        private SDL2_nmix.NMIX_SourceCallback _internalCallback; // Needs to be a class field to avoid GC collection.
        
        protected AudioStreamDelegate SampleGenerator { get; set; }
        
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
        }
        
        private void AudioCallback(IntPtr userData, IntPtr samples, int bufferSize)
        {
            unsafe
            {
                var span = new Span<byte>(
                    samples.ToPointer(), 
                    bufferSize
                );
                
                SampleGenerator.Invoke(span, Format);
            }
        }
    }
}