namespace Chroma.Input.EventArgs
{
    public class ControllerButtonEventArgs : System.EventArgs
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
