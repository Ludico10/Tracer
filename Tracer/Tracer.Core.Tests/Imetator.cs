using System.Threading;

namespace Tracer.Core.Tests
{
    internal class Imetator
    {
        ITracer _tracer;

        public Imetator(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void SimpleMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(500);
            _tracer.StopTrace();
        }

        public void RecursiveMethod(int deep)
        {
            _tracer.StartTrace();
            if (deep > 0) RecursiveMethod(deep - 1);
            else
            {
                Thread.Sleep(100);
                SimpleMethod();
            }
            Thread.Sleep(100);
            _tracer.StopTrace();
        }

        public void MultithreadMethod()
        {
            _tracer.StartTrace();
            SimpleMethod();
            Thread.Sleep(100);
            Thread thread = new Thread(() => SimpleMethod());
            thread.Start();
            thread.Join();
            _tracer.StopTrace();
        }

        public void StartlessMethod()
        {
            SimpleMethod();
            _tracer.StopTrace();
        }

        public void EndlessMethod()
        {
            _tracer.StartTrace();
            SimpleMethod();
        }
    }
}
