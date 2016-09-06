using System;
using System.Xml;
using Newtonsoft.Json;
using Zone.Import.Models;

namespace Zone.Import.Converters
{
    public class UrlPickerConverter : IDataTypeConverter
    {
        public string Name
        {
            get
            {
                return "URL Picker Converter (XML to JSON)";
            }
        }

        public string PropertyEditorAlias
        {
            get
            {
                return "Imulus.UrlPicker";
            }
        }

        /// <summary>
        /// Converts from XML to JSON
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Convert(string input)
        {
            /*
                <url-picker mode="Content">
                    <new-window>False</new-window>
                    <node-id>1196</node-id>
                    <url>/about-us/awards/</url>
                    <link-title></link-title>
                </url-picker>
             */
            try
            {
                var xd = new XmlDocument();
                xd.LoadXml(input);

                var xmlNode = xd.FirstChild;
                var urlPicker = new UrlPicker
                {
                    Type = xmlNode.Attributes["mode"].Value.ToLower(),
                    Meta = new UrlPicker.MetaInfo
                    {
                        Title = xmlNode.SelectSingleNode("link-title").InnerText,
                        NewWindow = bool.Parse(xmlNode.SelectSingleNode("new-window").InnerText)
                    },
                    TypeData = new UrlPicker.TypeInfo()
                };

                switch (urlPicker.Type)
                {
                    case "url":
                        urlPicker.TypeData.Url = xmlNode.SelectSingleNode("url").InnerText;
                        break;

                    case "content":
                        urlPicker.TypeData.ContentId = int.Parse(xmlNode.SelectSingleNode("node-id").InnerText);
                        break;

                    case "media":
                        urlPicker.TypeData.MediaId = int.Parse(xmlNode.SelectSingleNode("node-id").InnerText);
                        break;
                }

                var jsonValue = JsonConvert.SerializeObject(urlPicker);
                return jsonValue;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
