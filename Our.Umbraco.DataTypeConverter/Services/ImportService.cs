﻿using System.Collections.Generic;
using System.Linq;
using Our.Umbraco.DataTypeConverter.Converters;
using Our.Umbraco.DataTypeConverter.Helpers;
using Our.Umbraco.DataTypeConverter.Models;

namespace Our.Umbraco.DataTypeConverter.Services
{
    public class ImportService : IImportService
    {
        public IList<IDataTypeConverter> GetAllConverters()
        {
            // TODO: use reflection to find all converters
            var converters = new List<IDataTypeConverter>
            {
                new MultiNodeTreePickerConverter(),
                new UrlPickerConverter()
            };

            return converters;
        }

        public IDataTypeConverter GetConverter(string propertyEditorAlias)
        {
            var converter = GetAllConverters().FirstOrDefault(x => x.PropertyEditorAlias == propertyEditorAlias);
            return converter;
        }

        public ConversionResult ConvertContent(IDataTypeConverter converter, string input)
        {
            // if it doesn't look like XML, then just return null
            if (!XmlHelper.IsXmlWellFormed(input))
            {
                return null;
            }

            var newValue = converter.Convert(input);
            var result = new ConversionResult()
            {
                OldValue = input,
                NewValue = newValue
            };

            return result;
        }
    }
}
