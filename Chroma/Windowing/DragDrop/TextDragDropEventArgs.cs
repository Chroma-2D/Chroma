namespace Chroma.Windowing.DragDrop
{
    public sealed class TextDragDropEventArgs
    {
        public string Text { get; }

        internal TextDragDropEventArgs(string text)
        {
            Text = text;
        }
    }
}