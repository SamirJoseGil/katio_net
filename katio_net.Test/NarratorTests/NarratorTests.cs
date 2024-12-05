using NSubstitute;
using katio.Data;
using katio.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using katio.Business.Interfaces;
using katio.Business.Services;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using System.Net;

namespace katio.Test.NarratorTests;

[TestClass]
public class NarratorTests
{
    private readonly IRepository<int, Narrator> _narratorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INarratorService _narratorService;
    private List<Narrator> _narrators;

    public NarratorTests()
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

    // Test for creating narrator
    [TestMethod]
    public async Task CreateNarrator()
    {
        // Arrange
        var newNarrator = new Narrator
        {
            Name = "Maria Camila",
            LastName = "Gil Rojas",
            Genre = "Ficcion"
        };
        _narratorRepository.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>()).Returns(new List<Narrator>());
        _narratorRepository.AddAsync(newNarrator).Returns(Task.CompletedTask);

        // Act
        var result = await _narratorService.CreateNarrator(newNarrator);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for updating narrator
    [TestMethod]
    public async Task UpdateNarrator()
    {
        // Arrange
        var existingNarrator = new Narrator
        {
            Id = 1,
            Name = "Maria Updated",
            LastName = "Gil Updated",
            Genre = "Ficcion Updated"
        };

        var updatedNarrator = new Narrator
        {
            Name = "Maria Updated",
            LastName = "Gil Updated",
            Genre = "Ficcion Updated"
        };
        _unitOfWork.NarratorRepository.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>())
        .Returns(new List<Narrator> { existingNarrator });
        _unitOfWork.NarratorRepository.Update(Arg.Any<Narrator>()).Returns(Task.CompletedTask);
        _unitOfWork.SaveAsync().Returns(Task.CompletedTask);

        // Act
        var result = await _narratorService.UpdateNarrator(updatedNarrator);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for deleting narrator
    [TestMethod]
    public async Task DeleteNarrator()
    {
        // Arrange
        var narratorToDelete = _narrators.First();
        _narratorRepository.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>())
            .Returns(Task.FromResult(new List<Narrator> { narratorToDelete }));
        
        _narratorRepository.Delete(narratorToDelete.Id).Returns(Task.CompletedTask);

        // Act
        var result = await _narratorService.DeleteNarrator(narratorToDelete.Id);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
    }
    // Test for getting all narrators
    [TestMethod]
    public async Task GetAllNarrators()
    {
        // Arrange
        _narratorRepository.GetAllAsync().Returns(_narrators);

        // Act
        var result = await _narratorService.Index();

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for getting narrator omniscient
    [TestMethod]
    public async Task SearchNarratorAsync()
    {
        // Arrange
        var searchTerm = "John";
        var narrators = new List<Narrator>
    {
        new Narrator { Name = "John", LastName = "Doe", Genre = "Fiction" },
        new Narrator { Name = "Jane", LastName = "Smith", Genre = "Non-Fiction" }
    };

        _narratorRepository.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>())
        .Returns(narrators);

        // Act
        var result = await _narratorService.SearchNarratorAsync(searchTerm);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // Test for getting narrator by id
    [TestMethod]
    public async Task GetNarratorById()
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.FindAsync(narrator.Id).Returns(narrator);

        // Act
        var result = await _narratorService.GetNarratorById(narrator.Id);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // Test for getting narrator by name
    [TestMethod]
    public async Task GetNarratorsByName()
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>()).Returns(new List<Narrator> { narrator });

        // Act
        var result = await _narratorService.GetNarratorsByName(narrator.Name);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // Test for getting narrator by last name
    [TestMethod]
    public async Task GetNarratorsByLastName()
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>()).Returns(new List<Narrator> { narrator });

        // Act
        var result = await _narratorService.GetNarratorsByLastName(narrator.LastName);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }

    // Test for getting narrator by genre
    [TestMethod]
    public async Task GetNarratorsByGenre()
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>()).Returns(new List<Narrator> { narrator });

        // Act
        var result = await _narratorService.GetNarratorsByGenre(narrator.Genre);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
}