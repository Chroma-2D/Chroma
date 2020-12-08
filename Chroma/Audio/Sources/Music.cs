using System;
using System.IO;
using System.Runtime.InteropServices;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SDL;

namespace Chroma.Audio.Sources
{
    public class Music : AudioSource
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        internal IntPtr FileSourceHandle { get; private set; }

        internal SDL2_nmix.NMIX_FileSource FileSource 
            => Marshal.PtrToStructure<SDL2_nmix.NMIX_FileSource>(FileSourceHandle);

        public Music(Stream stream)
        {
            IntPtr rw;

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                var arr = ms.ToArray();
                
                unsafe
                {
                    fixed (byte* b = &arr[0])
                    {
                        rw = SDL2.SDL_RWFromConstMem(
                            new IntPtr(b),
                            arr.Length
                        );
                        
                        if (rw == IntPtr.Zero)
                        {
                            _log.Error($"Failed to load music from stream: {SDL2.SDL_GetError()}");
                            return;
                        }
                        
                        FileSourceHandle = SDL2_nmix.NMIX_NewFileSource(rw, "mod", false);
                        if (FileSourceHandle == IntPtr.Zero)
                        {
                            _log.Error($"Failed to load music from stream: {SDL2.SDL_GetError()}");
                            return;
                        }
                        Handle = FileSource.source;
                    }
                }
            }
        }

        public Music(string filePath)
            : this(new FileStream(filePath, FileMode.Open)) {}
    }
}