using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Chroma.Threading
{
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
            var scheduledAction = new ScheduledAction { Action = action };
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

        public static Task<T> RunOnMainThread<T>(Func<object> valueAction, bool immediateStart = false) where T : class
        {
            var scheduledAction = new ScheduledValueAction { ValueAction = valueAction };
            ActionQueue.Enqueue(scheduledAction);

            var task = new Task<T>((a) =>
            {
                var sched = a as ScheduledValueAction;

                while (!sched!.Completed)
                    Task.Delay(1);

                return sched.ReturnValue as T;
            }, scheduledAction);

            if (immediateStart)
            {
                task.Start();
            }

            return task;
        }
    }
}