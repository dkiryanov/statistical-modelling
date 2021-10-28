using System.IO;
using System.Xml.Serialization;
using WorkshopModelling.Settings;

namespace WorkshopModelling
{
    public class FileService
    {
        public SettingModel GetSettingsFromXml()
        {
            XmlSerializer reader = new XmlSerializer(typeof(SettingModel), new XmlRootAttribute("Setting"));

            using (StreamReader file = new StreamReader("Settings.xml"))
            {
                SettingModel settings = (SettingModel)reader.Deserialize(file);

                return settings;
            }
        }
    }
}