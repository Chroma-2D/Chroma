using System;

namespace Chroma.Threading
{
    internal sealed class ScheduledAction
    {
        public bool Completed { get; set; }
        public Action Action { get; set; }
    }
}