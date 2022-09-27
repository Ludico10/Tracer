using System.Diagnostics;
using System.Reflection;

namespace Tracer.Core
{
    public class MethodTracer
    {
        public string name;
        public string className;
        public List<MethodTracer> children;
        public TimeSpan exeTime;

        private Stopwatch Timer;

        public void AddChild(MethodTracer child)
        {
            children.Add(child);
        }

        public List<MethodTracer>? TreeCleaner()
        {
            foreach (MethodTracer child in children)
            {
                List<MethodTracer>? grandchild = child.TreeCleaner();
                if (grandchild != null)
                {
                    children.Remove(child);
                    children = children.Concat(grandchild).ToList();
                }
            }
            if (Timer.IsRunning)
            {
                Timer.Stop();
                return children;
            }
            else return null;
        }

        public MethodTracer(MethodBase method)
        {
            children = new List<MethodTracer>();
            name = method.Name;
            if (method.ReflectedType != null) className = method.ReflectedType.Name;
            else className = "-";
            Timer = new Stopwatch();
            Timer.Start();
        }

        public void MethodFinish()
        {
            Timer.Stop();
            exeTime = Timer.Elapsed;
        }
    }
}
