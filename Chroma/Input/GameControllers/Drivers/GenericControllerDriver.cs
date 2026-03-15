namespace Chroma.Input.GameControllers.Drivers;

public sealed class GenericControllerDriver(ControllerInfo info)
    : ControllerDriver(info)
{
    public override string Name => "Generic Chroma Controller Driver";
}