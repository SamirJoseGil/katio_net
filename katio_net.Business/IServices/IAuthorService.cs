using katio.Data.Models;

namespace katio.Business.Interfaces;

public interface IAuthorService
{
    Task<IEnumerable<string>> GetAllAuthors();
    // Task<string> GetAllAuthorsByName(string Name);
}

