using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Chroma.Natives.Bindings.SDL;

namespace Chroma.Windowing.DragDrop
{
    internal sealed class DragDropManager
    {
        private readonly Window _owner;

        private bool? _isFileDrop;

        private readonly List<string> _fileList = new();
        private readonly StringBuilder _textBuffer = new();

        internal DragDropManager(Window owner)
        {
            _owner = owner;
        }

        internal void BeginDrop()
        {
            _fileList.Clear();
            _textBuffer.Clear();

            _isFileDrop = null;
        }

        internal void OnFileDropped(IntPtr stringPtr)
        {
            if (stringPtr == IntPtr.Zero)
                return;

            _isFileDrop ??= true;

            if (_isFileDrop != true)
                throw new FrameworkException("Unexpected operation type change during a drop operation.");

            _fileList.Add(Marshal.PtrToStringAnsi(stringPtr));
            SDL2.SDL_free(stringPtr);
        }

        internal void OnTextDropped(IntPtr stringPtr)
        {
            if (stringPtr == IntPtr.Zero)
                return;

            _isFileDrop ??= false;

            if (_isFileDrop != false)
                throw new FrameworkException("Unexpected operation type change during a drop operation.");

            _textBuffer.AppendLine(Marshal.PtrToStringAnsi(stringPtr));
            SDL2.SDL_free(stringPtr);
        }

        internal void FinishDrop()
        {
            if (_isFileDrop == true)
                _owner.OnFilesDropped(new FileDragDropEventArgs(_fileList));
            else if (_isFileDrop == false)
                _owner.OnTextDropped(new TextDragDropEventArgs(_textBuffer.ToString()));
        }
    }
}