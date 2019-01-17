using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace SteamDiscovery
{
    [Serializable()]
    [XmlRoot("configurations")]
    public class Config_elements
    {
        public Config_elements() { }

        [XmlElement("api_key")]
        public string apiKey;
        [XmlElement("origin_steamid64")]
        public string originSteamID64;

        [XmlElement("max_layer")]
        public int maxLayer;

        [XmlElement("xml_destination")]
        public string xml_destination;
    }

    public class Configurations
    {
        public Config_elements config_elements = new Config_elements();

        public Configurations() { }

        private string GetConfigurationString()
        {
            string config_path = Path.Combine(Directory.GetCurrentDirectory(), "configuration.xml");

            if (!File.Exists(config_path))
            {
                throw new Exception("Unable to locate configuration.xml at \n " + Directory.GetCurrentDirectory() + @"\configuration.xml");
            }

            string result = string.Empty;

            FileStream fileToRead = new FileStream(config_path, FileMode.Open, FileAccess.Read);
            using (StreamReader reader = new StreamReader(fileToRead, true))
            {
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    result += line;
                }

                reader.Dispose();
            }

            return result;
        }

        public void ReadConfigurationFile()
        {
            string result = GetConfigurationString();

            if (result == string.Empty)
            {
                throw new Exception("Unable to desirialize configuration.xml");
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Config_elements));
            FileStream readStream = new FileStream("configuration.xml", FileMode.Open);
            Program.config.config_elements = (Config_elements)xmlSerializer.Deserialize(readStream);
            readStream.Close();

            Check_SteamID();
            Check_API_Key();
            Check_Resquest_Validity();
            Check_Destination_directory();
        }

        private void Check_Destination_directory()
        {
            if (string.IsNullOrEmpty(Program.config.config_elements.xml_destination))
            {
                Program.config.config_elements.xml_destination = Directory.GetCurrentDirectory();
            }

            if (!Directory.Exists(Program.config.config_elements.xml_destination))
            {
                Directory.CreateDirectory(Program.config.config_elements.xml_destination);
            }
        }

        private void Check_SteamID()
        {
            if(string.IsNullOrEmpty(config_elements.originSteamID64) || string.IsNullOrWhiteSpace(config_elements.originSteamID64))
            {
                throw new Exception("Unable to read properly >origin_steamid64< field in configuration.xml");
            }
        }

        private void Check_API_Key()
        {
            if (string.IsNullOrEmpty(config_elements.apiKey) || string.IsNullOrWhiteSpace(config_elements.apiKey))
            {
                throw new Exception("Unable to read >api_key< in configuration.xml");
            }
        }

        private void Check_Resquest_Validity()
        {
            try
            {
                string url = @"http://api.steampowered.com/ISteamUser/GetFriendList/v0001/?key=" +
                Program.config.config_elements.apiKey + "&steamid=" + Program.config.config_elements.originSteamID64 + "&format=xml&relationship=friend";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "\n ==> Check api key validity or origin steam ID");
            }
        }
    }
}
