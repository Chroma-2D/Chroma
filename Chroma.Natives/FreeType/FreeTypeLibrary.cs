using System;
using Chroma.Natives.FreeType.Native;

namespace Chroma.Natives.FreeType
{
    internal sealed class FreeTypeLibrary : IDisposable
    {
        public bool Disposed { get; private set; }

        public FreeTypeLibrary()
        {
            IntPtr lib;
            var err = FT.FT_Init_FreeType(out lib);
            if (err != FT_Error.FT_Err_Ok)
                throw new FreeTypeException(err);

            Native = lib;
        }

        public IntPtr Native { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (Native != IntPtr.Zero)
            {
                var err = FT.FT_Done_FreeType(Native);
                if (err != FT_Error.FT_Err_Ok)
                    throw new FreeTypeException(err);

                Native = IntPtr.Zero;
            }

            Disposed = true;
        }
    }
}