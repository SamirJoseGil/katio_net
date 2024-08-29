using katio.Business.Interfaces;
using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;
using katio.Business.Utilities;

namespace katio.Business.Services;

public class AuthorService : IAuthorService
{
    // Lista de autores
    private readonly katioContext _context;

    // Constructor
    public AuthorService(katioContext context)
    {
        _context = context;
    }
    // Crear Autores
    public async Task<BaseMessage<Author>> CreateAuthor(Author author)
    {
        var newAuthor = new Author()
        {
            Name = author.Name,
            LastName = author.LastName,
            Country = author.Country,
            Birthdate = author.Birthdate
        };
        try
        {
            await _context.Authors.AddAsync(newAuthor);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return ListAuthors.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return ListAuthors.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Author> { newAuthor });
    }
    // Actualizar Autores
    public async Task<Author> UpdateAuthor(Author author)
    {
        var result = _context.Authors.FirstOrDefault(b => b.Id == author.Id);
        if (result != null)
        {
            result.Name = author.Name;
            result.LastName = author.LastName;
            result.Country = author.Country;
            result.Birthdate = author.Birthdate;
            await _context.SaveChangesAsync();
        }
        return result;
    }
    // Eliminar Autores
    public async Task<BaseMessage<Author>> DeleteAuthor(int Id)
    {
        var result = _context.Authors.FirstOrDefault(b => b.Id == Id);
        if (result != null)
        {
            _context.Authors.Remove(result);
            await _context.SaveChangesAsync();
        }
        return result != null ? ListAuthors.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Author> { result }) :
            ListAuthors.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Author>());
    }
    // Traer todos los Autores
    public async Task<BaseMessage<Author>> Index()
    {
        var result = _context.Authors.ToList();
        return result.Any() ? ListAuthors.BuildResponse<Author>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            ListAuthors.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Author>());
    }
    // Traer los autores por nombre
    public async Task<BaseMessage<Author>> GetAuthorsByName(string name)
    {
        var result = _context.Authors.Where(b => b.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        return result.Any() ? ListAuthors.BuildResponse<Author>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            ListAuthors.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Author>());
    }
    // Traer los autores por apellido
    public async Task<BaseMessage<Author>> GetAuthorsByLastName(string LastName)
    {
        var result = _context.Authors.Where(b => b.LastName.Contains(LastName, StringComparison.OrdinalIgnoreCase)).ToList();
        return result.Any() ? ListAuthors.BuildResponse<Author>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            ListAuthors.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Author>());
    }
    // Traer los autores por pais - region
    public async Task<BaseMessage<Author>> GetAuthorsByCountry(string Country)
    {
        var result = _context.Authors.Where(b => b.Country.Contains(Country, StringComparison.OrdinalIgnoreCase)).ToList();
        return result.Any() ? ListAuthors.BuildResponse<Author>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            ListAuthors.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Author>());
    }
    // Traer los autores por rango de fecha de nacimiento
    public async Task<BaseMessage<Author>> GetAuthorsByBirthDate(DateTime StartDate, DateTime EndDate)
    {
        var result = _context.Authors.Where(b => b.Birthdate >= StartDate && b.Birthdate <= EndDate).ToList();
        return result.Any() ? ListAuthors.BuildResponse<Author>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            ListAuthors.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Author>());
    }
}
