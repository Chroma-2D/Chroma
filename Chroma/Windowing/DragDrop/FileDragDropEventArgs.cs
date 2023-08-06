using System.Collections.Generic;

namespace Chroma.Windowing.DragDrop
{
    public sealed class FileDragDropEventArgs
    {
        public List<string> Files { get; }

        internal FileDragDropEventArgs(IEnumerable<string> files)
        {
            Files = new List<string>(files);
        }
    }
}