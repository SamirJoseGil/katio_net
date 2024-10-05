using NSubstitute;

using katio.Data;
using katio.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using katio.Business.Interfaces;
using katio.Business.Services;

namespace katio.Test;


[TestClass]
public class BookTests
{
    private readonly IRepository<int, Book> _bookRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookService _bookService;

    public BookTests()
    {
        _bookRepository = Substitute.For<IRepository<int, Book>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _bookService = new BookService(_unitOfWork);
    }

    [TestMethod]
    public async Task GetAllBooks() 
    {
    }


}