using katio.Business.Interfaces;

using katio.Business.Services;

using katio.Data.Dto;

using katio.Data.Models;

using katio.Data;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

using System.Linq.Expressions;

using System.Net;



namespace katio.Test;




[TestClass]

public class GenreTests

{

    private readonly IRepository<int, Genre> _genreRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IGenreService _genreService;

    private List<Genre> _genres;



    public GenreTests()

    {

        _genreRepository = Substitute.For<IRepository<int, Genre>>();

        _unitOfWork = Substitute.For<IUnitOfWork>();

        _unitOfWork.GenreRepository.Returns(_genreRepository);

        _genreService = new GenreService(_unitOfWork);



        _genres = new List<Genre>()

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

    #region Simple methods test

    // Test for getting all genres

    [TestMethod]

    public async Task GetAllGenres()

    {

        // Arrange

        _genreRepository.GetAllAsync().Returns(_genres);



        // Act

        var result = await _genreService.Index();



        // Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(2, result.ResponseElements.Count());

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        Assert.AreEqual(BaseMessageStatus.OK_200, result.Message);

    }



    // Test for getting genre by id

    [TestMethod]

    public async Task GetGenreById()

    {

        // Arrange

        var genre = _genres.First();

        _genreRepository.FindAsync(genre.Id).Returns(genre);



        // Act

        var result = await _genreService.GetByGenreId(genre.Id);



        // Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(genre.Name, result.ResponseElements.First().Name);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        Assert.AreEqual(BaseMessageStatus.OK_200, result.Message);

    }



    // Test for getting genre by name

    [TestMethod]

    public async Task GetGenreByName()

    {

        // Arrange

        var genre = _genres.First();

        _genreRepository.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>()).Returns(new List<Genre> { genre });



        // Act

        var result = await _genreService.GetGenresByName(genre.Name);



        // Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(genre.Name, result.ResponseElements.First().Name);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        Assert.AreEqual(200, (int)result.StatusCode);

    }



    //Test to get genre by description

    [TestMethod]

    public async Task GetGenresByDescription()

    {

        // Arrange

        var genre = _genres.First();

        _genreRepository.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>()).Returns(new List<Genre> { genre });



        // Act

        var result = await _genreService.GetGenresByDescription(genre.Description);



        // Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(genre.Description, result.ResponseElements.First().Description);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        Assert.AreEqual(BaseMessageStatus.OK_200, result.Message);



    }

    //Test for create genre

    [TestMethod]

    public async Task CreateGenre()

    {

        // Arrange

        var newGenre = new Genre

        {

            Name = "Fantasy",

            Description = "La Fantasia es..."

        };

        _genreRepository.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>()).Returns(new List<Genre>());

        _genreRepository.AddAsync(newGenre).Returns(Task.CompletedTask);



        // Act

        var result = await _genreService.CreateGenre(newGenre);



        //Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        Assert.AreEqual(BaseMessageStatus.OK_200, result.Message);

        Assert.AreEqual(newGenre.Name, result.ResponseElements.First().Name);

        Assert.AreEqual(newGenre.Description, result.ResponseElements.First().Description);

    }

    //Test for update genre

    [TestMethod]

    public async Task UpdateGenre()

    {



        // Arrange

        var GenreToUpdate = _genres.First();

        _genreRepository.FindAsync(GenreToUpdate.Id).Returns(GenreToUpdate);



        var updatedGenre = new Genre

        {

            Name = "Fantasy",

            Description = "La Fantasia es..."

        };

        _genreRepository.FindAsync(updatedGenre.Id).Returns(updatedGenre);

        _genreRepository.Update(updatedGenre).Returns(Task.CompletedTask);



        // Act

        var result = await _genreService.UpdateGenre(updatedGenre);



        //Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        Assert.AreEqual(BaseMessageStatus.OK_200, result.Message);

        Assert.AreEqual(updatedGenre.Name, result.ResponseElements.First().Name);

        Assert.AreEqual(updatedGenre.Description, result.ResponseElements.First().Description);

    }



    //Test for delete genre

    [TestMethod]

    public async Task DeleteGenre()

    {

        // Arrange

        var deleteGenre = _genres.First();

        _genreRepository.FindAsync(deleteGenre.Id).Returns(deleteGenre);

        _genreRepository.Delete(deleteGenre).Returns(Task.CompletedTask);



        // Act

        var result = await _genreService.DeleteGenre(deleteGenre.Id);



        //Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        Assert.AreEqual(BaseMessageStatus.OK_200, result.Message);

        await _genreRepository.Received(1).Delete(deleteGenre);




    }




    #endregion

    #region Repository Exceptions



    // Repository Exeptions

    // Test for getting all genres

    [TestMethod]

    public async Task GetAllGenresRepositoryException()

    {

        // Arrange

        _genreRepository.When(x => x.GetAllAsync()).Do(x => throw new Exception("Repository error"));



        // Act

        var result = await _genreService.Index();



        // Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);

        Assert.AreEqual("500 Internal Server Error | Repository error", result.Message);

    }



    // Test for getting genre by id with repository exceptions

    [TestMethod]

    public async Task GetGenreByIdRepositoryException()

    {

        // Arrange

        var genre = _genres.First();

        _genreRepository.When(x => x.FindAsync(genre.Id)).Do(x => throw new Exception("Repository error"));



        // Act

        var result = await _genreService.GetByGenreId(genre.Id);



        // Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);

        Assert.AreEqual("500 Internal Server Error | Repository error", result.Message);

    }



    // Test for getting genre by name with repository exceptions

    [TestMethod]

    public async Task GetGenreByNameRepositoryException()

    {

        // Arrange

        var genre = _genres.First();

        _genreRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>())).Do(x => throw new Exception("Repository error"));



        // Act

        var result = await _genreService.GetGenresByName(genre.Name);



        // Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);

        Assert.AreEqual("500 Internal Server Error | Repository error", result.Message);

    }



    // Test for getting genre by Description with repository exceptions

    [TestMethod]

    public async Task GetGenreByDescriptionRepositoryException()

    {

        // Arrange

        var genre = _genres.First();

        _genreRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>())).Do(x => throw new Exception("Repository error"));



        // Act

        var result = await _genreService.GetGenresByDescription(genre.Description);



        // Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);

        Assert.AreEqual("500 Internal Server Error | Repository error", result.Message);

    }



    #endregion



    #region Test Methods Fail

    // Test for getting all genres Fail

    [TestMethod]

    public async Task GetAllGenresFail()

    {

        // Arrange

        _genreRepository.GetAllAsync().Returns(new List<Genre>());



        // Act

        var result = await _genreService.Index();



        // Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);

        Assert.AreEqual(BaseMessageStatus.GENRE_NOT_FOUND, result.Message);

        Assert.AreEqual(0, result.ResponseElements.Count());

    }



    // Test for creating genre Fail

    [TestMethod]

    public async Task CreateGenreFail()

    {

        // Arrange

        var newGenre = new Genre { Name = "Fantasy", Description = "La Fantasia es..." };

        var existingGenre = new Genre { Name = "Fantasy", Description = "La Fantasia es..." };

        _genreRepository.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>())

          .Returns(new List<Genre> { existingGenre });



        // Act

        var result = await _genreService.CreateGenre(newGenre);



        // Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(HttpStatusCode.Conflict, result.StatusCode);

        Assert.AreEqual($"{BaseMessageStatus.BAD_REQUEST_400} | El género ya está registrado en el sistema.", result.Message);

        Assert.IsTrue(result.Message.Contains(BaseMessageStatus.BAD_REQUEST_400));

    }



    //Test for Update genre Fail

    [TestMethod]

    public async Task UpdateGenreFail()

    {

        // Arrange

        var genreToUpdate = _genres.First();

        _genreRepository.FindAsync(genreToUpdate.Id).Returns(genreToUpdate);



        var updatedAuthor = new Genre { Name = "Hola", Description = "Super" };

        _genreRepository.FindAsync(genreToUpdate.Id).Returns(Task.FromResult<Genre?>(null));



        // Act

        var result = await _genreService.UpdateGenre(genreToUpdate);



        // Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);

        Assert.AreEqual(BaseMessageStatus.GENRE_NOT_FOUND, result.Message);

    }

    // Test for deleting genre Fail

    [TestMethod]

    public async Task DeleteGenreFail()

    {

        int genreId = 1;

        // Arrange

        _genreRepository.FindAsync(genreId).Returns((Genre)null);



        // Act

        var result = await _genreService.DeleteGenre(genreId);



        // Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);

        Assert.AreEqual(BaseMessageStatus.GENRE_NOT_FOUND, result.Message);

        Assert.AreEqual(0, result.ResponseElements.Count());

    }



    // get genre by id Fail

    [TestMethod]

    public async Task GetByGenreIdFail()

    {

        // Arrange

        var genre = _genres.First();

        _genreRepository.FindAsync(genre.Id).Returns(Task.FromResult<Genre?>(null));



        // Act

        var result = await _genreService.GetByGenreId(genre.Id);



        // Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);

        Assert.AreEqual(BaseMessageStatus.GENRE_NOT_FOUND, result.Message);

    }



    //get by genre name Fail

    [TestMethod]

    public async Task GetGenresByNameFail()

    {

        // Arrange

        var genre = _genres.First();

        _genreRepository.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>()).Returns(new List<Genre>());



        // Act

        var result = await _genreService.GetGenresByName(genre.Name);



        // Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);

        Assert.AreEqual(BaseMessageStatus.GENRE_NOT_FOUND, result.Message);

    }



    //get by genre description Fail

    [TestMethod]

    public async Task GetGenresByDescriptionFail()

    {

        // Arrange

        var genre = _genres.First();

        _genreRepository.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>()).Returns(new List<Genre>());



        // Act

        var result = await _genreService.GetGenresByDescription(genre.Description);



        // Assert

        Assert.IsNotNull(result);

        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);

        Assert.AreEqual(BaseMessageStatus.GENRE_NOT_FOUND, result.Message);



    }



    #endregion

}