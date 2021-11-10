namespace Chroma.Input.GameControllers.Drivers
{
    public sealed class DualSenseControllerDriver : ControllerDriver
    {
        public override string Name => "Sony DualSense Chroma Driver";

        public DualSenseControllerDriver(ControllerInfo info) : base(info)
        {
        }
    }
}