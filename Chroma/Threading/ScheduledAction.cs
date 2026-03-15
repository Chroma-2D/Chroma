namespace Chroma.Threading;

using System;

internal sealed class ScheduledAction(Action action) : SchedulerEntry
{
    public Action Action { get; } = action;
}