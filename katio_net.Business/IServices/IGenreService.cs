using katio.Data.Dto;
using katio.Data.Models;

namespace katio.Business.Interfaces;

public interface IGenreService
{
    Task<BaseMessage<Genre>> Index();
    Task<BaseMessage<Genre>> GetGenresByName(string Name);
    Task<BaseMessage<Genre>> GetGenresByDescription(string Description);
    Task<BaseMessage<Genre>> DeleteGenre(int Id);
    Task<BaseMessage<Genre>> CreateGenre(Genre genre);
    Task<Genre> UpdateGenre(Genre genre);
}
