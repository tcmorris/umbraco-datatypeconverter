using System;
using System.Xml;
using System.Collections.Generic;

namespace Our.Umbraco.DataTypeConverter.Converters
{
    public class MultiNodeTreePickerConverter : IDataTypeConverter
    {
        public string Name => "MNTP Converter (XML to CSV)";

        public string PropertyEditorAlias => "Umbraco.MultiNodeTreePicker";

        /// <summary>
        /// Converts from XML to CSV
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Convert(string input)
        {
            /*
                <MultiNodePicker type="content">
                    <nodeId>1196</nodeId>
                    <nodeId>1197</nodeId>
                </MultiNodePicker>
             */
            try
            {
                var xd = new XmlDocument();
                xd.LoadXml(input);

                var idList = new List<string>();

                foreach (XmlNode element in xd.SelectNodes("//nodeId"))
                {
                    idList.Add(element.InnerText);
                }

                return string.Join(",", idList);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
