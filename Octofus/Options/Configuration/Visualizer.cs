using System.Xml.Serialization;

namespace Octofus.Options.Configuration
{
    public class Visualizer
    {
        [XmlAttribute("leftPosition")]
        public int Left { get; set; }

        [XmlAttribute("topPosition")]
        public int Top { get; set; }
    }
}
