using katio.Data.Models;

namespace katio.Business.Interfaces;

public interface IBookService
{
    Task<IEnumerable<Books>> GetAllBooks();
    Task<IEnumerable<Books>> GetAllBooksByName(string Name);
    Task<IEnumerable<Books>> GetAllBookByISBN10(string ISBN10);
    Task<IEnumerable<Books>> GetAllBookByISBN13(string ISBN13);
    Task<IEnumerable<Books>> GetAllBookByEdition(string Edition);
    Task<IEnumerable<Books>> GetAllBookByDeweyIndex(string DeweyIndex);
    //Task<IEnumerable<Books>> GetAllBookByPublished(DateTime Published);
}
