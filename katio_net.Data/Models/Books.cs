namespace katio.Data.Models
{
    public class Books
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ISBN10 { get; set; } = string.Empty;
        public string ISBN13 { get; set; } = string.Empty;
        public DateTime Published { get; set; }
        public string Edition { get; set; } = string.Empty;
        public string DeweyIndex { get; set; } = string.Empty;
    }
}
