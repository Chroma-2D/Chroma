namespace Chroma.Input.EventArgs
{
    public class KeyEventArgs : System.EventArgs
    {
        public Scancode Scancode { get; }

        internal KeyEventArgs(Scancode scancode)
        {
            Scancode = scancode;
        }
    }
}