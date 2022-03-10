using System;

namespace Chroma.Input.GameControllers.Drivers.Sony.DualSense.TriggerEffectPresets
{
    public class LinearTriggerEffectPreset
    {
        private sbyte[] _zoneForces = new sbyte[10];

        public ushort ZoneEnableMask { get; private set; }
        public ulong EncodedZoneForces { get; private set; }

        public sbyte this[byte zone]
        {
            get
            {
                if (zone > 9)
                {
                    throw new IndexOutOfRangeException("Presets support up to 10 active zones.");
                }

                return _zoneForces[zone];
            }

            set
            {
                if (zone > 9)
                {
                    throw new IndexOutOfRangeException("Presets support up to 10 active zones (zero-indexed).");
                }

                if (value > 7)
                {
                    throw new ArgumentOutOfRangeException(nameof(zone), "Zone resistance is limited to values between 0 and 7.");
                }

                if (_zoneForces[zone] == value)
                    return;
                
                _zoneForces[zone] = value;
                
                RecalculateEncodedValues();
            }
        }

        public LinearTriggerEffectPreset()
        {
        }

        public LinearTriggerEffectPreset(
            sbyte zone0, sbyte zone1, sbyte zone2, sbyte zone3,
            sbyte zone4, sbyte zone5, sbyte zone6, sbyte zone7,
            sbyte zone8, sbyte zone9)
        {
            this[0] = zone0; this[1] = zone1; this[2] = zone2; this[3] = zone3;
            this[4] = zone4; this[5] = zone5; this[6] = zone6; this[7] = zone7;
            this[8] = zone8; this[9] = zone9;
        }

        private void RecalculateEncodedValues()
        {
            ZoneEnableMask = 0;
            EncodedZoneForces = 0;
            
            for (var i = 0; i < _zoneForces.Length; i++)
            {
                if (_zoneForces[i] >= 0)
                {
                    ZoneEnableMask |= (ushort)(1 << i);
                }

                EncodedZoneForces |= (ulong)_zoneForces[i] << (3 * i);
            }
        }
    }
}