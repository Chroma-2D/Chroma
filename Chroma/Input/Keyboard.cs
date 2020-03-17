using Chroma.Input.EventArgs;
using System;

namespace Chroma.Input
{
    public static class Keyboard
    {
        public static event EventHandler<KeyEventArgs> KeyDown;
        public static event EventHandler<KeyEventArgs> KeyUp;

        internal static void OnKeyUp(KeyEventArgs e)
            => KeyUp?.Invoke(null, e);

        internal static void OnKeyDown(KeyEventArgs e)
            => KeyDown?.Invoke(null, e);
    }
}
