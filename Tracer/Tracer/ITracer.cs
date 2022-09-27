using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Tracer.Core
{
    public interface ITracer
    {
        void StartTrace();
        void StopTrace();
        TraceResult GetTraceResult();
    }
}
