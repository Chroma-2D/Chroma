namespace Chroma.Input
{
    public sealed class TextInputEventArgs
    {
        public string Text { get; }

        internal TextInputEventArgs(string text)
        {
            Text = text;
        }

        public static TextInputEventArgs With(string text) => new(text);

        public TextInputEventArgs WithText(string text) => new(text);
    }
}