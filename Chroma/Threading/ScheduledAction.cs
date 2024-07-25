namespace Chroma.Threading;

using System;

internal sealed class ScheduledAction : SchedulerEntry
{
    public Action Action { get; set; }
}