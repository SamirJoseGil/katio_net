using katio.Business.Interfaces;
using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace katio.Business.Services;

public class BookService : IBookService
{
    // Lista de libros
    private readonly IUnitOfWork _unitOfWork;

    // Constructor
    public BookService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Traer todos los libros
    public async Task<BaseMessage<Book>> Index()
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync(includeProperties: "Author");
            return result.Any() ? Utilities.BuildResponse<Book>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse<Book>(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    public async Task<BaseMessage<Book>> SearchBookAsync(string searchTerm)
    {
        try
        {
            var parameter = Expression.Parameter(typeof(Book), "book");
            var searchExpressions = new List<Expression>();

            var lowerSearchTerm = Expression.Constant(searchTerm.ToLower(), typeof(string));

            // Search in Name
            var nameProperty = Expression.Property(parameter, nameof(Book.Name));
            var nameToLower = Expression.Call(nameProperty, "ToLower", null);
            var nameContains = Expression.Call(
                nameToLower,
                "Contains",
                null,
                lowerSearchTerm
            );
            searchExpressions.Add(nameContains);

            // Search in ISBN10
            var isbn10Property = Expression.Property(parameter, nameof(Book.ISBN10));
            var isbn10ToLower = Expression.Call(isbn10Property, "ToLower", null);
            var isbn10Contains = Expression.Call(
                isbn10ToLower,
                "Contains",
                null,
                lowerSearchTerm
            );
            searchExpressions.Add(isbn10Contains);

            // Search in ISBN13
            var isbn13Property = Expression.Property(parameter, nameof(Book.ISBN13));
            var isbn13ToLower = Expression.Call(isbn13Property, "ToLower", null);
            var isbn13Contains = Expression.Call(
                isbn13ToLower,
                "Contains",
                null,
                lowerSearchTerm
            );
            searchExpressions.Add(isbn13Contains);

            // Search in Edition
            var editionProperty = Expression.Property(parameter, nameof(Book.Edition));
            var editionToLower = Expression.Call(editionProperty, "ToLower", null);
            var editionContains = Expression.Call(
                editionToLower,
                "Contains",
                null,
                lowerSearchTerm
            );
            searchExpressions.Add(editionContains);

            // Search in DeweyIndex
            var deweyIndexProperty = Expression.Property(parameter, nameof(Book.DeweyIndex));
            var deweyIndexToLower = Expression.Call(deweyIndexProperty, "ToLower", null);
            var deweyIndexContains = Expression.Call(
                deweyIndexToLower,
                "Contains",
                null,
                lowerSearchTerm
            );
            searchExpressions.Add(deweyIndexContains);

            // Search in Published (if searchTerm is a valid date)
            if (DateOnly.TryParse(searchTerm, out var publishedDate))
            {
                var publishedProperty = Expression.Property(parameter, nameof(Book.Published));
                var publishedEquals = Expression.Equal(publishedProperty, Expression.Constant(publishedDate));
                searchExpressions.Add(publishedEquals);
            }

            // Combine all search expressions with OR
            var body = searchExpressions.Aggregate(Expression.OrElse);
            var lambda = Expression.Lambda<Func<Book, bool>>(body, parameter);

            var result = await _unitOfWork.BookRepository.GetAllAsync(lambda);
            return result.Any() ? Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    #region Create Update Delete

    // Crear un Libro
    public async Task<BaseMessage<Book>> CreateBook(Book book, IFormFile pdfFile)
    {
        // Verificar si el libro ya existe en base a ISBN
        var existingBook = await _unitOfWork.BookRepository.GetAllAsync(b => b.ISBN10 == book.ISBN10 || b.ISBN13 == book.ISBN13);

        if (existingBook.Any())
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.Conflict, BaseMessageStatus.BOOK_ALREADY_EXISTS);
        }
        try
        {
            // Homogeneizar el archivo PDF: convertir el nombre a minúsculas y quitar caracteres especiales
            var sanitizedFileName = Path.GetFileNameWithoutExtension(pdfFile.FileName)
                .ToLowerInvariant()
                .Replace(" ", "_") + Path.GetExtension(pdfFile.FileName).ToLowerInvariant();

            // Verificar si ya existe un libro con la misma ruta relativa en la base de datos
            var relativePath = Path.Combine("uploads", "books", sanitizedFileName);
            var existingFile = await _unitOfWork.BookRepository.GetAllAsync(b => b.PdfPath == relativePath);

            if (existingFile.Any())
            {
                return Utilities.BuildResponse<Book>(HttpStatusCode.Conflict, BaseMessageStatus.ALREADY_EXISTS_409);
            }

            // Ruta donde se guardará el archivo físicamente
            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "books");
            Directory.CreateDirectory(uploadsFolderPath); // Crear el directorio si no existe

            var filePath = Path.Combine(uploadsFolderPath, sanitizedFileName);

            // Guardar el archivo físicamente
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            // Asignar la ruta relativa al libro
            book.PdfPath = relativePath;

            // Guardar el libro en la base de datos
            await _unitOfWork.BookRepository.AddAsync(book);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }

        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Book> { book });
    }


    // Actualizar un Libro
    public async Task<BaseMessage<Book>> UpdateBook(Book book)
    {
        var existingBook = await _unitOfWork.BookRepository.GetAllAsync(b => b.ISBN10 == book.ISBN10 || b.ISBN13 == book.ISBN13);

        if (!existingBook.Any())
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        try
        {
            await _unitOfWork.BookRepository.Update(book);
            await _unitOfWork.SaveAsync();

        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Book> { book });
    }

    // Eliminar un libro
    public async Task<BaseMessage<Book>> DeleteBook(int id)
    {
        var existingBook = await _unitOfWork.BookRepository.GetAllAsync(b => b.Id == id);

        if (!existingBook.Any())
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        try
        {
            await _unitOfWork.BookRepository.Delete(id);
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Book> { });
    }

    #endregion

    #region Find By Book
    // Traer libros por id
    public async Task<BaseMessage<Book>> GetBookById(int id)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.FindAsync(id);
            return result != null ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Book> { result }) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por nombre
    public async Task<BaseMessage<Book>> GetBooksByName(string name)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.Name.ToLower().Contains(name.ToLower()));
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por ISBN10
    public async Task<BaseMessage<Book>> GetBooksByISBN10(string ISBN10)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.ISBN10 == ISBN10);
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por ISBN13
    public async Task<BaseMessage<Book>> GetBooksByISBN13(string ISBN13)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.ISBN13 == ISBN13);
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por rango de fecha de publicación
    public async Task<BaseMessage<Book>> GetBooksByPublished(DateOnly startDate, DateOnly endDate)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.Published >= startDate && b.Published <= endDate);
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por edición
    public async Task<BaseMessage<Book>> GetBooksByEdition(string edition)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.Edition.ToLower().Contains(edition.ToLower()));
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por índice Dewey
    public async Task<BaseMessage<Book>> GetBooksByDeweyIndex(string deweyIndex)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.DeweyIndex == deweyIndex);
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    #endregion

    #region Find By Author

    // Traer libros por autor
    public async Task<BaseMessage<Book>> GetBookByAuthorAsync(int authorId)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync
                ((b => b.AuthorId == authorId),
                includeProperties: "Author");
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por nombre del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorNameAsync(string authorName)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync
                (b => b.Author != null && b.Author.Name.ToLower().Contains(authorName.ToLower()),
                includeProperties: "Author");
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por apellido del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorLastNameAsync(string authorLastName)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync
                (b => b.Author != null && b.Author.LastName.ToLower().Contains(authorLastName.ToLower()),
                includeProperties: "Author");
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por país del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorCountryAsync(string authorCountry)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync(
                b => b.Author != null && b.Author.Country.ToLower().Contains(authorCountry.ToLower()),
                includeProperties: "Author");
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por nombre y apellido del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorFullNameAsync(string authorName, string authorLastName)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync((
                b => b.Author != null && b.Author.Name.ToLower().Contains(authorName.ToLower()) &&
                b.Author.LastName.ToLower().Contains(authorLastName.ToLower())),
                includeProperties: "Author");
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por rango de fecha de nacimiento del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorBirthDateRange(DateOnly startDate, DateOnly endDate)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync(
                b => b.Author != null && b.Author.BirthDate >= startDate && b.Author.BirthDate <= endDate,
                includeProperties: "Author");
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    #endregion
}