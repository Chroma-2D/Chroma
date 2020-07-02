using System;

namespace Chroma.Audio
{
    public class MusicEventArgs : EventArgs
    {
        public Music Music { get; }

        internal MusicEventArgs(Music music)
        {
            Music = music;
        }
    }
}