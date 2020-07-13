using System;

namespace Chroma.Windowing
{
    public class MessageBoxButton
    {
        public int ID { get; internal set; }
        public string Text { get; internal set; }
        public Action<int> Action { get; internal set; }
        
        internal MessageBoxButton() {}
    }
}