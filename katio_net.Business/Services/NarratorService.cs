using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;
using Microsoft.EntityFrameworkCore;
using katio.Business.Interfaces;


namespace katio.Business.Services;

public class NarratorService : INarratorService
{
    // Lista de narradores
    private readonly katioContext _context;

    // Constructor
    public NarratorService(katioContext context)
    {
        _context = context;
    }

    // Traer todos los Narradores
    public async Task<BaseMessage<Narrator>> Index()
    {
        var result = _context.Narrators.ToList();
        return result.Any() ? Utilities.BuildResponse<Narrator>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Narrator>());
    }

    #region Create Update Delete

    // Crear Narradores
    public async Task<BaseMessage<Narrator>> CreateNarrator(Narrator narrator)
    {
        var newNarrator = new Narrator()
        {
            Name = narrator.Name,
            LastName = narrator.LastName,
            Genre = narrator.Genre
        };
        try
        {
            await _context.Narrators.AddAsync(newNarrator);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Narrator>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Narrator> { newNarrator });
    }


    // Actualizar Narradores
    public async Task<Narrator> UpdateNarrator(Narrator narrator)
    {
        var result = _context.Narrators.FirstOrDefault(b => b.Id == narrator.Id);
        if (result != null)
        {
            result.Name = narrator.Name;
            result.LastName = narrator.LastName;
            result.Genre = narrator.Genre;
            await _context.SaveChangesAsync();
        }
        return result;
    }

    // Eliminar Narradores
    public async Task<BaseMessage<Narrator>> DeleteNarrator(int id)
    {
        var result = _context.Narrators.FirstOrDefault(b => b.Id == id);
        if (result != null)
        {
            _context.Narrators.Remove(result);
            await _context.SaveChangesAsync();
        }
        return result != null ? Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Narrator> { result }) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Narrator>());
    }

    #endregion

    #region  Find By Narrator

    // Buscar Narradores por Nombre
    public async Task<BaseMessage<Narrator>> GetNarratorsByName(string name)
    {
        var result = await _context.Narrators.Where(b => b.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase)).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<Narrator>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Narrator>());
    }
    // Buscar Narradores por Apellido  
    public async Task<BaseMessage<Narrator>> GetNarratorsByLastName(string lastName)
    {
        var result = await _context.Narrators.Where(b => b.LastName.Contains(lastName, StringComparison.InvariantCultureIgnoreCase)).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<Narrator>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Narrator>());
    }
    // Buscar Narradores por Genero
    public async Task<BaseMessage<Narrator>> GetNarratorsByGenre(string Genre)
    {
        var result = await _context.Narrators.Where(b => b.Genre.Contains(Genre, StringComparison.InvariantCultureIgnoreCase)).ToListAsync();
        return result.Any() ? Utilities.BuildResponse<Narrator>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Narrator>());
    }

    #endregion

}
