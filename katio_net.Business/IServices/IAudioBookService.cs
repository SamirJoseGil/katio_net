

using katio.Data.Dto;
using katio.Data.Models;

namespace katio.Business.Interfaces;

public interface IAudioBookService
{
    Task<BaseMessage<AudioBook>> Index();
    Task<BaseMessage<AudioBook>> CreateAudioBook(AudioBook audioBook);
    Task<BaseMessage<AudioBook>> DeleteAudioBook(int id);
    Task<AudioBook> UpdateAudioBook(AudioBook audioBook);

    Task<BaseMessage<AudioBook>> GetByAudioBookName(string name);
    Task<BaseMessage<AudioBook>> GetByAudioBookISBN10(string ISBN10);
    Task<BaseMessage<AudioBook>> GetByAudioBookISBN13(string ISBN13);
    Task<BaseMessage<AudioBook>> GetByAudioBookPublished(DateOnly startDate, DateOnly endDate);
    Task<BaseMessage<AudioBook>> GetByAudioBookEdition(string edition);
    Task<BaseMessage<AudioBook>> GetByAudioBookGenre(string genre);
    Task<BaseMessage<AudioBook>> GetByAudioBookLenghtInSeconds(int lenghtInSeconds);

    Task<BaseMessage<AudioBook>> GetAudioBookByAuthor(int authorId);
    Task<BaseMessage<AudioBook>> GetAudioBookByAuthorName(string authorName);
    Task<BaseMessage<AudioBook>> GetAudioBookByAuthorLastName(string authorCountry);
    Task<BaseMessage<AudioBook>> GetAudioBookByAuthorFullName(string authorName, string authorLastName);
    Task<BaseMessage<AudioBook>> GetAudioBookByAuthorCountry(string authorCountry);
    Task<BaseMessage<AudioBook>> GetAudioBookByAuthorBirthDateRange(DateOnly startDate, DateOnly endDate);
}
