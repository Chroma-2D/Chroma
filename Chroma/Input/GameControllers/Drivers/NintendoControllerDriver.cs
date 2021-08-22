namespace Chroma.Input.GameControllers.Drivers
{
    public abstract class NintendoControllerDriver : ControllerDriver
    {
        public abstract bool UseXboxButtonLayout { get; set; }

        protected NintendoControllerDriver(ControllerInfo info) : base(info)
        {
        }
    }
}