namespace Chroma.Input
{
    public class ControllerButtonEventArgs
    {
        public ControllerInfo Controller { get; }
        public ControllerButton Button { get; }

        internal ControllerButtonEventArgs(ControllerInfo controller, ControllerButton button)
        {
            Controller = controller;
            Button = button;
        }
    }
}