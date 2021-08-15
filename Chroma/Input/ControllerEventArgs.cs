namespace Chroma.Input
{
    public class ControllerEventArgs
    {
        public Controller Controller { get; }

        internal ControllerEventArgs(Controller controller)
        {
            Controller = controller;
        }
    }
}