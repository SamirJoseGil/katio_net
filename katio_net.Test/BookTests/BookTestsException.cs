using NSubstitute;
using katio.Data;
using katio.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using katio.Business.Interfaces;
using katio.Business.Services;
using System.Linq.Expressions;

namespace katio.Test.BookTests;

[TestClass]
public class BookTestsException
{
    // Variables
    private readonly IRepository<int, Book> _bookRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookService _bookService;
    private List<Book> _books;

    // Constructor
    public BookTestsException()
    {
        _bookRepository = Substitute.For<IRepository<int, Book>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.BookRepository.Returns(_bookRepository);
        _bookService = new BookService(_unitOfWork);

        _books = new List<Book>()
        {
            new Book
            {
                Name = "Cien años de soledad",
                ISBN10 = "8420471836",
                ISBN13 = "978-8420471839",
                Published = new DateOnly(1967, 06, 05),
                Edition = "RAE Obra Académica",
                DeweyIndex = "800",
                AuthorId = 1
            },
            new Book
            {
                Name = "Huellas",
                ISBN10 = "9584277278",
                ISBN13 = "978-958427275",
                Published = new DateOnly(2019, 01, 01),
                Edition = "1ra Edicion",
                DeweyIndex = "800",
                AuthorId = 3
            }
        };

    }

    // Test for getting all books with repository exception
    [TestMethod]
    public async Task GetAllBooksRepositoryException()
    {
        // Arrange
        _bookRepository.When(x => x.GetAllAsync()).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.Index();

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
        // Test for getting Book omniscient with repository exceptions
    [TestMethod]
    public async Task SearchBookAsyncRepositoryException()
    {
        // Arrange
        var searchTerm = "Book";
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.SearchBookAsync(searchTerm);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for creating book with repository exceptions
    [TestMethod]
    public async Task CreatebookRepositoryException()
    {
        // Arrange
        var newbook = new Book
        {
            Name = "El Otoño del Patriarca",
            ISBN10 = "1234567890",
            ISBN13 = "978-1234567897",
            Published = new DateOnly(1975, 03, 01),
            Edition = "Primera",
            DeweyIndex = "800",
            AuthorId = 1
        };
        _bookRepository.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>()).Returns(new List<Book>());
        _bookRepository.When(x => x.AddAsync(Arg.Any<Book>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.CreateBook(newbook);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for updating book with repository exception
    [TestMethod]
    public async Task UpdateBookRepositoryException()
    {
        // Arrange
        var bookToUpdate = _books.First();
        _bookRepository.FindAsync(bookToUpdate.Id).Returns(bookToUpdate);

        var updatedBook = new Book
        {
            Id = bookToUpdate.Id,
            Name = "Cien años de soledad (Edición Actualizada)",
            ISBN10 = "8420471836",
            ISBN13 = "978-8420471839",
            Published = new DateOnly(1967, 06, 05),
            Edition = "Edición Académica Actualizada",
            DeweyIndex = "800",
            AuthorId = 1
        };
        _bookRepository.FindAsync(updatedBook.Id).Returns(updatedBook);
        _bookRepository.When(x => x.Update(updatedBook)).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.UpdateBook(updatedBook);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for deleting book with repository exception
    [TestMethod]
    public async Task DeleteBookRepositoryException()
    {
        // Arrange
        var bookToDelete = _books.First();
        _bookRepository.FindAsync(bookToDelete.Id).Returns(bookToDelete);
        _bookRepository.When(x => x.Delete(bookToDelete)).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.DeleteBook(bookToDelete.Id);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting book by id with repository exception
    [TestMethod]
    public async Task GetBookByIdRepositoryException()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.FindAsync(book.Id)).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBookById(book.Id);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting book by name with repository exception
    [TestMethod]
    public async Task GetBookByNameRepositoryException()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBooksByName(book.Name);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting books by ISBN10 with repository exception
    [TestMethod]
    public async Task GetBooksByISBN10RepositoryException()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBooksByISBN10(book.ISBN10);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting books by ISBN13 with repository exception
    [TestMethod]
    public async Task GetBooksByISBN13RepositoryException()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBooksByISBN13(book.ISBN13);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting books by edition with repository exception
    [TestMethod]
    public async Task GetBooksByEditionRepositoryException()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBooksByEdition(book.Edition);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting books by published date range with repository exception
    [TestMethod]
    public async Task GetBooksByPublishedDateRangeRepositoryException()
    {
        // Arrange
        var startDate = new DateOnly(1960, 01, 01);
        var endDate = new DateOnly(1970, 12, 31);
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBooksByPublished(startDate, endDate);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting books by Dewey index with repository exception
    [TestMethod]
    public async Task GetBooksByDeweyIndexRepositoryException()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBooksByDeweyIndex(book.DeweyIndex);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting books by author ID with repository exception
    [TestMethod]
    public async Task GetBooksByAuthorIdRepositoryException()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>(), includeProperties: "Author")).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBookByAuthorAsync(book.AuthorId);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting books by author name with repository exception
    [TestMethod]
    public async Task GetBooksByAuthorNameRepositoryException()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>(), includeProperties: "Author")).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBookByAuthorNameAsync(book.Name);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // // Test for getting books by author last name with repository exception
    // [TestMethod]
    // public async Task GetBooksByAuthorLastNameRepositoryException()
    // {
    //     // Arrange
    //     var book = _books.First();
    //     _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>(), includeProperties: "Author")).Do(x => throw new Exception("Repository error"));

    //     // Act
    //     var result = await _bookService.GetBookByAuthorLastNameAsync();

    //     // Assert
    //     Assert.AreEqual((int)result.StatusCode, 500);
    // }
    // Test for getting books by author country with repository exception
    [TestMethod]
    public async Task GetBooksByAuthorCountryRepositoryException()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>(), includeProperties: "Author")).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBookByAuthorCountryAsync("Colombia");

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting books by author full name with repository exception
    [TestMethod]
    public async Task GetBooksByAuthorFullNameRepositoryException()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>(), includeProperties: "Author")).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBookByAuthorFullNameAsync("Gabriel", "García Márquez");

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting books by author birth date range with repository exception
    [TestMethod]
    public async Task GetBooksByAuthorBirthDateRangeRepositoryException()
    {
        // Arrange
        var startDate = new DateOnly(1800, 01, 01);
        var endDate = new DateOnly(2000, 12, 31);
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>(), includeProperties: "Author")).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBookByAuthorBirthDateRange(startDate, endDate);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
}