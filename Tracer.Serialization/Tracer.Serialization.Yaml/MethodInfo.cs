using Tracer.Core;

namespace Tracer.Serialization.Yaml
{
    public class MethodInfo
    {
        public string Name;

        public string Class;

        public string Time;

        public List<MethodInfo> Methods;

        public MethodInfo(string name, string className, TimeSpan time, List<MethodInfo> methods)
        {
            Name = name;
            Class = className;
            Time = string.Format("{0:f0}ms", time.TotalMilliseconds);
            Methods = methods;
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
