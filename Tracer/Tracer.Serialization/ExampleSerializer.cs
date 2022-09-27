using System.Reflection;
using Tracer.Core;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization
{
    public class ExampleSerializer
    {
        private readonly List<ITraceResultSerializer> _serializers;

        public ExampleSerializer(string directory)
        {
            _serializers = new List<ITraceResultSerializer>();
            LoadPlugins(directory);
        }

        public void LoadPlugins(string directory)
        {
            string[] plugins = Directory.GetFiles(directory, "*.dll");
            foreach (var plugin in plugins)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(plugin);
                    Type[] types = assembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (type.IsAssignableTo(typeof(ITraceResultSerializer)))
                        {
                            var serializer = (assembly.CreateInstance(type.FullName!) as ITraceResultSerializer)!;
                            _serializers.Add(serializer);
                        }
                    }
                }
                catch(Exception e){ Console.WriteLine(e); }
            }
        }

        public void Write(TraceResult traceResult, string fileNamePrefix)
        {
            foreach (var serializer in _serializers)
            {
                using var to = new FileStream($"{fileNamePrefix}.{serializer.Format}", FileMode.Create);
                serializer.Serialize(traceResult, to);
            }
        }
    }
}