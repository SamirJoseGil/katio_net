using katio.Business.Interfaces;
using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace katio.Business.Services;

public class AudioBookService : IAudioBookService
{
    // Lista de libros
    private readonly KatioContext _context;
    private readonly UnitOfWork _unitOfWork;

    // Constructor
    public AudioBookService(KatioContext context)
    {
        _context = context;
    }

    // Traer todos los Audiolibros
    public async Task<BaseMessage<AudioBook>> Index()
    {
        var result = await _context.AudioBooks.ToListAsync();
        return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    #region Create Update Delete
    // Crear un Audiolibro
    public async Task<BaseMessage<AudioBook>> CreateAudioBook(AudioBook audioBook)
    {
        var newAudioBook = new AudioBook()
        {
            Name = audioBook.Name,
            ISBN10 = audioBook.ISBN10,
            ISBN13 = audioBook.ISBN13,
            Published = audioBook.Published,
            Edition = audioBook.Edition,
            Genre = audioBook.Genre,
            LenghtInSeconds = audioBook.LenghtInSeconds,
            Path = audioBook.Path,
            AuthorId = audioBook.AuthorId
        };
        try
        {
            await _context.AudioBooks.AddAsync(newAudioBook);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { newAudioBook });
    }
    
    // Actualizar un Audiolibro
    public async Task<AudioBook> UpdateAudioBook(AudioBook audioBook)
    {
        var result = _context.AudioBooks.FirstOrDefault(b => b.Id == audioBook.Id);
        if (result != null)
        {
            result.Name = audioBook.Name;
            result.ISBN10 = audioBook.ISBN10;
            result.ISBN13 = audioBook.ISBN13;
            result.Published = audioBook.Published;
            result.Edition = audioBook.Edition;
            result.Genre = audioBook.Genre;
            result.LenghtInSeconds = audioBook.LenghtInSeconds;
            result.Path = audioBook.Path;
            result.AuthorId = audioBook.AuthorId;
            await _context.SaveChangesAsync();
        }
        return result;
    }

    // Eliminar un Audiolibro
    public async Task<BaseMessage<AudioBook>> DeleteAudioBook(int Id)
    {
        var result = _context.AudioBooks.FirstOrDefault(b => b.Id == Id);
        if (result != null)
        {
            _context.AudioBooks.Remove(result);
            await _context.SaveChangesAsync();
            return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { result });
        }
        return Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }
    #endregion

    #region Find By AudioBook
    // Buscar por id
    public async Task<BaseMessage<AudioBook>> GetAudioBookById(int id)
    {
        var result = await _unitOfWork.AudioBookRepository.FindAsync(id);
        return result != null ? Utilities.BuildResponse<AudioBook>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { result }) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    } 
    // Buscar por Nombre
    public async Task<BaseMessage<AudioBook>> GetByAudioBookName(string name)
    {
        var result = await _context.AudioBooks.Where(b => b.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase)).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por ISBN10
    public async Task<BaseMessage<AudioBook>> GetByAudioBookISBN10(string ISBN10)
    {
        var result = await _context.AudioBooks.Where(b => b.ISBN10 == ISBN10).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por ISBN13
    public async Task<BaseMessage<AudioBook>> GetByAudioBookISBN13(string ISBN13)
    {
        var result = await _context.AudioBooks.Where(b => b.ISBN13 == ISBN13).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por Rango de Fecha de Publicación
    public async Task<BaseMessage<AudioBook>> GetByAudioBookPublished(DateOnly startDate, DateOnly endDate)
    {
        var result = await _context.AudioBooks.Where(b => b.Published >= startDate && b.Published <= endDate).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por Edición
    public async Task<BaseMessage<AudioBook>> GetByAudioBookEdition(string edition)
    {
        var result = await _context.AudioBooks.Where(b => b.Edition.Contains(edition, StringComparison.InvariantCultureIgnoreCase)).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por Género
    public async Task<BaseMessage<AudioBook>> GetByAudioBookGenre(string genre)
    {
        var result = await _context.AudioBooks.Where(b => b.Genre.Contains(genre, StringComparison.InvariantCultureIgnoreCase)).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por Duración en Segundos
    public async Task<BaseMessage<AudioBook>> GetByAudioBookLenghtInSeconds(int lenghtInSeconds)
    {
        var result = await _context.AudioBooks.Where(b => b.LenghtInSeconds == lenghtInSeconds).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    #endregion

    #region Find By Author

    // Buscar por Autor
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthor(int authorId)
    {
        var result = await _context.AudioBooks.Where(b => b.AuthorId == authorId).Include(a => a.Author).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por Nombre de Autor   
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthorName(string authorName)
    {
        var result = await _context.AudioBooks.Where(b => b.Author.Name.Contains(authorName, StringComparison.InvariantCultureIgnoreCase)).Include(a => a.Author).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por Apellido del Autor
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthorLastName(string authorLastName)
    {
        var result = await _context.AudioBooks.Where(b => b.Author.LastName.Contains(authorLastName, StringComparison.InvariantCultureIgnoreCase)).Include(a => a.Author).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por Nombre y Apellido de Autor
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthorFullName(string authorName, string authorLastName)
    {
        var result = await _context.AudioBooks.Where(b => b.Author.Name.Contains(authorName, StringComparison.InvariantCultureIgnoreCase) && b.Author.LastName.Contains(authorLastName, StringComparison.InvariantCultureIgnoreCase)).Include(a => a.Author).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por País de Autor
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthorCountry(string authorCountry)
    {
        var result = await _context.AudioBooks.Where(b => b.Author.Country.Contains(authorCountry, StringComparison.InvariantCultureIgnoreCase)).Include(a => a.Author).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por Rango de Fecha de Nacimiento de Autor
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthorBirthDateRange(DateOnly startDate, DateOnly endDate)
    {
        var result = await _context.AudioBooks.Where(b => b.Author.BirthDate >= startDate && b.Author.BirthDate <= endDate).Include(a => a.Author).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    #endregion
}
