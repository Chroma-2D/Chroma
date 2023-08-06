namespace Chroma.Input
{
    public sealed class TextInputEventArgs
    {
        public string Text { get; }

        internal TextInputEventArgs(string text)
        {
            Text = text;
        }
    }
}