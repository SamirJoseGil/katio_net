using katio.Business.Interfaces;
using katio.Data.Models;

namespace katio.Business.Services;

public class AuthorService : IAuthorService
{
    // Lista de autores
    private readonly List<Authors> _authors;

    // Constructor
    public AuthorService()
    {
        _authors = new List<Authors>();
    }

    // Traer todos los autores
    public Task<IEnumerable<string>> GetAllAuthors()
    {
        throw new NotImplementedException();
    }



}
