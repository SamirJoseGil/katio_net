
namespace katio.Data.Models
{
    public class Authors
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Cpountry { get; set; } = string.Empty;
        public DateTime Birthdate  { get; set; }

    }
}
