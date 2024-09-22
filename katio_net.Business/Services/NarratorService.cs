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
    private readonly KatioContext _context;
    private readonly IUnitOfWork _unitOfWork;

    // Constructor
    public NarratorService(KatioContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }
    

    // Traer todos los Narradores
    public async Task<BaseMessage<Narrator>> Index()
        {
            var result = await _unitOfWork.NarratorRepository.GetAllAsync();
            return result.Any() ? Utilities.BuildResponse<Narrator>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result.ToList()) :
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
            await _unitOfWork.NarratorRepository.AddAsync(newNarrator);
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Narrator>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Narrator> { newNarrator });
    }


    // Actualizar Narradores
    public async Task<BaseMessage<Narrator>> UpdateNarrator(Narrator narrator)
    {
        var result = await _unitOfWork.NarratorRepository.FindAsync(narrator.Id);
        if (result == null)
        {
            return Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Narrator>());
        }

        result.Name = narrator.Name;
        result.LastName = narrator.LastName;
        result.Genre = narrator.Genre;

        await _unitOfWork.NarratorRepository.Update(result);
        await _unitOfWork.SaveAsync();

        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Narrator> { result });
    }


    // Eliminar Narradores
    public async Task<BaseMessage<Narrator>> DeleteNarrator(int id)
        {
            var result = await _unitOfWork.NarratorRepository.FindAsync(id);
            if (result != null)
            {
                await _unitOfWork.NarratorRepository.Delete(result);
                await _unitOfWork.SaveAsync();
            }
            return result != null ? Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Narrator> { result }) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Narrator>());
        }

    #endregion

    #region  Find By Narrator
    //Buscar narrador por Id
    public async Task<BaseMessage<Narrator>> GetNarratorById(int id)
    {
        var result = await _unitOfWork.NarratorRepository.FindAsync(id);
        return result != null ? Utilities.BuildResponse<Narrator>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Narrator> { result }) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Narrator>());
    
    }

    // Buscar Narradores por Nombre
    public async Task<BaseMessage<Narrator>> GetNarratorsByName(string name)
    {
        var result = await _unitOfWork.NarratorRepository.GetAllAsync(n => n.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));
        return result.Any() ? Utilities.BuildResponse<Narrator>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result.ToList()) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Narrator>());
    }
    // Buscar Narradores por Apellido  
    public async Task<BaseMessage<Narrator>> GetNarratorsByLastName(string lastName)
    {
        var result = await _unitOfWork.NarratorRepository.GetAllAsync(n => n.LastName.Contains(lastName, StringComparison.InvariantCultureIgnoreCase));
        return result.Any() ? Utilities.BuildResponse<Narrator>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result.ToList()) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Narrator>());
    }
    // Buscar Narradores por Genero
    public async Task<BaseMessage<Narrator>> GetNarratorsByGenre(string genre)
        {
            var result = await _unitOfWork.NarratorRepository.GetAllAsync(n => n.Genre.Contains(genre, StringComparison.InvariantCultureIgnoreCase));
            return result.Any() ? Utilities.BuildResponse<Narrator>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result.ToList()) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Narrator>());
        }

    #endregion

}
