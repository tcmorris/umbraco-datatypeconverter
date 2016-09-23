namespace Zone.Import.Config
{
    using System;
    using System.Xml.Linq;

    public class ImportConfig : IImportConfig
    {
        public bool ShouldRepublish { get; set; }

        public ImportConfig(string configFileName)
        {
            var config = XDocument.Load(configFileName);
            if (config == null)
            {
                throw new NullReferenceException("No config file exists:" + configFileName);
            }

            ShouldRepublish = bool.Parse(config.Root.Element("shouldRepublish").Value);
        }
    }
}
