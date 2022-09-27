using Tracer.Core;
using Tracer.Serialization;

namespace Tracer.Example
{
    public class OtherClass
    {
        ITracer _tracer;

        public OtherClass(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void InnerMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(200);
            _tracer.StopTrace();
        }
    }

    public class Program
    {
        ITracer _tracer;

        public Program(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void SimpleMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(100);
            _tracer.StopTrace();
        }

        public void RecursiveMethod(int deep)
        {
            _tracer.StartTrace();
            if (deep > 0) RecursiveMethod(deep - 1);
            else
            {
                Thread.Sleep(50);
                SimpleMethod();
            }
            Thread.Sleep(20*deep);
            _tracer.StopTrace();
        }

        public void MultithreadMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(25);
            Thread thread = new Thread(() => RecursiveMethod(3));
            thread.Start();
            thread.Join();
            _tracer.StopTrace();
        }

        public void MultiClassMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(100);
            OtherClass other = new OtherClass(_tracer);
            other.InnerMethod();
            _tracer.StopTrace();
        }

        static void Main()
        {
            Tracer.Core.Tracer tracer = new Tracer.Core.Tracer();
            Program program = new Program(tracer);
            program.SimpleMethod();
            program.RecursiveMethod(0);
            program.MultithreadMethod();
            program.MultiClassMethod();
            TraceResult result = tracer.GetTraceResult();
            ExampleSerializer serializer = new ExampleSerializer("Plugins");
            serializer.Write(result, "result");
        }
    }
}