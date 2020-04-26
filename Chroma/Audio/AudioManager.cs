using Chroma.MemoryManagement;
using Chroma.Natives.SDL;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Chroma.Audio
{
    public class AudioManager : DisposableResource
    {
        internal Stack<int> FreeMixingChannels { get; }
        internal SDL_mixer.ChannelFinishedDelegate ChannelFinished { get; }

        public int SamplingRate { get; } = 44100; // hz
        public int MixingChannelCount { get; } = 16;
        public int ChunkSize { get; } = 8192; // bytes

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
                SDL_mixer.Mix_AllocateChannels(MixingChannelCount);

                FreeMixingChannels = new Stack<int>();
                for (var i = 0; i < MixingChannelCount; i++)
                    FreeMixingChannels.Push(i);

                ChannelFinished = new SDL_mixer.ChannelFinishedDelegate(ChannelFinishedCallback);
                SDL_mixer.Mix_ChannelFinished(ChannelFinished);
            }
        }

        public AudioClip CreateClip(string filePath)
        {
            var handle = SDL_mixer.Mix_LoadWAV(filePath);

            if (handle != IntPtr.Zero)
                return new AudioClip(handle, this);

            Console.WriteLine(SDL2.SDL_GetError());
            // FIXME: throw an instance of audioexception here
            return null;
        }

        internal int GetFreeChannel()
        {
            if (FreeMixingChannels.Count == 0)
                return -1;

            return FreeMixingChannels.Pop();
        }

        private void ChannelFinishedCallback(int channel)
            => FreeMixingChannels.Push(channel);
    }
}
