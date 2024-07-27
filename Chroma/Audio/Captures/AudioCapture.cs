namespace Chroma.Audio.Captures;

using System.Threading;
using System.Threading.Tasks;
using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;
using Chroma.Natives.Bindings.SDL;

public abstract class AudioCapture : DisposableResource
{
    private static readonly Log _log = LogManager.GetForCurrentAssembly();
    private CancellationTokenSource? _finishTokenSource;

    private uint _deviceId;

    public AudioDevice Device { get; }
    public AudioFormat Format { get; }
    public ChannelMode ChannelMode { get; }
    public int Frequency { get; }
    public ushort BufferSize { get; }

    public CaptureStatus Status { get; protected set; } = CaptureStatus.Stopped;
    public ulong TotalSize { get; private set; }

    protected AudioCapture(
        AudioDevice? device = null, 
        AudioFormat? format = null, 
        ChannelMode channelMode = ChannelMode.Mono, 
        int frequency = 44100,
        ushort bufferSize = 4096)
    {
        Device = device ?? AudioInput.Instance.DefaultDevice;
        Format = format ?? AudioFormat.ChromaDefault;

        ChannelMode = channelMode;
        Frequency = frequency;
        BufferSize = bufferSize;
    }

    public virtual void Start()
    {
        if (Device.IsCurrentlyInUse)
        {
            _log.Error($"Device '{Device.Name}' is currently in use.");
            return;
        }
            
        _deviceId = AudioInput.Instance.OpenDevice(Device, Format, ChannelMode, Frequency, BufferSize);

        if (_deviceId == 0)
        {
            _log.Error("Attempted to start a capture that failed to initialize.");
            return;
        }
            
        if (Status == CaptureStatus.Recording)
        {
            _log.Error("Cannot start a capture that is already playing.");
            return;
        }

        if (_finishTokenSource != null)
            _finishTokenSource.Dispose();

        _finishTokenSource = new CancellationTokenSource();
        Task.Run(Record);
    }

    public virtual void Resume()
    {
        if (Status != CaptureStatus.Paused)
        {
            _log.Error("Cannot resume a capture that is not paused.");
            return;
        }

        SDL2.SDL_PauseAudioDevice(_deviceId, 0);
        Status = CaptureStatus.Recording;
    }

    public virtual void Pause()
    {
        if (Status != CaptureStatus.Recording)
        {
            _log.Error("Cannot pause a capture that is not recording.");
            return;
        }

        SDL2.SDL_PauseAudioDevice(_deviceId, 1);
        Status = CaptureStatus.Paused;
    }

    public virtual void Stop()
    {
        if (Status == CaptureStatus.Stopped)
        {
            _log.Error("Cannot stop a capture that is not recording.");
            return;
        }

        Terminate();
    }

    internal void Terminate()
    {
        if (Status == CaptureStatus.Stopped)
            return;
            
        if (_deviceId > 0)
        {
            SDL2.SDL_PauseAudioDevice(_deviceId, 1);
            SDL2.SDL_ClearQueuedAudio(_deviceId);
        }

        if (_finishTokenSource != null)
        {
            _finishTokenSource.Cancel(true);
            _finishTokenSource.Dispose();
            _finishTokenSource = null;
        }

        AudioInput.Instance.Untrack(this);
        AudioInput.Instance.CloseDevice(_deviceId, Device);
    }

    protected override void FreeManagedResources()
    {
        EnsureOnMainThread();
            
        if (Status != CaptureStatus.Stopped)
        {
            Terminate();

            while (Status != CaptureStatus.Stopped)
            {
                _log.Debug("Waiting for capture task to exit...");
                Thread.Sleep(1);
            }
        }

        if (_finishTokenSource != null)
        {
            _finishTokenSource.Dispose();
            _finishTokenSource = null;
        }
    }

    protected virtual void ProcessAudioBuffer(byte[] buffer)
    {
    }

    private void Record()
    {
        SDL2.SDL_PauseAudioDevice(_deviceId, 0);
        Status = CaptureStatus.Recording;

        try
        {
            while (true)
            {
                _finishTokenSource?.Token.ThrowIfCancellationRequested();

                var dataSize = SDL2.SDL_GetQueuedAudioSize(_deviceId);

                if (dataSize == 0)
                    continue;

                TotalSize += dataSize;
                var buffer = new byte[dataSize];

                unsafe
                {
                    fixed (byte* bp = &buffer[0])
                    {
                        SDL2.SDL_DequeueAudio(_deviceId, bp, dataSize);
                    }
                }

                ProcessAudioBuffer(buffer);
            }
        }
        catch
        {
            Status = CaptureStatus.Stopped;
        }
    }
}