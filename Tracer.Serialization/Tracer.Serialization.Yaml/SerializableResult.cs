namespace Tracer.Serialization.Yaml
{
    public class SerializableResult
    {
        public List<ThreadInfo> Threads;

        public SerializableResult(List<ThreadInfo> threads)
        {
            Threads = threads;
        }
    }
}
