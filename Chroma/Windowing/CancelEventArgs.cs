namespace Chroma.Windowing
{
    public class CancelEventArgs : System.EventArgs
    {
        public bool Cancel { get; set; }

        internal CancelEventArgs()
        {
        }
    }
}