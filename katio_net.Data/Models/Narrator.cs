namespace katio.Data.Models
{
    public class Narrator : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
    }
}


