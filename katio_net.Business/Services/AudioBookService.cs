using katio.Business.Interfaces;
using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace katio.Business.Services;

public class AudioBookService : IAudioBookService
{
    // Lista de libros
    private readonly IUnitOfWork _unitOfWork;

    // Constructor
    public AudioBookService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Traer todos los Audiolibros
    public async Task<BaseMessage<AudioBook>> Index()
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync();
            return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar un Audiolibro omniscient
    public async Task<BaseMessage<AudioBook>> SearchAudioBookAsync(string searchTerm)
    {
        try
        {
            var parameter = Expression.Parameter(typeof(AudioBook), "audioBook");
            var searchExpressions = new List<Expression>();

            var lowerSearchTerm = Expression.Constant(searchTerm.ToLower(), typeof(string));

            // Search in Name
            var nameProperty = Expression.Property(parameter, nameof(AudioBook.Name));
            var nameToLower = Expression.Call(nameProperty, "ToLower", null);
            var nameContains = Expression.Call(
                nameToLower,
                "Contains",
                null,
                lowerSearchTerm
            );
            searchExpressions.Add(nameContains);

            // Search in ISBN10
            var isbn10Property = Expression.Property(parameter, nameof(AudioBook.ISBN10));
            var isbn10ToLower = Expression.Call(isbn10Property, "ToLower", null);
            var isbn10Contains = Expression.Call(
                isbn10ToLower,
                "Contains",
                null,
                lowerSearchTerm
            );
            searchExpressions.Add(isbn10Contains);

            // Search in ISBN13
            var isbn13Property = Expression.Property(parameter, nameof(AudioBook.ISBN13));
            var isbn13ToLower = Expression.Call(isbn13Property, "ToLower", null);
            var isbn13Contains = Expression.Call(
                isbn13ToLower,
                "Contains",
                null,
                lowerSearchTerm
            );
            searchExpressions.Add(isbn13Contains);

            // Search in Edition
            var editionProperty = Expression.Property(parameter, nameof(AudioBook.Edition));
            var editionToLower = Expression.Call(editionProperty, "ToLower", null);
            var editionContains = Expression.Call(
                editionToLower,
                "Contains",
                null,
                lowerSearchTerm
            );
            searchExpressions.Add(editionContains);

            // Search in Genre
            var genreProperty = Expression.Property(parameter, nameof(AudioBook.Genre));
            var genreToLower = Expression.Call(genreProperty, "ToLower", null);
            var genreContains = Expression.Call(
                genreToLower,
                "Contains",
                null,
                lowerSearchTerm
            );
            searchExpressions.Add(genreContains);

            // Search in Published (if searchTerm is a valid date)
            if (DateOnly.TryParse(searchTerm, out var publishedDate))
            {
                var publishedProperty = Expression.Property(parameter, nameof(AudioBook.Published));
                var publishedEquals = Expression.Equal(publishedProperty, Expression.Constant(publishedDate));
                searchExpressions.Add(publishedEquals);
            }

            // Combine all search expressions with OR
            var body = searchExpressions.Aggregate(Expression.OrElse);
            var lambda = Expression.Lambda<Func<AudioBook, bool>>(body, parameter);

            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(lambda);
            return result.Any() ? Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    #region Create Update Delete

    // Crear un Audiolibro
    public async Task<BaseMessage<AudioBook>> CreateAudioBook(AudioBook audioBook, IFormFile audioFile)
    {
        var existingAudioBook = await _unitOfWork.AudioBookRepository.GetAllAsync(ab => ab.ISBN10 == audioBook.ISBN10 || ab.ISBN13 == audioBook.ISBN13);

        if (existingAudioBook.Any())
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.Conflict, BaseMessageStatus.AUDIOBOOK_ALREADY_EXISTS);
        }
        try
        {
            // Homogeneizar el archivo de audio: convertir el nombre a minusculas y quitar caracteres especiales
            var sanitizedFileName = Path.GetFileNameWithoutExtension(audioFile.FileName)
            .ToLowerInvariant()
            .Replace(" ", "_") + Path.GetExtension(audioFile.FileName).ToLowerInvariant();

            // Verificar si ya existe un audio con la misma ruta en la BD
            var relativePath = Path.Combine("uploads", "audiobooks", sanitizedFileName);
            var existingfile = await _unitOfWork.AudioBookRepository.GetAllAsync(ab => ab.AudioPath == relativePath);

            if (existingfile.Any())
            {
                return Utilities.BuildResponse<AudioBook>(HttpStatusCode.Conflict, BaseMessageStatus.ALREADY_EXISTS_409);
            }

            // Ruta donde se guardara el archivo fisicamente
            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "audiobooks");
            Directory.CreateDirectory(uploadsFolderPath); // Crear el directorio si no existes

            var filePath = Path.Combine(uploadsFolderPath, sanitizedFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await audioFile.CopyToAsync(stream);
            }

            audioBook.AudioPath = relativePath;


            await _unitOfWork.AudioBookRepository.AddAsync(audioBook);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { audioBook });
    }

    // Actualizar un Audiolibro
    public async Task<BaseMessage<AudioBook>> UpdateAudioBook(AudioBook audioBook)
    {
        var existingAudioBook = await _unitOfWork.AudioBookRepository.GetAllAsync(ab => ab.ISBN10 == audioBook.ISBN10 || ab.ISBN13 == audioBook.ISBN13);

        if (!existingAudioBook.Any())
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND);
        }
        try
        {
            await _unitOfWork.AudioBookRepository.Update(audioBook);
            await _unitOfWork.SaveAsync();

        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { audioBook });
    }

    // Eliminar un Audiolibro
    public async Task<BaseMessage<AudioBook>> DeleteAudioBook(int id)
    {
        var existingAudioBook = await _unitOfWork.AudioBookRepository.GetAllAsync(ab => ab.Id == id);

        if (!existingAudioBook.Any())
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        try
        {
            await _unitOfWork.AudioBookRepository.Delete(id);
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { });
    }
    #endregion

    #region Find By AudioBook
    // Buscar por id
    public async Task<BaseMessage<AudioBook>> GetAudioBookById(int id)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.FindAsync(id);
            return result != null ? Utilities.BuildResponse<AudioBook>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { result }) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    // Buscar por Nombre
    public async Task<BaseMessage<AudioBook>> GetByAudioBookName(string name)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.Name.ToLower().Contains(name.ToLower()));
            return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por ISBN10
    public async Task<BaseMessage<AudioBook>> GetByAudioBookISBN10(string ISBN10)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.ISBN10 == ISBN10);
            return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por ISBN13
    public async Task<BaseMessage<AudioBook>> GetByAudioBookISBN13(string ISBN13)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.ISBN13 == ISBN13);
            return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por Rango de Fecha de Publicación
    public async Task<BaseMessage<AudioBook>> GetByAudioBookPublished(DateOnly startDate, DateOnly endDate)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.Published >= startDate && b.Published <= endDate);
            return result.Any() ? Utilities.BuildResponse<AudioBook>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por Edición
    public async Task<BaseMessage<AudioBook>> GetByAudioBookEdition(string edition)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.Edition.ToLower().Contains(edition.ToLower()));
            return result.Any() ? Utilities.BuildResponse<AudioBook>
                    (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                    Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por Género
    public async Task<BaseMessage<AudioBook>> GetByAudioBookGenre(string genre)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.Genre.ToLower().Contains(genre.ToLower()));
            return result.Any()
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }


    // Buscar por Duración en Segundos
    public async Task<BaseMessage<AudioBook>> GetByAudioBookLenghtInSeconds(int lenghtInSeconds)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.FindAsync(lenghtInSeconds);
            return result != null
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { result })
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }


    #endregion

    #region Find By Narrator

    // Buscar por Narrador
    public async Task<BaseMessage<AudioBook>> GetAudioBookByNarrator(int narratorId)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.NarratorId == narratorId, includeProperties: "Narrator");
            return (result != null)
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }


    // Buscar por Nombre de Narrador
    public async Task<BaseMessage<AudioBook>> GetAudioBookByNarratorName(string narratorName)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.Narrator.Name.ToLower().Contains(narratorName.ToLower()),
            includeProperties: "Narrator");
            return result.Any()
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por Apellido del Narrador
    public async Task<BaseMessage<AudioBook>> GetAudioBookByNarratorLastName(string narratorLastName)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.Narrator.LastName.ToLower().Contains(narratorLastName.ToLower()),
            includeProperties: "Narrator");
            return result.Any()
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por Nombre y Apellido de Narrador
    public async Task<BaseMessage<AudioBook>> GetAudioBookByNarratorFullName(string narratorName, string narratorLastName)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.Narrator.Name.ToLower().Contains(narratorName.ToLower()) &&
            a.Narrator.LastName.ToLower().Contains(narratorLastName.ToLower()),
            includeProperties: "Narrator");
            return result.Any()
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }


    public async Task<BaseMessage<AudioBook>> GetAudioBookByNarratorGenre(string genre)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.Narrator.Genre.ToLower().Contains(genre.ToLower()),
            includeProperties: "Narrator");
            return result.Any()
                    ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                    : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    #endregion
}