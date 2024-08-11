using katio.Business.Interfaces;
using katio.Data.Models;
using System.Xml.Linq;

namespace katio.Business.Services;

public class BookService : IBookService
{
    private readonly List<Books> _books;
    public BookService()
    {
        _books = InitializeBooks(); 
    }

    // Traer todos los libros
    public async Task<IEnumerable<Books>> GetAllBooks()
    {
        return await Task.FromResult(_books);
    }
    // Traer los libros por nombre
    public async Task<IEnumerable<Books>> GetAllBooksByName(string Name)
    {
        var result = _books.Where(b => b.Name.Contains(Name, StringComparison.OrdinalIgnoreCase));
        return await Task.FromResult(result);
    }
    // Traer los libros por ISBN10
    public async Task<IEnumerable<Books>> GetAllBookByISBN10(string ISBN10)
    {
        var result = _books.Where(b => b.ISBN10.Contains(ISBN10, StringComparison.OrdinalIgnoreCase));
        return await Task.FromResult(result);
    }
    // Traer los libros por ISBN13
    public async Task<IEnumerable<Books>> GetAllBookByISBN13(string ISBN13)
    {
        var result = _books.Where(b => b.ISBN13.Contains(ISBN13, StringComparison.OrdinalIgnoreCase));
        return await Task.FromResult(result);
    }
    // Traer los libros por edición
    public async Task<IEnumerable<Books>> GetAllBookByEdition(string Edition)
    {
        var result = _books.Where(b => b.Edition.Contains(Edition, StringComparison.OrdinalIgnoreCase));
        return await Task.FromResult(result);
    }
    // Traer los libros por índice Dewey
    public async Task<IEnumerable<Books>> GetAllBookByDeweyIndex(string DeweyIndex)
    {
        var result = _books.Where(b => b.DeweyIndex.Contains(DeweyIndex, StringComparison.OrdinalIgnoreCase));
        return await Task.FromResult(result);
    }
    /*
    public async Task<IEnumerable<Books>> GetAllBookByPublished(DateTime Published)
    {
        var result = _books.Where(b => b.Published == Published);
        return await Task.FromResult(result);
    }
    */





    // This method implement a list of books to retorn
    private List<Books> InitializeBooks()
    {
        return new List<Books>
        {
            new Books
            {
                Name = "El principito",
                ISBN10 = "12345678910",
                ISBN13 = "1234567890123",
                Edition = "ElColombiano",
                DeweyIndex = "813.54",
                Published = new DateTime(1943, 4, 6),
                Id = 1
            },
            new Books
            {
                Name = "El alquimista",
                ISBN10 = "12345678980",
                ISBN13 = "1234567890123",
                Edition = "ElColombiano",
                DeweyIndex = "813.54",
                Published = new DateTime(1988, 4, 6),
                Id = 2
            },
            new Books
            {
                Name = "El arte de la guerra",
                ISBN10 = "12345678970",
                ISBN13 = "1234567890123",
                Edition = "ElColombiano",
                DeweyIndex = "813.54",
                Published = new DateTime(1988, 4, 6),
                Id = 3
            },
        };
    }
}
