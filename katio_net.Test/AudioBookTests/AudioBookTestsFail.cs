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

public class AudioBookTestsFail
{
    private readonly IRepository<int, AudioBook> _audioBookRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAudioBookService _audioBookService;
    private List<AudioBook> _audioBooks;

    public AudioBookTestsFail()
    {
        _audioBookRepository = Substitute.For<IRepository<int, AudioBook>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.AudioBookRepository.Returns(_audioBookRepository);
        _audioBookService = new AudioBookService(_unitOfWork);
        _audioBooks = new List<AudioBook>()
    {
        new AudioBook {
        Id = 1,
        Name = "Cien años de soledad",
        ISBN10 = "8420471836",
        ISBN13 = "978-8420471839",
        Published = new DateOnly(1967, 06, 05),
        Edition = "RAE Obra Académica",
        Genre = "Ficcion",
        LenghtInSeconds = 1,
        Path = "C:/Users/Usuario/Downloads/Cien a�os de soledad.mp3",
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
        Path = "C:/Users/Usuario/Downloads/Huellas.mp3",
        NarratorId = 3
      }};
    }
    // Test for creating AudioBook Fail
    [TestMethod]
    public async Task CreateAudioBookFail_NotFound()
    {
        // Arrange
        var existingAudioBook = new AudioBook
        {
            Id = 1,
            Name = "Cien años de soledad",
            ISBN10 = "8420471836",
            ISBN13 = "978-8420471839",
            Published = new DateOnly(1967, 06, 05),
            Edition = "RAE Obra Académica",
            Genre = "Ficcion",
            LenghtInSeconds = 1,
            Path = "C:/Users/Usuario/Downloads/Cien a�os de soledad.mp3",
            NarratorId = 1

        };
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).ReturnsForAnyArgs(new List<AudioBook> { existingAudioBook });

        // Act
        var result = await _audioBookService.CreateAudioBook(existingAudioBook);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for updating audio book Fail
    [TestMethod]
    public async Task UpdateAuthor_NotFound()
    {
        // Arrange
        _audioBookRepository.Update(Arg.Any<AudioBook>()).Throws(new Exception());
        _unitOfWork.AudioBookRepository.Returns(_audioBookRepository);

        // Act
        var result = await _audioBookService.UpdateAudioBook(new AudioBook());

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for deleting audio book Fail
    [TestMethod]
    public async Task DeleteAuthor_NotFound()
    {
        // Arrange
        var authorToDelete = _audioBooks.First();
        _audioBookRepository.FindAsync(authorToDelete.Id).ReturnsForAnyArgs(Task.FromResult<AudioBook>(null));

        // Act
        var result = await _audioBookService.DeleteAudioBook(authorToDelete.Id);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for find by audiobook  Fail
    [TestMethod]
    public async Task GetAudioBook_NotFound()
    {
        // Arrange
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())
        .ReturnsForAnyArgs(new List<AudioBook>());

        // Act
        var result = await _audioBookService.Index();

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for search audiobook Omniscient fail
    [TestMethod]
    public async Task SearchAudioBookAsync_NotFound()
    {
        // Arrange
        _audioBookRepository.GetAllAsync().Returns(new List<AudioBook>());

        // Act
        var result = await _audioBookService.SearchAudioBookAsync(searchTerm: "omniscient");

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for getting audiobook by id Fail
    [TestMethod]
    public async Task GetAudioBookById_NotFound()
    {
        // Arrange
        var audioBook = _audioBooks.First();
        _audioBookRepository.FindAsync(audioBook.Id).ReturnsForAnyArgs(Task.FromResult<AudioBook>(null));

        // Act
        var result = await _audioBookService.GetAudioBookById(audioBook.Id);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for find by audiobook name Fail
    [TestMethod]
    public async Task GetAudioBookName_NotFound()
    {
        var audioBookName = "audiobook";
        var audioBook = _audioBooks.First();

        audioBook = new AudioBook { Name = audioBookName };
        // Arrange
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())
        .ReturnsForAnyArgs(new List<AudioBook>());

        // Act
        var result = await _audioBookService.GetByAudioBookName(audioBookName);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());

    }
    // Test for find by audiobook ISB10 Fail
    [TestMethod]
    public async Task GetByAudioBookISBN10_NotFound()
    {
        var audioBookISBN10 = "Audiobook1";
        var audioBook = _audioBooks.First();

        audioBook = new AudioBook { ISBN10 = audioBookISBN10 };
        // Arrange
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())
        .ReturnsForAnyArgs(new List<AudioBook>());

        // Act
        var result = await _audioBookService.GetByAudioBookISBN10(audioBookISBN10);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());

    }
    // Test for find by audiobook ISB13 Fail
    [TestMethod]
    public async Task GetByAudioBookISBN13_NotFound()
    {
        var audioBookISBN13 = "Audiobook1";
        var audioBook = _audioBooks.First();

        audioBook = new AudioBook { ISBN13 = audioBookISBN13 };
        // Arrange
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())
        .ReturnsForAnyArgs(new List<AudioBook>());

