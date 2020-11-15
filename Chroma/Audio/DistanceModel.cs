using Chroma.Natives.OpenAL;

namespace Chroma.Audio
{
    public enum DistanceModel
    {
        None = 0,
        LinearDistance = Al.AL_LINEAR_DISTANCE,
        LinearDistanceClamped = Al.AL_LINEAR_DISTANCE_CLAMPED,
        InverseDistance = Al.AL_INVERSE_DISTANCE,
        InverseDistanceClamped = Al.AL_INVERSE_DISTANCE_CLAMPED,
        ExponentDistance = Al.AL_EXPONENT_DISTANCE,
        ExponentDistanceClamped = Al.AL_EXPONENT_DISTANCE_CLAMPED,
    }
}