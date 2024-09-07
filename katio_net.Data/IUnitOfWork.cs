using katio.Data.Models;
using katio.Data;

namespace Katio.Data;
public interface IUnitOfWork
{
    IRepository<int, Book> BookRepository { get; }
    IRepository<int, Author> AuthorRepository { get; }
    Task SaveAsync();
}