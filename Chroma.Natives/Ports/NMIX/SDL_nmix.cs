using System;
using System.Runtime.InteropServices;
using static Chroma.Natives.Bindings.SDL.SDL2;

namespace Chroma.Natives.Ports.NMIX
{
    internal static partial class SDL2_nmix
    {
        private static SDL_AudioSpec mixer;
        private static uint audio_device;

        private static unsafe NMIX_Source* playing_sources = null;
        private static float master_gain = 1f;

        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct NMIX_Source
        {
            internal int rate;
            internal ushort format; // SDL_AudioFormat
            internal byte channels;
            internal float pan;
            internal float gain;
            internal IntPtr callback; // NMIX_SourceCallback*
            internal void* userdata; // void*
            internal bool eof; // SDL_bool
            internal void* in_buffer; // void*
            internal int in_buffer_size;
            internal IntPtr stream; // SDL_AudioStream*
            internal void* out_buffer; // void*
            internal int out_buffer_size;
            internal NMIX_Source* prev;
            internal NMIX_Source* next;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void NMIX_SourceCallback(IntPtr userdata, IntPtr stream, int stream_size);

        private static ushort SDL_AUDIO_SAMPLELEN(ushort x)
            => (ushort)((x & SDL_AUDIO_MASK_BITSIZE) / 8);

        private static float clampf(float x, float min, float max)
            => Math.Clamp(x, min, max);

        private static float mix_samples(float a, float b)
            => clampf(a + b, -1, 1);

        private static void apply_panning(float pan, ref float left, ref float right)
        {
            var amp = pan / 2 + 0.5f;
            left *= 1 - amp;
            right *= amp;
        }

        private static unsafe void nmix_callback(void* userdata, byte* _buffer, int buffer_size)
        {
            SDL_memset(_buffer, 0, new IntPtr(buffer_size));
            ushort sample_size = SDL_AUDIO_SAMPLELEN(mixer.format);

            if (playing_sources == null)
                return;

            NMIX_Source* s = playing_sources;
            while (s != null)
            {
                NMIX_Source* next = s->next;

                int bytes_written = 0;
                while (bytes_written < buffer_size)
                {
                    int copy_size = SDL_AudioStreamAvailable(s->stream);

                    if (copy_size > s->out_buffer_size)
                        copy_size = s->out_buffer_size;

                    if (bytes_written + copy_size > buffer_size)
                        copy_size = buffer_size - bytes_written;

                    int bytes_read = SDL_AudioStreamGet(s->stream, s->out_buffer, copy_size);

                    float* buffer = (float*)(_buffer + bytes_written);
                    float* out_buffer = (float*)s->out_buffer;

                    for (int i = 0; i < bytes_read / sample_size; i += 2)
                    {
                        float sl = out_buffer[i] * s->gain * master_gain;
                        float sr = out_buffer[i + 1] * s->gain * master_gain;
                        apply_panning(s->pan, ref sl, ref sr);

                        buffer[i] = mix_samples(buffer[i], sl);
                        buffer[i + 1] = mix_samples(buffer[i + 1], sr);
                    }

                    bytes_written += bytes_read;

                    if (SDL_AudioStreamAvailable(s->stream) == 0)
                    {
                        if (!s->eof)
                        {
                            var del = Marshal.GetDelegateForFunctionPointer<NMIX_SourceCallback>(s->callback);
                            del((IntPtr)s->userdata, (IntPtr)s->in_buffer, s->in_buffer_size);

                            if (SDL_AudioStreamPut(s->stream, s->in_buffer, s->in_buffer_size) != 0)
                            {
                                Console.Error.WriteLine($"SDL_nmix: FATAL: {SDL_GetError()}");
                                return;
                            }
                        }
                        else
                        {
                            NMIX_Pause((IntPtr)s);
                            bytes_written = buffer_size;
                        }
                    }
                }

                s = next;
            }
        }

        internal static int NMIX_OpenAudio(string device, int rate, ushort samples)
        {
            if (audio_device != 0)
            {
                SDL_SetError("NMIX device is already opened.");
                return -1;
            }

            SDL_GetVersion(out var linked);
            if (linked.major < 2 || linked.minor < 7)
            {
                SDL_SetError("SDL_nmix requires SDL 2.0.7 or later.");
                return -1;
            }

            if (SDL_WasInit(SDL_INIT_AUDIO) == 0)
            {
                if (SDL_InitSubSystem(SDL_INIT_AUDIO) < 0)
                    return -1;
            }

            unsafe
            {
                SDL_AudioSpec wanted_spec = new()
                {
                    freq = rate,
                    format = AUDIO_F32SYS,
                    channels = 2,
                    samples = samples,
                    callback = nmix_callback,
                    userdata = IntPtr.Zero
                };

                audio_device = SDL_OpenAudioDevice(
                    device,
                    0,
                    ref wanted_spec,
                    out mixer,
                    (int)SDL_AUDIO_ALLOW_FREQUENCY_CHANGE
                );

                if (audio_device == 0)
                    return -1;
            }

            NMIX_PausePlayback(false);
            return 0;
        }

        internal static int NMIX_CloseAudio()
        {
            if (audio_device == 0)
            {
                SDL_SetError("NMIX device is already closed.");
                return -1;
            }

            SDL_PauseAudioDevice(audio_device, 1);
            SDL_CloseAudioDevice(audio_device);
            audio_device = 0;

            return 0;
        }

        internal static void NMIX_PausePlayback(bool pause_on)
            => SDL_PauseAudioDevice(audio_device, pause_on ? 1 : 0);

        internal static float NMIX_GetMasterGain()
            => master_gain;

        internal static void NMIX_SetMasterGain(float gain)
            => master_gain = clampf(gain, 0, 2);

        internal static ref SDL_AudioSpec NMIX_GetAudioSpec()
            => ref mixer;

        internal static uint NMIX_GetAudioDevice()
            => audio_device;

        internal static IntPtr NMIX_NewSource(ushort format, byte channels, int rate, NMIX_SourceCallback callback, IntPtr userdata)
        {
            if (audio_device == 0)
            {
                SDL_SetError("Please open NMIX device before creating sources.");
                return IntPtr.Zero;
            }

            unsafe
            {
                NMIX_Source* source = (NMIX_Source*)SDL_malloc(new IntPtr(sizeof(NMIX_Source)));
                if (source == null)
                {
                    SDL_OutOfMemory();
                    return IntPtr.Zero;
                }

                source->format = format;
                source->channels = channels;
                source->rate = rate;
                source->pan = 0;
                source->gain = 1;
                source->callback = Marshal.GetFunctionPointerForDelegate(callback);
                source->userdata = (void*)userdata;
                source->eof = false;

                int in_nb_samples = source->rate * mixer.samples / mixer.freq;
                source->in_buffer_size = in_nb_samples * source->channels * SDL_AUDIO_SAMPLELEN(source->format);
                source->in_buffer = (void*)SDL_malloc(new IntPtr(source->in_buffer_size));
                if (source->in_buffer == null)
                {
                    SDL_OutOfMemory();
                    return IntPtr.Zero;
                }

                source->out_buffer_size = (int)mixer.size;
                source->out_buffer = (void*)SDL_malloc(new IntPtr(source->out_buffer_size));
                if (source->out_buffer == null)
                {
                    SDL_OutOfMemory();
                    return IntPtr.Zero;
                }

                source->stream = SDL_NewAudioStream(
                    source->format,
                    source->channels,
                    source->rate,
                    mixer.format,
                    mixer.channels,
                    mixer.freq
                );

                if (source->stream == IntPtr.Zero)
                {
                    SDL_OutOfMemory();
                    return IntPtr.Zero;
                }

                source->prev = null;
                source->next = null;

                return (IntPtr)source;
            }
        }

        internal static void NMIX_FreeSource(IntPtr src)
        {
            if (src == IntPtr.Zero)
                return;

            unsafe
            {
                var source = (NMIX_Source*)src;

                SDL_LockAudioDevice(audio_device);

                if (NMIX_IsPlaying(src))
                    NMIX_Pause(src);

                SDL_free((IntPtr)source->in_buffer);
                SDL_free((IntPtr)source->out_buffer);
                SDL_FreeAudioStream(source->stream);
                SDL_free(src);
                SDL_UnlockAudioDevice(audio_device);
            }
        }

        internal static int NMIX_Play(IntPtr src)
        {
            if (src == IntPtr.Zero)
                return -1;

            SDL_LockAudioDevice(audio_device);

            if (NMIX_IsPlaying(src))
            {
                SDL_UnlockAudioDevice(audio_device);
                SDL_SetError("NMIX source is already playing.");
                return -1;
            }

            unsafe
            {
                var source = (NMIX_Source*)src;

                source->eof = false;

                if (playing_sources == null)
                {
                    playing_sources = source;
                }
                else
                {
                    NMIX_Source* v = playing_sources;
                    while (v->next != null)
                        v = v->next;

                    v->next = source;
                    source->prev = v;
                }
            }
            
            SDL_UnlockAudioDevice(audio_device);
            return 0;
        }

        internal static void NMIX_Pause(IntPtr src)
        {
            if (src == IntPtr.Zero)
                return;

            SDL_LockAudioDevice(audio_device);

            if (!NMIX_IsPlaying(src))
            {
                SDL_UnlockAudioDevice(audio_device);
                return;
            }

            unsafe
            {
                var source = (NMIX_Source*)src;

                if (source->next != null)
                {
                    source->next->prev = source->prev;
                }

                if (source->prev != null)
                {
                    source->prev->next = source->next;
                }
                else
                {
                    playing_sources = source->next;
                }

                source->prev = null;
                source->next = null;

                SDL_UnlockAudioDevice(audio_device);
            }
        }

        internal static bool NMIX_IsPlaying(IntPtr src)
        {
            if (src == IntPtr.Zero)
                return false;

            unsafe
            {
                var source = (NMIX_Source*)src;
                return source == playing_sources || source->prev != null;
            }
        }

        internal static float NMIX_GetPan(IntPtr src)
        {
            if (src == IntPtr.Zero)
                return 0;

            unsafe
            {
                return ((NMIX_Source*)src)->pan;
            }
        }

        internal static void NMIX_SetPan(IntPtr src, float pan)
        {
            if (src == IntPtr.Zero)
                return;

            unsafe
            {
                ((NMIX_Source*)src)->pan = clampf(pan, -1, 1);
            }
        }
        
        internal static float NMIX_GetGain(IntPtr src)
        {
            if (src == IntPtr.Zero)
                return 0;

            unsafe
            {
                return ((NMIX_Source*)src)->gain;
            }
        }
        
        internal static void NMIX_SetGain(IntPtr src, float gain)
        {
            if (src == IntPtr.Zero)
                return;

            unsafe
            {
                ((NMIX_Source*)src)->gain = clampf(gain, 0, 2);
            }
        }
    }
}