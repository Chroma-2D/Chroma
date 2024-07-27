namespace Chroma.Input.GameControllers.Drivers.Sony.DualSense;

using System.Runtime.InteropServices;
using Chroma.Input.GameControllers.Drivers.Sony.DualSense.TriggerEffectPresets;

public class TriggerEffect
{
    private DS5EffectState _state;
    private DualSenseControllerDriver _driver;

    internal TriggerEffect(DualSenseControllerDriver driver)
    {
        _driver = driver;

        _state = new DS5EffectState
        {
            ucEnableBits1 = DS5EffectState.EnableBits1.ModifyLeftTrigger
                            | DS5EffectState.EnableBits1.ModifyRightTrigger
        };
    }

    public void ClearLeft()
    {
        unsafe
        {
            fixed (DS5EffectState* state = &_state)
            {
                TriggerEffectBuilder.Reset(state->rgucLeftTriggerEffect);
                PushToDevice();
            }
        }
    }

    public void ClearRight()
    {
        unsafe
        {
            fixed (DS5EffectState* state = &_state)
            {
                TriggerEffectBuilder.Reset(state->rgucRightTriggerEffect);
                PushToDevice();
            }
        }
    }

    public void Linear(byte leftActuationPoint, byte leftForce,
        byte rightActuationPoint, byte rightForce)
    {
        unsafe
        {
            fixed (DS5EffectState* state = &_state)
            {
                if (leftForce > 0)
                {
                    TriggerEffectBuilder.Linear(
                        state->rgucLeftTriggerEffect,
                        leftActuationPoint, 
                        leftForce
                    );
                }

                if (rightForce > 0)
                {
                    TriggerEffectBuilder.Linear(
                        state->rgucRightTriggerEffect, 
                        rightActuationPoint,
                        rightForce
                    );
                }

                PushToDevice();
            }
        }
    }

    public void Linear(LinearTriggerEffectPreset? leftPreset, LinearTriggerEffectPreset? rightPreset)
    {
        unsafe
        {
            fixed (DS5EffectState* state = &_state)
            {
                if (leftPreset != null)
                {
                    TriggerEffectBuilder.LinearPreset(
                        state->rgucLeftTriggerEffect,
                        leftPreset
                    );
                }

                if (rightPreset != null)
                {
                    TriggerEffectBuilder.LinearPreset(
                        state->rgucRightTriggerEffect,
                        rightPreset
                    );
                }
                    
                PushToDevice();
            }
        }
    }
        
    public void ActuationZone(byte leftActuationStart, byte leftActuationEnd, byte leftForce,
        byte rightActuationStart, byte rightActuationEnd, byte rightForce)
    {
        unsafe
        {
            fixed (DS5EffectState* state = &_state)
            {
                if (leftForce > 0)
                {
                    TriggerEffectBuilder.ActuationZone(
                        state->rgucLeftTriggerEffect, 
                        leftActuationStart,
                        leftActuationEnd,
                        leftForce
                    );
                }

                if (rightForce > 0)
                {
                    TriggerEffectBuilder.ActuationZone(
                        state->rgucRightTriggerEffect, 
                        rightActuationStart,
                        rightActuationEnd,
                        rightForce
                    );
                }

                PushToDevice();
            }
        }
    }

    public void ActuationZoneRumble(byte leftDropOffThreshold, byte leftForce, byte leftRumbleFrequency, 
        byte rightDropOffThreshold, byte rightForce, byte rightRumbleFrequency)
    {
        unsafe
        {
            fixed (DS5EffectState* state = &_state)
            {
                if (leftForce > 0)
                {
                    TriggerEffectBuilder.ActuationZoneRumble(
                        state->rgucLeftTriggerEffect, 
                        leftDropOffThreshold,
                        leftForce,
                        leftRumbleFrequency
                    );
                }
            
                if (rightForce > 0)
                {
                    TriggerEffectBuilder.ActuationZoneRumble(
                        state->rgucRightTriggerEffect, 
                        rightDropOffThreshold,
                        rightForce,
                        rightRumbleFrequency
                    );
                }
            
                PushToDevice();
            }
        }
    }

    private unsafe void PushToDevice()
    {
        fixed (DS5EffectState* state = &_state)
        {
            _driver.SendEffect((byte*)state, Marshal.SizeOf<DS5EffectState>());
        }
    }
}