using NSubstitute;
using katio.Data;
using katio.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using katio.Business.Interfaces;
using katio.Business.Services;
using System.Linq.Expressions;
using System.Net;

namespace katio.Test.NarratorTests;

[TestClass]
public class NarratorTestsException
{
    private readonly IRepository<int, Narrator> _narratorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INarratorService _narratorService;
    private List<Narrator> _narrators;

    public NarratorTestsException()
    {
        _narratorRepository = Substitute.For<IRepository<int, Narrator>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.NarratorRepository.Returns(_narratorRepository);
        _narratorService = new NarratorService(_unitOfWork);

        _narrators = new List<Narrator>
        {
            new Narrator
            {
                Id = 1,
                Name = "Maria Camila",
                LastName = "Gil Rojas",
                Genre = "Ficcion"
            },
            new Narrator
            {
                Id = 2,
                Name = "Juan",
                LastName = "Perez",
                Genre = "Ficcion"
            }
        };
    }

    // Test for creating a narrator with repository exceptions
    [TestMethod]
    public async Task CreateNarratorRepositoryException()
    {
        // Arrange
        var narrator = new Narrator
        {
            Name = "Maria Camila",
            LastName = "Gil Rojas",
            Genre = "Ficcion"
        };
        _narratorRepository.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>()).Returns(new List<Narrator>());
        _narratorRepository.When(x => x.AddAsync(Arg.Any<Narrator>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _narratorService.CreateNarrator(narrator);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for updating a narrator with repository exceptions
    [TestMethod]
    public async Task UpdateNarratorRepositoryException()
    {
        // Arrange
        var narratorToUpdate = _narrators.First();
        _narratorRepository.FindAsync(narratorToUpdate.Id).Returns(narratorToUpdate);
        var updatedNarrator = new Narrator
        {
            Id = narratorToUpdate.Id,
            Name = "John",
            LastName = "Doe",
            Genre = "Fiction"
        };
        _narratorRepository.FindAsync(narratorToUpdate.Id).Returns(narratorToUpdate);
        _narratorRepository.When(x => x.Update(Arg.Any<Narrator>())).Do(x => throw new Exception("Repository exception"));

        // Act
        var result = await _narratorService.UpdateNarrator(updatedNarrator);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for deleting a narrator with repository exceptions
    [TestMethod]
    public async Task DeleteNarratorRepositoryException()
    {
        // Arrange
        var narratorToDelete = _narrators.First();
        _narratorRepository.FindAsync(narratorToDelete.Id).Returns(narratorToDelete);
        _narratorRepository.When(x => x.Delete(narratorToDelete)).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _narratorService.DeleteNarrator(narratorToDelete.Id);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting all narrators with repository exceptions
    [TestMethod]
    public async Task GetAllNarratorsRepositoryException()
    {
        // Arrange
        _narratorRepository.When(x => x.GetAllAsync()).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _narratorService.Index();

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting a narrator omniscient with repository exceptions
    [TestMethod]
    public async Task SearchNarratorAsyncRepositoryException()
    {
        // Arrange
        var searchTerm = "John";
        _narratorRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _narratorService.SearchNarratorAsync(searchTerm);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting a narrator by Id with repository exceptions
    [TestMethod]
    public async Task GetNarratorByIdRepositoryException()
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.When(x => x.FindAsync(narrator.Id)).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _narratorService.GetNarratorById(narrator.Id);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }

    // Test for getting a narrator by name with repository exceptions
    [TestMethod]
    public async Task GetNarratorsByNameRepositoryException()
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _narratorService.GetNarratorsByName(narrator.Name);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }

    // Test for getting a narrator by last name with repository exceptions
    [TestMethod]
    public async Task GetNarratorsByLastNameRepositoryException()
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _narratorService.GetNarratorsByLastName(narrator.LastName);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }

    // Test for getting a narrator by genre with repository exceptions
    [TestMethod]
    public async Task GetNarratorsByGenreRepositoryException()
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _narratorService.GetNarratorsByGenre(narrator.Genre);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
}