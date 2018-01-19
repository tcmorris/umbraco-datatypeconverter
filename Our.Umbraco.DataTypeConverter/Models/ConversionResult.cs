namespace Our.Umbraco.DataTypeConverter.Models
{
    public class ConversionResult
    {
        public bool IsSuccess => !string.IsNullOrWhiteSpace(NewValue) && OldValue != NewValue;

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}
