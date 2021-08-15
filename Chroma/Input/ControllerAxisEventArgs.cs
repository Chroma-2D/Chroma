namespace Chroma.Input
{
    public class ControllerAxisEventArgs
    {
        public Controller Controller { get; }
        public ControllerAxis Axis { get; }
        public short Value { get; }

        internal ControllerAxisEventArgs(Controller controller, ControllerAxis axis, short value)
        {
            Controller = controller;
            Axis = axis;
            Value = value;
        }
    }
}