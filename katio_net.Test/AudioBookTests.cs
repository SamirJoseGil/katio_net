using NSubstitute;

using katio.Data;
using katio.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using katio.Business.Interfaces;
using katio.Business.Services;

namespace katio.Test;

[TestClass]
public class AudioBookTests
{
    private readonly IRepository<int, AudioBook> _audioBookRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAudioBookService _audioBookService;

    public AudioBookTests()
    {
        _audioBookRepository = Substitute.For<IRepository<int, AudioBook>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _audioBookService = new AudioBookService(_unitOfWork);
    }

    [TestMethod]
    public async Task GetAllAudioBooks() 
    {
    }
}
