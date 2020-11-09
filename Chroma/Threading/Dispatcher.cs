using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chroma.Threading
{
    public class Dispatcher
    {
        internal static Queue<ScheduledAction> ActionQueue { get; } = new Queue<ScheduledAction>();

        public static Task RunOnMainThread(Action action)
        {
            var scheduledAction = new ScheduledAction { Action = action };
            
            ActionQueue.Enqueue(scheduledAction);

            return new Task(async () =>
            {
                while (!scheduledAction.Completed)
                    await Task.Delay(1);
            });
        }
    }
}