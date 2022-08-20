using Chroma.Input.GameControllers.Drivers.Sony.DualSense.TriggerEffectPresets;

namespace Chroma.Input.GameControllers.Drivers.Sony.DualSense
{
    internal static class TriggerEffectBuilder
    {
        private const int TriggerEffectDataLength = 11;

        private static unsafe void ClearDestination(byte* destination)
        {
            for (var i = 0; i < TriggerEffectDataLength; i++)
                destination[i] = 0;
        }

        internal static unsafe void Reset(byte* destination)
        {
            ClearDestination(destination);
            destination[0] = (byte)TriggerEffectType.None;
        }

        internal static unsafe void Linear(byte* destination, byte actuationPoint, byte resistanceForce)
        {
            ClearDestination(destination);
          
            destination[0] = (byte)TriggerEffectType.Linear;
            destination[1] = actuationPoint;
            destination[2] = resistanceForce;
        }

        internal static unsafe void LinearPreset(byte* destination, LinearTriggerEffectPreset preset)
        {
            ClearDestination(destination);

            var activationMask = preset.ZoneEnableMask;
            var encodedForces = preset.EncodedZoneForces;

            destination[0] = (byte)TriggerEffectType.LinearPreset;
            destination[1] = (byte)(activationMask & 0xFF);
            destination[2] = (byte)((activationMask >> 8) & 0xFF);
            destination[3] = (byte)(encodedForces & 0xFF);
            destination[4] = (byte)((encodedForces >> 8) & 0xFF);
            destination[5] = (byte)((encodedForces >> 16) & 0xFF);
            destination[6] = (byte)((encodedForces >> 24) & 0xFF);
            destination[7] = (byte)((encodedForces >> 32) & 0xFF);
            destination[8] = (byte)((encodedForces >> 40) & 0xFF);
        }

        internal static unsafe void ActuationZone(byte* destination, byte actuationStart, byte actuationEnd,
            byte resistanceForce)
        {
            ClearDestination(destination);

            destination[0] = (byte)TriggerEffectType.ActuationZone;
            destination[1] = actuationStart;
            destination[2] = actuationEnd;
            destination[3] = resistanceForce;
        }

        internal static unsafe void ActuationZoneRumble(byte* destination, byte actuationStart, byte resistanceForce,
            byte rumbleFrequency)
        {
            ClearDestination(destination);

            destination[0] = (byte)TriggerEffectType.ActuationZoneRumble;
            destination[1] = rumbleFrequency;
            destination[2] = resistanceForce;
            destination[3] = actuationStart;
        }
    }
}