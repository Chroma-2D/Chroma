using System;

namespace Chroma.Natives.FreeType.Native
{
    internal class FT_ListNode : NativeObject
    {
        private FT_ListNodeRec rec;

        public FT_ListNode(IntPtr reference) : base(reference)
        {
        }

        public FT_ListNode Previous
        {
            get
            {
                if (rec.prev == IntPtr.Zero)
                    return null;

                return new FT_ListNode(rec.prev);
            }
        }

        public FT_ListNode Next
        {
            get
            {
                if (rec.next == IntPtr.Zero)
                    return null;

                return new FT_ListNode(rec.next);
            }
        }

        public IntPtr Data => rec.data;

        public override IntPtr Reference
        {
            get => base.Reference;

            set
            {
                base.Reference = value;
                rec = PInvokeHelper.PtrToStructure<FT_ListNodeRec>(value);
            }
        }
    }
}