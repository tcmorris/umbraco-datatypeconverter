using System.Collections.Generic;
using Our.Umbraco.DataTypeConverter.Converters;
using Our.Umbraco.DataTypeConverter.Models;

namespace Our.Umbraco.DataTypeConverter.Services
{
    public interface IImportService
    {
        /// <summary>
        /// Get list of converters
        /// </summary>
        /// <returns></returns>
        IList<IDataTypeConverter> GetAllConverters();

        /// <summary>
        /// Get converter by alias
        /// </summary>
        /// <param name="propertyEditorAlias">Property editor alias</param>
        /// <returns></returns>
        IDataTypeConverter GetConverter(string propertyEditorAlias);

        /// <summary>
        /// Convert content using given converter and input value
        /// </summary>
        /// <param name="converter">Data type converter</param>
        /// <param name="input">Input value</param>
        /// <returns></returns>
        ConversionResult ConvertContent(IDataTypeConverter converter, string input);
    }
}
