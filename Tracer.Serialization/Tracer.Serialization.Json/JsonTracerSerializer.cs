using System.Text.Json;
using Tracer.Core;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization.Json
{
    public class JsonTracerSerializer : ITraceResultSerializer
    {
        public string Format => "json";

        public void Serialize(TraceResult traceResult, Stream to)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            List<ThreadInfo> threadsInfo = new List<ThreadInfo>();
            foreach (ThreadTracer thread in traceResult.Threads)
            {
                List<MethodInfo> rootMethods = new List<MethodInfo>();
                foreach (MethodTracer method in thread.Methods)
                {
                    rootMethods.Add(new MethodInfo(method.name, method.className, method.exeTime, MethodInfo.GetInnerMethods(method)));
                }
                threadsInfo.Add(new ThreadInfo(thread.id, thread.exeTime, rootMethods));
            }
            SerializableResult customTracerResult = new SerializableResult(threadsInfo);
            JsonSerializer.Serialize(to, customTracerResult, options);
        }
    }
}
