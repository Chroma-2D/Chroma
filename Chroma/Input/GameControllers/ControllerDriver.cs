using System;
using System.Collections.Generic;
using System.Text;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Hardware;
using Chroma.Natives.SDL;

namespace Chroma.Input.GameControllers
{
    public abstract class ControllerDriver
    {
        protected static Log _log = LogManager.GetForCurrentAssembly();

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

        public ControllerDriver(ControllerInfo info)
        {
            Info = info;
        }

        public T As<T>() where T : ControllerDriver
            => this as T;

        public bool Is<T>() where T : ControllerDriver
            => this is T;

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

        public virtual string RetrieveMapping()
        {
            if (Info == null || Info.InstancePointer == IntPtr.Zero)
                return string.Empty;

            return SDL2.SDL_GameControllerMapping(Info.InstancePointer);
        }

        public virtual void SendEffect(byte[] data)
        {
            if (Info == null || Info.InstancePointer == IntPtr.Zero)
                return;

            unsafe
            {
                fixed (byte* bp = &data[0])
                {
                    if (SDL2.SDL_GameControllerSendEffect(Info.InstancePointer, bp, data.Length) < 0)
                        _log.Error($"Failed to send effect: {SDL2.SDL_GetError()}");
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

        internal void OnButtonPressed(ControllerButton button)
            => _buttonStates.Add(button);

        internal void OnButtonReleased(ControllerButton button)
            => _buttonStates.Remove(button);

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Driver \"{Name}\":");
            sb.AppendLine(Info.ToString());

            return sb.ToString();
        }
    }
}