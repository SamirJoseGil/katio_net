using NSubstitute;
using katio.Data;
using katio.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using katio.Business.Interfaces;
using katio.Business.Services;
using System.Linq.Expressions;

namespace katio.Test.AudioBookTests;

[TestClass]

public class AudioBookTests
{
    private readonly IRepository<int, AudioBook> _audioBookRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAudioBookService _audioBookService;
    private List<AudioBook> _audioBooks;

    public AudioBookTests()
    {
        _audioBookRepository = Substitute.For<IRepository<int, AudioBook>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.AudioBookRepository.Returns(_audioBookRepository);
        _audioBookService = new AudioBookService(_unitOfWork);
        _audioBooks = new List<AudioBook>()
    { new AudioBook {
        Id = 1,
            Name = "Cien años de soledad",
            ISBN10 = "8420471836",
            ISBN13 = "978-8420471839",
            Published = new DateOnly(1967, 06, 05),
            Edition = "RAE Obra Académica",
            Genre = "Ficcion",
            LenghtInSeconds = 1,
            NarratorId = 1,
            FrontPage = null
      }, new AudioBook {
        Id = 2,
            Name = "Huellas",
            ISBN10 = "9584277278",
            ISBN13 = "978-958427275",
            Published = new DateOnly(2019, 01, 01),
            Edition = "1ra Edicion",
            Genre = "Ficcion",
            LenghtInSeconds = 10,
            NarratorId = 3,
            FrontPage = null
      }};
    }

