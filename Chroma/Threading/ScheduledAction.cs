using System;

namespace Chroma.Threading
{
    internal sealed class ScheduledAction : SchedulerEntry
    {
        public Action Action { get; set; }
    }
}