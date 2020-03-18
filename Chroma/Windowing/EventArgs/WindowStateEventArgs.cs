namespace Chroma.Windowing.EventArgs
{
    public class WindowStateEventArgs : System.EventArgs
    {
        public WindowState State { get; }

        internal WindowStateEventArgs(WindowState state)
        {
            State = state;
        }
    }
}
