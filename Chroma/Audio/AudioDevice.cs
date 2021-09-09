namespace Chroma.Audio
{
    public class AudioDevice
    {
        public static AudioDevice DefaultOutput { get; }

        internal uint OpenIndex { get; private set; }
        public int Index { get; }
        
        public bool IsCapture { get; }
        public bool IsCurrentlyInUse => OpenIndex != 0;

        public string Name { get; }

        static AudioDevice()
        {
            DefaultOutput = new AudioDevice(-666, false, "[System Default]");
        }

        internal AudioDevice(int index, bool isCapture, string name)
        {
            Index = index;
            IsCapture = isCapture;
            Name = name;
        }
        
        internal void Lock(uint openIndex)
        {
            OpenIndex = openIndex;
        }

        internal void Unlock()
        {
            OpenIndex = 0;
        }
    }
}