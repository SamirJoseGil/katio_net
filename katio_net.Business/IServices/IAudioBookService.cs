using katio.Data.Dto;
using katio.Data.Models;
using Microsoft.AspNetCore.Http;

namespace katio.Business.Interfaces;

public interface IAudioBookService
{
    Task<BaseMessage<AudioBook>> Index();
    Task<BaseMessage<AudioBook>> SearchAudioBookAsync(string searchTerm);
    Task<BaseMessage<AudioBook>> CreateAudioBook(AudioBook audioBook, IFormFile audioFile);
    Task<BaseMessage<AudioBook>> DeleteAudioBook(int id);
    Task<BaseMessage<AudioBook>> UpdateAudioBook(AudioBook audioBook);
    Task<BaseMessage<AudioBook>> GetAudioBookById(int id);
    Task<BaseMessage<AudioBook>> GetByAudioBookName(string name);
    Task<BaseMessage<AudioBook>> GetByAudioBookISBN10(string ISBN10);
    Task<BaseMessage<AudioBook>> GetByAudioBookISBN13(string ISBN13);
    Task<BaseMessage<AudioBook>> GetByAudioBookPublished(DateOnly startDate, DateOnly endDate);
    Task<BaseMessage<AudioBook>> GetByAudioBookEdition(string edition);
    Task<BaseMessage<AudioBook>> GetByAudioBookGenre(string genre);
    Task<BaseMessage<AudioBook>> GetByAudioBookLenghtInSeconds(int lenghtInSeconds);

    Task<BaseMessage<AudioBook>> GetAudioBookByNarrator(int narratorId);
    Task<BaseMessage<AudioBook>> GetAudioBookByNarratorName(string narratorName);
    Task<BaseMessage<AudioBook>> GetAudioBookByNarratorLastName(string narratorLastName);
    Task<BaseMessage<AudioBook>> GetAudioBookByNarratorFullName(string NarratorName, string narratorLastName);
    Task<BaseMessage<AudioBook>> GetAudioBookByNarratorGenre(string genre);
}
