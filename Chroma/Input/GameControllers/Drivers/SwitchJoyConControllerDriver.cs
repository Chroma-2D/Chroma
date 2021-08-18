using System.Collections.Generic;
using System.Numerics;
using Chroma.Input.GameControllers.Drivers.Capabilities;
using Chroma.Natives.SDL;

namespace Chroma.Input.GameControllers.Drivers
{
    public class SwitchJoyConControllerDriver : ControllerDriver, IGyroscopeEnabled, IAccelerometerEnabled
    {
        private Dictionary<ControllerButton, ControllerButton> _leftButtonRemappings = new()
        {
            {ControllerButton.View, ControllerButton.Menu},
            {ControllerButton.Special, ControllerButton.Logo},
            {ControllerButton.LeftBottomPaddle, ControllerButton.RightBumper},
            {ControllerButton.RightBottomPaddle, ControllerButton.LeftBumper},
            {ControllerButton.LeftStick, ControllerButton.LeftStick},
            {ControllerButton.DpadUp, ControllerButton.X},
            {ControllerButton.DpadDown, ControllerButton.B},
            {ControllerButton.DpadLeft, ControllerButton.Y},
            {ControllerButton.DpadRight, ControllerButton.A},
        };

        private Dictionary<ControllerButton, ControllerButton> _rightButtonRemappings = new()
        {
            {ControllerButton.Menu, ControllerButton.Menu},
            {ControllerButton.Logo, ControllerButton.Logo},
            {ControllerButton.RightTopPaddle, ControllerButton.RightBumper},
            {ControllerButton.LeftTopPaddle, ControllerButton.LeftBumper},
            {ControllerButton.RightStick, ControllerButton.LeftStick},
            {ControllerButton.X, ControllerButton.X},
            {ControllerButton.B, ControllerButton.B},
            {ControllerButton.Y, ControllerButton.Y},
            {ControllerButton.A, ControllerButton.A},
        };

        private Dictionary<ControllerAxis, ControllerAxis> _leftAxisRemappings = new()
        {
            {ControllerAxis.LeftStickX, ControllerAxis.LeftStickX},
            {ControllerAxis.LeftStickY, ControllerAxis.LeftStickY}
        };

        private Dictionary<ControllerAxis, ControllerAxis> _rightAxisRemappings = new()
        {
            {ControllerAxis.LeftStickX, ControllerAxis.RightStickX},
            {ControllerAxis.LeftStickY, ControllerAxis.RightStickY}
        };

        private Vector3 _gyroscopeState;
        private Vector3 _accelerometerState;

        public override string Name { get; } = "Nintendo JoyCon Chroma Driver";

        public bool GyroscopeEnabled
        {
            get => SDL2.SDL_GameControllerIsSensorEnabled(Info.InstancePointer, SDL2.SDL_SensorType.SDL_SENSOR_GYRO);

            set
            {
                if (SDL2.SDL_GameControllerSetSensorEnabled(Info.InstancePointer, SDL2.SDL_SensorType.SDL_SENSOR_GYRO,
                    value) < 0)
                    _log.Error($"Failed to enable gyroscope: {SDL2.SDL_GetError()}");
            }
        }

        public bool AccelerometerEnabled
        {
            get => SDL2.SDL_GameControllerIsSensorEnabled(Info.InstancePointer, SDL2.SDL_SensorType.SDL_SENSOR_ACCEL);
            set
            {
                if (SDL2.SDL_GameControllerSetSensorEnabled(Info.InstancePointer, SDL2.SDL_SensorType.SDL_SENSOR_ACCEL,
                    value) < 0)
                    _log.Error($"Failed to enable accelerometer: {SDL2.SDL_GetError()}");
            }
        }

        public bool UseInputRemapping { get; set; } = true;
        public bool IgnoreMissingMappings { get; set; } = true;

        public bool IsLeftSide { get; }
        public bool IsRightSide => !IsLeftSide;

        public SwitchJoyConControllerDriver(ControllerInfo info) : base(info)
        {
            IsLeftSide = info.ProductInfo.ProductId == 0x2006;
        }

