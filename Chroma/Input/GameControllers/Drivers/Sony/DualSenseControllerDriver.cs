namespace Chroma.Input.GameControllers.Drivers.Sony;

using System.Runtime.InteropServices;
using Chroma.Graphics;
using Chroma.Input.GameControllers.Drivers.Sony.DualSense;

public sealed class DualSenseControllerDriver : SonyControllerDriver
{
    private DualSenseMicrophoneLedMode _micLedMode;

    public override string Name => "Sony DualSense Chroma Driver";

    public TriggerEffect TriggerEffect { get; }

    public DualSenseMicrophoneLedMode MicrophoneLedMode
    {
        get => _micLedMode;
        set
        {
            _micLedMode = value;

            var state = new DS5EffectState();
            state.ucEnableBits2 = DS5EffectState.EnableBits2.ModifyMicrophoneLight;
            state.ucMicLightMode = _micLedMode;

            unsafe
            {
                SendEffect((byte*)&state, Marshal.SizeOf<DS5EffectState>());
            }
        }
    }

    internal DualSenseControllerDriver(ControllerInfo info)
        : base(info)
    {
        TriggerEffect = new TriggerEffect(this);
        MicrophoneLedMode = DualSenseMicrophoneLedMode.Off;
        SetTouchpadLights(0);
    }

    public override void SetLedColor(Color color)
    {
        var state = new DS5EffectState
        {
            ucEnableBits2 = DS5EffectState.EnableBits2.ModifyLedColor,
            ucLedRed = color.R,
            ucLedGreen = color.G,
            ucLedBlue = color.B,
            ucLedBrightness = color.A,
        };
            
        unsafe
        {
            SendEffect((byte*)&state, Marshal.SizeOf<DS5EffectState>());
        }
    }

    public void SetTouchpadLights(byte mask)
    {
        var state = new DS5EffectState()
        {
            ucEnableBits2 = DS5EffectState.EnableBits2.ModifyTouchpadLights,
            ucPadLights = (byte)(mask & 0x1F) // DualSense has 5 LEDs under the touchpad, so we can cut this...
        };
            
        unsafe
        {
            SendEffect((byte*)&state, Marshal.SizeOf<DS5EffectState>());
        }
    }
}