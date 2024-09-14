using katio.Business.Interfaces;
using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace katio.Business.Services;

public class BookService : IBookService
{
    // Lista de libros
    private readonly IUnitOfWork _unitOfWork;

    // Constructor
    public BookService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
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
            await _unitOfWork.BookRepository.AddAsync(newBook);
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Book> { newBook });
    }

    // Actualizar un Libro
    public async Task<BaseMessage<Book>> UpdateBook(Book book)
    {
        var result= await _unitOfWork.BookRepository.FindAsync(book.Id);
        if (result == null)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        result.Name = book.Name;
        result.ISBN10 = book.ISBN10;
        result.ISBN13 = book.ISBN13;
        result.Published = book.Published;
        result.Edition = book.Edition;
        result.DeweyIndex = book.DeweyIndex;

        try 
        {
            await _unitOfWork.BookRepository.Update(result);
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Book> { result });
    }

    // Eliminar un libro
    public async Task<BaseMessage<Book>> DeleteBook(int id)
    {
        var result = await _unitOfWork.BookRepository.FindAsync(id);
        if (result == null)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        try
        {
            await _unitOfWork.BookRepository.Delete(result);
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Book> { result });
    }

    #endregion

    #region Find By Book

    // Traer libros por id
    public async Task<BaseMessage<Book>> GetBookById(int id)
    {
        var result = await _unitOfWork.BookRepository.FindAsync(id);
        return result != null ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Book> { result }) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por nombre
    public async Task<BaseMessage<Book>> GetBooksByName(string name)
    {
        var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));
        //return result;
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por ISBN10
    public async Task<BaseMessage<Book>> GetBooksByISBN10(string ISBN10)
    {
        var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.ISBN10 == ISBN10);
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por ISBN13
    public async Task<BaseMessage<Book>> GetBooksByISBN13(string ISBN13)
    {
        var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.ISBN13 == ISBN13);
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por rango de fecha de publicación
    public async Task<BaseMessage<Book>> GetBooksByPublished(DateOnly startDate, DateOnly endDate)
    {
        var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.Published >= startDate && b.Published <= endDate);
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por edición
    public async Task<BaseMessage<Book>> GetBooksByEdition(string edition)
    {
        var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.Edition.Contains(edition, StringComparison.InvariantCultureIgnoreCase));
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por índice Dewey
    public async Task<BaseMessage<Book>> GetBooksByDeweyIndex(string deweyIndex)
    {
        var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.DeweyIndex == deweyIndex);
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    #endregion

    #region Find By Author

    // Traer libros por autor
    public async Task<BaseMessage<Book>> GetBookByAuthorAsync(int authorId)
    {
        var result = await _unitOfWork.BookRepository.GetAllAsync
            ((b => b.AuthorId == authorId), 
            includeProperties: "Author");
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por nombre del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorNameAsync(string authorName)
    {
        var result = await _unitOfWork.BookRepository.GetAllAsync
            (b => b.Author.Name.Contains(authorName, StringComparison.InvariantCultureIgnoreCase), 
            includeProperties: "Author");
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por apellido del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorLastNameAsync(string authorLastName)
    {
        var result = await _unitOfWork.BookRepository.GetAllAsync
            (b => b.Author.LastName.Contains(authorLastName, StringComparison.InvariantCultureIgnoreCase),
            includeProperties: "Author");
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por país del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorCountryAsync(string authorCountry)
    {
        var result = await _unitOfWork.BookRepository.GetAllAsync(
            b => b.Author.Country.Contains(authorCountry, StringComparison.InvariantCultureIgnoreCase),
            includeProperties: "Author");
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por nombre y apellido del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorFullNameAsync(string authorName, string authorLastName)
    {
        var result = await _unitOfWork.BookRepository.GetAllAsync((
            b => b.Author.Name.Contains(authorName, StringComparison.InvariantCultureIgnoreCase) &&
            b.Author.LastName.Contains(authorLastName, StringComparison.InvariantCultureIgnoreCase)),
            includeProperties: "Author");
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    // Traer libros por rango de fecha de nacimiento del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorBirthDateRange(DateOnly startDate, DateOnly endDate)
    {
        var result = await _unitOfWork.BookRepository.GetAllAsync(
            b => b.Author.BirthDate >= startDate && b.Author.BirthDate <= endDate,
            includeProperties: "Author");
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
    }

    #endregion
}