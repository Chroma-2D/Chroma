using System;
using System.Collections.Generic;
using Chroma.Hardware;
using Chroma.Input.GameControllers.Drivers;

namespace Chroma.Input.GameControllers
{
    public static class ControllerFactory
    {
        public delegate ControllerDriver ControllerFactoryDelegate(ControllerInfo info);

        private static Dictionary<ProductInfo, ControllerFactoryDelegate> _factoryDelegateLookup = new();
        
        internal static ControllerDriver Create(ControllerInfo info)
        {
            var type = info.Type;
            
            if (_factoryDelegateLookup.ContainsKey(info.ProductInfo))
                return _factoryDelegateLookup[info.ProductInfo](info);
            
            switch (type)
            {
                case ControllerType.Virtual:
                case ControllerType.Xbox360:
                case ControllerType.XboxOne:
                case ControllerType.PlayStation3:
                case ControllerType.PlayStation4:
                case ControllerType.PlayStation5:
                case ControllerType.NintendoSwitch:
                case ControllerType.Unknown:
                    return new GenericControllerDriver(info);
                
                default:
                    throw new NotSupportedException("Unrecognized controller type.");
            }
        }

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
    }
}