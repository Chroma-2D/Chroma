namespace Chroma.Input.EventArgs
{
    public class TextInputEventArgs
    {
        public string Text { get; }

        internal TextInputEventArgs(string text)
        {
            Text = text;
        }
    }
}