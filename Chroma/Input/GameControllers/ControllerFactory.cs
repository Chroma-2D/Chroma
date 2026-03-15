namespace Chroma.Input.GameControllers;

using System;
using System.Collections.Generic;
using Chroma.Hardware;
using Chroma.Input.GameControllers.Drivers;
using Chroma.Input.GameControllers.Drivers.Nintendo;
using Chroma.Input.GameControllers.Drivers.Sony;

internal static class ControllerFactory
{
    public delegate ControllerDriver ControllerFactoryDelegate(ControllerInfo info);

    private static readonly Dictionary<ProductInfo, ControllerFactoryDelegate> _factoryDelegateLookup = new();

    public static bool IsDriverPresent(ProductInfo info)
        => _factoryDelegateLookup.ContainsKey(info);

    public static bool RegisterDriver(ProductInfo info, ControllerFactoryDelegate provider)
        => _factoryDelegateLookup.TryAdd(info, provider);

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

        if (_factoryDelegateLookup.TryGetValue(info.ProductInfo, out var factoryDelegate))
            return factoryDelegate(info);

        return type switch
        {
            ControllerType.PlayStation5 => new DualSenseControllerDriver(info),
            ControllerType.PlayStation4 => new DualShockControllerDriver(info),
            ControllerType.NintendoSwitch => CreateNintendoDriverInstance(info),
            ControllerType.Xbox360 or 
            ControllerType.XboxOne or 
            ControllerType.PlayStation3 or 
            ControllerType.Virtual or 
            ControllerType.Unknown or 
            ControllerType.AmazonLuna or 
            ControllerType.GoogleStadia => new GenericControllerDriver(info),
            _ => throw new InvalidOperationException("Unrecognized controller type.")
        };
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