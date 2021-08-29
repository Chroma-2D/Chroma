namespace Chroma.Windowing.DragDrop
{
    public class TextDragDropEventArgs
    {
        public string Text { get; }

        internal TextDragDropEventArgs(string text)
        {
            Text = text;
        }
    }
}