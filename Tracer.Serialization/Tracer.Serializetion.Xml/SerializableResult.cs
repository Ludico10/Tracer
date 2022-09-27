using System.Xml.Serialization;

namespace Tracer.Serializetion.Xml
{
    [XmlRoot("root")]
    public class SerializableResult
    {
        [XmlElement("threads")]
        public List<ThreadInfo> Threads = new();

        public SerializableResult(List<ThreadInfo> threads)
        {
            Threads = threads;
        }

        public SerializableResult()
        {

        }
    }
}
