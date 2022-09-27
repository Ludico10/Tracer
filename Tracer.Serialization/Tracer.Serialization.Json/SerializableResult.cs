using System.Text.Json.Serialization;

namespace Tracer.Serialization.Json
{
    public class SerializableResult
    {
        [JsonInclude, JsonPropertyName("threads")]
        public List<ThreadInfo> Threads = new();

        public SerializableResult(List<ThreadInfo> threads)
        {
            Threads = threads;
        }
    }
}
