using System;
using System.Numerics;

namespace Chroma.Input.GameControllers
{
    public sealed class ControllerSensorEventArgs
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
