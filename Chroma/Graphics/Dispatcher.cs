using System;
using System.Collections.Generic;

namespace Chroma.Graphics
{
    public class Dispatcher
    {
        internal static Queue<Action> ActionQueue { get; } = new Queue<Action>();

        public static void ScheduleToRunOnMainThread(Action action)
            => ActionQueue.Enqueue(action);
    }
}