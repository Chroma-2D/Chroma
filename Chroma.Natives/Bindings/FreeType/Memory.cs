using System;
using System.Runtime.InteropServices;
using Chroma.Natives.Bindings.FreeType.Native;

namespace Chroma.Natives.Bindings.FreeType
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr FT_Alloc_Func(NativeReference<Memory> memory, IntPtr size);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void FT_Free_Func(NativeReference<Memory> memory, IntPtr block);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr FT_Realloc_Func(NativeReference<Memory> memory, IntPtr currentSize, IntPtr newSize, IntPtr block);

    internal class Memory : NativeObject
    {
        private FT_MemoryRec rec;

        public Memory(IntPtr reference) : base(reference)
        {
        }

        public IntPtr User => rec.user;
        public FT_Alloc_Func Allocate => rec.alloc;
        public FT_Free_Func Free => rec.free;
        public FT_Realloc_Func Reallocate => rec.realloc;

        public override IntPtr Reference
        {
            get => base.Reference;

            set
            {
                base.Reference = value;
                rec = PInvokeHelper.PtrToStructure<FT_MemoryRec>(value);
            }
        }

        public unsafe int GzipUncompress(byte[] input, byte[] output)
        {
            var len = (IntPtr)output.Length;

            fixed (byte* inPtr = input, outPtr = output)
            {
                var err = FT.FT_Gzip_Uncompress(Reference, (IntPtr)outPtr, ref len, (IntPtr)inPtr, (IntPtr)input.Length);

                if (err != FT_Error.FT_Err_Ok)
                    throw new FreeTypeException(err);
            }

            return (int)len;
        }
    }
}
