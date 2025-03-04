namespace Exchange.Presentation.Models
{
    public class QueryModel
    {
        public string FirstCurrency { get; set; } = string.Empty;

        public string SecondCurrency { get; set; } = string.Empty;

        public int Limit { get; set; } = 0;

        public int Frame { get; set; } = 60;

        public int From { get; set; } = 0;

        public int To { get; set; } = 0;
    }
}
