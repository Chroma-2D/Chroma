using System;
using System.Runtime.InteropServices;
using static Chroma.Natives.Bindings.SDL.SDL2;
using static Chroma.Natives.Bindings.SDL.SDL2_sound;

namespace Chroma.Natives.Ports.NMIX
{
    internal static partial class SDL2_nmix
    {
        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct NMIX_FileSource
        {
            internal IntPtr rw;
            internal Sound_Sample* sample;
            internal NMIX_Source* source;
            internal bool loop_on;
            internal byte* buffer;
            internal int bytes_left;
            internal bool  predecoded;
        }

        private static unsafe void sdlsound_callback(IntPtr userdata, IntPtr _buffer, int buffer_size)
        {
            NMIX_FileSource* s = (NMIX_FileSource*)userdata;
            byte* buffer = (byte*)_buffer;

            int bytes_written = 0;
            while (bytes_written < buffer_size)
            {
                int copy_size = buffer_size - bytes_written;
                if (copy_size > s->bytes_left)
                    copy_size = s->bytes_left;

                SDL_memcpy((IntPtr)(buffer + bytes_written), (IntPtr)s->buffer, new IntPtr(copy_size));
                s->bytes_left -= copy_size;
                s->buffer += copy_size;
                bytes_written += copy_size;

                if (s->bytes_left == 0)
                {
                    s->bytes_left = (int)s->sample->buffer_size;
                    s->buffer = (byte*)s->sample->buffer;

                    if (s->predecoded)
                    {
                        s->source->eof = true;
                    }
                    else
                    {
                        if (s->sample->flags.HasFlag(Sound_SampleFlags.SOUND_SAMPLEFLAG_EOF)
                            || s->sample->flags.HasFlag(Sound_SampleFlags.SOUND_SAMPLEFLAG_EAGAIN))
                        {
                            s->source->eof = true;
                        }
                        else
                        {
                            s->bytes_left = Sound_Decode((IntPtr)s->sample);
                        }
                    }

                    if (s->source->eof)
                    {
                        if (s->loop_on)
                        {
                            NMIX_Rewind((IntPtr)s);
                        }
                        else
                        {
                            SDL_memset(buffer + bytes_written, 0, new IntPtr(buffer_size - bytes_written));
                            break;
                        }
                    }
                }
            }
        }

        internal static IntPtr NMIX_NewFileSource(IntPtr rw, string ext, bool predecode)
        {
            if (NMIX_GetAudioDevice() == 0)
            {
                SDL_SetError("Please open NMIX device before creating sources.");
                return IntPtr.Zero;
            }

            unsafe
            {
                NMIX_FileSource* s = (NMIX_FileSource*)SDL_malloc(new IntPtr(sizeof(NMIX_FileSource)));
                if (s == null)
                {
                    SDL_OutOfMemory();
                    return IntPtr.Zero;
                }

                s->rw = rw;

                ref var spec = ref NMIX_GetAudioSpec();
                s->sample = (Sound_Sample*)Sound_NewSample(s->rw, ext, IntPtr.Zero, spec.size);
                if (s->sample == null)
                {
                    SDL_free((IntPtr)s);
                    SDL_SetError($"SDL_sound error: {Sound_GetError()}");
                    return IntPtr.Zero;
                }

                s->source = (NMIX_Source*)NMIX_NewSource(
                    s->sample->actual.format,
                    s->sample->actual.channels,
                    (int)s->sample->actual.rate,
                    sdlsound_callback,
                    (IntPtr)s
                );
                if (s->source == null)
                {
                    Sound_FreeSample((IntPtr)s->sample);
                    SDL_free((IntPtr)s);
                    return IntPtr.Zero;
                }

                s->buffer = (byte*)s->sample->buffer;
                s->bytes_left = 0;
                s->loop_on = false;
                s->predecoded = predecode;
                if (predecode)
                {
                    s->bytes_left = Sound_DecodeAll((IntPtr)s->sample);
                    s->buffer = (byte*)s->sample->buffer;
                }

                return (IntPtr)s;
            }
        }

        internal static int NMIX_GetDuration(IntPtr src)
        {
            if (src == IntPtr.Zero)
                return -1;

            unsafe
            {
                var s = (NMIX_FileSource*)src;
                return Sound_GetDuration((IntPtr)s->sample);
            }
        }

        internal static int NMIX_Seek(IntPtr src, int ms)
        {
            if (src == IntPtr.Zero)
                return -1;

            unsafe
            {
                var s = (NMIX_FileSource*)src;
                if (Sound_Seek((IntPtr)s->sample, (uint)ms) == 0)
                {
                    SDL_SetError($"Error while seeking source: {Sound_GetError()}");
                    return -1;
                }

                return 0;
            }
        }

        internal static int NMIX_Rewind(IntPtr src)
        {
            if (src == IntPtr.Zero)
                return -1;

            SDL_LockAudioDevice(NMIX_GetAudioDevice());
            
            unsafe
            {
                var s = (NMIX_FileSource*)src;

                s->source->eof = false;
                s->bytes_left = 0;
                s->buffer = (byte*)s->sample->buffer;

                if (s->predecoded)
                {
                    s->bytes_left = (int)s->sample->buffer_size;
                }
                else
                {
                    if (Sound_Rewind((IntPtr)s->sample) == 0)
                    {
                        SDL_SetError($"Error while rewinding source: {Sound_GetError()}");
                        SDL_UnlockAudioDevice(NMIX_GetAudioDevice());
                        return -1;
                    }
                }
            }

            SDL_UnlockAudioDevice(NMIX_GetAudioDevice());
            return 0;
        }

        internal static bool NMIX_GetLoop(IntPtr src)
        {
            if (src == IntPtr.Zero)
                return false;

            unsafe
            {
                return ((NMIX_FileSource*)src)->loop_on;
            }
        }
        
        internal static void NMIX_SetLoop(IntPtr src, bool loop_on)
        {
            if (src == IntPtr.Zero)
                return;

            unsafe
            {
                ((NMIX_FileSource*)src)->loop_on = loop_on;
            }
        }

        internal static void NMIX_FreeFileSource(IntPtr src)
        {
            if (src == IntPtr.Zero)
                return;

            SDL_LockAudioDevice(NMIX_GetAudioDevice());
            unsafe
            {
                var s = (NMIX_FileSource*)src;

                if (s->sample != null)
                {
                    Sound_FreeSample((IntPtr)s->sample);
                }

                if (s->source != null)
                {
                    NMIX_FreeSource((IntPtr)s->source);
                }
            }

            SDL_free(src);
            SDL_UnlockAudioDevice(NMIX_GetAudioDevice());
        }
    }
}