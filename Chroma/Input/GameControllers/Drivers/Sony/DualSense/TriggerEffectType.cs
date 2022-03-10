namespace Chroma.Input.GameControllers.Drivers.Sony.DualSense
{
    internal enum TriggerEffectType : byte
    {
        None = 0x05,
        Linear = 0x01,
        LinearPreset = 0x21,
        ActuationZone = 0x02,
        ActuationZoneRumble = 0x06,
    }
}