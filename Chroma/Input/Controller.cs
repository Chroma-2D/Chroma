using Chroma.Input.Internal;
using Chroma.SDL2;
using System;
using System.Collections.Generic;

namespace Chroma.Input
{
    public static class Controller
    {
        internal static Dictionary<int, ushort> DeadZones { get; }
        
        static Controller()
        {
            DeadZones = new Dictionary<int, ushort>();
        }

        public static void SetDeadZone(int playerIndex, ushort value)
        {
            var controller = ControllerRegistry.Instance.GetControllerInfo(playerIndex);

            if (controller == null)
                return;

            if (!DeadZones.ContainsKey(playerIndex))
                DeadZones.Add(playerIndex, value);
            else
                DeadZones[playerIndex] = value;
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

            if (DeadZones.ContainsKey(playerIndex))
            {
                if (Math.Abs((int)axisValue) < DeadZones[playerIndex])
                    return 0;
            }

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
    }
}
