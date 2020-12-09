using System;
using System.IO;
using System.Runtime.InteropServices;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SDL;

namespace Chroma.Audio.Sources
{
    public class FileBasedAudioSource : AudioSource
    {        
        private readonly Log _log = LogManager.GetForCurrentAssembly();
        private SDL2_nmix.NMIX_SourceCallback _originalSourceCallback;
        private SDL2_nmix.NMIX_SourceCallback _internalSourceCallback;

        internal IntPtr FileSourceHandle { get; private set; }
        internal IntPtr RwOpsHandle { get; private set; }

        internal unsafe SDL2_nmix.NMIX_FileSource* FileSource
            => (SDL2_nmix.NMIX_FileSource*)FileSourceHandle.ToPointer();

        internal unsafe SDL2_sound.Sound_Sample* SoundSample
            => (SDL2_sound.Sound_Sample*)FileSource->sample.ToPointer();

        public override PlaybackStatus Status { get; set; }

        public float Duration
        {
            get
            {
                EnsureFileSourceHandleValid();
                return SDL2_nmix.NMIX_GetDuration(FileSourceHandle);
            }
        }

        public bool IsLooping
        {
            get
            {
                EnsureFileSourceHandleValid();
                return SDL2_nmix.NMIX_GetLoop(FileSourceHandle);
            }

            set
            {
                EnsureFileSourceHandleValid();
                SDL2_nmix.NMIX_SetLoop(FileSourceHandle, value);
            }
        }

        internal FileBasedAudioSource(string filePath, bool decodeWhole)
        {
            RwOpsHandle = SDL2.SDL_RWFromFile(filePath, "rb");

            if (RwOpsHandle == IntPtr.Zero)
            {
                _log.Error($"Failed to initialize RWops from file: {SDL2.SDL_GetError()}");
                return;
            }

            FileSourceHandle = SDL2_nmix.NMIX_NewFileSource(
                RwOpsHandle,
                Path.GetExtension(filePath).TrimStart('.'),
                decodeWhole
            );

            if (FileSourceHandle == IntPtr.Zero)
            {
                _log.Error($"Failed to initialize audio source from file: {SDL2.SDL_GetError()}");
                return;
            }

            unsafe
            {
                Handle = FileSource->source;
            }
            
            HookSourceCallback();
        }

        internal FileBasedAudioSource(Stream stream, bool decodeWhole)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                var arr = ms.ToArray();

                unsafe
                {
                    fixed (byte* b = &arr[0])
                    {
                        RwOpsHandle = SDL2.SDL_RWFromConstMem(
                            new IntPtr(b),
                            arr.Length
                        );

                        if (RwOpsHandle == IntPtr.Zero)
                        {
                            _log.Error($"Failed to initialize RWops from stream: {SDL2.SDL_GetError()}");
                            return;
                        }

                        FileSourceHandle = SDL2_nmix.NMIX_NewFileSource(RwOpsHandle, null, decodeWhole);
                        if (FileSourceHandle == IntPtr.Zero)
                        {
                            _log.Error($"Failed to load initialize audio source from stream: {SDL2.SDL_GetError()}");
                            return;
                        }

                        Handle = FileSource->source;
                    }
                }
            }
         
            HookSourceCallback();
        }

        public override void Play()
        {
            if (Status == PlaybackStatus.Playing)
                Stop();
            
            base.Play();
            Status = PlaybackStatus.Playing;
        }

        public override void Pause()
        {
            if (Status == PlaybackStatus.Paused || Status == PlaybackStatus.Stopped)
                return;
            
            base.Pause();
            Status = PlaybackStatus.Paused;
        }

        public override void Stop()
        {
            EnsureHandleValid();
            EnsureFileSourceHandleValid();

            SDL2_nmix.NMIX_Pause(Handle);
            SDL2_nmix.NMIX_Rewind(FileSourceHandle);

            Status = PlaybackStatus.Stopped;
        }

        public void Rewind()
        {
            EnsureFileSourceHandleValid();

            SDL2_nmix.NMIX_Rewind(FileSourceHandle);
        }

        public void Seek(int milliseconds)
        {
            EnsureFileSourceHandleValid();

            SDL2_nmix.NMIX_Seek(FileSourceHandle, milliseconds);
        }

        protected override void FreeNativeResources()
        {
            if (FileSourceHandle != IntPtr.Zero)
            {
                SDL2_nmix.NMIX_FreeFileSource(Handle);
                FileSourceHandle = IntPtr.Zero;
            }

            if (RwOpsHandle != IntPtr.Zero)
            {
                SDL2.SDL_FreeRW(RwOpsHandle);
                RwOpsHandle = IntPtr.Zero;
            }
        }

        private void EnsureFileSourceHandleValid()
        {
            if (RwOpsHandle == IntPtr.Zero)
                throw new InvalidOperationException("RWops handle is invalid.");

            if (FileSourceHandle == IntPtr.Zero)
                throw new InvalidOperationException("File source handle is invalid.");
        }
        
        private void HookSourceCallback()
        {
            unsafe
            {
                _originalSourceCallback = Marshal.GetDelegateForFunctionPointer<SDL2_nmix.NMIX_SourceCallback>(Source->callback);
                _internalSourceCallback = InternalSourceCallback;

                var ptr = Marshal.GetFunctionPointerForDelegate(_internalSourceCallback);

                Source->callback = ptr;
            }
        }

        private void InternalSourceCallback(IntPtr userdata, IntPtr buffer, int bufferSize)
        {
            unsafe
            {
                var span = new Span<byte>(
                    SoundSample->buffer.ToPointer(), 
                    (int)SoundSample->buffer_size
                );

                for (var i = 0; i < Filters.Count; i++)
                    Filters[i]?.Invoke(span, AudioFormat.FromSdlFormat(SoundSample->actual.format));
                
                _originalSourceCallback(userdata, buffer, bufferSize);
            }

            unsafe
            {
                if (Source->eof > 0)
                {
                    Stop();
                    AudioManager.Instance.OnAudioSourceFinished(this);
                }
            }
        }
    }
}