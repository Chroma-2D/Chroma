namespace Chroma.Input.GameControllers
{
    public sealed class ControllerAxisEventArgs
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

        public ControllerAxisEventArgs With(ControllerAxis axis, short value) => new(
            Controller,
            axis,
            value
        );

        public ControllerAxisEventArgs WithAxis(ControllerAxis axis) => With(
            axis,
            Value
        );

        public ControllerAxisEventArgs WithValue(short value) => With(
            Axis,
            value
        );
    }
}