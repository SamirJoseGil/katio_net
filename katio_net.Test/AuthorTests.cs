using NSubstitute;

using katio.Data;
using katio.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using katio.Business.Interfaces;
using katio.Business.Services;

namespace katio.Test;


[TestClass]
public class AuthorTests
{
    private readonly IRepository<int, Author> _authorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorService _authorService;


    public AuthorTests()
    {
        _authorRepository = Substitute.For<IRepository<int, Author>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _authorService = new AuthorService(_unitOfWork);
    }

    [TestMethod]
    public async Task GetAllAuthors() 
    {
    }
}
