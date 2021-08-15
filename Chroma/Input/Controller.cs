using System;
using System.Collections.Generic;
using Chroma.Diagnostics.Logging;
using Chroma.Hardware;
using Chroma.Natives.SDL;

namespace Chroma.Input
{
    public class Controller
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();

        protected Dictionary<ControllerAxis, ushort> _deadZones = new();
        protected HashSet<ControllerButton> _buttonStates = new();

        public IReadOnlyDictionary<ControllerAxis, ushort> DeadZones => _deadZones;
        public IReadOnlySet<ControllerButton> ActiveButtons => _buttonStates;

        public ControllerInfo Info { get; }
        public static int DeviceCount => ControllerRegistry.Instance.DeviceCount;

        public virtual BatteryStatus BatteryStatus
        {
            get
            {
                if (Info == null || Info.InstancePointer == IntPtr.Zero)
                    return BatteryStatus.Unknown;

                var joystickInstance = SDL2.SDL_GameControllerGetJoystick(Info.InstancePointer);
                return (BatteryStatus)SDL2.SDL_JoystickCurrentPowerLevel(joystickInstance);
            }
        }

        internal Controller(ControllerInfo info)
        {
            Info = info;
        }

        public T As<T>() where T : Controller
            => this as T;

        public virtual void SetDeadZone(ControllerAxis axis, ushort value)
        {
            if (!_deadZones.ContainsKey(axis))
                _deadZones.Add(axis, value);
            else
                _deadZones[axis] = value;
        }

        public virtual void SetDeadZoneAllAxes(ushort value)
        {
            SetDeadZone(ControllerAxis.LeftStickX, value);
            SetDeadZone(ControllerAxis.RightStickX, value);
            SetDeadZone(ControllerAxis.LeftStickY, value);
            SetDeadZone(ControllerAxis.RightStickY, value);
            SetDeadZone(ControllerAxis.LeftTrigger, value);
            SetDeadZone(ControllerAxis.RightTrigger, value);
        }

        public virtual bool CanIgnoreAxisMotion(ControllerAxis axis, short axisValue)
        {
            return _deadZones.ContainsKey(axis)
                   && Math.Abs((int)axisValue) < _deadZones[axis];
        }

        public virtual short GetRawAxisValue(ControllerAxis axis)
        {
            if (Info == null || Info.InstancePointer == IntPtr.Zero)
                return 0;

            var axisValue = SDL2.SDL_GameControllerGetAxis(
                Info.InstancePointer,
                (SDL2.SDL_GameControllerAxis)axis
            );

            return axisValue;
        }

        public virtual float GetRawAxisValueNormalized(ControllerAxis axis)
            => GetRawAxisValue(axis) / 32768f;

        public virtual short GetAxisValue(ControllerAxis axis)
        {
            var axisValue = GetRawAxisValue(axis);

            if (CanIgnoreAxisMotion(axis, axisValue))
                return 0;

            return axisValue;
        }

        public virtual float GetAxisValueNormalized(ControllerAxis axis)
            => GetAxisValue(axis) / 32768f;

        public virtual bool IsButtonDown(ControllerButton button)
            => _buttonStates.Contains(button);

        public virtual bool IsButtonUp(ControllerButton button)
            => !IsButtonDown(button);

        public virtual void Rumble(ushort leftIntensity, ushort rightIntensity, uint duration)
        {
            if (Info == null || Info.InstancePointer == IntPtr.Zero)
                return;

            SDL2.SDL_GameControllerRumble(
                Info.InstancePointer,
                leftIntensity,
                rightIntensity,
                duration
            );
        }

        public string RetrieveMapping()
        {
            if (Info == null || Info.InstancePointer == IntPtr.Zero)
                return string.Empty;

            return SDL2.SDL_GameControllerMapping(Info.InstancePointer);
        }

        public static void SetDeadZone(int playerIndex, ControllerAxis axis, ushort value)
        {
            var controller = ControllerRegistry.Instance.GetController(playerIndex);

            if (controller == null)
                return;

            controller.SetDeadZone(axis, value);
        }

        public static void SetDeadZoneAllAxes(int playerIndex, ushort value)
        {
            var controller = ControllerRegistry.Instance.GetController(playerIndex);

            if (controller == null)
                return;

            controller.SetDeadZoneAllAxes(value);
        }

        public static bool CanIgnoreAxisMotion(int playerIndex, ControllerAxis axis, short axisValue)
        {
            var controller = ControllerRegistry.Instance.GetController(playerIndex);

            if (controller == null)
                return false;

            return controller.CanIgnoreAxisMotion(axis, axisValue);
        }

        public static short GetRawAxisValue(int playerIndex, ControllerAxis axis)
        {
            var controller = ControllerRegistry.Instance.GetController(playerIndex);

            if (controller == null)
                return 0;

            return controller.GetRawAxisValue(axis);
        }

        public static float GetRawAxisValueNormalized(int playerIndex, ControllerAxis axis)
        {
            var controller = ControllerRegistry.Instance.GetController(playerIndex);

            if (controller == null)
                return 0;

            return controller.GetRawAxisValueNormalized(axis);
        }

        public static short GetAxisValue(int playerIndex, ControllerAxis axis)
        {
            var controller = ControllerRegistry.Instance.GetController(playerIndex);

            if (controller == null)
                return 0;

            return controller.GetAxisValue(axis);
        }

        public static float GetAxisValueNormalized(int playerIndex, ControllerAxis axis)
        {
            var controller = ControllerRegistry.Instance.GetController(playerIndex);

            if (controller == null)
                return 0;

            return controller.GetAxisValueNormalized(axis);
        }

        public static bool IsButtonDown(int playerIndex, ControllerButton button)
        {
            var controller = ControllerRegistry.Instance.GetController(playerIndex);

            if (controller == null)
                return false;

            return controller.IsButtonDown(button);
        }

        public static bool IsButtonUp(int playerIndex, ControllerButton button)
            => IsButtonDown(playerIndex, button);

        public static void Rumble(int playerIndex, ushort leftIntensity, ushort rightIntensity, uint duration)
        {
            var controller = ControllerRegistry.Instance.GetController(playerIndex);

            if (controller == null)
                return;

            controller.Rumble(leftIntensity, rightIntensity, duration);
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

        public static string RetrieveMapping(int playerIndex)
        {
            var controller = ControllerRegistry.Instance.GetController(playerIndex);

            if (controller == null)
                return string.Empty;

            return controller.RetrieveMapping();
        }

        public static IReadOnlySet<ControllerButton> GetActiveButtons(int playerIndex)
        {
            var controller = ControllerRegistry.Instance.GetController(playerIndex);

            if (controller == null)
                return null;

            return controller.ActiveButtons;
        }

        internal static void OnConnected(Game game, ControllerEventArgs e)
        {
            game.OnControllerConnected(e);
        }

        internal static void OnDisconnected(Game game, ControllerEventArgs e)
        {
            game.OnControllerDisconnected(e);
        }

        internal static void OnButtonReleased(Game game, ControllerButtonEventArgs e)
        {
            e.Controller._buttonStates.Remove(e.Button);
            game.OnControllerButtonReleased(e);
        }

        internal static void OnButtonPressed(Game game, ControllerButtonEventArgs e)
        {
            e.Controller._buttonStates.Add(e.Button);
            game.OnControllerButtonPressed(e);
        }
    }
}