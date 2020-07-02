namespace Chroma.Windowing.EventArgs
{
    public class CancelEventArgs : System.EventArgs
    {
        public bool Cancel { get; set; }

        internal CancelEventArgs()
        {
        }
    }
}