using System;
using System.Runtime.InteropServices;

namespace Chroma.NALO
{
    internal class NativeLibrary
    {
        public delegate IntPtr SymbolLookupDelegate(IntPtr handle, string name);

        private SymbolLookupDelegate SymbolLookup { get; }

        public string FilePath { get; }
        public IntPtr Handle { get; }

        public IntPtr this[string symbol] => SymbolLookup(Handle, symbol);

        public NativeLibrary(string filePath, IntPtr handle, SymbolLookupDelegate symbolLookup)
        {
            FilePath = filePath;
            Handle = handle;

            SymbolLookup = symbolLookup;
        }

        public T LoadFunction<T>(string symbolName, bool throwIfLookupFails = false)
        {
            var symbolAddress = this[symbolName];

            if (symbolAddress == IntPtr.Zero)
            {
                if (throwIfLookupFails)
                    throw new NativeLibraryException($"Symbol {symbolName} was not found.");

                return default;
            }

            return Marshal.GetDelegateForFunctionPointer<T>(symbolAddress);
        }
    }
}
