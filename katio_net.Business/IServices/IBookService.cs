using katio.Data.Dto;
using katio.Data.Models;

namespace katio.Business.Interfaces;

public interface IBookService
{
    Task<BaseMessage<Book>> GetAllBooks();
    Task<IEnumerable<Book>> GetAllBooksByName(string Name);
    Task<IEnumerable<Book>> GetAllBookByISBN10(string ISBN10);
    Task<IEnumerable<Book>> GetAllBookByISBN13(string ISBN13);
    Task<IEnumerable<Book>> GetAllBookByEdition(string Edition);
    Task<IEnumerable<Book>> GetAllBookByDeweyIndex(string DeweyIndex);
    Task<IEnumerable<Book>> GetAllBookByPublished(DateTime StartDate, DateTime EndDate);
    Task <BaseMessage<Book>> CreateBook(Book book);
    Task<Book> UpdateBook(Book book);
}
