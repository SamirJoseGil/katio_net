using katio.Business.Interfaces;
using katio.Data.Models;

namespace katio.Business.Services;

public class AuthorService : IAuthorService
{
    // Lista de autores
    private readonly List<Author> _authors;

    // Constructor
    public AuthorService()
    {
        _authors = new List<Author>();
    }

    // Traer todos los autores
    public Task<IEnumerable<string>> GetAllAuthors()
    {
        throw new NotImplementedException();
    }



}
