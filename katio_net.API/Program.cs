using katio.Business.Services;
using katio.Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using katio.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<katioContext>(opt => opt.UseInMemoryDatabase("katio"));


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

PopularDB(app);

app.UseHttpsRedirection();

app.MapControllers();

app.Run();


#region DB Population

async void PopularDB(WebApplication app)
{
    using (var scope = app.Services.CreateAsyncScope())
    {
        var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Cien años de soledad",
            ISBN10 = "842047183",
            ISBN13 = "978-842047183",
            Edition = "RAE Obra Académica",
            DeweyIndex = "800",
            Published = new DateTime(1967, 06, 05),
        });

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Huellas",
            ISBN10 = "958427275",
            ISBN13 = "978-958427275",
            Edition = "Planeta",
            DeweyIndex = "800",
            Published = new DateTime(2019, 01, 01),
        });

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "María",
            ISBN10 = "148027292",
            ISBN13 = "978-148027292",
            Edition = "1ra Edición",
            DeweyIndex = "800",
            Published = new DateTime(1867, 01, 01),
        });

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Sin Remedio",
            ISBN10 = "3161484100",
            ISBN13 = "978-3161484100",
            Edition = "Alfaguara",
            DeweyIndex = "800",
            Published = new DateTime(1984, 01, 01),
        });

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "El amor en los tiempos del cólera",
            ISBN10 = "843221754",
            ISBN13 = "978-843221754",
            Edition = "RAE Obra Académica",
            DeweyIndex = "800",
            Published = new DateTime(1985, 01, 01),
        });

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "La hojarasca",
            ISBN10 = "843221754",
            ISBN13 = "978-843221754",
            Edition = "RAE Obra Académica",
            DeweyIndex = "800",
            Published = new DateTime(1955, 01, 01),
        });

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "El coronel no tiene quien le escriba",
            ISBN10 = "843221754",
            ISBN13 = "978-843221754",
            Edition = "RAE Obra Académica",
            DeweyIndex = "800",
            Published = new DateTime(1961, 01, 01),
        });

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "La mala hora",
            ISBN10 = "843221754",
            ISBN13 = "978-843221754",
            Edition = "RAE Obra Académica",
            DeweyIndex = "800",
            Published = new DateTime(1962, 01, 01),
        });

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Los funerales de la Mamá Grande",
            ISBN10 = "843221754",
            ISBN13 = "978-843221754",
            Edition = "RAE Obra Académica",
            DeweyIndex = "800",
            Published = new DateTime(1962, 01, 01),
        });

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "La increíble y triste historia de la cándida Eréndira y de su abuela desalmada",
            ISBN10 = "843221754",
            ISBN13 = "978-843221754",
            Edition = "RAE Obra Académica",
            DeweyIndex = "800",
            Published = new DateTime(1972, 01, 01),
        });


        var authorService = scope.ServiceProvider.GetRequiredService<IAuthorService>();

        await authorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Gabriel",
            LastName = "García Márquez",
            Country = "Colombia",
            Birthdate = new DateTime(1927, 03, 06),
        });

        await authorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Jorge",
            LastName = "Franco",
            Country = "Colombia",
            Birthdate = new DateTime(1962, 07, 02),
        });

        await authorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Mario",
            LastName = "Mendoza",
            Country = "Colombia",
            Birthdate = new DateTime(1964, 06, 08),
        });

        await authorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Jorge",
            LastName = "Isaacs",
            Country = "Colombia",
            Birthdate = new DateTime(1837, 04, 01),
        });

        await authorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Laura",
            LastName = "Restrepo",
            Country = "Colombia",
            Birthdate = new DateTime(1950, 01, 01),
        });

        await authorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Héctor",
            LastName = "Abad Faciolince",
            Country = "Colombia",
            Birthdate = new DateTime(1958, 01, 01),
        });

        await authorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Fernando",
            LastName = "Vallejo",
            Country = "Colombia",
            Birthdate = new DateTime(1942, 01, 01),
        });

        await authorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Alonso",
            LastName = "Salazar",
            Country = "Colombia",
            Birthdate = new DateTime(1965, 01, 01),
        });

        await authorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Hernán",
            LastName = "Rivera Letelier",
            Country = "Chile",
            Birthdate = new DateTime(1950, 01, 01),
        });

        await authorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Isabel",
            LastName = "Allende",
            Country = "Chile",
            Birthdate = new DateTime(1942, 01, 01),
        });

        await authorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Roberto",
            LastName = "Bolaño",
            Country = "Chile",
            Birthdate = new DateTime(1953, 01, 01),
        });

        await authorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Pablo",
            LastName = "Neruda",
            Country = "Chile",
            Birthdate = new DateTime(1904, 01, 01),
        });
    }
}
#endregion
