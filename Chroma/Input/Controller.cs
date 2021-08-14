﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chroma.Diagnostics.Logging;
using Chroma.Hardware;
using Chroma.Input.Internal;
using Chroma.Natives.SDL;

namespace Chroma.Input
{
    public static class Controller
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();

        private static readonly HashSet<ControllerButton>[] _activeButtons =
            new HashSet<ControllerButton>[ControllerRegistry.MaxSupportedPlayers];
        
        public static IReadOnlyList<IReadOnlySet<ControllerButton>> ActiveKeys => Array.AsReadOnly(_activeButtons);
        public static int DeviceCount => ControllerRegistry.Instance.DeviceCount;

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

        public static void SetDeadZoneAllAxes(int playerIndex, ushort value)
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

            return SDL2.SDL_GameControllerName(controller.InstancePointer);
        }

        public static short GetAxisValue(int playerIndex, ControllerAxis axis)
        {
            var controller = ControllerRegistry.Instance.GetControllerInfo(playerIndex);

            if (controller == null)
                return 0;

            var axisValue = SDL2.SDL_GameControllerGetAxis(
                controller.InstancePointer,
                (SDL2.SDL_GameControllerAxis)axis
            );

            if (CanIgnoreAxisMotion(playerIndex, axis, axisValue))
                return 0;

            return axisValue;
        }

        public static float GetAxisValueNormalized(int playerIndex, ControllerAxis axis)
            => GetAxisValue(playerIndex, axis) / 32768f;

        public static bool IsButtonDown(int playerIndex, ControllerButton button)
            => _activeButtons[playerIndex] != null && _activeButtons[playerIndex].Contains(button);

        public static bool IsButtonUp(int playerIndex, ControllerButton button) => !IsButtonDown(playerIndex, button);

        public static void Vibrate(int playerIndex, ushort lowFreq, ushort highFreq, uint duration)
        {
            var controller = ControllerRegistry.Instance.GetControllerInfo(playerIndex);

            if (controller == null)
                return;

            SDL2.SDL_GameControllerRumble(
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

            var joystickInstance = SDL2.SDL_GameControllerGetJoystick(controller.InstancePointer);
            return (BatteryStatus)SDL2.SDL_JoystickCurrentPowerLevel(joystickInstance);
        }


        public static string RetrieveMapping(int playerIndex)
        {
            var controller = ControllerRegistry.Instance.GetControllerInfo(playerIndex);

            if (controller == null)
                return null;

            return SDL2.SDL_GameControllerMapping(controller.InstancePointer);
        }

        public static void AddMapping(string controllerMapping)
        {
            if (string.IsNullOrEmpty(controllerMapping))
            {
                _log.Warning("Tried to add a null or empty controller mapping.");
                return;
            }

            if (SDL2.SDL_GameControllerAddMapping(controllerMapping) < 0)
                _log.Error($"Failed to add a controller mapping: {SDL2.SDL_GetError()}.");
        }

        internal static void OnControllerConnected(Game game, ControllerEventArgs e)
        {
            _activeButtons[e.Controller.PlayerIndex] = new();
            game.OnControllerConnected(e);
        }

        internal static void OnControllerDisconnected(Game game, ControllerEventArgs e)
        {
            _activeButtons[e.Controller.PlayerIndex].Clear();
            game.OnControllerDisconnected(e);
        }

        internal static void OnButtonReleased(Game game, ControllerButtonEventArgs e)
        {
            _activeButtons[e.Controller.PlayerIndex] ??= new();
            _activeButtons[e.Controller.PlayerIndex].Remove(e.Button);

            game.OnControllerButtonReleased(e);
        }

        internal static void OnButtonPressed(Game game, ControllerButtonEventArgs e)
        {
            _activeButtons[e.Controller.PlayerIndex] ??= new();
            _activeButtons[e.Controller.PlayerIndex].Add(e.Button);

            game.OnControllerButtonPressed(e);
        }
    }
}