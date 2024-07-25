namespace Chroma.Threading;

internal abstract class SchedulerEntry
{
    public virtual bool Completed { get; set; }
}