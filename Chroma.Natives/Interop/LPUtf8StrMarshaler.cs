using System;
using System.Text;
using System.Runtime.InteropServices;
using Chroma.Natives.SDL;

namespace Chroma.Natives.Interop
{
    internal unsafe class LPUtf8StrMarshaler : ICustomMarshaler
    {
        public const string LeaveAllocated = "LeaveAllocated";

        private bool _leaveAllocated;

        private static readonly ICustomMarshaler _leaveAllocatedInstance = new LPUtf8StrMarshaler(true);
        private static ICustomMarshaler _defaultInstance = new LPUtf8StrMarshaler(false);

        public static ICustomMarshaler GetInstance(string cookie)
        {
            return cookie switch
            {
                "LeaveAllocated" => _leaveAllocatedInstance,
                _ => _defaultInstance,
            };
        }

        public LPUtf8StrMarshaler(bool leaveAllocated)
            => _leaveAllocated = leaveAllocated;

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero)
                return null;
            
            var ptr = (byte*)pNativeData;

            while (*ptr != 0)
            {
                ptr++;
            }
            var bytes = new byte[ptr - (byte*)pNativeData];
            Marshal.Copy(pNativeData, bytes, 0, bytes.Length);

            return Encoding.UTF8.GetString(bytes);
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            if (ManagedObj == null)
                return IntPtr.Zero;

            if (!(ManagedObj is string str))
            {
                throw new ArgumentException("ManagedObj must be a string.", "ManagedObj");
            }

            var bytes = Encoding.UTF8.GetBytes(str);
            
            var mem = SDL2.SDL_malloc((IntPtr)bytes.Length + 1);
            Marshal.Copy(bytes, 0, mem, bytes.Length);
            ((byte*)mem)[bytes.Length] = 0;
            
            return mem;
        }

        public void CleanUpManagedData(object ManagedObj)
        {
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            if (!_leaveAllocated)
                SDL2.SDL_free(pNativeData);
        }

        public int GetNativeDataSize()
        {
            return -1;
        }
    }
}