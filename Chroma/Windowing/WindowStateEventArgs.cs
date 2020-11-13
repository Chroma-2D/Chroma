using System;

namespace Chroma.Windowing
{
    public class WindowStateEventArgs : EventArgs
    {
        public WindowState State { get; }

        internal WindowStateEventArgs(WindowState state)
        {
            State = state;
        }
    }
}