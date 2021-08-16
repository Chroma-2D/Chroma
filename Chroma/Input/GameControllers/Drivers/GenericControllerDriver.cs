namespace Chroma.Input.GameControllers.Drivers
{
    public sealed class GenericControllerDriver : ControllerDriver
    {
        public override string Name { get; } = "Generic Chroma Controller Driver";

        public GenericControllerDriver(ControllerInfo info) 
            : base(info)
        {
        }
    }
}