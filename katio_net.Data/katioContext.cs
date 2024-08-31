using Microsoft.EntityFrameworkCore;
using katio.Data.Models;

namespace katio.Data;

public class katioContext : DbContext
{
    public katioContext(DbContextOptions<katioContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; } = null;
    public DbSet<Author> Authors { get; set; } = null;
    public DbSet<Narrator> Narrators { get; set; } = null;
    public DbSet<Genre> Genres { get; set; } = null;
    public DbSet<AudioBook> AudioBooks { get; set; } = null;



    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);


    //    modelBuilder.Entity<Book>()
    //        .HasOne(b => b.Author)
    //        .WithMany(a => a.Books)
    //        .HasForeignKey(b => b.AuthorId);
    //}
}
