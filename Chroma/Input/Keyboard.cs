using System;
using System.Collections.Generic;
using System.Text;
using Chroma.Input.EventArgs;

namespace Chroma.Input
{
    public static class Keyboard
    {
        public static event EventHandler<KeyEventArgs> KeyDown;

        internal static void OnKeyDown(KeyEventArgs e)
            => KeyDown?.Invoke(null, e);
    }
}
