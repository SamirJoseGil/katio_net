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


    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);


    //    // Datos para la tabla de libros
    //    modelBuilder.Entity<Book>().HasData(
    //        new Book
    //        {
    //            Name = "Cien años de soledad",
    //            ISBN10 = "842047183",
    //            ISBN13 = "978-842047183",
    //            Edition = "RAE Obra Académica",
    //            DeweyIndex = "800",
    //            Published = new DateTime(1967, 06, 05),
    //            Id = 1
    //        },
    //        new Book
    //        {
    //            Name = "Huellas",
    //            ISBN10 = "958427275",
    //            ISBN13 = "978-958427275",
    //            Edition = "Planeta",
    //            DeweyIndex = "800",
    //            Published = new DateTime(2019, 01, 01),
    //            Id = 2
    //        },
    //        new Book
    //        {
    //            Name = "María",
    //            ISBN10 = "148027292",
    //            ISBN13 = "978-148027292",
    //            Edition = "1ra Edición",
    //            DeweyIndex = "800",
    //            Published = new DateTime(1867, 01, 01),
    //            Id = 3
    //        },

    //        new Book
    //        {
    //            Name = "Sin Remedio",
    //            ISBN10 = "3161484100",
    //            ISBN13 = "978-3161484100",
    //            Edition = "Alfaguara",
    //            DeweyIndex = "800",
    //            Published = new DateTime(1984, 01, 01),
    //            Id = 4
    //        }
    //    );

    //    // Datos para la tabla de autores
    //    modelBuilder.Entity<Author>().HasData(
    //        new Author
    //        {
    //            Name = "Gabriel",
    //            LastName = "García Márquez",
    //            Country = "Colombia",
    //            Birthdate = new DateTime(1927, 03, 06),
    //            Id = 1
    //        },
    //        new Author
    //        {
    //            Name = "Jorge",
    //            LastName = "Franco",
    //            Country = "Colombia",
    //            Birthdate = new DateTime(1962, 07, 02),
    //            Id = 2
    //        },
    //        new Author
    //        {
    //            Name = "Jorge",
    //            LastName = "Isaacs",
    //            Country = "Colombia",
    //            Birthdate = new DateTime(1837, 04, 01),
    //            Id = 3
    //        },
    //        new Author
    //        {
    //            Name = "Mario",
    //            LastName = "Mendoza",
    //            Country = "Colombia",
    //            Birthdate = new DateTime(1964, 06, 01),
    //            Id = 4
    //        }
    //    );
    //}
}
