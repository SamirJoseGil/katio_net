using katio.Business.Interfaces;
using katio.Data.Models;
using System.Xml.Linq;
using katio.Business.Utilities;
using katio.Data.Dto;
using katio.Data;
using System.Net;

namespace katio.Business.Services;

public class BookService : IBookService
{
    private readonly katioContext _context;

    public BookService(katioContext context)
    {
        _context = context; 
    }

    public async Task<BaseMessage<Book>> CreateBook(Book book)
    {
        var newBook = new Book()
        {
            Name = book.Name,
            ISBN10 = book.ISBN10,
            ISBN13 = book.ISBN13,
            Published = book.Published,
            Edition = book.Edition,
            DeweyIndex = book.DeweyIndex
        };
        try
        {
           await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex) 
        {
            return Utilities.ListBooks.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.ListBooks.BuildResponse(HttpStatusCode.OK, BaseMessage.OK_200, new List<Book> { newBook });
    }

    // Traer todos los libros
    public async Task<BaseMessage<Book>> GetAllBooks()
    {
        var result = _context.Books.ToList();
        return result.Any() ? Utilities.ListBooks.BuildResponse<Book>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.ListBooks.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }



    //TODO DE ACA HACIA ABAJO NADA FUNCIONA, REVISAR
    // Traer los libros por nombre
    public async Task<IEnumerable<Book>> GetAllBooksByName(string Name)
    {
        var result = _.Where(b => b.Name.Contains(Name, StringComparison.OrdinalIgnoreCase));
        return await Task.FromResult(result);
    }

    // Traer los libros por ISBN10
    public async Task<IEnumerable<Book>> GetAllBookByISBN10(string ISBN10)
    {
        var result = _books.Where(b => b.ISBN10.Contains(ISBN10, StringComparison.OrdinalIgnoreCase));
        return await Task.FromResult(result);
    }

    // Traer los libros por ISBN13
    public async Task<IEnumerable<Book>> GetAllBookByISBN13(string ISBN13)
    {
        var result = _books.Where(b => b.ISBN13.Contains(ISBN13, StringComparison.OrdinalIgnoreCase));
        return await Task.FromResult(result);
    }

    // Traer los libros por edición
    public async Task<IEnumerable<Book>> GetAllBookByEdition(string Edition)
    {
        var result = _books.Where(b => b.Edition.Contains(Edition, StringComparison.OrdinalIgnoreCase));
        return await Task.FromResult(result);
    }

    // Traer los libros por índice Dewey
    public async Task<IEnumerable<Book>> GetAllBookByDeweyIndex(string DeweyIndex)
    {
        var result = _books.Where(b => b.DeweyIndex.Contains(DeweyIndex, StringComparison.OrdinalIgnoreCase));
        return await Task.FromResult(result);
    }

    // Traer un libro por rango de publicacion 
    public async Task<IEnumerable<Book>> GetAllBookByPublished(DateTime StartDate, DateTime EndDate)
    {
        var result = _books.Where(b => b.Published >= StartDate && b.Published <= EndDate);
        return await Task.FromResult(result);
    }

    // Crear un libro

    // Editar un libro
    public async Task<Book> UpdateBook(Book book)
    {
        var bookToUpdate = _books.FirstOrDefault(b => b.Id == book.Id);
        if (bookToUpdate != null)
        {
            bookToUpdate.Name = book.Name;
            bookToUpdate.ISBN10 = book.ISBN10;
            bookToUpdate.ISBN13 = book.ISBN13;
            bookToUpdate.Edition = book.Edition;
            bookToUpdate.DeweyIndex = book.DeweyIndex;
            bookToUpdate.Published = book.Published;
        }
        return await Task.FromResult(bookToUpdate);
    }
}
