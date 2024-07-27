namespace Chroma.Threading;

using System;

internal sealed class ScheduledAction : SchedulerEntry
{
    public Action Action { get; }

    public ScheduledAction(Action action)
        => Action = action;
}