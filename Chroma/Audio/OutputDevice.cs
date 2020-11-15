using System;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.OpenAL;

namespace Chroma.Audio
{
    public class OutputDevice
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();
        
        internal IntPtr Handle { get; private set; }
        internal IntPtr ContextHandle { get; private set; }

        internal AudioManager Manager { get; }

        public string Name { get; }

        public bool IsValid => Handle != IntPtr.Zero;
        public bool HasValidContext => ContextHandle != IntPtr.Zero;

        internal OutputDevice(AudioManager manager, string name)
        {
            Manager = manager;
            Name = name;
        }

        internal void Open()
        {
            Handle = Alc.alcOpenDevice(Name);

            if (!IsValid)
            {
                _log.Error($"Failed to open output device '{Name}'.");
                return;
            }
            
            CreateContext();
        }

        internal void Close()
        {
            if (HasValidContext)
            {
                Alc.alcDestroyContext(ContextHandle);
                ContextHandle = IntPtr.Zero;
            }

            if (IsValid)
            {
                Alc.alcCloseDevice(Handle);
                Handle = IntPtr.Zero;
            }
        }
        
        internal void CreateContext()
        {
            ContextHandle = Alc.alcCreateContext(Handle, null);

            if (!HasValidContext)
            {
                Close();
                _log.Error($"Failed to initialize OpenAL context on device '{Name}'. The device has been closed.");
            }
        }
    }
}