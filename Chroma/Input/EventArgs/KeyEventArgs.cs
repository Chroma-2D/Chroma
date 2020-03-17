namespace Chroma.Input.EventArgs
{
    public class KeyEventArgs : System.EventArgs
    {
        public Scancode Scancode { get; }
        public bool IsRepeat { get; }
        public bool IsDown { get; }

        internal KeyEventArgs(Scancode scancode, bool isDown, bool isRepeat)
        {
            Scancode = scancode;
            IsDown = isDown;
            IsRepeat = isRepeat;
        }
    }
}