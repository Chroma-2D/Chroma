using System;
using System.Collections.Generic;
using Chroma.Hardware;
using Chroma.Input.GameControllers.Drivers;
using Chroma.Natives.SDL;

namespace Chroma.Input.GameControllers
{
    internal static class ControllerFactory
    {
        public delegate ControllerDriver ControllerFactoryDelegate(ControllerInfo info);

        private static Dictionary<ProductInfo, ControllerFactoryDelegate> _factoryDelegateLookup = new();

        public static bool IsDriverPresent(ProductInfo info)
            => _factoryDelegateLookup.ContainsKey(info);

        public static bool RegisterDriver(ProductInfo info, ControllerFactoryDelegate provider)
        {
            if (_factoryDelegateLookup.ContainsKey(info))
                return false;

            _factoryDelegateLookup.Add(info, provider);
            return true;
        }

        public static bool UnregisterDriver(ProductInfo info)
        {
            if (!IsDriverPresent(info))
                return false;

            _factoryDelegateLookup.Remove(info);
            return true;
        }
        
        internal static ControllerDriver Create(ControllerInfo info)
        {
            var type = info.Type;

            if (_factoryDelegateLookup.ContainsKey(info.ProductInfo))
                return _factoryDelegateLookup[info.ProductInfo](info);

            switch (type)
            {
                case ControllerType.PlayStation5:
                    return new DualSenseControllerDriver(info);

                case ControllerType.PlayStation4:
                    return new DualShockControllerDriver(info);

                case ControllerType.NintendoSwitch:
                    return CreateNintendoDriverInstance(info);

                case ControllerType.Xbox360:
                case ControllerType.XboxOne:
                case ControllerType.PlayStation3:
                case ControllerType.Virtual:
                case ControllerType.Unknown:
                    return new GenericControllerDriver(info);

                default:
                    throw new InvalidOperationException("Unrecognized controller type.");
            }
        }
        
        private static ControllerDriver CreateNintendoDriverInstance(ControllerInfo info)
        {
            if (info.ProductInfo.VendorId != 0x057E)
                throw new InvalidOperationException("Vendor ID mismatch, expected Nintendo VID.");

            switch (info.ProductInfo.ProductId)
            {
                case 0x2006:
                case 0x2007:
                    return new SwitchJoyConControllerDriver(info);
                case 0x2009:
                    return new SwitchProControllerDriver(info);
            }

            throw new InvalidOperationException("Unsupported product ID.");
        }
    }
}