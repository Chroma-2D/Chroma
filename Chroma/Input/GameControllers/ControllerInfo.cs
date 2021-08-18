using System;
using System.Collections.Generic;
using System.Text;
using Chroma.Hardware;

namespace Chroma.Input.GameControllers
{
    public class ControllerInfo
    {
        private Dictionary<ControllerAxis, bool> _supportedAxes;
        private Dictionary<ControllerButton, bool> _supportedButtons;

        internal IntPtr JoystickPointer { get; }
        internal IntPtr InstancePointer { get; }
        internal int InstanceId { get; }

        public Guid Guid { get; }
        public int PlayerIndex { get; }
        public string Name { get; }
        public string SerialNumber { get; }
        public ProductInfo ProductInfo { get; }
        public ControllerType Type { get; }
        public bool HasConfigurableLed { get; }
        public bool HasGyroscope { get; }
        public bool HasAccelerometer { get; }
        public int TouchpadCount { get; }
        public int[] TouchpadFingerLimit { get; }

        public IReadOnlyDictionary<ControllerAxis, bool> SupportedAxes => _supportedAxes;
        public IReadOnlyDictionary<ControllerButton, bool> SupportedButtons => _supportedButtons;
        
        internal ControllerInfo(
            IntPtr joystickPointer,
            IntPtr instancePointer,
            int instanceId,
            Guid guid,
            int playerIndex,
            string name,
            string serialNumber,
            ProductInfo productInfo,
            ControllerType type,
            bool hasConfigurableLed,
            bool hasGyroscope,
            bool hasAccelerometer,
            int touchpadCount,
            int[] touchpadFingerLimit,
            Dictionary<ControllerAxis, bool> supportedAxes,
            Dictionary<ControllerButton, bool> supportedButtons)
        {
            JoystickPointer = joystickPointer; 
            InstancePointer = instancePointer;
            InstanceId = instanceId;

            Guid = guid;
            PlayerIndex = playerIndex;
            Name = name;
            SerialNumber = serialNumber;
            ProductInfo = productInfo;
            Type = type;
            HasConfigurableLed = hasConfigurableLed;
            HasGyroscope = hasGyroscope;
            HasAccelerometer = hasAccelerometer;
            TouchpadCount = touchpadCount;
            TouchpadFingerLimit = touchpadFingerLimit;
            
            _supportedAxes = supportedAxes;
            _supportedButtons = supportedButtons;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"\"{Guid}\" {{");
            sb.AppendLine($"  Index: {PlayerIndex}");
            sb.AppendLine($"  Name: {Name}");
            sb.AppendLine($"  S/N: {SerialNumber}");
            sb.AppendLine($"  VID/PID: {ProductInfo}");
            sb.AppendLine($"  Type: {Type}");
            sb.AppendLine($"  Has configurable LED: {HasConfigurableLed}");
            sb.AppendLine($"  Has gyroscope: {HasGyroscope}");
            sb.AppendLine($"  Has accelerometer: {HasAccelerometer}");
            sb.AppendLine($"  Touchpad count: {TouchpadCount}");
            sb.AppendLine("  Supported axes {");

            foreach (var kvp in _supportedAxes)
                sb.AppendLine($"    {kvp.Key}: {kvp.Value}");

            sb.AppendLine("  }");

            sb.AppendLine("  Supported buttons {");
            foreach (var kvp in _supportedButtons)
                sb.AppendLine($"    {kvp.Key}: {kvp.Value}");

            sb.AppendLine("  }");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}