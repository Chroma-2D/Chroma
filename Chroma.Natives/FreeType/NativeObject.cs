using System;

namespace Chroma.Natives.FreeType
{
    internal abstract class NativeObject
    {
        private IntPtr _reference;

        protected NativeObject(IntPtr reference)
        {
            _reference = reference;
        }

        public virtual IntPtr Reference
        {
            get => _reference;
            set => _reference = value;
        }
    }
}
