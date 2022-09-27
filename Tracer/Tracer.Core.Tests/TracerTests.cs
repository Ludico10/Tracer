using Xunit;

namespace Tracer.Core.Tests
{
    public class TracerTests
    {
        [Fact]
        public void SingleTest()
        {
            ITracer tracer = new Tracer();
            Imetator imetator = new Imetator(tracer);
            imetator.SimpleMethod();
            TraceResult res = tracer.GetTraceResult();

            Assert.Equal(1, res.Threads.Count);
            Assert.Single(res.Threads[0].Methods);
            SimpleTest(res.Threads[0].Methods[0]);    
        }

        private void SimpleTest(MethodTracer method)
        {
            Assert.Equal("SimpleMethod", method.name);
            Assert.Equal("Imetator", method.className);
            Assert.Empty(method.children);
            Assert.NotInRange(method.exeTime.TotalMilliseconds, 0, 500);
        }

        [Fact]
        public void RecursiveTest()
        {
            ITracer tracer = new Tracer();
            Imetator imetator = new Imetator(tracer);
            int deep = 4;
            imetator.RecursiveMethod(deep);
            TraceResult res = tracer.GetTraceResult();

            Assert.Equal(1, res.Threads.Count);
            Assert.Single(res.Threads[0].Methods);
            double time = EveryLvlTest(deep + 1, res.Threads[0].Methods[0]);
            Assert.NotInRange(time, 0, 600 + 100 * (deep + 1));
        }

        private double EveryLvlTest(int deep, MethodTracer method)
        {
            if (deep > 0)
            {
                Assert.Single(method.children);
                Assert.Equal("RecursiveMethod", method.name);
                Assert.Equal("Imetator", method.className);
                Assert.NotInRange(method.exeTime.TotalMilliseconds, 0, EveryLvlTest(deep - 1, method.children[0]) + 100);
            }
            else SimpleTest(method);
            return method.exeTime.TotalMilliseconds;
        }

        [Fact]
        public void MultithreadTest()
        {
            ITracer tracer = new Tracer();
            Imetator imetator = new Imetator(tracer);
            imetator.MultithreadMethod();
            TraceResult res = tracer.GetTraceResult();

            Assert.Equal(2, res.Threads.Count);
            Assert.Single(res.Threads[0].Methods);
            MethodTracer method = res.Threads[0].Methods[0];
            Assert.Equal("MultithreadMethod", method.name);
            Assert.Equal("Imetator", method.className);
            Assert.Equal(res.Threads[0].exeTime, method.exeTime.TotalMilliseconds);
            Assert.Single(method.children);
            SimpleTest(method.children[0]);
            Assert.NotInRange(method.exeTime.TotalMilliseconds, 0, method.children[0].exeTime.TotalMilliseconds);

            Assert.NotEqual(res.Threads[0].id, res.Threads[1].id);
            Assert.Single(res.Threads[1].Methods);
            SimpleTest(res.Threads[1].Methods[0]);
            Assert.Equal(res.Threads[1].exeTime, res.Threads[1].Methods[0].exeTime.TotalMilliseconds);
        }

        [Fact]
        public void StartlessTest()
        {
            ITracer tracer = new Tracer();
            Imetator imetator = new Imetator(tracer);
            imetator.StartlessMethod();
            TraceResult res = tracer.GetTraceResult();

            Assert.Single(res.Threads[0].Methods);
            SimpleTest(res.Threads[0].Methods[0]);
            Assert.Equal(res.Threads[0].exeTime, res.Threads[0].Methods[0].exeTime.TotalMilliseconds);
        }

        [Fact]
        public void EndlessTest()
        {
            ITracer tracer = new Tracer();
            Imetator imetator = new Imetator(tracer);
            imetator.EndlessMethod();
            TraceResult res = tracer.GetTraceResult();

            Assert.Single(res.Threads[0].Methods);
            SimpleTest(res.Threads[0].Methods[0]);
            Assert.Equal(res.Threads[0].exeTime, res.Threads[0].Methods[0].exeTime.TotalMilliseconds);
        }
    }
}