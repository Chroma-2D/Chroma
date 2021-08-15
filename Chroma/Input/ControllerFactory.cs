using System;

namespace Chroma.Input
{
    internal static class ControllerFactory
    {
        internal static Controller Create(ControllerType type, ControllerInfo info)
        {
            switch (type)
            {
                case ControllerType.Unknown:
                case ControllerType.Virtual:
                case ControllerType.Xbox360:
                case ControllerType.XboxOne:
                case ControllerType.PlayStation3:
                case ControllerType.PlayStation4:
                case ControllerType.PlayStation5:
                case ControllerType.NintendoSwitch:
                    return new Controller(info);

                default:
                    throw new NotSupportedException("Unrecognized controller type.");
            }
        }
    }
}