using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Chroma.Threading
{
    public sealed class Dispatcher
    {
        internal static ConcurrentQueue<ScheduledAction> ActionQueue { get; } = new();

        public static int MainThreadId { get; internal set; }

        public static bool IsMainThread
            => Environment.CurrentManagedThreadId == MainThreadId;
        
        internal Dispatcher()
        {
        }
        
        public static Task RunOnMainThread(Action action)
        {
            var scheduledAction = new ScheduledAction { Action = action };
            ActionQueue.Enqueue(scheduledAction);

            return new Task((a) =>
            {
                var sched = a as ScheduledAction;
                
                while (!sched!.Completed)
                    Task.Delay(1);
            }, scheduledAction);
        }
    }
}