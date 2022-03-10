namespace Chroma.Input.GameControllers.Drivers.Sony
{
    public sealed class DualShockControllerDriver : SonyControllerDriver
    {
        public override string Name => "Sony DualShock 4 Chroma Driver";

        internal DualShockControllerDriver(ControllerInfo info)
            : base(info)
        {
        }
    }
}