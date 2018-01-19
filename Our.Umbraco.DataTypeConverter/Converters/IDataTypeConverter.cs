namespace Our.Umbraco.DataTypeConverter.Converters
{
    public interface IDataTypeConverter
    {
        /// <summary>
        /// The name of the converter
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Property editor type to convert to
        /// </summary>
        string PropertyEditorAlias { get; }

        
        /// <summary>
        /// Convert input value to new format
        /// </summary>
        /// <param name="input">input value</param>
        /// <returns>new value</returns>
        string Convert(string input);
    }
}
