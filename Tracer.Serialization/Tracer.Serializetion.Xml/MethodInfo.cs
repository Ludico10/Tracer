using System.Xml.Serialization;
using Tracer.Core;

namespace Tracer.Serializetion.Xml
{
    public class MethodInfo
    {
        [XmlAttribute("name")]
        public string Name;

        [XmlAttribute("class")]
        public string ClassName;

        [XmlAttribute("time")]
        public string Time;

        [XmlElement("method")]
        public List<MethodInfo> Methods;

        public MethodInfo(string name, string className, TimeSpan time, List<MethodInfo> methods)
        {
            Name = name;
            ClassName = className;
            Time = string.Format("{0:f0}ms", time.TotalMilliseconds);
            Methods = methods;
        }

        public MethodInfo()
        {
            Name = String.Empty;
            ClassName = String.Empty;
            Time = String.Empty;
            Methods = new List<MethodInfo>();
        }

        public static List<MethodInfo> GetInnerMethods(MethodTracer method)
        {
            List<MethodInfo> innerMethods = new List<MethodInfo>();
            foreach (MethodTracer methodTrace in method.children)
            {
                innerMethods.Add(new MethodInfo(methodTrace.name, methodTrace.className, methodTrace.exeTime, GetInnerMethods(methodTrace)));
            }
            return innerMethods;
        }
    }
}
