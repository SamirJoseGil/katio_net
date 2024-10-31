using NSubstitute;
using katio.Data;
using katio.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using katio.Business.Interfaces;
using katio.Business.Services;
using System.Linq.Expressions;
using katio.Data.Dto;
using System.Net;
using NSubstitute.ExceptionExtensions;

namespace katio.Test;

[TestClass]
public class BookTests
{
    // Variables
    private readonly IRepository<int, Book> _bookRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookService _bookService;
    private List<Book> _books;

    // Constructor
    public BookTests()
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

    #region Test Methods Simple

    // Test for getting all books
    [TestMethod]
    public async Task GetAllBooks()
    {
        // Arrange
        _bookRepository.GetAllAsync().Returns(_books);

        // Act
        var result = await _bookService.Index();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.ResponseElements.Count());
    }

    // Test for creating a book
    [TestMethod]
    public async Task CreateBook()
    {
        // Arrange
        var newBook = new Book
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
        _bookRepository.AddAsync(newBook).Returns(Task.CompletedTask);

        // Act
        var result = await _bookService.CreateBook(newBook);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // Test for updating a book
    [TestMethod]
    public async Task UpdateBook()
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

        _bookRepository.Update(updatedBook).Returns(Task.CompletedTask);

        // Act
        var result = await _bookService.UpdateBook(updatedBook);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // Test for deleting a book
    [TestMethod]
    public async Task DeleteBook()
    {
        // Arrange
        var bookToDelete = _books.First();
        _bookRepository.FindAsync(bookToDelete.Id).Returns(bookToDelete);
        _bookRepository.Delete(bookToDelete).Returns(Task.CompletedTask);

        // Act
        var result = await _bookService.DeleteBook(bookToDelete.Id);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // Test for getting a book by ID
    [TestMethod]
    public async Task GetBookById()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.FindAsync(book.Id).Returns(book);

        // Act
        var result = await _bookService.GetBookById(book.Id);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // Test for getting a book by name
    [TestMethod]
    public async Task GetBookByName()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>()).Returns(new List<Book> { book });

        // Act
        var result = await _bookService.GetBooksByName(book.Name);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // Test to get books by ISBN10
    [TestMethod]
    public async Task GetBooksByISBN10()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>()).Returns(new List<Book> { book });

        // Act
        var result = await _bookService.GetBooksByISBN10(book.ISBN10);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // Test to get books by ISBN13
    [TestMethod]
    public async Task GetBooksByISBN13()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>()).Returns(new List<Book> { book });

        // Act
        var result = await _bookService.GetBooksByISBN13(book.ISBN13);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // Test to get books by published date range
    [TestMethod]
    public async Task GetBooksByPublishedDateRange()
    {
        // Arrange
        var startDate = new DateOnly(1960, 01, 01);
        var endDate = new DateOnly(1970, 12, 31);
        var expectedBooks = _books.Where(b => b.Published >= startDate && b.Published <= endDate).ToList();
        _bookRepository.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>()).Returns(expectedBooks);

        // Act
        var result = await _bookService.GetBooksByPublished(startDate, endDate);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
        Assert.AreEqual(1, result.ResponseElements.Count());
    }

    // Test to get books by edition
    [TestMethod]
    public async Task GetBooksByEdition()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>()).Returns(new List<Book> { book });

        // Act
        var result = await _bookService.GetBooksByEdition(book.Edition);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // Test to get books by Dewey index
    [TestMethod]
    public async Task GetBooksByDeweyIndex()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>()).Returns(new List<Book> { book });

        // Act
        var result = await _bookService.GetBooksByDeweyIndex(book.DeweyIndex);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // Test to get books by author ID
    [TestMethod]
    public async Task GetBooksByAuthorId()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>(), includeProperties: "Author").Returns(new List<Book> { book });

        // Act
        var result = await _bookService.GetBookByAuthorAsync(book.AuthorId);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // Test to get books by author name
    [TestMethod]
    public async Task GetBooksByAuthorName()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>(), includeProperties: "Author").Returns(new List<Book> { book });

        // Act
        var result = await _bookService.GetBookByAuthorNameAsync(book.Name);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // // Test to get books by author last name
    // [TestMethod]
    // public async Task GetBooksByAuthorLastName()
    // {
    //     // Arrange
    //     var book = _books.First();
    //     _bookRepository.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>(), includeProperties: "Author").Returns(new List<Book> { book });

    //     // Act
    //     var result = await _bookService.GetBookByAuthorLastNameAsync("García Márquez");

    //     // Assert
    //     Assert.IsTrue(result.ResponseElements.Any());
    // }

    // Test to get books by author country
    [TestMethod]
    public async Task GetBooksByAuthorCountry()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>(), includeProperties: "Author").Returns(new List<Book> { book });

        // Act
        var result = await _bookService.GetBookByAuthorCountryAsync("Colombia");

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // Test to get books by author full name
    [TestMethod]
    public async Task GetBooksByAuthorFullName()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>(), includeProperties: "Author").Returns(new List<Book> { book });

        // Act
        var result = await _bookService.GetBookByAuthorFullNameAsync("Gabriel", "García Márquez");

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // Test to get books by author birth date range
    [TestMethod]
    public async Task GetBooksByAuthorBirthDateRange()
    {
        // Arrange
        var startDate = new DateOnly(1800, 01, 01);
        var endDate = new DateOnly(2000, 12, 31);
        _bookRepository.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>(), includeProperties: "Author").Returns(_books);

        // Act
        var result = await _bookService.GetBookByAuthorBirthDateRange(startDate, endDate);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    #endregion

    #region Repository Exceptions

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

    #endregion

    #region Test Methods Fail

    // Test for creating book fail
    [TestMethod]
    public async Task CreateBookFail()
    {
        // Arrange
        var existingBook = new Book
        {
            Name = "El Otoño del Patriarca",
            ISBN10 = "1234567890",
            ISBN13 = "978-1234567897",
            Published = new DateOnly(1975, 03, 01),
            Edition = "Primera",
            DeweyIndex = "800",
            AuthorId = 1
        };
        var newBook = new Book
        {
            Name = "El Otoño del Patriarca",
            ISBN10 = "1234567890",
            ISBN13 = "978-1234567897",
            Published = new DateOnly(1975, 03, 01),
            Edition = "Primera",
            DeweyIndex = "800",
            AuthorId = 1
        };
        _bookRepository.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>()).ReturnsForAnyArgs(new List<Book> { existingBook });

        // Act
        var result = await _bookService.CreateBook(newBook);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }

    // Test for failing to updating a book
    [TestMethod]
    public async Task UpdateBookFail()
    {
        // Arrange
        _bookRepository.Update(Arg.Any<Book>()).ThrowsAsyncForAnyArgs(new Exception());
        _unitOfWork.BookRepository.Returns(_bookRepository);

        // Act
        var result = await _bookService.UpdateBook(new Book());

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }

    // Test for failing to delete a book
    [TestMethod]
    public async Task DeleteBookFail()
    {
        // Arrange
        var bookToDelete = _books.First();
        _bookRepository.FindAsync(bookToDelete.Id).ReturnsForAnyArgs(Task.FromResult<Book?>(null));

        // Act
        var result = await _bookService.DeleteBook(bookToDelete.Id);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }

    // Test for getting all books Fail
    [TestMethod]
    public async Task GetAllBooksFail()
    {
        // Arrange
        _bookRepository.GetAllAsync().ReturnsForAnyArgs(new List<Book>());

        // Act
        var result = await _bookService.Index();

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }

    // Test for failing to get book by ID
    [TestMethod]
    public async Task GetBookByIdFail()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.FindAsync(book.Id)).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBookById(book.Id);

        // Assert
    Assert.AreEqual((int)result.StatusCode, 500);
    }

    // Test for failing to get book by name
    [TestMethod]
    public async Task GetBookByNameFail()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBooksByName(book.Name);

        // Assert
    Assert.AreEqual((int)result.StatusCode, 500);
    }

    // Test for failing to get books by ISBN10
    [TestMethod]
    public async Task GetBooksByISBN10Fail()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBooksByISBN10("NonExistentISBN");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
    }

    // Test for failing to get books by ISBN13
    [TestMethod]
    public async Task GetBooksByISBN13Fail()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBooksByISBN13("NonExistentISBN13");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
    }

    // Test for failing to get books by edition
    [TestMethod]
    public async Task GetBooksByEditionFail()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBooksByEdition("NonExistentEdition");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
    }

    // Test for failing to get books by published date range
    [TestMethod]
    public async Task GetBooksByPublishedDateRangeFail()
    {
        // Arrange
        var startDate = new DateOnly(1960, 01, 01);
        var endDate = new DateOnly(1970, 12, 31);
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBooksByPublished(startDate, endDate);

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
    }

    // Test for failing to get books by Dewey index
    [TestMethod]
    public async Task GetBooksByDeweyIndexFail()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _bookService.GetBooksByDeweyIndex("NonExistentDewey");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
    }

    #endregion
}
