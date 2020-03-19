using Chroma.SDL2;
using System;
using System.Collections.Generic;

namespace Chroma.Input
{
    public static class Controller
    {
        internal static Dictionary<int, short> DeadZones { get; }

        static Controller()
        {
            DeadZones = new Dictionary<int, short>();
        }

        public static void SetDeadZone(int index, short value)
        {
            if (!DeadZones.ContainsKey(index))
                DeadZones.Add(index, value);
            else
                DeadZones[index] = value;
        }

        public static string GetName(int index)
            => SDL.SDL_GameControllerNameForIndex(index);

        public static string GetMappings(int index)
            => SDL.SDL_GameControllerMappingForDeviceIndex(index);

        public static short GetAxisValue(int index, ControllerAxis axis)
        {
            var instance = SDL.SDL_GameControllerFromInstanceID(index);

            var axisValue = SDL.SDL_GameControllerGetAxis(
                instance,
                (SDL.SDL_GameControllerAxis)axis
            );

            if (DeadZones.ContainsKey(index))
            {
                if (Math.Abs((int)axisValue) < DeadZones[index])
                    return 0;
            }

            return axisValue;
        }

        public static float GetAxisValueNormalized(int index, ControllerAxis axis)
            => GetAxisValue(index, axis) / 32768f;

        public static bool IsButtonPressed(int index, ControllerButton button)
        {
            var instance = SDL.SDL_GameControllerFromInstanceID(index);

            return SDL.SDL_GameControllerGetButton(
                instance,
                (SDL.SDL_GameControllerButton)button
            ) > 0;
        }
    }
}
