using System;

namespace Chroma.Audio
{
    public class AudioException : Exception
    {
        public AudioException(string message) : base(message)
        {
        }
    }
}