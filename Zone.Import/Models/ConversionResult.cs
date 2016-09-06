namespace Zone.Import.Models
{
    public class ConversionResult
    {
        public bool IsSuccess
        {
            get
            {
                return !string.IsNullOrWhiteSpace(NewValue) && OldValue != NewValue;
            }
        }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}
