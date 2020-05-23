using Chroma.Natives.SDL;

namespace Chroma
{
    public static class Clipboard
    {
        public static bool HasText => SDL2.SDL_HasClipboardText();

        public static string Text
        {
            get => SDL2.SDL_GetClipboardText();
            set => SDL2.SDL_SetClipboardText(value);
        }

        public static void SetText(string text)
            => Text = text;
    }
}