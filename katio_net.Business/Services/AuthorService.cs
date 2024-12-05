using katio.Business.Interfaces;
using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;
using System.Linq.Expressions;

namespace katio.Business.Services;

public class AuthorService : IAuthorService
{
    // Lista de autores
    private readonly IUnitOfWork _unitOfWork;

    // Constructor
    public AuthorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Traer todos los Autores
    public async Task<BaseMessage<Author>> Index()
    {
        try
        {
            var result = await _unitOfWork.AuthorRepository.GetAllAsync();
            return result.Any() ? Utilities.BuildResponse<Author>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND, new List<Author>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    public async Task<BaseMessage<Author>> SearchAuthorAsync(string searchTerm)
    {
        try
        {
            var parameter = Expression.Parameter(typeof(Author), "author");
            var searchExpressions = new List<Expression>();

            var lowerSearchTerm = Expression.Constant(searchTerm.ToLower(), typeof(string));

            // Search in Name
            var nameProperty = Expression.Property(parameter, nameof(Author.Name));
            var nameToLower = Expression.Call(nameProperty, "ToLower", null);
            var nameContains = Expression.Call(
                nameToLower,
                "Contains",
                null,
                lowerSearchTerm
            );
            searchExpressions.Add(nameContains);

            // Search in LastName
            var lastNameProperty = Expression.Property(parameter, nameof(Author.LastName));
            var lastNameToLower = Expression.Call(lastNameProperty, "ToLower", null);
            var lastNameContains = Expression.Call(
                lastNameToLower,
                "Contains",
                null,
                lowerSearchTerm
            );
            searchExpressions.Add(lastNameContains);

            // Search in Country
            var countryProperty = Expression.Property(parameter, nameof(Author.Country));
            var countryToLower = Expression.Call(countryProperty, "ToLower", null);
            var countryContains = Expression.Call(
                countryToLower,
                "Contains",
                null,
                lowerSearchTerm
            );
            searchExpressions.Add(countryContains);

            // Search in BirthDate (if searchTerm is a valid date)
            if (DateOnly.TryParse(searchTerm, out var birthDate))
            {
                var birthDateProperty = Expression.Property(parameter, nameof(Author.BirthDate));
                var birthDateEquals = Expression.Equal(birthDateProperty, Expression.Constant(birthDate));
                searchExpressions.Add(birthDateEquals);
            }

            // Combine all search expressions with OR
            var body = searchExpressions.Aggregate(Expression.OrElse);
            var lambda = Expression.Lambda<Func<Author, bool>>(body, parameter);

            var result = await _unitOfWork.AuthorRepository.GetAllAsync(lambda);
            return result.Any() ? Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND, new List<Author>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    #region Create Update Delete

    // Crear Autores
    public async Task<BaseMessage<Author>> CreateAuthor(Author author)
    {
        var existingAuthor = await _unitOfWork.AuthorRepository.GetAllAsync(a => a.Name == author.Name && a.LastName == author.LastName);

        if (existingAuthor.Any())
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.Conflict, BaseMessageStatus.AUTHOR_ALREADY_EXISTS);
        }
        try
        {
            await _unitOfWork.AuthorRepository.AddAsync(author);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Author> { author });
    }

    // Actualizar Autores
    public async Task<BaseMessage<Author>> UpdateAuthor(Author author)
    {
        var existingAuthor = await _unitOfWork.AuthorRepository.GetAllAsync(a => a.Name == author.Name && a.LastName == author.LastName);

        if (!existingAuthor.Any())
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND);
        }
        try
        {
            await _unitOfWork.AuthorRepository.Update(author);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Author> { author });
    }

    // Eliminar Autores
    public async Task<BaseMessage<Author>> DeleteAuthor(int id)
    {
        var existingAuthor = await _unitOfWork.AuthorRepository.GetAllAsync(a => a.Id == id);

        if (!existingAuthor.Any())
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND);
        }
        try
        {
            await _unitOfWork.AuthorRepository.Delete(id);
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }

        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Author> { });
    }

    #endregion

    #region Find By Author
    //Traer autores por Id
    public async Task<BaseMessage<Author>> GetAuthorById(int id)
    {
        try
        {
            var author = await _unitOfWork.AuthorRepository.FindAsync(id);
            return author != null ? Utilities.BuildResponse<Author>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Author> { author }) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND, new List<Author>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer los autores por nombre
    public async Task<BaseMessage<Author>> GetAuthorsByName(string name)
    {
        try
        {
            var result = await _unitOfWork.AuthorRepository.GetAllAsync(b => b.Name.ToLower().Contains(name.ToLower()));
            return result.Any() ? Utilities.BuildResponse<Author>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND, new List<Author>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    // Traer los autores por apellido
    //este metodo
    public async Task<BaseMessage<Author>> GetAuthorsByLastName(string LastName)
    {
        try
        {
            var result = await _unitOfWork.AuthorRepository.GetAllAsync(b => b.LastName.ToLower().Contains(LastName.ToLower()));
            return result.Any() ? Utilities.BuildResponse<Author>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND, new List<Author>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    // Traer los autores por pais - region
    public async Task<BaseMessage<Author>> GetAuthorsByCountry(string Country)
    {
        try
        {
            var result = await _unitOfWork.AuthorRepository.GetAllAsync(b => b.Country.ToLower().Contains(Country.ToLower()));
            return result.Any() ? Utilities.BuildResponse<Author>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND, new List<Author>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    // Traer los autores por rango de fecha de nacimiento
    public async Task<BaseMessage<Author>> GetAuthorsByBirthDate(DateOnly StartDate, DateOnly EndDate)
    {
        try
        {
            var result = await _unitOfWork.AuthorRepository.GetAllAsync(b => b.BirthDate >= StartDate && b.BirthDate <= EndDate);
            return result.Any() ? Utilities.BuildResponse<Author>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND, new List<Author>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    #endregion
}
