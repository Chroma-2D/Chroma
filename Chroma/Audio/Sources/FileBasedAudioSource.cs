﻿using System;
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

                        foreach (var decoder in AudioManager.Instance.Decoders)
                        {
                            foreach (var format in decoder.SupportedFormats)
                            {
                                SDL2.SDL_RWseek(RwOpsHandle, 0, SDL2.RW_SEEK_SET);
                                FileSourceHandle = SDL2_nmix.NMIX_NewFileSource(RwOpsHandle, format, decodeWhole);

                                if (FileSourceHandle != IntPtr.Zero)
                                    break;
                            }
                        }

                        if (FileSourceHandle == IntPtr.Zero)
                        {
                            _log.Error($"Failed to initialize audio source from stream: {SDL2.SDL_GetError()}");
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
            EnsureHandleValid();

            if (Status == PlaybackStatus.Playing)
            {
                if (SDL2_nmix.NMIX_Pause(Handle) < 0)
                {
                    _log.Error($"Failed to play the audio source [pause]: {SDL2.SDL_GetError()}");
                    return;
                }

                if (SDL2_nmix.NMIX_Rewind(FileSourceHandle) < 0)
                {
                    _log.Error($"Failed to play the audio source [rewind]: {SDL2.SDL_GetError()}");
                    return;
                }

                Status = PlaybackStatus.Stopped;
            }

            if (SDL2_nmix.NMIX_Play(Handle) < 0)
            {
                _log.Error($"Failed to play the audio source [play]: {SDL2.SDL_GetError()}");
                return;
            }

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

            if (SDL2_nmix.NMIX_Pause(Handle) < 0)
            {
                _log.Error($"Failed to stop the audio source [pause]: {SDL2.SDL_GetError()}");
                return;
            }

            if (SDL2_nmix.NMIX_Rewind(FileSourceHandle) < 0)
            {
                _log.Error($"Failed to stop the audio source [rewind]: {SDL2.SDL_GetError()}");
                return;
            }

            Status = PlaybackStatus.Stopped;
        }

        public void Rewind()
        {
            EnsureFileSourceHandleValid();

            if (SDL2_nmix.NMIX_Rewind(FileSourceHandle) < 0)
            {
                _log.Error($"Failed to rewind the requested audio source: {SDL2.SDL_GetError()}");
            }
        }

        public void Seek(int milliseconds)
        {
            EnsureFileSourceHandleValid();

            if (SDL2_nmix.NMIX_Seek(FileSourceHandle, milliseconds) < 0)
            {
                _log.Error($"Failed to seek to {milliseconds}ms: {SDL2.SDL_GetError()}");
            }
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
            EnsureHandleValid();

            unsafe
            {
                _originalSourceCallback =
                    Marshal.GetDelegateForFunctionPointer<SDL2_nmix.NMIX_SourceCallback>(Source->callback);
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

                if (Source->eof > 0)
                {
                    if (!IsLooping)
                    {
                        Pause();
                        Status = PlaybackStatus.Stopped;
                    }

                    Rewind();
                    AudioManager.Instance.OnAudioSourceFinished(this, IsLooping);
                }
            }
        }
    }
}