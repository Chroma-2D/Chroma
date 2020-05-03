using Chroma.MemoryManagement;
using Chroma.Natives.SDL;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Chroma.Audio
{
    public class AudioManager
    {
        public int SamplingRate { get; } = 44100; // hz

        private int _mixingChannelCount;
        public int MixingChannelCount
        {
            get => _mixingChannelCount;
            set
            {
                _mixingChannelCount = value;
                SDL_mixer.Mix_AllocateChannels(_mixingChannelCount);
            }
        }
        
        public int ChunkSize { get; } = 8192; // bytes

        public int PlayingChannelCount => SDL_mixer.Mix_Playing(-1);
        public int PausedChannelCount => SDL_mixer.Mix_Paused(-1);
        
        internal AudioManager()
        {
            var result = SDL_mixer.Mix_OpenAudio(
                SamplingRate,
                AudioFormat.Default.SdlMixerFormat,
                SDL_mixer.MIX_DEFAULT_CHANNELS,
                ChunkSize
            );

            if (result == -1)
            {
                Console.WriteLine($"SDL_mixer: {SDL2.SDL_GetError()}");
            }
            else
            {
                MixingChannelCount = 16;
            }
        }
        
        public Sound CreateSound(string filePath)
        {
            var handle = SDL_mixer.Mix_LoadWAV(filePath);

            if (handle != IntPtr.Zero)
            {
                return new Sound(handle, this);
            }

            Console.WriteLine(SDL2.SDL_GetError());
            // TODO: throw an instance of audioexception here
            return null;
        }
    }
}
