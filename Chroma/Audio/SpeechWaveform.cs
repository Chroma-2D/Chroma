using Chroma.Natives.SoLoud;

namespace Chroma.Audio
{
    public enum SpeechWaveform
    {
        Sawtooth = SoLoud.SPEECH_KW_SAW,
        Triangle = SoLoud.SPEECH_KW_TRIANGLE,
        Sine = SoLoud.SPEECH_KW_SIN,
        Square = SoLoud.SPEECH_KW_SQUARE,
        Pulse = SoLoud.SPEECH_KW_PULSE,
        Noise = SoLoud.SPEECH_KW_NOISE,
        Warble = SoLoud.SPEECH_KW_WARBLE
    }
}