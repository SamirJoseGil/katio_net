using katio.Business.Interfaces;
using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Katio.Data;

namespace katio.Business.Services;

public class BookService : IBookService
{
    // Lista de libros
    private readonly UnitOfWork _unitOfWork;
    

    // Constructor
    public BookService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Traer todos los libros
    public async Task<BaseMessage<Book>> Index()
    {
        var result = await _unitOfWork.BookRepository.GetAllAsync();
        return result.Any() ? Utilities.BuildResponse<Book>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    #region Create Update Delete

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
            DeweyIndex = book.DeweyIndex,
            AuthorId = book.AuthorId
        };
        try
        {
            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Book> { newBook });
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
        return result != null ? Utilities.BuildResponse<Book>(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Book> { result }) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    #endregion

    #region Find By Book

    // Traer libros por nombre
    public async Task<IEnumerable<Book>> GetBooksByName(string name)
    {
        var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));
        return result;
        //    (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
        //    Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por ISBN10
    public async Task<BaseMessage<Book>> GetBooksByISBN10(string ISBN10)
    {
        var result = await _context.Books.Where(b => b.ISBN10 == ISBN10).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por ISBN13
    public async Task<BaseMessage<Book>> GetBooksByISBN13(string ISBN13)
    {
        var result = await _context.Books.Where(b => b.ISBN13 == ISBN13).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por rango de fecha de publicación
    public async Task<BaseMessage<Book>> GetBooksByPublished(DateOnly startDate, DateOnly endDate)
    {
        var result = await _context.Books.Where(b => b.Published >= startDate && b.Published <= endDate).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por edición
    public async Task<BaseMessage<Book>> GetBooksByEdition(string edition)
    {
        var result = await _context.Books
            .Where(b => b.Edition.Contains(edition, StringComparison.InvariantCultureIgnoreCase))
            .ToListAsync();
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por índice Dewey
    public async Task<BaseMessage<Book>> GetBooksByDeweyIndex(string deweyIndex)
    {
        var result = _context.Books.Where(b => b.DeweyIndex == deweyIndex).ToList();
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    #endregion

    #region Find By Author

    // Traer libros por autor
    public async Task<BaseMessage<Book>> GetBookByAuthorAsync(int authorId)
    {
        var result = await _context.Books
            .Where(b => b.AuthorId == authorId)
            .Include(a => a.Author)
            .ToListAsync();

        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por nombre del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorNameAsync(string authorName)
    {
        var result = await _context.Books
            .Include(a => a.Author)
            .Where(b => b.Author.Name.Contains(authorName, StringComparison.InvariantCultureIgnoreCase))
            .ToListAsync();

        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por apellido del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorLastNameAsync(string authorLastName)
    {
        var result = await _context.Books
            .Include(a => a.Author)
            .Where(b => b.Author.LastName.Contains(authorLastName, StringComparison.InvariantCultureIgnoreCase))
            .ToListAsync();

        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por país del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorCountryAsync(string authorCountry)
    {
        var result = await _context.Books
            .Include(a => a.Author)
            .Where(b => b.Author.Country.Contains(authorCountry, StringComparison.InvariantCultureIgnoreCase))
            .ToListAsync();

        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por nombre y apellido del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorFullNameAsync(string authorName, string authorLastName)
    {
        var result = await _context.Books
            .Include(a => a.Author)
            .Where(b =>
                b.Author.Name.Contains(authorName, StringComparison.InvariantCultureIgnoreCase) &&
                b.Author.LastName.Contains(authorLastName, StringComparison.InvariantCultureIgnoreCase))
            .ToListAsync();

        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por rango de fecha de nacimiento del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorBirthDateRange(DateOnly startDate, DateOnly endDate)
    {
        var result = await _context.Books
            .Include(a => a.Author)
            .Where(b =>
                b.Author.BirthDate >= startDate &&
                b.Author.BirthDate <= endDate)
            .ToListAsync();

        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    #endregion
}