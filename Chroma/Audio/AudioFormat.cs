using Chroma.Natives.SDL;
using System;
using System.Collections.Generic;

namespace Chroma.Audio
{
    public class AudioFormat
    {
        private readonly Dictionary<SampleFormat, ushort> SdlFormatBE = new()
        {
            {SampleFormat.U8, SDL2.AUDIO_U8},
            {SampleFormat.S8, SDL2.AUDIO_S8},
            {SampleFormat.U16, SDL2.AUDIO_U16MSB},
            {SampleFormat.S16, SDL2.AUDIO_S16MSB},
            {SampleFormat.S32, SDL2.AUDIO_S32MSB},
            {SampleFormat.F32, SDL2.AUDIO_F32MSB}
        };

        private readonly Dictionary<SampleFormat, ushort> SdlFormatLE = new()
        {
            {SampleFormat.U8, SDL2.AUDIO_U8},
            {SampleFormat.S8, SDL2.AUDIO_S8},
            {SampleFormat.U16, SDL2.AUDIO_U16LSB},
            {SampleFormat.S16, SDL2.AUDIO_S16LSB},
            {SampleFormat.S32, SDL2.AUDIO_S32LSB},
            {SampleFormat.F32, SDL2.AUDIO_F32LSB}
        };

        private readonly Dictionary<SampleFormat, ushort> SdlFormatSYS = new()
        {
            {SampleFormat.U8, SDL2.AUDIO_U8},
            {SampleFormat.S8, SDL2.AUDIO_S8},
            {SampleFormat.U16, SDL2.AUDIO_U16SYS},
            {SampleFormat.S16, SDL2.AUDIO_S16SYS},
            {SampleFormat.S32, SDL2.AUDIO_S32SYS},
            {SampleFormat.F32, SDL2.AUDIO_F32SYS}
        };

        public static readonly AudioFormat Default = BitConverter.IsLittleEndian
            ? new AudioFormat(SampleFormat.U16, ByteOrder.LittleEndian)
            : new AudioFormat(SampleFormat.U16, ByteOrder.BigEndian);

        public static readonly AudioFormat ChromaDefault = BitConverter.IsLittleEndian
            ? new AudioFormat(SampleFormat.F32, ByteOrder.LittleEndian)
            : new AudioFormat(SampleFormat.F32, ByteOrder.BigEndian);

        public SampleFormat SampleFormat { get; private set; }
        public ByteOrder ByteOrder { get; private set; }
        
        public byte BitsPerSample
        {
            get
            {
                return SampleFormat switch
                {
                    SampleFormat.F32 => 32,
                    SampleFormat.S32 => 32,
                    SampleFormat.S16 => 16,
                    SampleFormat.U16 => 16,
                    SampleFormat.U8 => 8,
                    SampleFormat.S8 => 8,
                    _ => throw new AudioException($"Sample format '{SampleFormat}' is not supported.")
                };
            }
        }

        public AudioFormat(SampleFormat sampleFormat)
        {
            ByteOrder = ByteOrder.LittleEndian;
            SampleFormat = sampleFormat;
        }

        public AudioFormat(SampleFormat sampleFormat, ByteOrder byteOrder)
        {
            SampleFormat = sampleFormat;
            ByteOrder = byteOrder;
        }

        internal ushort SdlFormat
        {
            get
            {
                return ByteOrder switch
                {
                    ByteOrder.Native => SdlFormatSYS[SampleFormat],
                    ByteOrder.BigEndian => SdlFormatBE[SampleFormat],
                    ByteOrder.LittleEndian => SdlFormatLE[SampleFormat],
                    _ => throw new AudioException("Unknown byte order specified."),
                };
            }
        }

        internal static AudioFormat FromSdlFormat(ushort format)
        {
            var (sampleFormat, byteOrder) = DetectFormat(format);
            return new AudioFormat(sampleFormat, byteOrder);
        }
        
        private static (SampleFormat, ByteOrder) DetectFormat(ushort sdlFormat)
        {
            switch (sdlFormat)
            {
                case SDL2.AUDIO_U8:
                    return (SampleFormat.U8, ByteOrder.LittleEndian);
                
                case SDL2.AUDIO_S8:
                    return (SampleFormat.S8, ByteOrder.LittleEndian);
                
                case SDL2.AUDIO_U16LSB:
                    return (SampleFormat.U16, ByteOrder.LittleEndian);
                
                case SDL2.AUDIO_U16MSB:
                    return (SampleFormat.U16, ByteOrder.BigEndian);
                
                case SDL2.AUDIO_S16LSB:
                    return (SampleFormat.S16, ByteOrder.LittleEndian);
                
                case SDL2.AUDIO_S16MSB:
                    return (SampleFormat.S16, ByteOrder.BigEndian);
                
                case SDL2.AUDIO_S32LSB:
                    return (SampleFormat.S32, ByteOrder.LittleEndian);
                
                case SDL2.AUDIO_S32MSB:
                    return (SampleFormat.S32, ByteOrder.BigEndian);

                case SDL2.AUDIO_F32LSB:
                    return (SampleFormat.F32, ByteOrder.LittleEndian);
                
                case SDL2.AUDIO_F32MSB:
                    return (SampleFormat.F32, ByteOrder.BigEndian);
             
                default: throw new AudioException("Unsupported SDL audio format.");
            }
        }
    }
}