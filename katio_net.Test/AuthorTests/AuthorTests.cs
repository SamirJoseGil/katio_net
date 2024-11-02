using NSubstitute;
using katio.Data;
using katio.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using katio.Business.Interfaces;
using katio.Business.Services;
using System.Linq.Expressions;

namespace katio.Test.AuthorTests;

[TestClass]
public class AuthorTests
{
    private readonly IRepository<int, Author> _authorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorService _authorService;
    private List<Author> _authors;

    public AuthorTests()
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

    // Test for creating author
    [TestMethod]
    public async Task CreateAuthor()
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
        _authorRepository.AddAsync(newAuthor).Returns(Task.CompletedTask);

        // Act
        var result = await _authorService.CreateAuthor(newAuthor);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for updating author
    [TestMethod]
    public async Task UpdateAuthor()
    {
        // Arrange
        var authorToUpdate = _authors.First();
        _authorRepository.FindAsync(authorToUpdate.Id).Returns(authorToUpdate);
        var updatedAuthor = new Author
        {
            Id = authorToUpdate.Id,
            Name = "Gabriel Updated",
            LastName = "García Márquez Updated",
            Country = "Colombia Updated",
            BirthDate = new DateOnly(1950, 01, 01)
        };
        _authorRepository.FindAsync(updatedAuthor.Id).Returns(updatedAuthor);
        _authorRepository.Update(updatedAuthor).Returns(Task.CompletedTask);        

        // Act
        var result = await _authorService.UpdateAuthor(updatedAuthor);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for deleting author
    [TestMethod]
    public async Task DeleteAuthor()
    {
        // Arrange
        var authorToDelete = _authors.First();
        _authorRepository.FindAsync(authorToDelete.Id).Returns(authorToDelete);
        _authorRepository.Delete(authorToDelete).Returns(Task.CompletedTask);

        // Act
        var result = await _authorService.DeleteAuthor(authorToDelete.Id);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for getting all authors
    [TestMethod]
    public async Task GetAllAuthors() 
    {
        // Arrange
        _authorRepository.GetAllAsync().Returns(_authors);

        // Act
        var result = await _authorService.Index();

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for getting author by id
    [TestMethod]
    public async Task GetAuthorById()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.FindAsync(author.Id).Returns(author);

        // Act
        var result = await _authorService.GetAuthorById(author.Id);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for getting author by name
    [TestMethod]
    public async Task GetAuthorByName()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>()).Returns(new List<Author> { author });

        // Act
        var result = await _authorService.GetAuthorsByName(author.Name);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for getting author by last name
    [TestMethod]
    public async Task GetAuthorByLastName()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>()).Returns(new List<Author> { author });

        // Act
        var result = await _authorService.GetAuthorsByLastName(author.LastName);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for getting author by birth date
    [TestMethod]
    public async Task GetAuthorsByBirthDate()
    {
        // Arrange
        var endDate = new DateOnly(1950, 12, 31);
        var startDate = new DateOnly(1830, 01, 01);
        var expectedAuthors = _authors.Where(a => a.BirthDate >= startDate && a.BirthDate <= endDate).ToList();
        _authorRepository.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>()).Returns(expectedAuthors);

        // Act
        var result = await _authorService.GetAuthorsByBirthDate(startDate, endDate);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for getting author by country
    [TestMethod]
    public async Task GetAuthorByCountry()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>()).Returns(new List<Author> { author });

        // Act
        var result = await _authorService.GetAuthorsByCountry(author.Country);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
}