        public override short GetRawAxisValue(ControllerAxis axis)
        {
            if (UseInputRemapping)
            {
                if (TryRemapAxis(axis, out var remappedAxis))
                    return base.GetRawAxisValue(remappedAxis);
                else if (IgnoreMissingMappings)
                    return 0;
            }

            return base.GetRawAxisValue(axis);
        }

        public Vector3 ReadGyroscopeSensor()
        {
            var data = new float[3];

            unsafe
            {
                fixed (float* dataPtr = &data[0])
                {
                    if (SDL2.SDL_GameControllerGetSensorData(Info.InstancePointer, SDL2.SDL_SensorType.SDL_SENSOR_GYRO,
                        dataPtr, 3) < 0)
                    {
                        _log.Error($"Failed to retrieve gyroscope data: {SDL2.SDL_GetError()}");
                        return Vector3.Zero;
                    }
                }
            }

            _gyroscopeState.X = data[0];
            _gyroscopeState.Y = data[1];
            _gyroscopeState.Z = data[2];

            return _gyroscopeState;
        }

        public Vector3 ReadAccelerometerSensor()
        {
            var data = new float[3];

            unsafe
            {
                fixed (float* dataPtr = &data[0])
                {
                    if (SDL2.SDL_GameControllerGetSensorData(Info.InstancePointer, SDL2.SDL_SensorType.SDL_SENSOR_ACCEL,
                        dataPtr, 3) < 0)
                    {
                        _log.Error($"Failed to retrieve accelerometer data: {SDL2.SDL_GetError()}");
                        return Vector3.Zero;
                    }
                }
            }

            _accelerometerState.X = data[0];
            _accelerometerState.Y = data[1];
            _accelerometerState.Z = data[2];

            return _accelerometerState;
        }
        
        internal override void OnButtonPressed(ControllerButtonEventArgs e)
        {
            if (UseInputRemapping)
            {
                if (TryRemapButton(e.Button, out var remappedButton))
                    e.Button = remappedButton;
                else if (IgnoreMissingMappings)
                    return;
            }

            base.OnButtonPressed(e);
        }

        internal override void OnButtonReleased(ControllerButtonEventArgs e)
        {
            if (UseInputRemapping)
            {
                if (TryRemapButton(e.Button, out var remappedButton))
                    e.Button = remappedButton;
                else if (IgnoreMissingMappings)
                    return;
            }

            base.OnButtonReleased(e);
        }

        internal override void OnAxisMoved(ControllerAxisEventArgs e)
        {
            if (UseInputRemapping)
            {
                if (TryRemapAxis(e.Axis, out var remappedAxis))
                    e.Axis = remappedAxis;
                else if (IgnoreMissingMappings)
                    return;
            }
            
            base.OnAxisMoved(e);
        }
        
        private bool TryRemapButton(ControllerButton button, out ControllerButton remappedButton)
        {
            if (IsLeftSide && _leftButtonRemappings.ContainsKey(button))
            {
                remappedButton = _leftButtonRemappings[button];
                return true;
            }

            if (IsRightSide && _rightButtonRemappings.ContainsKey(button))
            {
                remappedButton = _rightButtonRemappings[button];
                return true;
            }

            remappedButton = button;
            return false;
        }

        private bool TryRemapAxis(ControllerAxis axis, out ControllerAxis remappedAxis)
        {
            if (IsLeftSide && _leftAxisRemappings.ContainsKey(axis))
            {
                remappedAxis = _leftAxisRemappings[axis];
                return true;
            }

            if (IsRightSide && _rightAxisRemappings.ContainsKey(axis))
            {
                remappedAxis = _rightAxisRemappings[axis];
                return true;
            }

            remappedAxis = axis;
            return false;
        }


        void IGyroscopeEnabled.OnGyroscopeStateChanged(float x, float y, float z)
        {
            _gyroscopeState.X = x;
            _gyroscopeState.Y = y;
            _gyroscopeState.Z = z;
        }

        void IAccelerometerEnabled.OnAccelerometerStateChanged(float x, float y, float z)
        {
            _accelerometerState.X = x;
            _accelerometerState.Y = y;
            _accelerometerState.Z = z;
        }
    }
}