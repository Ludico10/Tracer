namespace Tracer.Core
{
    public class TraceResult
    {
        public IReadOnlyList<ThreadTracer> Threads { get; }

        public TraceResult(IReadOnlyList<ThreadTracer> threads)
        {
            Threads = threads;
        }
    }
}