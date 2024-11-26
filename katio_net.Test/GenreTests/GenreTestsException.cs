using katio.Business.Interfaces;
using katio.Business.Services;
using katio.Data.Dto;
using katio.Data.Models;
using katio.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq.Expressions;
using System.Net;

namespace katio.Test.GenreTests;

[TestClass]
public class GenreTestsException
{
    private readonly IRepository<int, Genre> _genreRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenreService _genreService;
    private List<Genre> _genres;

    public GenreTestsException()
    {
        _genreRepository = Substitute.For<IRepository<int, Genre>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.GenreRepository.Returns(_genreRepository);
        _genreService = new GenreService(_unitOfWork);
        _genres = new List<Genre>
        {
            new Genre 
            { 
                Name = "Fantasy", 
                Description = "La Fantasia es..." 
            },
            new Genre 
            { 
                Name = "Science Fiction", 
                Description = "La Ciencia Ficcion es..." 
            }
        };
    }

    // Test for creating genre
    [TestMethod]
    public async Task CreateGenreRepositoryException()
    {
        // Arange
        var newGenre = new Genre 
        { 
            Name = "Fantasy", 
            Description = "La Fantasia es..." 
        };
        _genreRepository.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>()).Returns(new List<Genre>());
        _genreRepository.When(x => x.AddAsync(Arg.Any<Genre>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _genreService.CreateGenre(newGenre);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for updating genre
    [TestMethod]
    public async Task UpdateGenreRepositoryException()
    {
        // Arrange
        var existingGenre = new Genre
        {
            Id = 1,
            Name = "Science Fiction",
            Description = "A genre about futuristic and scientific concepts"
        };

        var updatedGenre = new Genre
        {
            Id = existingGenre.Id,
            Name = "Fantasy",
            Description = "La Fantasía es un género literario..."
        };

        _unitOfWork.GenreRepository.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>())
            .Returns(new List<Genre> { existingGenre });
        _genreRepository.When(x => x.Update(Arg.Any<Genre>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _genreService.UpdateGenre(updatedGenre);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }

    // Test for deleting genre
    [TestMethod]
    public async Task DeleteGenreRepositoryException()
    {
        var genreToDelete = new Genre
        {
            Id = 1,
            Name = "Fantasy",
            Description = "La Fantasía es un género literario..."
        };

        // Arange
        _unitOfWork.GenreRepository.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>())
            .Returns(new List<Genre> { genreToDelete });

        _unitOfWork.GenreRepository.When(x => x.Delete(genreToDelete.Id))
            .Do(x => throw new Exception("Repository error"));
        // Act
        var result = await _genreService.DeleteGenre(genreToDelete.Id);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting all genres
    [TestMethod]
    public async Task GetAllGenresRepositoryException()
    {
        // Arange
        _genreRepository.When(x => x.GetAllAsync()).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _genreService.Index();
        
        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting genre by id
    [TestMethod]
    public async Task GetGenreByIdRepositoryException()
    {
        // Arange
        var genre = _genres.First();
        _genreRepository.When(x => x.FindAsync(genre.Id)).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _genreService.GetByGenreId(genre.Id);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting genre by name
    [TestMethod]
    public async Task GetGenreByNameRepositoryException()
    {
        // Arange
        var genre = _genres.First();
        _genreRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _genreService.GetGenresByName(genre.Name);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting genre by description
    [TestMethod]
    public async Task GetGenreByDescriptionRepositoryException()
    {
        // Arange
        var genre = _genres.First();
        _genreRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _genreService.GetGenresByDescription(genre.Description);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
}