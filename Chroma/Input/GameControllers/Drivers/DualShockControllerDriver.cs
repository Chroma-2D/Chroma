using Chroma.Graphics;
using Chroma.Input.GameControllers.Drivers.Capabilities;
using Chroma.Natives.SDL;
using System.Collections.Generic;
using System.Numerics;

namespace Chroma.Input.GameControllers.Drivers
{
    public sealed class DualShockControllerDriver : TouchpadEnabledControllerDriver, IGyroscopeEnabled
    {
        private Vector3 _gyroscopeState;

        public override string Name { get; } = "Sony DualShock 4 Chroma Driver";

        public new IReadOnlyCollection<ControllerTouchPoint> TouchPoints => _touchPoints[0];

        public bool GyroscopeEnabled
        {
            get => SDL2.SDL_GameControllerIsSensorEnabled(Info.InstancePointer, SDL2.SDL_SensorType.SDL_SENSOR_GYRO);

            set
            {
                if (SDL2.SDL_GameControllerSetSensorEnabled(Info.InstancePointer, SDL2.SDL_SensorType.SDL_SENSOR_GYRO, value) < 0)
                    _log.Error($"Failed to enable gyroscope: {SDL2.SDL_GetError()}");
            }
        }

        public Vector3 Gyroscope => _gyroscopeState;

        internal DualShockControllerDriver(ControllerInfo info)
            : base(info)
        {
        }

        public void SetLedColor(Color color)
        {
            if (SDL2.SDL_GameControllerSetLED(Info.InstancePointer, color.R, color.G, color.B) < 0)
                _log.Error(SDL2.SDL_GetError());
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

        void IGyroscopeEnabled.OnGyroscopeStateChanged(float x, float y, float z)
        {
            _gyroscopeState.X = x;
            _gyroscopeState.Y = y;
            _gyroscopeState.Z = z;
        }
    }
}