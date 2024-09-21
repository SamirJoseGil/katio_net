using katio.Business.Interfaces;
using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace katio.Business.Services;

public class AuthorService : IAuthorService
{
    // Lista de autores
    private readonly KatioContext _context;
    private readonly IUnitOfWork _unitOfWork;

    // Constructor
    public AuthorService(KatioContext context)
    {
        _context = context;
    }

    // Traer todos los Autores
    public async Task<BaseMessage<Author>> Index()
    {
        var result = await _unitOfWork.AuthorRepository.GetAllAsync();
        return result.Any() ? Utilities.BuildResponse<Author>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Author>());
    }
    #region Get author by id
    public async Task<BaseMessage<Author>> GetAuthorById(int id)
    {
        var author = await _unitOfWork.AuthorRepository.FindAsync(id);
        return author != null ? Utilities.BuildResponse<Author>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Author> { author }) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND, new List<Author>());
    }
    #endregion

    #region Create Update Delete

    // Crear Autores
    public async Task<BaseMessage<Author>> CreateAuthor(Author author)
    {
        var newAuthor = new Author()
        {
            Name = author.Name,
            LastName = author.LastName,
            Country = author.Country,
            BirthDate = author.BirthDate
        };
        try
        {
            await _unitOfWork.AuthorRepository.AddAsync(newAuthor);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Author> { newAuthor });
    }

    // Actualizar Autores
    public async Task<Author> UpdateAuthor(Author author)
    {
        var result = await _unitOfWork.AuthorRepository.FindAsync(author.Id);
        if (result != null)
        {
            result.Name = author.Name;
            result.LastName = author.LastName;
            result.Country = author.Country;
            result.BirthDate = author.BirthDate;
            await _unitOfWork.SaveAsync();
        }
        return result;
    }

    // Eliminar Autores
    public async Task<BaseMessage<Author>> DeleteAuthor(int Id)
    {
        var result = await _unitOfWork.AuthorRepository.GetAllAsync(b => b.Id == Id);
    
        if (result.Any())
        {
            await _unitOfWork.AuthorRepository.Delete(Id); 
            await _context.SaveChangesAsync();
            return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result);
        }
        return Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Author>());
    }


    #endregion

    #region Find By Author

    // Traer los autores por nombre
    public async Task<BaseMessage<Author>> GetAuthorsByName(string name)
    {
        var result = await _unitOfWork.AuthorRepository.GetAllAsync(b => b.Name.ToLower().Contains(name.ToLower()));
        return result.Any() ? Utilities.BuildResponse<Author>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Author>());
    }
    // Traer los autores por apellido
    public async Task<BaseMessage<Author>> GetAuthorsByLastName(string LastName)
    {
        var result = await _unitOfWork.AuthorRepository.GetAllAsync(b => b.LastName.ToLower().Contains(LastName.ToLower()));
        return result.Any() ? Utilities.BuildResponse<Author>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Author>());
    }
    // Traer los autores por pais - region
    public async Task<BaseMessage<Author>> GetAuthorsByCountry(string Country)
    {
        var result = await _unitOfWork.AuthorRepository.GetAllAsync(b => b.Country.ToLower().Contains(Country.ToLower()));
        return result.Any() ? Utilities.BuildResponse<Author>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Author>());
    }
    // Traer los autores por rango de fecha de nacimiento
    public async Task<BaseMessage<Author>> GetAuthorsByBirthDate(DateOnly StartDate, DateOnly EndDate)
    {
        var result = await _unitOfWork.AuthorRepository.GetAllAsync(b => b.BirthDate >= StartDate && b.BirthDate <= EndDate);
        return result.Any() ? Utilities.BuildResponse<Author>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Author>());
    }

    #endregion

}
