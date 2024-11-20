using NSubstitute;
using katio.Data;
using katio.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using katio.Business.Interfaces;
using katio.Business.Services;
using System.Linq.Expressions;

namespace katio.Test.AuthorTests;

[TestClass]
public class AuthorTestsException
{
    private readonly IRepository<int, Author> _authorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorService _authorService;
    private List<Author> _authors;

    public AuthorTestsException()
    {
        _authorRepository = Substitute.For<IRepository<int, Author>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.AuthorRepository.Returns(_authorRepository);
        _authorService = new AuthorService(_unitOfWork);

        _authors = new List<Author>()
        {
            new Author 
            {
                Name = "Gabriel",
                LastName = "García Márquez",
                Country = "Colombia",
                BirthDate = new DateOnly(1940, 03, 03)
            },
            new Author 
            {
                Name = "Jorge",
                LastName = "Isaacs",
                Country = "Colombia",
                BirthDate = new DateOnly(1836, 04, 01)            
            }
        };
    }

    // Test for creating author with repository exceptions
    [TestMethod]
    public async Task CreateAuthorRepositoryException()
    {
        // Arrange
        var newAuthor = new Author
        {
            Name = "Gabriel",
            LastName = "García Márquez",
            Country = "Colombia",
            BirthDate = new DateOnly(1940, 03, 03)
        };
        _authorRepository.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>()).Returns(new List<Author>());
        _authorRepository.When(x => x.AddAsync(Arg.Any<Author>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.CreateAuthor(newAuthor);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for updating author with repository exceptions
    [TestMethod]
    public async Task UpdateAuthorRepositoryException()
    {
        // Arrange
        var authorToUpdate = _authors.First();
        _authorRepository.FindAsync(authorToUpdate.Id).Returns(authorToUpdate);
        var updatedAuthor = new Author
        {
            Id = authorToUpdate.Id,
            Name = "Gabriel",
            LastName = "García Márquez",
            Country = "Colombia",
            BirthDate = new DateOnly(1940, 03, 03)
        };
        _authorRepository.FindAsync(authorToUpdate.Id).Returns(authorToUpdate);
        _authorRepository.When(x => x.Update(Arg.Any<Author>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.UpdateAuthor(updatedAuthor);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for deleting author with repository exceptions
    [TestMethod]
    public async Task DeleteAuthorRepositoryException()
    {
        // Arrange
        var authorToDelete = _authors.First();
        _authorRepository.FindAsync(authorToDelete.Id).Returns(authorToDelete);
        _authorRepository.When(x => x.Delete(Arg.Any<Author>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.DeleteAuthor(authorToDelete.Id);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting all authors with repository exceptions
    [TestMethod]
    public async Task GetAllAuthorsRepositoryException()
    {
        // Arrange
        _authorRepository.When(x => x.GetAllAsync()).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.Index();

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting author omniscient with repository exceptions
    [TestMethod]
    public async Task SearchAuthorAsyncRepositoryException()
    {
        // Arrange
        var searchTerm = "Author";
        _authorRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.SearchAuthorAsync(searchTerm);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting author by id with repository exceptions
    [TestMethod]
    public async Task GetAuthorByIdRepositoryException()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.When(x => x.FindAsync(author.Id)).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.GetAuthorById(author.Id);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting author by name with repository exceptions
    [TestMethod]
    public async Task GetAuthorByNameRepositoryException()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.GetAuthorsByName(author.Name);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting author by last name with repository exceptions
    [TestMethod]
    public async Task GetAuthorByLastNameRepositoryException()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.GetAuthorsByLastName(author.LastName);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting author by birth date with repository exceptions
    [TestMethod]
    public async Task GetAuthorsByBirthDateRepositoryException()
    {
        // Arrange
        var startDate = new DateOnly(1830, 01, 01);
        var endDate = new DateOnly(1950, 12, 31);
        _authorRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.GetAuthorsByBirthDate(startDate, endDate);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting author by country with repository exceptions
    [TestMethod]
    public async Task GetAuthorByCountryRepositoryException()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.GetAuthorsByCountry(author.Country);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
}