        // Act
        var result = await _audioBookService.GetByAudioBookISBN13(audioBookISBN13);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());

    }
    // Test for find by audiobook published Fail
    [TestMethod]
    public async Task GetByAudioBookPublished_NotFound()
    {
        var start = new DateOnly(2004, 4, 05);
        var endDate = new DateOnly(2005, 4, 05);
        // Arrange
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())
        .ReturnsForAnyArgs(new List<AudioBook>());

        // Act
        var result = await _audioBookService.GetByAudioBookPublished(start, endDate);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());

    }
    // Test for find by audiobook Edition Fail
    [TestMethod]
    public async Task GetByAudioBookEdition_NotFound()
    {
        var audioBookEdition = "Edition";
        var audioBook = _audioBooks.First();

        audioBook = new AudioBook { Edition = audioBookEdition };
        // Arrange
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())
        .ReturnsForAnyArgs(new List<AudioBook>());

        // Act
        var result = await _audioBookService.GetByAudioBookEdition(audioBookEdition);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());

    }
    // Test for find by audiobook Genre Fail
    [TestMethod]
    public async Task GetByAudioBookGenre_NotFound()
    {
        var audioBookGenre = "Genre";
        var audioBook = _audioBooks.First();

        audioBook = new AudioBook { Genre = audioBookGenre };
        // Arrange
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())
        .ReturnsForAnyArgs(new List<AudioBook>());

        // Act
        var result = await _audioBookService.GetByAudioBookGenre(audioBookGenre);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());

    }
    // Test for find by audiobook Length in seconds Fail
    [TestMethod]
    public async Task GetByAudioBookLenghtInSeconds_NotFound()
    {
        var audioBookLength = 0;
        var audioBook = _audioBooks.First();

        audioBook = new AudioBook { LenghtInSeconds = audioBookLength };
        // Arrange
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())
        .ReturnsForAnyArgs(new List<AudioBook>());

        // Act
        var result = await _audioBookService.GetByAudioBookLenghtInSeconds(audioBookLength);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());

    }
    // Test for find by narrator  Fail
    [TestMethod]
    public async Task GetAudioBookByNarrator_NotFound()
    {
        var narratorId = 0;
        var audioBook = _audioBooks.First();

        audioBook = new AudioBook { NarratorId = narratorId };
        // Arrange
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())
        .ReturnsForAnyArgs(new List<AudioBook>());

        // Act
        var result = await _audioBookService.GetAudioBookByNarrator(narratorId);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());

    }
    // Test for find by narrator name Fail
    [TestMethod]
    public async Task GetAudioBookByNarratorName_NotFound()
    {
        // Arrange
        var narratorName = "Narrator1";
        var audioBook = _audioBooks.First();

        audioBook.Narrator = new Narrator { Name = narratorName };

        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())
        .ReturnsForAnyArgs(new List<AudioBook>());

        // Act
        var result = await _audioBookService.GetAudioBookByNarratorName(narratorName);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());

    }
    // Test for find by narrator Last Name Fail
    [TestMethod]
    public async Task GetAudioBookByNarratorLastName_NotFound()
    {
        // Arrange
        var narratorLastName = "Narrator1";
        var audioBook = _audioBooks.First();

        audioBook.Narrator = new Narrator { LastName = narratorLastName };

        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())
        .ReturnsForAnyArgs(new List<AudioBook>());

        // Act
        var result = await _audioBookService.GetAudioBookByNarratorLastName(narratorLastName);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());

    }
    // Test for find by narrator Full name Fail
    [TestMethod]
    public async Task GetAudioBookByNarratorFullName_NotFound()
    {
        // Arrange
        var narratorName = "name";
        var narratorLastName = "Narrator1";
        var audioBook = _audioBooks.First();

        audioBook.Narrator = new Narrator { Name = narratorName, LastName = narratorLastName };

        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())
        .ReturnsForAnyArgs(new List<AudioBook>());

        // Act
        var result = await _audioBookService.GetAudioBookByNarratorFullName(narratorName, narratorLastName);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());

    }
    // Test for find by narrator genre Fail
    [TestMethod]
    public async Task GetAudioBookByNarratorGenre_NotFound()
    {
        // Arrange
        var narratorGenre = "Genre1";
        var audioBook = _audioBooks.First();

        audioBook.Narrator = new Narrator { Genre = narratorGenre };

        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())
        .ReturnsForAnyArgs(new List<AudioBook>());

        // Act
        var result = await _audioBookService.GetAudioBookByNarratorGenre(narratorGenre);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
}