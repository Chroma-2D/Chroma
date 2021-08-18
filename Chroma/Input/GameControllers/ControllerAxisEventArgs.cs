namespace Chroma.Input.GameControllers
{
    public class ControllerAxisEventArgs
    {
        public ControllerDriver Controller { get; }
        public ControllerAxis Axis { get; internal set; }
        public short Value { get; internal set; }

        internal ControllerAxisEventArgs(ControllerDriver controller, ControllerAxis axis, short value)
        {
            Controller = controller;
            Axis = axis;
            Value = value;
        }
    }
}