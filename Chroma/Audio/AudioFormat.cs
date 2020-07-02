using Chroma.Natives.SDL;
using System;
using System.Collections.Generic;

namespace Chroma.Audio
{
    public class AudioFormat
    {
        private Dictionary<SampleFormat, ushort> SdlMixerFormatBE = new Dictionary<SampleFormat, ushort>
        {
            {SampleFormat.U8, SDL2.AUDIO_U8},
            {SampleFormat.S8, SDL2.AUDIO_S8},
            {SampleFormat.U16, SDL2.AUDIO_U16MSB},
            {SampleFormat.S16, SDL2.AUDIO_S16MSB},
            {SampleFormat.S32, SDL2.AUDIO_S32MSB},
            {SampleFormat.F32, SDL2.AUDIO_F32MSB}
        };

        private Dictionary<SampleFormat, ushort> SdlMixerFormatLE = new Dictionary<SampleFormat, ushort>
        {
            {SampleFormat.U8, SDL2.AUDIO_U8},
            {SampleFormat.S8, SDL2.AUDIO_S8},
            {SampleFormat.U16, SDL2.AUDIO_U16LSB},
            {SampleFormat.S16, SDL2.AUDIO_S16LSB},
            {SampleFormat.S32, SDL2.AUDIO_S32LSB},
            {SampleFormat.F32, SDL2.AUDIO_F32LSB}
        };

        private Dictionary<SampleFormat, ushort> SdlMixerFormatSYS = new Dictionary<SampleFormat, ushort>
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

        internal ushort SdlMixerFormat
        {
            get
            {
                return ByteOrder switch
                {
                    ByteOrder.Native => SdlMixerFormatSYS[SampleFormat],
                    ByteOrder.BigEndian => SdlMixerFormatBE[SampleFormat],
                    ByteOrder.LittleEndian => SdlMixerFormatLE[SampleFormat],
                    _ => throw new FormatException("Unknown byte order specified."),
                };
            }
        }
    }
}