namespace Chroma.Audio.Captures;

using System.IO;

public class StreamAudioCapture : AudioCapture
{
    private readonly Stream _stream;
        
    public StreamAudioCapture(
        Stream stream,
        AudioDevice device = null, 
        AudioFormat format = null, 
        ChannelMode channelMode = ChannelMode.Mono, 
        int frequency = 44100, ushort bufferSize = 4096) 
        : base(device, format, channelMode, frequency, bufferSize)
    {
        _stream = stream;
    }
        
    protected override void ProcessAudioBuffer(byte[] buffer)
    {
        _stream.Write(buffer, 0, buffer.Length);
    }
}