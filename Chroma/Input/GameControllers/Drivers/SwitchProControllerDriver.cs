using System.Collections.Generic;
using System.Numerics;
using Chroma.Input.GameControllers.Drivers.Capabilities;
using Chroma.Natives.SDL;

namespace Chroma.Input.GameControllers.Drivers
{
    public sealed class SwitchProControllerDriver  : NintendoControllerDriver, IGyroscopeEnabled, IAccelerometerEnabled
    {
        private Vector3 _gyroscopeState;
        private Vector3 _accelerometerState;
        
        private Dictionary<ControllerButton, ControllerButton> _remappedXboxLayout = new()
        {
            {ControllerButton.X, ControllerButton.Y},
            {ControllerButton.Y, ControllerButton.X},
            {ControllerButton.B, ControllerButton.A},
            {ControllerButton.A, ControllerButton.B}
        };
        
        public override string Name { get; } = "Nintendo Pro Controller Chroma Driver";
        
        public override bool UseXboxButtonLayout { get; set; }
        
        public Vector3 Gyroscope => _gyroscopeState;
        public Vector3 Accelerometer => _accelerometerState;
        
        public bool GyroscopeEnabled
        {
            get => SDL2.SDL_GameControllerIsSensorEnabled(Info.InstancePointer, SDL2.SDL_SensorType.SDL_SENSOR_GYRO);

            set
            {
                if (SDL2.SDL_GameControllerSetSensorEnabled(Info.InstancePointer, SDL2.SDL_SensorType.SDL_SENSOR_GYRO, value) < 0)
                    _log.Error($"Failed to enable gyroscope: {SDL2.SDL_GetError()}");
            }
        }

        public bool AccelerometerEnabled
        {
            get => SDL2.SDL_GameControllerIsSensorEnabled(Info.InstancePointer, SDL2.SDL_SensorType.SDL_SENSOR_ACCEL);
            set
            {
                if (SDL2.SDL_GameControllerSetSensorEnabled(Info.InstancePointer, SDL2.SDL_SensorType.SDL_SENSOR_ACCEL, value) < 0)
                    _log.Error($"Failed to enable accelerometer: {SDL2.SDL_GetError()}");
            }
        }
        
        public SwitchProControllerDriver(ControllerInfo info) : base(info)
        {
            UseXboxButtonLayout = false;
        }

        internal override void OnButtonPressed(ControllerButtonEventArgs e)
        {
            if (UseXboxButtonLayout && _remappedXboxLayout.TryGetValue(e.Button, out var remappedButton))
            {
                e.Button = remappedButton;
                base.OnButtonPressed(e);
                return;
            }

            base.OnButtonPressed(e);
        }

        internal override void OnButtonReleased(ControllerButtonEventArgs e)
        {
            if (UseXboxButtonLayout && _remappedXboxLayout.TryGetValue(e.Button, out var remappedButton))
            {
                e.Button = remappedButton;
                base.OnButtonReleased(e);
                return;
            }

            base.OnButtonReleased(e);
        }

        public Vector3 ReadGyroscopeSensor()
        {
            var data = new float[3];

            unsafe
            {
                fixed (float* dataPtr = &data[0])
                {
                    if (SDL2.SDL_GameControllerGetSensorData(Info.InstancePointer, SDL2.SDL_SensorType.SDL_SENSOR_GYRO, dataPtr, 3) < 0)
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
                    if (SDL2.SDL_GameControllerGetSensorData(Info.InstancePointer, SDL2.SDL_SensorType.SDL_SENSOR_ACCEL, dataPtr, 3) < 0)
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