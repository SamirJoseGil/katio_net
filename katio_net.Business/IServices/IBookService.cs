using katio.Data.Dto;
using katio.Data.Models;

namespace katio.Business.Interfaces;

public interface IBookService
{
    Task<BaseMessage<Book>> Index();
    Task<BaseMessage<Book>> GetBooksByName(string Name);
    Task<BaseMessage<Book>> GetBooksByISBN10(string ISBN10);
    Task<BaseMessage<Book>> GetBooksByISBN13(string ISBN13);
    Task<BaseMessage<Book>> GetBooksByEdition(string Edition);
    Task<BaseMessage<Book>> GetBooksByDeweyIndex(string DeweyIndex);
    Task<BaseMessage<Book>> GetBooksByPublished(DateTime StartDate, DateTime EndDate);
    Task<BaseMessage<Book>> DeleteBook(int id);
    Task<BaseMessage<Book>> CreateBook(Book book);
    Task<Book> UpdateBook(Book book);
}
