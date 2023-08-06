namespace Chroma.Windowing
{
    public sealed class WindowStateEventArgs
    {
        public WindowState State { get; }

        internal WindowStateEventArgs(WindowState state)
        {
            State = state;
        }
    }
}