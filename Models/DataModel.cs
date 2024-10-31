namespace B1SecondTaskWebAPI.Models
{
    public class DataModel
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public string Class { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public decimal ActiveDecimal { get; set; }
        public decimal PassiveDecimal { get; set; }
        public decimal DebitDecimal { get; set; }
        public decimal CreditDecimal { get; set; }
        public decimal ActiveDecimal2 { get; set; }
        public decimal PassiveDecimal2 { get; set; }

        public FileModel File { get; set; }
    }
}
