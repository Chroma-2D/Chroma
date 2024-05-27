namespace Chroma.Input.GameControllers
{
    public sealed class ControllerButtonEventArgs
    {
        public ControllerDriver Controller { get; }
        public ControllerButton Button { get; internal set; }

        internal ControllerButtonEventArgs(ControllerDriver controller, ControllerButton button)
        {
            Controller = controller;
            Button = button;
        }

        public ControllerButtonEventArgs WithButton(ControllerButton button) => new(
            Controller,
            button
        );
    }
}