    // Test for getting all audio books
    [TestMethod]
    public async Task GetAllAudioBooks()
    {
        // Arrange
        _audioBookRepository.GetAllAsync().Returns(_audioBooks);

        // Act
        var result = await _audioBookService.Index();

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for searching audiobook omniscient
    [TestMethod]
    public async Task SearchAudioBookAsync()
    {
        // Arrange
        var searchTerm = "Cien";
        var audioBooks = _audioBooks;
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())
        .Returns(audioBooks);

        // Act
        var result = await _audioBookService.SearchAudioBookAsync(searchTerm);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for getting an audiobook by id
    [TestMethod]
    public async Task GetAudioBookById()
    {
        // Arrange
        var audioBook = _audioBooks.First();
        _audioBookRepository.FindAsync(audioBook.Id).Returns(audioBook);

        // Act
        var result = await _audioBookService.GetAudioBookById(audioBook.Id);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for getting audioBook by name
    [TestMethod]
    public async Task GetAudioBookByName()
    {
        // Arrange
        var audioBook = _audioBooks.First();
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).Returns(new List<AudioBook> { audioBook });

        // Act
        var result = await _audioBookService.GetByAudioBookName(audioBook.Name);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for getting audiobook be ISBN10
    [TestMethod]
    public async Task GetByAudioBookISBN10()
    {
        //Arrange
        var audioBook = _audioBooks.First();
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).Returns(new List<AudioBook> { audioBook });

        // Act
        var result = await _audioBookService.GetByAudioBookISBN10(audioBook.ISBN10);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for getting audiobook be ISBN13
    [TestMethod]
    public async Task GetByAudioBookISBN13()
    {
        //Arrange
        var audioBook = _audioBooks.First();
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).Returns(new List<AudioBook> { audioBook });

        // Act
        var result = await _audioBookService.GetByAudioBookISBN13(audioBook.ISBN13);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for getting audiobooks by date of published
    [TestMethod]
    public async Task GetByAudioBookPublished()
    {
        // Arrange
        var startDate = new DateOnly(1966, 06, 05);
        var endDate = new DateOnly(2020, 06, 06);
        var expectedAudioBook = _audioBooks.Where(a => a.Published >= startDate && a.Published <= endDate).ToList();
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).Returns(expectedAudioBook);

        // Act
        var result = await _audioBookService.GetByAudioBookPublished(startDate, endDate);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    //Test for getting audiobook by edition
    [TestMethod]
    public async Task GetByAudioBookEdition()
    {
        //Arrange
        var audioBook = _audioBooks.First();
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).Returns(new List<AudioBook> { audioBook });

        // Act
        var result = await _audioBookService.GetByAudioBookEdition(audioBook.Edition);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    //Test for getting audiobook by genre
    [TestMethod]
    public async Task GetByAudioBookGenre()
    {
        //Arrange
        var audioBook = _audioBooks.First();
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).Returns(new List<AudioBook> { audioBook });

        // Act
        var result = await _audioBookService.GetByAudioBookGenre(audioBook.Genre);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    //Test foe getting audiobook by lenght in seconds
    [TestMethod]
    public async Task GetByAudioBookLenghtInSeconds()
    {
        //Arrange
        var audioBook = _audioBooks.First();
        _audioBookRepository.FindAsync(audioBook.LenghtInSeconds).Returns(audioBook);

        // Act
        var result = await _audioBookService.GetByAudioBookLenghtInSeconds(audioBook.LenghtInSeconds);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for creating Audio book
    [TestMethod]
    public async Task CreateAudioBook()
    {
        // Arrange
        var newAudioBook = new AudioBook
        {
            Name = "Cien años de soledad",
            ISBN10 = "8420471836",
            ISBN13 = "978-8420471839",
            Published = new DateOnly(1967, 06, 05),
            Edition = "RAE Obra Académica",
            Genre = "Ficcion",
            LenghtInSeconds = 1,
            NarratorId = 1

        };
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).Returns(new List<AudioBook>());
        _audioBookRepository.AddAsync(newAudioBook).Returns(Task.CompletedTask);

        // Act
        var result = await _audioBookService.CreateAudioBook(newAudioBook, audioFile: null);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for updating Audio book
    [TestMethod]
    public async Task UpdateAudioBook()
    {
        // Arrange
        var audioBookToUpdate = _audioBooks.First();
        _audioBookRepository.FindAsync(audioBookToUpdate.Id).Returns(audioBookToUpdate);
        var updateAudioBook = new AudioBook
        {
            Name = "Cien años de soledad",
            ISBN10 = "8420471836",
            ISBN13 = "978-8420471839",
            Published = new DateOnly(1967, 06, 05),
            Edition = "RAE Obra Académica",
            Genre = "Ficcion",
            LenghtInSeconds = 1,
            NarratorId = 1

        };
        _audioBookRepository.FindAsync(updateAudioBook.Id).Returns(updateAudioBook);
        _audioBookRepository.Update(updateAudioBook).Returns(Task.CompletedTask);

        // Act
        var result = await _audioBookService.UpdateAudioBook(updateAudioBook);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 200);
    }
    // Test for deleting Audio book
    [TestMethod]
    public async Task DeleteAudioBook()
    {
        // Arrange
        var AudioBookToDelete = _audioBooks.First();
        _audioBookRepository.FindAsync(AudioBookToDelete.Id).Returns(AudioBookToDelete);
        _audioBookRepository.Delete(AudioBookToDelete).Returns(Task.CompletedTask);

        // Act
        var result = await _audioBookService.DeleteAudioBook(AudioBookToDelete.Id);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for find by narrator
    [TestMethod]
    public async Task GetAudioBookByNarrator()
    {
        // Arrange
        var audioBook = _audioBooks.First();
        _audioBookRepository.FindAsync(audioBook.NarratorId).Returns(audioBook);

        // Act
        var result = await _audioBookService.GetAudioBookByNarrator(audioBook.NarratorId);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for find by narrator name
    [TestMethod]
    public async Task GetAudioBookByNarratorName()
    {
        // Arrange
        var narratorName = "Narrator1";
        var audioBook = _audioBooks.First();
        audioBook.Narrator = new Narrator { Name = narratorName };
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).ReturnsForAnyArgs(new List<AudioBook> { audioBook });

        // Act
        var result = await _audioBookService.GetAudioBookByNarratorName(narratorName);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for find by narrator name
    [TestMethod]
    public async Task GetAudioBookByNarratorLastName()
    {
        // Arrange
        var narratorlastName = "Narrator1";
        var audioBook = _audioBooks.First();
        audioBook.Narrator = new Narrator { LastName = narratorlastName };
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).ReturnsForAnyArgs(new List<AudioBook> { audioBook });

        // Act
        var result = await _audioBookService.GetAudioBookByNarratorLastName(narratorlastName);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
    // Test for find by narrator full name
    [TestMethod]
    public async Task GetAudioBookByNarratorFullName()
    {
        // Arrange
        var narratorlastName = "Narrator1";
        var narratorName = "Narrator2";
        var audioBook = _audioBooks.First();
        audioBook.Narrator = new Narrator { LastName = narratorlastName, Name = narratorName };
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).ReturnsForAnyArgs(new List<AudioBook> { audioBook });

        // Act
        var result = await _audioBookService.GetAudioBookByNarratorFullName(narratorName, narratorlastName);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());

    }
    // Test for find by narrator Genre
    [TestMethod]
    public async Task GetAudioBookByNarratorGenre()
    {
        // Arrange
        var narratorGenre = "Genre1";
        var audioBook = _audioBooks.First();
        audioBook.Narrator = new Narrator { Genre = narratorGenre };
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).ReturnsForAnyArgs(new List<AudioBook> { audioBook });

        // Act
        var result = await _audioBookService.GetAudioBookByNarratorGenre(narratorGenre);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }
}