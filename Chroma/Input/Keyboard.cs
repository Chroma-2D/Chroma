using System.Collections.Generic;
using Chroma.Natives.SDL;

namespace Chroma.Input
{
    public static class Keyboard
    {
        private static readonly HashSet<KeyCode> _keyCodeStates = new();
        private static readonly HashSet<ScanCode> _scanCodeStates = new();

        public static IReadOnlySet<KeyCode> ActiveKeys => _keyCodeStates;
        public static KeyModifiers ActiveModifiers => (KeyModifiers)SDL2.SDL_GetModState();

        public static bool IsKeyDown(KeyCode keyCode)
            => _keyCodeStates.Contains(keyCode);

        public static bool IsKeyDown(ScanCode scanCode)
            => _scanCodeStates.Contains(scanCode);

        public static bool IsKeyUp(KeyCode keyCode)
            => !IsKeyDown(keyCode);

        public static bool IsKeyUp(ScanCode scanCode)
            => !IsKeyDown(scanCode);

        public static string GetKeyName(KeyCode keyCode)
            => SDL2.SDL_GetKeyName((SDL2.SDL_Keycode)keyCode);

        public static string GetKeyName(ScanCode scanCode)
            => SDL2.SDL_GetScancodeName((SDL2.SDL_Scancode)scanCode);

        public static ScanCode KeyCodeToScanCode(KeyCode keyCode)
            => (ScanCode)SDL2.SDL_GetScancodeFromKey((SDL2.SDL_Keycode)keyCode);

        public static KeyCode ScanCodeToKeyCode(ScanCode scanCode)
            => (KeyCode)SDL2.SDL_GetKeyFromScancode((SDL2.SDL_Scancode)scanCode);

        internal static void OnKeyReleased(Game game, KeyEventArgs e)
        {
            _keyCodeStates.Remove(e.KeyCode);
            _scanCodeStates.Remove(e.ScanCode);

            game.OnKeyReleased(e);
        }

        internal static void OnKeyPressed(Game game, KeyEventArgs e)
        {
            _keyCodeStates.Add(e.KeyCode);
            _scanCodeStates.Add(e.ScanCode);

            game.OnKeyPressed(e);
        }
    }
}