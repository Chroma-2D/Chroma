namespace Chroma.Input.GameControllers.Drivers.Nintendo
{
    public abstract class NintendoControllerDriver : ControllerDriver
    {
        public abstract bool UseXboxButtonLayout { get; set; }

        protected NintendoControllerDriver(ControllerInfo info) 
            : base(info)
        {
        }
    }
}