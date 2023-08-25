using System;
using Chroma.Input.GameControllers;

namespace Chroma
{
    public static partial class Extensions
    {
        public static void Rumble(
            this ControllerDriver controllerDriver,
            float leftIntensity,
            float rightIntensity,
            uint duration)
        {
            if (leftIntensity < 0.0f)
                leftIntensity = 0.0f;
            else if (leftIntensity > 1.0f)
                leftIntensity = 1.0f;

            if (rightIntensity < 0.0f)
                rightIntensity = 0.0f;
            else if (rightIntensity > 1.0f)
                rightIntensity = 1.0f;

            controllerDriver.Rumble(
                (ushort)(ushort.MaxValue * leftIntensity),
                (ushort)(ushort.MaxValue * rightIntensity),
                duration
            );
        }

        public static void RumbleTriggers(
            this ControllerDriver controllerDriver,
            float leftIntensity,
            float rightIntensity,
            uint duration)
        {
            if (leftIntensity < 0.0f)
                leftIntensity = 0.0f;
            else if (leftIntensity > 1.0f)
                leftIntensity = 1.0f;

            if (rightIntensity < 0.0f)
                rightIntensity = 0.0f;
            else if (rightIntensity > 1.0f)
                rightIntensity = 1.0f;
            
            controllerDriver.RumbleTriggers(
                (ushort)(ushort.MaxValue * leftIntensity),
                (ushort)(ushort.MaxValue * rightIntensity),
                duration
            );
        }
    }
}