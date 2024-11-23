using NSubstitute;
using katio.Data;
using katio.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using katio.Business.Interfaces;
using katio.Business.Services;
using System.Linq.Expressions;
using NSubstitute.ExceptionExtensions;

namespace katio.Test.AudioBookTests;

[TestClass]

public class AudioBookTestsException
{
    private readonly IRepository<int, AudioBook> _audioBookRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAudioBookService _audioBookService;
    private List<AudioBook> _audioBooks;

    public AudioBookTestsException()
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
        NarratorId = 1
      }, new AudioBook {
        Id = 2,
        Name = "Huellas",
        ISBN10 = "9584277278",
        ISBN13 = "978-958427275",
        Published = new DateOnly(2019, 01, 01),
        Edition = "1ra Edicion",
        Genre = "Ficcion",
        LenghtInSeconds = 1,
        NarratorId = 3
      }};
    }
    // Test for getting all audio books with repository exceptions
    [TestMethod]
    public async Task GetAllAudioBooksRepositoryException()
    {
        // Arrange
        _audioBookRepository.When(x => x.GetAllAsync()).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _audioBookService.Index();

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting audioBook omniscient with repository exceptions
    [TestMethod]
    public async Task SearchAudioBookAsyncRepositoryException()
    {
        // Arrange
        var searchTerm = "AudioBook";
        _audioBookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _audioBookService.SearchAudioBookAsync(searchTerm);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting audioBook by id with repository exceptions
    [TestMethod]
    public async Task GetAudioBookByIdRepositoryException()
    {
        // Arrange
        var audioBook = _audioBooks.First();
        _audioBookRepository.When(x => x.FindAsync(audioBook.Id)).Do(x => throw new Exception());

        // Act
        var result = await _audioBookService.GetAudioBookById(audioBook.Id);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting audioBook by name with repository exceptions
    [TestMethod]
    public async Task GetAudioBookByNameRepositoryException()
    {
        // Arrange
        var book = _audioBooks.First();
        _audioBookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _audioBookService.GetByAudioBookName(Arg.Any<string>());

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting audioBook by ISBN10 with repository exceptions
    [TestMethod]
    public async Task GetByAudioBookISBN10RepositoryException()
    {
        // Arrange
        var book = _audioBooks.First();
        _audioBookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _audioBookService.GetByAudioBookISBN10(Arg.Any<string>());

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting audioBook by ISBN13 with repository exceptions
    [TestMethod]
    public async Task GetByAudioBookISBN13RepositoryException()
    {
        // Arrange
        var book = _audioBooks.First();
        _audioBookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _audioBookService.GetByAudioBookISBN13(Arg.Any<string>());

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting audioBook by published with repository exceptions
    [TestMethod]
    public async Task GetByAudioBookPublishedRepositoryException()
    {
        // Arrange
        var startDate = new DateOnly(1960, 01, 01);
        var endDate = new DateOnly(1970, 12, 31);
        _audioBookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())).Do(x => throw new Exception("Repository error"));


        // Act
        var result = await _audioBookService.GetByAudioBookPublished(startDate, endDate);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting audioBook by Edition with repository exceptions
    [TestMethod]
    public async Task GetByAudioBookEditionRepositoryException()

    {
        // Arrange
        var book = _audioBooks.First();
        _audioBookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _audioBookService.GetByAudioBookEdition(Arg.Any<string>());

        // Asssert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting audioBook by Genre with repository exceptions
    [TestMethod]
    public async Task GetByAudioBookGenreRepositoryException()
    {
        // Arrange
        var book = _audioBooks.First();
        _audioBookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _audioBookService.GetByAudioBookGenre(Arg.Any<string>());

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for getting audioBook by LenghtInSeconds with repository exceptions
    [TestMethod]
    public async Task GetByAudioBookLenghtInSecondsRepositoryException()
    {
        // Arrange
        var audioBook = _audioBooks.First();
        _audioBookRepository.When(x => x.FindAsync(audioBook.LenghtInSeconds)).Do(x => throw new Exception());

        // Act
        var result = await _audioBookService.GetAudioBookById(audioBook.Id);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for find by narrator name exeption
    [TestMethod]
    public async Task GetAudioBookByNarratorName_Exeption()
    {
        // Arrange
        var book = _audioBooks.First();
        _audioBookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>(), includeProperties: "Narrator")).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _audioBookService.GetByAudioBookName(Arg.Any<string>());

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for find ba narrator id exeption
    [TestMethod]
    public async Task GetAudioBookByNarratorId_Exeption()
    {
        // Arrange
        var audioBook = _audioBooks.First();
        _audioBookRepository.When(x => x.FindAsync(audioBook.LenghtInSeconds)).Do(x => throw new Exception());

        // Act
        var result = await _audioBookService.GetAudioBookByNarrator(audioBook.NarratorId);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for find by narrator last name exeption
    [TestMethod]
    public async Task GetAudioBookByNarratorLastName_Exeption()
    {
        // Arrange
        var book = _audioBooks.First();
        _audioBookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>(), includeProperties: "Narrator")).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _audioBookService.GetAudioBookByNarratorLastName(Arg.Any<string>());

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for find by narrator full name exeption
    [TestMethod]
    public async Task GetAudioBookByNarratorFullNameExeption()
    {
        // Arrange
        var book = _audioBooks.First();
        _audioBookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>(), includeProperties: "Narrator")).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _audioBookService.GetAudioBookByNarratorFullName(Arg.Any<string>(), Arg.Any<string>());

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for find by narrator genre exeption
    [TestMethod]
    public async Task GetAudioBookByNarratorGenreExeption()
    {
        // Arrange
        var book = _audioBooks.First();
        _audioBookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>(), includeProperties: "Narrator")).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _audioBookService.GetAudioBookByNarratorGenre(Arg.Any<string>());

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for creating audio book with repository exceptions
    [TestMethod]
    public async Task CreateAudioBookRepositoryException()
    {
        // Arrange
        var newAudioBook = new AudioBook
        {
            Id = 1,
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
        _audioBookRepository.When(x => x.AddAsync(Arg.Any<AudioBook>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _audioBookService.CreateAudioBook(newAudioBook, audioFile: null);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for updating audio book with repository exceptions
    [TestMethod]
    public async Task UpdateAudioBookRepositoryException()
    {
        // Arrange
        var audioBookToUpdate = _audioBooks.First();
        _audioBookRepository.FindAsync(audioBookToUpdate.Id).Returns(audioBookToUpdate);
        var updatedAudioBook = new AudioBook
        {
            Id = 1,
            Name = "Cien años de soledad",
            ISBN10 = "8420471836",
            ISBN13 = "978-8420471839",
            Published = new DateOnly(1967, 06, 05),
            Edition = "RAE Obra Académica",
            Genre = "Ficcion",
            LenghtInSeconds = 1,
            NarratorId = 1
        };
        _audioBookRepository.FindAsync(audioBookToUpdate.Id).Returns(audioBookToUpdate);
        _audioBookRepository.When(x => x.Update(Arg.Any<AudioBook>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _audioBookService.UpdateAudioBook(updatedAudioBook);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
    // Test for deleting audio book with repository exceptions
    [TestMethod]
    public async Task DeleteAuthorRepositoryException()
    {
        // Arrange
        var audioBookToDelete = _audioBooks.First();
        _audioBookRepository.FindAsync(audioBookToDelete.Id).Returns(audioBookToDelete);
        _audioBookRepository.When(x => x.Delete(Arg.Any<AudioBook>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _audioBookService.DeleteAudioBook(audioBookToDelete.Id);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }
}