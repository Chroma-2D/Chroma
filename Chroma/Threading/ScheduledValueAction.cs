namespace Chroma.Threading;

using System;

internal class ScheduledValueAction : SchedulerEntry
{
    public Func<object> ValueAction { get; set; }
    public object ReturnValue { get; set; }
}