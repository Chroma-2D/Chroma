using System;
using System.IO;
using System.Runtime.InteropServices;
using Chroma.Natives.Bindings.SDL;

namespace Chroma.MemoryManagement
{
    internal class SdlRwOps : DisposableResource
    {
        private readonly Stream _stream;
        private unsafe SDL2.SDL_RWops* _rwOps;

        private delegate long SdlRwOpsSizeDelegate(IntPtr context);

        private delegate long SdlRwOpsSeekDelegate(IntPtr context, long offset, int whence);

        private unsafe delegate ulong SdlRwOpsReadDelegate(IntPtr context, void* ptr, ulong size, ulong maxnum);

        private unsafe delegate ulong SdlRwOpsWriteDelegate(IntPtr context, void* ptr, ulong size, ulong num);

        private delegate int SdlRwOpsCloseDelegate(IntPtr context);

        private SdlRwOpsSizeDelegate _size;
        private SdlRwOpsSeekDelegate _seek;
        private SdlRwOpsReadDelegate _read;
        private SdlRwOpsWriteDelegate _write;
        private SdlRwOpsCloseDelegate _close;

        internal IntPtr RwOpsHandle
        {
            get
            {
                unsafe
                {
                    return (IntPtr)_rwOps;
                }
            }
        }

        internal bool KeepOpen { get; set; }

        internal SdlRwOps(Stream stream, bool keepOpen = false)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream), "The underlying stream cannot be null.");

            KeepOpen = keepOpen;

            unsafe
            {
                _rwOps = (SDL2.SDL_RWops*)SDL2.SDL_AllocRW();

                if (_rwOps == null)
                {
                    throw new FrameworkException($"Failed to allocate SDL RWops: {SDL2.SDL_GetError()}");
                }

                _size = Size;
                _seek = Seek;
                _read = Read;
                _write = Write;
                _close = Close;

                _rwOps->type = SDL2.SDL_RWOPS_UNKNOWN;
                _rwOps->size = Marshal.GetFunctionPointerForDelegate(_size);
                _rwOps->seek = Marshal.GetFunctionPointerForDelegate(_seek);
                _rwOps->read = Marshal.GetFunctionPointerForDelegate(_read);
                _rwOps->write = Marshal.GetFunctionPointerForDelegate(_write);
                _rwOps->close = Marshal.GetFunctionPointerForDelegate(_close);
            }
        }

        private long Size(IntPtr context)
        {
            long length;

            try
            {
                length = _stream.Length;
            }
            catch (Exception e)
            {
                SDL2.SDL_SetError(e.Message);
                length = 0;
            }

            return length;
        }

        private long Seek(IntPtr context, long offset, int whence)
        {
            if (!_stream.CanSeek)
            {
                SDL2.SDL_SetError("This does not support seeking.");
                return -1;
            }

            var seekOrigin = (SeekOrigin)whence;
            return _stream.Seek(offset, seekOrigin);
        }

        private unsafe ulong Read(IntPtr context, void* ptr, ulong size, ulong maxnum)
        {
            if (!_stream.CanRead)
            {
                SDL2.SDL_SetError("Attempted to read from a write-only stream.");
                return 0;
            }

            var intSize = (int)size;
            var intMaxnum = (int)maxnum;

            var data = new Span<byte>(ptr, intSize * intMaxnum);

            try
            {
                return (ulong)(_stream.Read(data) / intSize);
            }
            catch
            {
                return 0;
            }
        }

        private unsafe ulong Write(IntPtr context, void* ptr, ulong size, ulong maxnum)
        {
            if (!_stream.CanWrite)
            {
                SDL2.SDL_SetError("Attempted to write to a read-only stream.");
                return 0;
            }

            var intSize = (int)size;
            var intMaxnum = (int)maxnum;

            var data = new ReadOnlySpan<byte>(ptr, intSize * intMaxnum);
            _stream.Write(data);

            return maxnum;
        }

        private int Close(IntPtr context)
        {
            if (KeepOpen)
                return 0;

            if (RwOpsHandle == IntPtr.Zero)
            {
                SDL2.SDL_SetError("Attempted to free a null RWops handle.");
                return -1;
            }

            Dispose();

            return 0;
        }

        protected override void FreeManagedResources()
        {
            _size = null;
            _seek = null;
            _read = null;
            _write = null;
            _close = null;

            _stream.Dispose();
        }

        protected override void FreeNativeResources()
        {
            if (RwOpsHandle != IntPtr.Zero)
            {
                SDL2.SDL_FreeRW(RwOpsHandle);

                unsafe
                {
                    _rwOps = null;
                }
            }
        }
    }
}