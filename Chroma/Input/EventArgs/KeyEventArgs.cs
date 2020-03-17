namespace Chroma.Input.EventArgs
{
    public class KeyEventArgs : System.EventArgs
    {
        public ScanCode ScanCode { get; }
        public KeyCode KeyCode { get; }
        public KeyModifiers Modifiers { get; }

        public bool IsRepeat { get; }

        internal KeyEventArgs(ScanCode scanCode, KeyCode keyCode, KeyModifiers modifiers, bool isRepeat)
        {
            ScanCode = scanCode;
            KeyCode = keyCode;
            Modifiers = modifiers;

            IsRepeat = isRepeat;
        }
    }
}