using System;
using Chroma.MemoryManagement;

namespace Chroma.Audio
{
    public abstract class AudioObject : DisposableResource
    {
        internal IntPtr Handle { get; private set; }

        internal AudioObject(IntPtr handle)
            => Handle = handle;

        protected void ValidateHandle()
        {
            if (Handle == IntPtr.Zero)
                throw new InvalidOperationException("Audio object handle is not valid.");
        }

        protected void DestroyHandle()
            => Handle = IntPtr.Zero;
    }
}