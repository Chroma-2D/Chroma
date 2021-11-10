using System;
using System.Collections.Generic;

namespace Chroma.Windowing.DragDrop
{
    public class FileDragDropEventArgs : EventArgs
    {
        public List<string> Files { get; }

        internal FileDragDropEventArgs(IEnumerable<string> files)
        {
            Files = new List<string>(files);
        }
    }
}