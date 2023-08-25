using System;
using System.Collections.Generic;
using System.Text;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Hardware;
using Chroma.Natives.Bindings.SDL;

namespace Chroma.Input.GameControllers
{
    public abstract class ControllerDriver
    {
        protected static readonly Log _log = LogManager.GetForCurrentAssembly();

        protected Dictionary<ControllerAxis, ushort> _deadZones = new();
        protected HashSet<ControllerButton> _buttonStates = new();

        public IReadOnlyDictionary<ControllerAxis, ushort> DeadZones => _deadZones;
        public IReadOnlySet<ControllerButton> ActiveButtons => _buttonStates;

        public ControllerInfo Info { get; }
        public abstract string Name { get; }

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

        internal ControllerDriver(ControllerInfo info)
        {
            Info = info;
        }

        public T As<T>() where T : ControllerDriver
            => this as T;

        public bool Is<T>() where T : ControllerDriver
            => this is T;

        public virtual void SetDeadZone(ControllerAxis axis, ushort value)
        {
            if (!_deadZones.TryAdd(axis, value))
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

        public virtual bool CanIgnoreAxisMotion(ControllerAxis axis, int axisValue)
        {
            return _deadZones.ContainsKey(axis)
                   && Math.Abs(axisValue) < _deadZones[axis];
        }

        public virtual int GetRawAxisValue(ControllerAxis axis)
        {
            if (Info == null || Info.InstancePointer == IntPtr.Zero)
                return 0;

            return SDL2.SDL_GameControllerGetAxis(
                Info.InstancePointer,
                (SDL2.SDL_GameControllerAxis)axis
            );
        }

        public virtual float GetRawAxisValueNormalized(ControllerAxis axis)
            => GetRawAxisValue(axis) / 32768f;

        public virtual int GetAxisValue(ControllerAxis axis)
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

            if (SDL2.SDL_GameControllerRumble(
                    Info.InstancePointer,
                    leftIntensity,
                    rightIntensity,
                    duration
                ) < 0)
            {
                _log.Error(
                    $"Failed to rumble controller at player index {Info.PlayerIndex}, '{Info.Name}': {SDL2.SDL_GetError()}");
            }
        }

        public virtual void RumbleTriggers(ushort leftIntensity, ushort rightIntensity, uint duration)
        {
            if (Info == null || Info.InstancePointer == IntPtr.Zero)
                return;

            if (SDL2.SDL_GameControllerRumbleTriggers(
                    Info.InstancePointer,
                    leftIntensity,
                    rightIntensity,
                    duration
                ) < 0)
            {
                _log.Error(
                    $"Failed to rumble controller triggers at player index {Info.PlayerIndex}, '{Info.Name}': {SDL2.SDL_GetError()}");
            }
        }

        public virtual string RetrieveMapping()
        {
            if (Info == null || Info.InstancePointer == IntPtr.Zero)
                return string.Empty;

            return SDL2.SDL_GameControllerMapping(Info.InstancePointer);
        }

        internal unsafe void SendEffect(byte* ptr, int length)
        {
            if (SDL2.SDL_GameControllerSendEffect(Info.InstancePointer, ptr, length) < 0)
                _log.Error($"Failed to send effect: {SDL2.SDL_GetError()}");
        }

        public virtual void SendEffect(byte[] data)
        {
            if (Info == null || Info.InstancePointer == IntPtr.Zero)
                return;

            unsafe
            {
                fixed (byte* bp = &data[0])
                {
                    SendEffect(bp, data.Length);
                }
            }
        }

        public virtual void SetLedColor(Color color)
        {
            if (!Info.HasConfigurableLed)
                return;

            if (SDL2.SDL_GameControllerSetLED(Info.InstancePointer, color.R, color.G, color.B) < 0)
                _log.Error($"Failed to set LED color: {SDL2.SDL_GetError()}");
        }

        internal virtual void OnButtonPressed(ControllerButtonEventArgs e)
            => _buttonStates.Add(e.Button);

        internal virtual void OnButtonReleased(ControllerButtonEventArgs e)
            => _buttonStates.Remove(e.Button);

        internal virtual void OnAxisMoved(ControllerAxisEventArgs e)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Driver \"{Name}\":");
            sb.AppendLine(Info.ToString());

            return sb.ToString();
        }
    }
}