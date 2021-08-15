namespace Chroma.Input
{
    public class ControllerButtonEventArgs
    {
        public Controller Controller { get; }
        public ControllerButton Button { get; }

        internal ControllerButtonEventArgs(Controller controller, ControllerButton button)
        {
            Controller = controller;
            Button = button;
        }
    }
}