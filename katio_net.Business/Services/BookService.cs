using katio.Business.Interfaces;
using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;
using katio.Business.Utilities;

namespace katio.Business.Services;

public class BookService : IBookService
{
    // Lsita de libros
    private readonly katioContext _context;
    // Constructor
    public BookService(katioContext context)
    {
        _context = context; 
    }
    // Crear un Libro
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
            return ListBooks.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return ListBooks.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Book> { newBook });
    }
    // Actualizar un Libro
    public async Task<Book> UpdateBook(Book book)
    {
        var result = _context.Books.FirstOrDefault(b => b.Id == book.Id);
        if (result != null)
        {
            result.Name = book.Name;
            result.ISBN10 = book.ISBN10;
            result.ISBN13 = book.ISBN13;
            result.Published = book.Published;
            result.Edition = book.Edition;
            result.DeweyIndex = book.DeweyIndex;
            await _context.SaveChangesAsync();
        }
        return result;
    }
    // Eliminar un libro
    public async Task<BaseMessage<Book>> DeleteBook(int Id)
    {
        var result = _context.Books.FirstOrDefault(b => b.Id == Id);
        if (result != null)
        {
            _context.Books.Remove(result);
            await _context.SaveChangesAsync();
        }
        return result != null ? ListBooks.BuildResponse<Book>(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Book> { result }) :
            ListBooks.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }
    // Traer todos los libros
    public async Task<BaseMessage<Book>> Index()
    {
        var result = _context.Books.ToList();
        return result.Any() ? ListBooks.BuildResponse<Book>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            ListBooks.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }
    // Traer libros por nombre
    public async Task<BaseMessage<Book>> GetBooksByName(string Name)
    {
        var result = _context.Books.Where(b => b.Name == Name).ToList();
        return result.Any() ? ListBooks.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            ListBooks.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }
    // Traer libros por ISBN10
    public async Task<BaseMessage<Book>> GetBooksByISBN10(string ISBN10)
    {
        var result = _context.Books.Where(b => b.ISBN10 == ISBN10).ToList();
        return result.Any() ? ListBooks.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            ListBooks.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }
    // Traer libros por ISBN13
    public async Task<BaseMessage<Book>> GetBooksByISBN13(string ISBN13)
    {
        var result = _context.Books.Where(b => b.ISBN13 == ISBN13).ToList();
        return result.Any() ? ListBooks.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            ListBooks.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }
    // Traer libros por rango de fecha de publicación
    public async Task<BaseMessage<Book>> GetBooksByPublished(DateTime StartDate, DateTime EndDate)
    {
        var result = _context.Books.Where(b => b.Published >= StartDate && b.Published <= EndDate).ToList();
        return result.Any() ? ListBooks.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            ListBooks.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }
    // Traer libros por edición
    public async Task<BaseMessage<Book>> GetBooksByEdition(string Edition)
    {
        var result = _context.Books.Where(b => b.Edition == Edition).ToList();
        return result.Any() ? ListBooks.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            ListBooks.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }
    // Traer libros por índice Dewey
    public async Task<BaseMessage<Book>> GetBooksByDeweyIndex(string DeweyIndex)
    {
        var result = _context.Books.Where(b => b.DeweyIndex == DeweyIndex).ToList();
        return result.Any() ? ListBooks.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            ListBooks.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }
}
