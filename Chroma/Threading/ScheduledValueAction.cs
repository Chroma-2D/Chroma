using System;

namespace Chroma.Threading
{
    internal class ScheduledValueAction : SchedulerEntry
    {
        public Func<object> ValueAction { get; set; }
        public object ReturnValue { get; set; }
    }
}