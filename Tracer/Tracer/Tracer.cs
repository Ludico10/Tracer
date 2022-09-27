namespace Tracer.Core
{
    public class Tracer : ITracer
    {
        private List<ThreadTracer> threads;

        public Tracer()
        {
            threads = new List<ThreadTracer>();
        }

        private ThreadTracer? SearchThread(int cur)
        {
            foreach (ThreadTracer info in threads)
            {
                if (info.id == cur)
                {
                    return info;
                }
            }
            return null;
        }

        public void StartTrace()
        {
            Object locker = new Object();
            lock (locker)
            {
                int curID = Thread.CurrentThread.ManagedThreadId;
                ThreadTracer? cur = SearchThread(curID);
                if (cur == null)
                {
                    cur = new ThreadTracer(curID);
                    threads.Add(cur);
                }
                cur.ThreadStart();
            }
        }

        public void StopTrace()
        {
            Object locker = new Object();
            lock (locker)
            {
                int curID = Thread.CurrentThread.ManagedThreadId;
                ThreadTracer? cur = SearchThread(curID);
                if (cur != null)
                {
                    cur.ThreadStop();
                }
            }
        }

        public TraceResult GetTraceResult()
        {
            foreach (ThreadTracer thread in threads) thread.ThreadCleaner();
            return new TraceResult(threads);
        }
    }
}