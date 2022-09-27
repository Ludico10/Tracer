using Tracer.Core;
using Tracer.Serialization.Abstractions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Tracer.Serialization.Yaml
{
    public class YamlTracerSerializer: ITraceResultSerializer
    {
        public string Format => "yaml";

        public void Serialize(TraceResult traceResult, Stream to)
        {
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

            var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            var yaml = serializer.Serialize(customTracerResult);
            using var sw = new StreamWriter(to);
            sw.Write(yaml);
            sw.Flush();
        }
    }
}
