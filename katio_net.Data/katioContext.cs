using Microsoft.EntityFrameworkCore;
using katio.Data.Models;

namespace katio.Data
{
    public class katioContext : DbContext
    {
        public katioContext(DbContextOptions<katioContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; } = null;
    }
}
