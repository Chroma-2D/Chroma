using System;

namespace Chroma.MemoryManagement
{
    public class DisposableResource : IDisposable
    {
        public bool Disposed { get; private set; }

        public event EventHandler Disposing;

        ~DisposableResource()
        {
            Dispose(false);
        }

        protected virtual void FreeManagedResources()
        {
        }

        protected virtual void FreeNativeResources()
        {
        }

        private void Dispose(bool disposing)
        {
            EnsureNotDisposed();

            if (disposing)
            {
                FreeManagedResources();
            }

            FreeNativeResources();
            Disposed = true;
        }

        public void Dispose()
        {
            Disposing?.Invoke(this, EventArgs.Empty);
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected void EnsureNotDisposed()
        {
            if (Disposed)
                throw new InvalidOperationException("This object has already been disposed.");
        }
    }
}