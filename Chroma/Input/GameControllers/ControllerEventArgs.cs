namespace Chroma.Input.GameControllers;

public sealed class ControllerEventArgs
{
    public ControllerDriver Controller { get; }

    internal ControllerEventArgs(ControllerDriver controller)
    {
        Controller = controller;
    }
}