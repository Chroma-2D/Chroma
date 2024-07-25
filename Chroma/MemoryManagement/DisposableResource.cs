using System;
using Chroma.Threading;

namespace Chroma.MemoryManagement
{
    public class DisposableResource : IDisposable
    {
        public bool Disposed { get; private set; }

        public event EventHandler Disposing;

        ~DisposableResource()
        {
            Dispatcher.RunOnMainThread(() => Dispose(false));
        }

        protected virtual void FreeManagedResources()
        {
        }

        protected virtual void FreeNativeResources()
        {
        }
        
        protected void EnsureOnMainThread()
        {
            if (!Dispatcher.IsMainThread)
            {
                throw new InvalidOperationException(
                    "This operation is not thread-safe and must be scheduled to run on main thread."
                );
            }
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
                throw new ObjectDisposedException(null, "This object has already been disposed.");
        }
    }
}