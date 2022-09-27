using System.Xml;
using System.Xml.Serialization;
using Tracer.Core;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serializetion.Xml
{
    public class XmlTracerResultSerializer : ITraceResultSerializer
    {
        public string Format => "xml";

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

            using var xmlWriter = XmlWriter.Create(to, new XmlWriterSettings { Indent = true });
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SerializableResult));
            xmlSerializer.Serialize(xmlWriter, customTracerResult);
        }
    }
}
