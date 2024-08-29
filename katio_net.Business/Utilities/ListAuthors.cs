using katio.Data.Dto;
using katio.Data.Models;
using System.Net;

namespace katio.Business.Utilities;
public static class ListAuthors
{
    #region BaseMessage Response 
    public static BaseMessage<T> BuildResponse<T>(HttpStatusCode statusCode, string message, List<T>? elements = null)
    where T : class
    {
        return new BaseMessage<T>()
        {
            StatusCode = statusCode,
            Message = message,
            TotalElements = elements != null && elements.Any() ? elements.Count : 0,
            ResponseElements = elements ?? new List<T>()
        };
    }
    #endregion
    public static List<Author> CreateAuthorList()
    {
        List<Author> authorlist = new List<Author>()
        {
            new Author()
            {
                Name = "",
                LastName = "",
                Country = "",
                Birthdate = new DateTime(),
                Id = 1,
            },
        };
        return authorlist;
    }

}


/*
    Name = "",
    LastName = "",
    Country = "",
    Birthdate = new DateTime(),
    Id = 1
*/
