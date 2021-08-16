using System;
using System.Numerics;

namespace Chroma.Input.GameControllers
{
    public class ControllerSensorEventArgs : EventArgs
    {
        public ControllerDriver Controller { get; }
        public Vector3 Values { get; }

        internal ControllerSensorEventArgs(ControllerDriver controller, float x, float y, float z)
        {
            Controller = controller;
            Values = new(x, y, z);
        }
    }
}
