using System;
using System.Numerics;

namespace Chroma.Input.GameControllers
{
    public sealed class ControllerSensorEventArgs
    {
        public ControllerDriver Controller { get; }
        public Vector3 Values { get; }

        private ControllerSensorEventArgs(ControllerDriver controller, Vector3 values)
        {
            Controller = controller;
            Values = values;
        }
        
        internal ControllerSensorEventArgs(ControllerDriver controller, float x, float y, float z)
            : this(controller, new(x, y, z)) { }

        public ControllerSensorEventArgs With(float x, float y, float z) => new(
            Controller,
            x, y, z
        );

        public ControllerSensorEventArgs With(Vector3 values) => new(
            Controller,
            values
        );

        public ControllerSensorEventArgs WithX(float x) => With(
            x,
            Values.Y,
            Values.Z
        );

        public ControllerSensorEventArgs WithY(float y) => With(
            Values.X,
            y,
            Values.Z
        );

        public ControllerSensorEventArgs WithZ(float z) => With(
            Values.X,
            Values.Y,
            z
        );
    }
}
