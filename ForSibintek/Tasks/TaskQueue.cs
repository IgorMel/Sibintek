using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace ForSibintek.Tasks
{
    public class TaskQueue<T>:IDisposable where T: class
    {
        object locker = new object();
        Action<T> action;
        Thread[] workers;
        Queue<T> taskQ = new Queue<T>();
        static int currentCount ;
        private int count;
        public int Count { set
            {
                count = value;
                if (count == currentCount)
                    Dispose();
            }
        }
        public TaskQueue(int workCount,Action<T> action)
        {
            currentCount = 0;
            workers = new Thread[workCount];
            this.action = action;
            for (int i=0;i<workCount;i++)
                (workers[i] = new Thread(Consume)).Start();
        }

        public void Dispose()
        {
            Console.WriteLine("Обработка завершина");
            foreach (var worker in workers)
                EnqueueTask(null);
        }

        private void Consume()
        {
            while(true)
            {
                T value;
                lock (locker)
                {
                    while (taskQ.Count == 0)
                        Monitor.Wait(locker);
                    value = taskQ.Dequeue();
                }
                if (value == null)
                    return;
                Interlocked.Increment(ref currentCount);
                if (count > 0 && currentCount == count)
                    Dispose();
                action(value);
            }
        }

        public void EnqueueTask(T task)
        {
            lock (locker)
            {
                taskQ.Enqueue(task);
                Monitor.Pulse(locker);
            }
        }


    }
}
