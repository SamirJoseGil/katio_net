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

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Payloads;



namespace katio.Test;



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

    {

      new AudioBook

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

      },

      new AudioBook

      {

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

      }

    };

    }


    #region Test Methods Simple

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

            Path = "C:/Users/Usuario/Downloads/Cien a�os de soledad.mp3",

            NarratorId = 1

        };

        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).Returns(new List<AudioBook>());

        _audioBookRepository.AddAsync(newAudioBook).Returns(Task.CompletedTask);



        // Act

        var result = await _audioBookService.CreateAudioBook(newAudioBook);



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

            Path = "C:/Users/Usuario/Downloads/Cien a�os de soledad.mp3",

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



    #endregion

    #region Test Narrator in audiobook

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
        var result = await _audioBookService.GetAudioBookByNarratorFullName(narratorName,narratorlastName);

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
    #endregion

    #region Test Fail(NotFound)


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
        _audioBookRepository.Update(Arg.Any<AudioBook>()).ThrowsAsyncForAnyArgs(new Exception());
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
        var start = new DateOnly (2004, 4, 05); 
        var endDate = new DateOnly (2005, 4, 05);
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
 

    #endregion

    #region Repository Exeptions

    // Repository Exeptions

    // Test for getting all audio books with repository exceptions

    [TestMethod]

    public async Task GetAllAudioBooksRepositoryException()

    {

        // Arrange

        _audioBookRepository

      .GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())

      .ThrowsForAnyArgs(new Exception());



        // Act

        var result = await _audioBookService.Index();



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

        _audioBookRepository

      .GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())

      .ThrowsForAnyArgs(new Exception());

        var result = await _audioBookService.GetByAudioBookName(Arg.Any<string>());



        Assert.AreEqual((int)result.StatusCode, 500);

    }



    // Test for getting audioBook by ISBN10 with repository exceptions

    [TestMethod]

    public async Task GetByAudioBookISBN10RepositoryException()

    {

        // Arrange

        _audioBookRepository

      .GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())

      .ThrowsForAnyArgs(new Exception());

        var result = await _audioBookService.GetByAudioBookISBN10(Arg.Any<string>());



        Assert.AreEqual((int)result.StatusCode, 500);

    }



    // Test for getting audioBook by ISBN13 with repository exceptions

    [TestMethod]

    public async Task GetByAudioBookISBN13RepositoryException()

    {

        // Arrange

        _audioBookRepository

      .GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())

      .ThrowsForAnyArgs(new Exception());

        var result = await _audioBookService.GetByAudioBookISBN13(Arg.Any<string>());



        Assert.AreEqual((int)result.StatusCode, 500);

    }



    // Test for getting audioBook by published with repository exceptions

    [TestMethod]

    public async Task GetByAudioBookPublishedRepositoryException()

    {

        // Arrange

        _audioBookRepository

      .GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())

      .ThrowsForAnyArgs(new Exception());

        // var result = await _audioBookService.GetByAudioBookPublished(Arg.Any<DateOnly>());



        //Assert.AreEqual((int)result.StatusCode, 500);

    }



    // Test for getting audioBook by Edition with repository exceptions

    [TestMethod]

    public async Task GetByAudioBookEditionRepositoryException()

    {

        // Arrange

        _audioBookRepository

      .GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())

      .ThrowsForAnyArgs(new Exception());

        var result = await _audioBookService.GetByAudioBookEdition(Arg.Any<string>());



        Assert.AreEqual((int)result.StatusCode, 500);

    }



    // Test for getting audioBook by Genre with repository exceptions

    [TestMethod]

    public async Task GetByAudioBookGenreRepositoryException()

    {

        // Arrange

        _audioBookRepository

      .GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())

      .ThrowsForAnyArgs(new Exception());

        var result = await _audioBookService.GetByAudioBookGenre(Arg.Any<string>());



        Assert.AreEqual((int)result.StatusCode, 500);

    }



    // Test for getting audioBook by LenghtInSeconds with repository exceptions

    [TestMethod]

    public async Task GetByAudioBookLenghtInSecondsRepositoryException()

    {

        // Arrange

        var audioBook = _audioBooks.First();

        _audioBookRepository.When(x => x.FindAsync(audioBook.LenghtInSeconds)).Do(x => throw new Exception());

        var result = await _audioBookService.GetAudioBookById(audioBook.Id);



        Assert.AreEqual((int)result.StatusCode, 500);

    }

    // Test for find by narrator name exeption
    [TestMethod]

    public async Task GetAudioBookByNarratorName_Exeption()

    {

        // Arrange

        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).ThrowsForAnyArgs(new Exception());

        var result = await _audioBookService.GetByAudioBookName(Arg.Any<string>());


        Assert.AreEqual((int)result.StatusCode, 500);

    }

    // Test for find ba narrator id exeption
    [TestMethod]
    public async Task GetAudioBookByNarratorId_Exeption()
    {
        
        // Arrange

        var audioBook = _audioBooks.First();

        _audioBookRepository.When(x => x.FindAsync(audioBook.NarratorId)).Do(x => throw new Exception());

        var result = await _audioBookService.GetAudioBookByNarrator(audioBook.NarratorId);



        Assert.AreEqual((int)result.StatusCode, 500);
    }

    // Test for find by narrator last name exeption
    [TestMethod]

    public async Task GetAudioBookByNarratorLastName_Exeption()

    {

        // Arrange

        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).ThrowsForAnyArgs(new Exception());

        var result = await _audioBookService.GetAudioBookByNarratorLastName(Arg.Any<string>());


        Assert.AreEqual((int)result.StatusCode, 500);


    }

    // Test for find by narrator full name exeption
    [TestMethod]

    public async Task GetAudioBookByNarratorFullNameExeption()

    {

        // Arrange

        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).ThrowsForAnyArgs(new Exception());

        var result = await _audioBookService.GetAudioBookByNarratorFullName(Arg.Any<string>(),Arg.Any<string>());

        Assert.AreEqual((int)result.StatusCode, 500);


    }

    // Test for find by narrator genre exeption
    [TestMethod]

    public async Task GetAudioBookByNarratorGenreExeption()

    {
        // Arrange

        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).ThrowsForAnyArgs(new Exception());

        var result = await _audioBookService.GetAudioBookByNarratorGenre(Arg.Any<string>());

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
            Path = "C:/Users/Usuario/Downloads/Cien a�os de soledad.mp3",
            NarratorId = 1
        };
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).Returns(new List<AudioBook>());
        _audioBookRepository.When(x => x.AddAsync(Arg.Any<AudioBook>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _audioBookService.CreateAudioBook(newAudioBook);

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
            Path = "C:/Users/Usuario/Downloads/Cien a�os de soledad.mp3",
            NarratorId = 1
        };
        _audioBookRepository.FindAsync(audioBookToUpdate.Id).Returns(audioBookToUpdate);
        _audioBookRepository.When(x => x.Update(Arg.Any<AudioBook>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _audioBookService.UpdateAudioBook(updatedAudioBook);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }

    #endregion

}