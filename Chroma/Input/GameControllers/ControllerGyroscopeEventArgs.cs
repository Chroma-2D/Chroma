using System;
using System.Numerics;

namespace Chroma.Input.GameControllers
{
    public class ControllerGyroscopeEventArgs : EventArgs
    {
        public ControllerDriver Controller { get; }
        public Vector3 Position { get; }

        internal ControllerGyroscopeEventArgs(ControllerDriver controller, float x, float y, float z)
        {
            Controller = controller;
            Position = new(x, y, z);
        }
    }
}
