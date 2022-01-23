using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Chroma.Threading
{
    public class Dispatcher
    {
        internal static ConcurrentQueue<ScheduledAction> ActionQueue { get; } = new();
        
        public static int MainThreadId { get; internal set; }

        public static bool IsMainThread
            => Environment.CurrentManagedThreadId == MainThreadId;
        
        public static Task RunOnMainThread(Action action)
        {
            var scheduledAction = new ScheduledAction { Action = action };
            ActionQueue.Enqueue(scheduledAction);

            return Task.Factory.StartNew(async (a) =>
            {
                var sched = a as ScheduledAction;
                
                while (!sched!.Completed)
                    await Task.Delay(1);
            }, scheduledAction);
        }
    }
}