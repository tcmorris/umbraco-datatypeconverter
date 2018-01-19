namespace Our.Umbraco.DataTypeConverter.Config
{
    using System.Configuration;

    public class DataTypeConverterConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeConverterConfig"/> class
        /// with details from configuration.
        /// </summary>
        public DataTypeConverterConfig()
        {
            ShouldRepublish = bool.Parse(ConfigurationManager.AppSettings["DataTypeConverter:ShouldRepublish"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeConverterConfig"/> class
        /// with provided values.
        /// </summary>
        public DataTypeConverterConfig(bool shouldRepublish = false)
        {
            ShouldRepublish = shouldRepublish;
        }

        public bool ShouldRepublish { get; set; }
    }
}
