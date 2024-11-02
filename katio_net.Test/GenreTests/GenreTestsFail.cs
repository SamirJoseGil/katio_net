using katio.Business.Interfaces;
using katio.Business.Services;
using katio.Data.Dto;
using katio.Data.Models;
using katio.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq.Expressions;
using System.Net;
using NSubstitute.ExceptionExtensions;

namespace katio.Test.GenreTests;

[TestClass]
public class GenreTestsFail
{
    private readonly IRepository<int, Genre> _genreRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenreService _genreService;
    private List<Genre> _genres;

    public GenreTestsFail()
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
    public async Task CreateGenreFail()
    {
        // Arange
        var newGenre = new Genre 
        { 
            Name = "Fantasy", 
            Description = "La Fantasia es..." 
        };
        var existingGenre = new Genre 
        { 
            Name = "Fantasy", 
            Description = "La Fantasia es..." 
        };

        _genreRepository.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>()).ReturnsForAnyArgs(new List<Genre> { existingGenre });

        // Act
        var result = await _genreService.CreateGenre(newGenre);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for updating genre
    [TestMethod]
    public async Task UpdateGenreFail()
    {
        // Arrange
        _genreRepository.Update(Arg.Any<Genre>()).ThrowsAsyncForAnyArgs(new Exception());
        _unitOfWork.GenreRepository.Returns(_genreRepository);

        // Act
        var result = await _genreService.UpdateGenre(new Genre());

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for deleting genre
    [TestMethod]
    public async Task DeleteGenreFail()
    {
        // Arrange
        var genreToDelete = _genres.First();
        _genreRepository.FindAsync(genreToDelete.Id).ReturnsForAnyArgs(Task.FromResult<Genre>(null));

        // Act
        var result = await _genreService.DeleteGenre(genreToDelete.Id);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for getting all genres
    [TestMethod]
    public async Task GetAllGenresFail()
    {
        // Arrange
        _genreRepository.GetAllAsync().ReturnsForAnyArgs(new List<Genre>());

        // Act
        var result = await _genreService.Index();

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for getting genre by Id
    [TestMethod]
    public async Task GetByGenreIdFail()
    {
        // Arrange
        var genre = _genres.First();
        _genreRepository.FindAsync(genre.Id).ReturnsForAnyArgs(Task.FromResult<Genre>(null));

        // Act
        var result = await _genreService.GetByGenreId(genre.Id);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for getting genre by name
    [TestMethod]
    public async Task GetGenresByNameFail()
    {
        // Arrange
        var genre = _genres.First();
        _genreRepository.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>()).ReturnsForAnyArgs(new List<Genre>());

        // Act
        var result = await _genreService.GetGenresByName(genre.Name);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for getting genre by description
    [TestMethod]
    public async Task GetGenresByDescriptionFail()
    {
        // Arrange
        var genre = _genres.First();
        _genreRepository.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>()).ReturnsForAnyArgs(new List<Genre>());

        // Act
        var result = await _genreService.GetGenresByDescription(genre.Description);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
}