namespace Chroma;

using Chroma.Input.GameControllers;

public static partial class Extensions
{
    extension(ControllerDriver controllerDriver)
    {
        public void Rumble(float leftIntensity,
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

        public void RumbleTriggers(float leftIntensity,
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