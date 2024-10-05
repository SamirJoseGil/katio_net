using NSubstitute;

using katio.Data;
using katio.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using katio.Business.Interfaces;
using katio.Business.Services;

namespace katio.Test;


[TestClass]
public class GenreTests
{
    private readonly IRepository<int, Genre> _genreRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenreService _genreService;

    public GenreTests()
    {
        _genreRepository = Substitute.For<IRepository<int, Genre>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _genreService = new GenreService(_unitOfWork);
    }

    [TestMethod]
    public async Task GetAllGenres() 
    {
    }
}
