using System.Diagnostics;
using System.Reflection;

namespace Tracer.Core
{
    public class ThreadTracer
    {
        public int id;
        public double exeTime;
        public List<MethodTracer> Methods;

        private List<MethodTracer> proc;

        public ThreadTracer(int _id)
        {
            Methods = new List<MethodTracer>();
            proc = new List<MethodTracer>();
            id = _id;
        }

        private MethodTracer? MethodSearch(int i)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame? curFrame = stackTrace.GetFrame(i);
            if (curFrame != null)
            {
                MethodBase? curMethod = curFrame.GetMethod();
                if (curMethod != null)
                {
                    string curClass = "-";
                    if (curMethod.ReflectedType != null) curClass = curMethod.ReflectedType.Name;
                    MethodTracer? res = null;
                    foreach (MethodTracer mi in proc)
                    {
                        if (mi.name == curMethod.Name && mi.className == curClass)
                        {
                            res = mi;
                        }
                    }
                    return res;
                }
            }
            return null;
        }

        private MethodTracer? ParentSearch()
        {
            StackTrace stackTrace = new StackTrace();
            for (int i = 5; i < stackTrace.FrameCount; i++)
            {
                MethodTracer? mi = MethodSearch(i);
                if (mi != null)
                    return mi;
            }
            return null;
        }

        public void ThreadStart()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame? curFrame = stackTrace.GetFrame(2);
            if (curFrame != null)
            {
                MethodBase? curMethod = curFrame.GetMethod();
                if (curMethod != null)
                {
                    MethodTracer info = new MethodTracer(curMethod);
                    proc.Add(info);
                }
            }
        }

        public void ThreadStop()
        {
            MethodTracer? info = MethodSearch(3);
            if (info != null)
            {
                proc.Remove(info);
                info.MethodFinish();
                MethodTracer? parent = ParentSearch();
                if (parent == null)
                {
                    Methods.Add(info);
                    ThreadCleaner();
                    exeTime += info.exeTime.TotalMilliseconds;
                    info.TreeCleaner();
                }
                else
                {
                    parent.AddChild(info);
                }
            }
        }

        public void ThreadCleaner()
        {
            foreach (MethodTracer method in proc)
            {
                List<MethodTracer>? forgotten = method.TreeCleaner();
                if (forgotten != null)
                {
                    Methods = Methods.Concat(forgotten).ToList();
                    foreach (MethodTracer meth in forgotten) exeTime += meth.exeTime.TotalMilliseconds;
                }
            }
            proc.Clear();
        }
    }
}