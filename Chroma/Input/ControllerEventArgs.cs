namespace Chroma.Input
{
    public class ControllerEventArgs
    {
        public ControllerInfo Controller { get; }

        internal ControllerEventArgs(ControllerInfo controller)
        {
            Controller = controller;
        }
    }
}