using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.FreeType.Native
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate FT_Error FT_List_Iterator(NativeReference<FT_ListNode> node, IntPtr user);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void FT_List_Destructor(NativeReference<Memory> memory, IntPtr data, IntPtr user);

    internal sealed class FT_List
    {
        private IntPtr reference;
        private FT_ListRec rec;

        public FT_List(IntPtr reference)
        {
            Reference = reference;
        }

        public FT_ListNode Head => new(rec.head);
        public FT_ListNode Tail => new(rec.tail);

        public IntPtr Reference
        {
            get => reference;

            set
            {
                reference = value;
                rec = PInvokeHelper.PtrToStructure<FT_ListRec>(reference);
            }
        }

        public FT_ListNode Find(IntPtr data)
            => new(FT.FT_List_Find(Reference, data));

        public void Add(FT_ListNode node)
            => FT.FT_List_Add(Reference, node.Reference);

        public void Insert(FT_ListNode node)
            => FT.FT_List_Insert(Reference, node.Reference);

        public void Remove(FT_ListNode node)
            => FT.FT_List_Remove(Reference, node.Reference);

        public void Up(FT_ListNode node)
            => FT.FT_List_Up(Reference, node.Reference);

        public void Iterate(FT_List_Iterator iterator, IntPtr user)
        {
            var err = FT.FT_List_Iterate(Reference, iterator, user);

            if (err != FT_Error.FT_Err_Ok)
                throw new FreeTypeException(err);
        }

        public void Finalize(FT_List_Destructor destroy, Memory memory, IntPtr user)
            => FT.FT_List_Finalize(Reference, destroy, memory.Reference, user);
    }
}