namespace Chroma.Input
{
    public class KeyEventArgs
    {
        public ScanCode ScanCode { get; }
        public KeyCode KeyCode { get; }
        public KeyModifiers Modifiers { get; }

        public bool IsAnyControlPressed
            => AnyModifierOfTypePressed(KeyModifiers.LeftControl, KeyModifiers.RightControl);

        public bool IsAnyAltPressed
            => AnyModifierOfTypePressed(KeyModifiers.LeftAlt, KeyModifiers.RightAlt);

        public bool IsAnySuperPressed
            => AnyModifierOfTypePressed(KeyModifiers.LeftSuper, KeyModifiers.RightSuper);

        public bool IsAnyShiftPressed
            => AnyModifierOfTypePressed(KeyModifiers.LeftShift, KeyModifiers.RightShift);

        public bool IsRepeat { get; }

        internal KeyEventArgs(ScanCode scanCode, KeyCode keyCode, KeyModifiers modifiers, bool isRepeat)
        {
            ScanCode = scanCode;
            KeyCode = keyCode;
            Modifiers = modifiers;

            IsRepeat = isRepeat;
        }

        private bool AnyModifierOfTypePressed(params KeyModifiers[] keys)
        {
            var result = 0;

            for (var i = 0; i < keys.Length; i++)
                result |= (int)(Modifiers & keys[i]);

            return result != 0;
        }
    }
}