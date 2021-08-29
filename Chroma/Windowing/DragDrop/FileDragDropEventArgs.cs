using System;
using System.Collections.Generic;

namespace Chroma.Windowing.DragDrop
{
    public class FileDragDropEventArgs : EventArgs
    {
        public List<string> Files { get; }

        internal FileDragDropEventArgs(List<string> files)
        {
            Files = new List<string>(files);
        }
    }
}