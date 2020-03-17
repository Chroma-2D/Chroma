using Chroma.Input.EventArgs;
using System;
using System.Collections.Generic;

namespace Chroma.Input
{
    public static class Keyboard
    {
        private static readonly Dictionary<KeyCode, bool> _keyCodeStates;
        private static readonly Dictionary<ScanCode, bool> _scanCodeStates;

        static Keyboard()
        {
            _keyCodeStates = new Dictionary<KeyCode, bool>();
            _scanCodeStates = new Dictionary<ScanCode, bool>();
        }

        public static event EventHandler<KeyEventArgs> KeyDown;
        public static event EventHandler<KeyEventArgs> KeyUp;

        public static bool IsKeyDown(KeyCode keyCode)
            => _keyCodeStates.ContainsKey(keyCode) && _keyCodeStates[keyCode];

        public static bool IsKeyDown(ScanCode scanCode)
            => _scanCodeStates.ContainsKey(scanCode) && _scanCodeStates[scanCode];

        public static bool IsKeyUp(KeyCode keyCode) => !IsKeyDown(keyCode);
        public static bool IsKeyUp(ScanCode scanCode) => !IsKeyDown(scanCode);

        internal static void OnKeyUp(KeyEventArgs e)
        {
            if (!_keyCodeStates.ContainsKey(e.KeyCode))
                _keyCodeStates.Add(e.KeyCode, false);
            else
                _keyCodeStates[e.KeyCode] = false;

            if (!_scanCodeStates.ContainsKey(e.ScanCode))
                _scanCodeStates.Add(e.ScanCode, false);
            else
                _scanCodeStates[e.ScanCode] = false;

            KeyUp?.Invoke(null, e);
        }

        internal static void OnKeyDown(KeyEventArgs e)
        {
            if (!_keyCodeStates.ContainsKey(e.KeyCode))
                _keyCodeStates.Add(e.KeyCode, true);
            else
                _keyCodeStates[e.KeyCode] = true;

            if (!_scanCodeStates.ContainsKey(e.ScanCode))
                _scanCodeStates.Add(e.ScanCode, true);
            else
                _scanCodeStates[e.ScanCode] = true;

            KeyDown?.Invoke(null, e);
        }
    }
}
