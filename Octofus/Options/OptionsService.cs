using Octofus.Data;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Documents;

namespace Octofus.Options
{
    public static class OptionsService
    {
        public static Configuration.Settings ReadOptions()
        {
            var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "settings.xml");
            return Configuration.Settings.Deserialize(filePath);
        }

        public static void SaveOptions(Configuration.Settings configuration)
        {
            var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "settings.xml");
            configuration.Serialize(filePath);
        }

        public static void SaveWindowPosition(WindowPosition windowPosition) 
        { 
            var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "settings.xml");
            var actualConf = Configuration.Settings.Deserialize(filePath);

            actualConf.Visualizer.Top = (int)windowPosition.Top;
            actualConf.Visualizer.Left = (int)windowPosition.Left;

            actualConf.Serialize(filePath);
        }

        public static void SaveCharactersSettings(List<CharacterSettings> characterSettings)
        {
            var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "settings.xml");
            var actualConf = Configuration.Settings.Deserialize(filePath);

            actualConf.Characters.Clear();

            var cptr = 0;
            foreach (var characterSetting in characterSettings) 
            {
                actualConf.Characters.Add(new Configuration.Character
                {
                    Key = characterSetting.Key,
                    Name = characterSetting.AccountName,
                    Position = cptr
                });

                cptr++;
            }

            actualConf.Serialize(filePath);
        }
    }
}
