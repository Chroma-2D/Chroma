namespace Chroma.Audio.Sources;

using System;
using System.IO;
using System.Runtime.InteropServices;
using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;
using Chroma.Natives.Bindings.SDL;
using Chroma.Natives.Ports.NMIX;

public abstract class FileBasedAudioSource : AudioSource
{
    private static readonly Log _log = LogManager.GetForCurrentAssembly();

    private readonly SdlRwOps _sdlRwOps;

    private SDL2_nmix.NMIX_SourceCallback _originalSourceCallback;
    private SDL2_nmix.NMIX_SourceCallback _internalSourceCallback;

    internal IntPtr FileSourceHandle { get; private set; }

    internal unsafe SDL2_nmix.NMIX_FileSource* FileSource
        => (SDL2_nmix.NMIX_FileSource*)FileSourceHandle.ToPointer();

    internal unsafe SDL2_sound.Sound_Sample* SoundSample => FileSource->sample;

    public override PlaybackStatus Status { get; set; }

    public bool CanSeek
    {
        get
        {
            unsafe
            {
                EnsureFileSourceHandleValid();
                return SoundSample->flags.HasFlag(SDL2_sound.Sound_SampleFlags.SOUND_SAMPLEFLAG_CANSEEK) &&
                       Duration > 0;
            }
        }
    }

    public double Duration
    {
        get
        {
            EnsureFileSourceHandleValid();
            return SDL2_nmix.NMIX_GetDuration(FileSourceHandle) / 1000.0;
        }
    }

    public double Position { get; private set; }

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
        : this(new FileStream(filePath, FileMode.Open, FileAccess.Read), decodeWhole,
            Path.GetExtension(filePath).Replace(".", ""))
    {
    }

    internal FileBasedAudioSource(Stream stream, bool decodeWhole, string fileFormat = null)
    {
        _sdlRwOps = new SdlRwOps(stream, true);

        if (_sdlRwOps.RwOpsHandle == IntPtr.Zero)
        {
            throw new AudioException($"Failed to initialize RWops from stream: {SDL2.SDL_GetError()}");
        }

        if (!string.IsNullOrEmpty(fileFormat))
        {
            SDL2.SDL_RWseek(_sdlRwOps.RwOpsHandle, 0, SDL2.RW_SEEK_SET);
            FileSourceHandle = SDL2_nmix.NMIX_NewFileSource(_sdlRwOps.RwOpsHandle, fileFormat, decodeWhole);
        }

        if (FileSourceHandle == IntPtr.Zero)
        {
            var found = false;
            foreach (var decoder in AudioOutput.Instance.Decoders)
            {
                foreach (var format in decoder.SupportedFormats)
                {
                    SDL2.SDL_RWseek(_sdlRwOps.RwOpsHandle, 0, SDL2.RW_SEEK_SET);
                    FileSourceHandle = SDL2_nmix.NMIX_NewFileSource(_sdlRwOps.RwOpsHandle, format, decodeWhole);

                    if (FileSourceHandle != IntPtr.Zero)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                    break;
            }
        }

        if (FileSourceHandle == IntPtr.Zero)
        {
            throw new AudioException(
                $"Failed to initialize audio source from stream: {SDL2.SDL_GetError()}\n" +
                $"Are you sure the data in the stream is valid and not you're not reading past the end of it?"
            );
        }

        unsafe
        {
            Handle = (IntPtr)FileSource->source;
        }

        HookSourceCallback();
    }

    public override void Play()
    {
        EnsureHandleValid();

        if (Status == PlaybackStatus.Playing)
        {
            SDL2_nmix.NMIX_Pause(Handle);

            if (SDL2_nmix.NMIX_Rewind(FileSourceHandle) < 0)
            {
                _log.Error($"Failed to play the audio source [rewind]: {SDL2.SDL_GetError()}");
                return;
            }

            Status = PlaybackStatus.Stopped;
            Position = 0;
        }

        if (SDL2_nmix.NMIX_Play(Handle) < 0)
        {
            _log.Error($"Failed to play the audio source [play]: {SDL2.SDL_GetError()}");
            return;
        }
        else
        {
            Status = PlaybackStatus.Playing;
        }
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

        if (SDL2_nmix.NMIX_Rewind(FileSourceHandle) < 0)
        {
            _log.Error($"Failed to stop the audio source [rewind]: {SDL2.SDL_GetError()}");
            return;
        }

        Position = 0;
        Status = PlaybackStatus.Stopped;
    }

    public void Rewind()
    {
        EnsureFileSourceHandleValid();

        if (SDL2_nmix.NMIX_Rewind(FileSourceHandle) < 0)
        {
            _log.Error($"Failed to rewind the requested audio source: {SDL2.SDL_GetError()}");
            return;
        }

        Position = 0;
    }

    public void Seek(double seconds, SeekOrigin origin = SeekOrigin.Begin)
    {
        EnsureFileSourceHandleValid();

        if (!CanSeek)
        {
            _log.Error("This audio source does not support seeking.");
            return;
        }

        if (Status == PlaybackStatus.Stopped)
        {
            _log.Error("Cannot seek while stopped.");
            return;
        }

        var targetPosition = origin switch
        {
            SeekOrigin.Begin => seconds,
            SeekOrigin.Current => Position + seconds,
            SeekOrigin.End => Duration - seconds,
            _ => throw new AudioException("Invalid seek origin.")
        };

        if (targetPosition >= Duration)
        {
            targetPosition = targetPosition - Duration;
        }
        else if (targetPosition < 0)
        {
            targetPosition = Duration + targetPosition;
        }

        Pause();

        if (SDL2_nmix.NMIX_Seek(FileSourceHandle, (int)(targetPosition * 1000)) < 0)
        {
            _log.Error($"Failed to seek to {seconds}: {SDL2.SDL_GetError()}");
            return;
        }

        Play();

        Position = targetPosition;
    }

    protected override void FreeNativeResources()
    {
        EnsureOnMainThread();

        if (FileSourceHandle != IntPtr.Zero)
        {
            SDL2_nmix.NMIX_FreeFileSource(FileSourceHandle);
            FileSourceHandle = IntPtr.Zero;
        }

        _sdlRwOps.Dispose();
        // SDL_sound closes RWops after initializing audio samples from file.
        Handle = IntPtr.Zero;
    }

    private void EnsureFileSourceHandleValid()
    {
        if (FileSourceHandle == IntPtr.Zero)
            throw new AudioException("File source handle is invalid.");
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

            var actualAudioFormat = AudioFormat.FromSdlFormat(SoundSample->actual.format);

            for (var i = 0; i < Filters.Count; i++)
                Filters[i]?.Invoke(span, actualAudioFormat);

            _originalSourceCallback(userdata, buffer, bufferSize);

            var sampleDuration = (double)bufferSize / (
                SoundSample->actual.rate * SoundSample->actual.channels * (actualAudioFormat.BitsPerSample / 8)
            );

            Position += sampleDuration;

            if (Position >= Duration || Source->eof)
            {
                Position = 0;
                    
                if (!IsLooping)
                {
                    Status = PlaybackStatus.Stopped;
                }

                AudioOutput.Instance.OnAudioSourceFinished(this, IsLooping);
            }
        }
    }
}