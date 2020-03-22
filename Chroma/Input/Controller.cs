using Chroma.Hardware;
using Chroma.Input.Internal;
using Chroma.SDL2;
using System;

namespace Chroma.Input
{
    public static class Controller
    {
        public static bool CanIgnoreAxisMotion(int playerIndex, ControllerAxis axis, short axisValue)
        {
            var controller = ControllerRegistry.Instance.GetControllerInfo(playerIndex);

            if (controller == null)
                return true;

            if (controller.DeadZones.ContainsKey(axis))
            {
                if (Math.Abs((int)axisValue) < controller.DeadZones[axis])
                    return true;
            }

            return false;
        }

        public static void SetDeadZone(int playerIndex, ControllerAxis axis, ushort value)
        {
            var controller = ControllerRegistry.Instance.GetControllerInfo(playerIndex);

            if (controller == null)
                return;

            if (!controller.DeadZones.ContainsKey(axis))
                controller.DeadZones.Add(axis, value);
            else
                controller.DeadZones[axis] = value;
        }

        public static void SetDeadZoneUniform(int playerIndex, ushort value)
        {
            var controller = ControllerRegistry.Instance.GetControllerInfo(playerIndex);

            if (controller == null)
                return;

            controller.DeadZones.Clear();

            SetDeadZone(playerIndex, ControllerAxis.LeftStickX, value);
            SetDeadZone(playerIndex, ControllerAxis.RightStickX, value);
            SetDeadZone(playerIndex, ControllerAxis.LeftStickY, value);
            SetDeadZone(playerIndex, ControllerAxis.RightStickY, value);
            SetDeadZone(playerIndex, ControllerAxis.LeftTrigger, value);
            SetDeadZone(playerIndex, ControllerAxis.RightTrigger, value);
        }

        public static string GetName(int playerIndex)
        {
            var controller = ControllerRegistry.Instance.GetControllerInfo(playerIndex);

            if (controller == null)
                return null;

            return SDL.SDL_GameControllerName(controller.InstancePointer);
        }

        public static string GetMappings(int playerIndex)
        {
            var controller = ControllerRegistry.Instance.GetControllerInfo(playerIndex);

            if (controller == null)
                return null;

            return SDL.SDL_GameControllerMapping(controller.InstancePointer);
        }

        public static short GetAxisValue(int playerIndex, ControllerAxis axis)
        {
            var controller = ControllerRegistry.Instance.GetControllerInfo(playerIndex);

            if (controller == null)
                return 0;

            var axisValue = SDL.SDL_GameControllerGetAxis(
                controller.InstancePointer,
                (SDL.SDL_GameControllerAxis)axis
            );

            if (CanIgnoreAxisMotion(playerIndex, axis, axisValue))
                return 0;

            return axisValue;
        }

        public static float GetAxisValueNormalized(int playerIndex, ControllerAxis axis)
            => GetAxisValue(playerIndex, axis) / 32768f;

        public static bool IsButtonPressed(int playerIndex, ControllerButton button)
        {
            var controller = ControllerRegistry.Instance.GetControllerInfo(playerIndex);

            if (controller == null)
                return false;

            return SDL.SDL_GameControllerGetButton(
                controller.InstancePointer,
                (SDL.SDL_GameControllerButton)button
            ) > 0;
        }

        public static void Vibrate(int playerIndex, ushort lowFreq, ushort highFreq, uint duration)
        {
            var controller = ControllerRegistry.Instance.GetControllerInfo(playerIndex);

            if (controller == null)
                return;

            SDL.SDL_GameControllerRumble(
                controller.InstancePointer,
                lowFreq,
                highFreq,
                duration
            );
        }

        public static BatteryStatus GetBatteryLevel(int playerIndex)
        {
            var controller = ControllerRegistry.Instance.GetControllerInfo(playerIndex);

            if (controller == null)
                return BatteryStatus.Unknown;

            var joystickInstance = SDL.SDL_GameControllerGetJoystick(controller.InstancePointer);
            return (BatteryStatus)SDL.SDL_JoystickCurrentPowerLevel(joystickInstance);
        }
    }
}
