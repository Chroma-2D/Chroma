namespace Chroma.Input.GameControllers
{
    public class ControllerEventArgs
    {
        public ControllerDriver Controller { get; }

        internal ControllerEventArgs(ControllerDriver controller)
        {
            Controller = controller;
        }
    }
}