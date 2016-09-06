using System;
using System.Xml.Linq;

namespace Zone.Import.Config
{
    public class ImportConfig : IImportConfig
    {
        public ConfigSettings _config;

        public ImportConfig(string configFileName)
        {
            _config = LoadConfig(configFileName);
        }

        public bool ShouldRepublish
        {
            get
            {
                return _config.ShouldRepublish;
            }
        }

        private ConfigSettings LoadConfig(string configFileName)
        {
            var config = XDocument.Load(configFileName);
            if (config == null)
            {
                throw new NullReferenceException("No config file exists:" + configFileName);
            }

            return new ConfigSettings
            {
                ShouldRepublish = bool.Parse(config.Root.Element("shouldRepublish").Value)
            };
        }
    }

    public class ConfigSettings
    {
        public bool ShouldRepublish { get; set; }
    }
}
