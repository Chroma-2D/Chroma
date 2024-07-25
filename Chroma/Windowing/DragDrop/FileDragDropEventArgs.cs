namespace Chroma.Windowing.DragDrop;

using System.Collections.Generic;

public sealed class FileDragDropEventArgs
{
    public List<string> Files { get; }

    internal FileDragDropEventArgs(IEnumerable<string> files)
    {
        Files = new List<string>(files);
    }
}