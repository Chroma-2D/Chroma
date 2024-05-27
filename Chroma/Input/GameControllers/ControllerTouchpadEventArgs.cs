using System;
using System.Numerics;

namespace Chroma.Input.GameControllers
{
    public sealed class ControllerTouchpadEventArgs
    {
        public ControllerDriver Controller { get; }
        public int TouchpadIndex { get; }
        
        public int FingerIndex { get; }
        public Vector2 Position { get; }
        public float Pressure { get; }

        internal ControllerTouchpadEventArgs(ControllerDriver controller, int touchpadIndex, int fingerIndex, Vector2 position, float pressure)
        {
            Controller = controller;
            TouchpadIndex = touchpadIndex;
            FingerIndex = fingerIndex;
            Position = position;
            Pressure = pressure;
        }

        public ControllerTouchpadEventArgs With(int touchpadIndex, int fingerIndex, Vector2 position, float pressure) => new(
            Controller,
            touchpadIndex, 
            fingerIndex,
            position,
            pressure
        );

        public ControllerTouchpadEventArgs WithTouchpadIndex(int touchpadIndex) => With(
            touchpadIndex,
            FingerIndex,
            Position,
            Pressure
        );

        public ControllerTouchpadEventArgs WithFingerIndex(int fingerIndex) => With(
            TouchpadIndex,
            fingerIndex,
            Position,
            Pressure
        );

        public ControllerTouchpadEventArgs WithPosition(Vector2 position) => With(
            TouchpadIndex,
            FingerIndex,
            position,
            Pressure
        );

        public ControllerTouchpadEventArgs WithPressure(float pressure) => With(
            TouchpadIndex,
            FingerIndex,
            Position,
            pressure
        );
    }
}
