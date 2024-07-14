using System.Xml.Serialization;

namespace Octofus.Options.Configuration
{
    public class Character
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("position")]
        public int Position { get; set; }

        [XmlAttribute("key")]
        public string Key { get; set; }
    }
}
