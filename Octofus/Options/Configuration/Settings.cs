using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Octofus.Options.Configuration
{
    public class Settings
    {
        public Settings()
        {
            Characters = new List<Character>();    
        }

        [XmlElement("Visualizer")]
        public Visualizer Visualizer { get; set; }

        [XmlElement("Character")]
        public List<Character> Characters { get; set; }

        public static Settings Deserialize(string path)
        {
            if (File.Exists(path))
            {
                var serializer = new XmlSerializer(typeof(Settings));
                using (var stream = File.OpenRead(path))
                {
                    return serializer.Deserialize(stream) as Settings;
                }
            }

            return new Settings();
        }

        public void Serialize(string path)
        {
            var serializer = new XmlSerializer(typeof(Settings));
            using (var stream = File.Create(path))
            {
                serializer.Serialize(stream, this);
            }
        }
    }
}
