namespace Chroma.Threading;

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public sealed class Dispatcher
{
    internal static ConcurrentQueue<SchedulerEntry> ActionQueue { get; } = new();

    public static int MainThreadId { get; internal set; }

    public static bool IsMainThread
        => Environment.CurrentManagedThreadId == MainThreadId;

    internal Dispatcher()
    {
    }

    public static Task RunOnMainThread(Action action, bool immediateStart = false)
    {
        var scheduledAction = new ScheduledAction(action);
        ActionQueue.Enqueue(scheduledAction);

        var task = new Task((a) =>
        {
            var sched = a as ScheduledAction;

            while (!sched!.Completed)
                Task.Delay(1);
        }, scheduledAction);

        if (immediateStart)
        {
            task.Start();
        }

        return task;
    }

    public static Task<T?> RunOnMainThread<T>(Func<object?> valueAction, bool immediateStart = false)
    {
        var scheduledAction = new ScheduledValueAction { ValueAction = valueAction };
        ActionQueue.Enqueue(scheduledAction);

        var task = new Task<T?>((a) =>
        {
            var sched = a as ScheduledValueAction;

            while (!sched!.Completed)
                Task.Delay(1);

            return (T?)sched.ReturnValue;
        }, scheduledAction);

        if (immediateStart)
        {
            task.Start();
        }

        return task;
    }
}