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
    }
}
