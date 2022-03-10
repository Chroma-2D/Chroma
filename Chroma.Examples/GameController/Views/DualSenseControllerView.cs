using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Chroma.Input;
using Chroma.Input.GameControllers;
using Chroma.Input.GameControllers.Drivers.Sony;
using Chroma.Input.GameControllers.Drivers.Sony.DualSense;
using Chroma.Input.GameControllers.Drivers.Sony.DualSense.TriggerEffectPresets;
using Chroma.Windowing;

namespace GameController.Views
{
    public class DualSenseControllerView : DualShockControllerView
    {
        private LinearTriggerEffectPreset _leftTriggerPreset;
        private LinearTriggerEffectPreset _rightTriggerPreset;
        private byte _currentTouchpadLightMask = 0;
        
        public override string ViewName { get; } = "DualSense controllers";

        public override Vector2 PositionOnScreen => new(
            0,
            _window.Height / 2f
        );

        public override List<ControllerType> AcceptedControllers { get; } = new()
        {
            ControllerType.PlayStation5
        };

        public DualSenseControllerView(Window window) : base(window)
        {
            _leftTriggerPreset = new LinearTriggerEffectPreset(
                0, 0, 0, 0, -1, -1, -1, -1, -1, -1
            );

            _rightTriggerPreset = new LinearTriggerEffectPreset(
                5, 0, 0, 0, 0, 7, 7, 7, 7, 7
            );
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            foreach (var kvp in _controllers)
            {
                if (kvp.Key is DualSenseControllerDriver ds5)
                {
                    if (ds5.AccelerometerEnabled)
                        kvp.Value.Accelerometer = Vector3.Normalize(ds5.ReadAccelerometerSensor());
                    else kvp.Value.Accelerometer = Vector3.Zero;

                    if (ds5.GyroscopeEnabled)
                        kvp.Value.Gyroscope = Vector3.Normalize(ds5.ReadGyroscopeSensor());
                    else kvp.Value.Gyroscope = Vector3.Zero;

                    kvp.Value.TouchPoints = ds5.TouchPoints.ToArray();
                }
            }
        }

        public override void OnButtonPressed(ControllerButtonEventArgs e)
        {
            var ds5 = e.Controller.As<DualSenseControllerDriver>();

            if (e.Controller.IsButtonDown(ControllerButton.RightBumper))
            {
                ds5.TriggerEffect.ClearRight();

                if (e.Button == ControllerButton.A)
                {
                    ds5.TriggerEffect.Linear(0, 0, 96, 255);
                }
                else if (e.Button == ControllerButton.B)
                {
                    ds5.TriggerEffect.Linear(null, _rightTriggerPreset);
                }
                else if (e.Button == ControllerButton.X)
                {
                    ds5.TriggerEffect.ActuationZone(0, 0, 0, 30, 90, 255);
                }
                else if (e.Button == ControllerButton.Y)
                {
                    ds5.TriggerEffect.ActuationZoneRumble(0, 0, 0, 30, 255, 60);
                }
            }

            if (e.Controller.IsButtonDown(ControllerButton.LeftBumper))
            {
                ds5.TriggerEffect.ClearLeft();

                if (e.Button == ControllerButton.A)
                {
                    ds5.TriggerEffect.Linear(96, 255, 0, 0);
                }
                else if (e.Button == ControllerButton.B)
                {
                    ds5.TriggerEffect.Linear(_leftTriggerPreset, null);
                }
                else if (e.Button == ControllerButton.X)
                {
                    ds5.TriggerEffect.ActuationZone(30, 255, 255, 0, 0, 0);
                }
                else if (e.Button == ControllerButton.Y)
                {
                    ds5.TriggerEffect.ActuationZoneRumble(30, 255, 30, 0, 0, 0);
                }
            }

            if (e.Button == ControllerButton.Menu)
            {
                if (ds5.MicrophoneLedMode == DualSenseMicrophoneLedMode.Off)
                {
                    ds5.MicrophoneLedMode = DualSenseMicrophoneLedMode.Pulse;
                }
                else if (ds5.MicrophoneLedMode == DualSenseMicrophoneLedMode.Pulse)
                {
                    ds5.MicrophoneLedMode = DualSenseMicrophoneLedMode.Solid;
                }
                else if (ds5.MicrophoneLedMode == DualSenseMicrophoneLedMode.Solid)
                {
                    ds5.MicrophoneLedMode = DualSenseMicrophoneLedMode.Off;
                }
            }

            if (e.Button == ControllerButton.DpadUp)
            {
                ds5.SetTouchpadLights(_currentTouchpadLightMask++);
            }
        }

        public override void OnAxisMoved(ControllerAxisEventArgs e)
        {
            base.OnAxisMoved(e);

            if (e.Axis == ControllerAxis.LeftTrigger)
            {
                var norm = e.Controller.GetAxisValueNormalized(e.Axis);
                if (norm >= 0.60f && norm <= 0.60f)
                {
                    e.Controller.Rumble(32768, 32768, 100);
                }
            }
        }
    }
}