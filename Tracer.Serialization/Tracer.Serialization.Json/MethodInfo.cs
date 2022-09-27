using System.Text.Json.Serialization;
using Tracer.Core;

namespace Tracer.Serialization.Json
{
    public class MethodInfo
    {
        [JsonInclude, JsonPropertyName("name")]
        public string Name;

        [JsonInclude, JsonPropertyName("class")]
        public string ClassName;

        [JsonInclude, JsonPropertyName("time")]
        public string Time;

        [JsonInclude, JsonPropertyName("methods")]
        public List<MethodInfo> Methods;

        public MethodInfo(string name, string className, TimeSpan time, List<MethodInfo> methods)
        {
            Name = name;
            ClassName = className;
            Time = String.Format("{0:f0}ms", time.TotalMilliseconds);
            Methods = methods;
        }

        public static List<MethodInfo> GetInnerMethods(MethodTracer method)
        {
            List<MethodInfo> innerMethods = new List<MethodInfo>();
            foreach (MethodTracer methodTracer in method.children)
            {
                innerMethods.Add(new MethodInfo(methodTracer.name, methodTracer.className, methodTracer.exeTime, GetInnerMethods(methodTracer)));
            }
            return innerMethods;
        }
    }
}
