namespace Chroma.Input
{
    public class ControllerEventArgs : System.EventArgs
    {
        public ControllerInfo Controller { get; }

        internal ControllerEventArgs(ControllerInfo controller)
        {
            Controller = controller;
        }
    }
}