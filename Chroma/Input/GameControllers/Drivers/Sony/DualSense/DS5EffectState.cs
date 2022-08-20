using System;
using System.Runtime.InteropServices;

namespace Chroma.Input.GameControllers.Drivers.Sony.DualSense
{
    [StructLayout(LayoutKind.Sequential, Size = 47)]
    internal unsafe struct DS5EffectState
    {
        public EnableBits1 ucEnableBits1;
        public EnableBits2 ucEnableBits2;
        public byte ucRumbleRight;
        public byte ucRubmleLeft;
        public byte ucHeadphoneVolume;
        public byte ucSpeakerVolume;
        public byte ucMicrophoneVolume;
        public byte ucAudioEnableBits;
        public DualSenseMicrophoneLedMode ucMicLightMode;
        public byte ucAudioMuteBits;
        public fixed byte rgucRightTriggerEffect[11];
        public fixed byte rgucLeftTriggerEffect[11];
        public fixed byte rgucUnknown1[6];
        public byte ucLedFlags;
        public fixed byte rgucUnknown2[2];
        public byte ucLedAnim;
        public byte ucLedBrightness;
        public byte ucPadLights;
        public byte ucLedRed;
        public byte ucLedGreen;
        public byte ucLedBlue;
        
        [Flags]
        internal enum EnableBits1 : byte
        {
            ModifyRumbleEmulation = 0x01,
            ModifyAudioHaptics = 0x02,
            ModifyRightTrigger = 0x04,
            ModifyLeftTrigger = 0x08
        }

        [Flags]
        internal enum EnableBits2 : byte
        {
            ModifyMicrophoneLight = 0x01,
            ModifyLedColor = 0x04,
            ResetLedState = 0x08,
            ModifyTouchpadLights = 0x10
        }
    }
}