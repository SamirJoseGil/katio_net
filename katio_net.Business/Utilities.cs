using katio.Data.Dto;
using System.Net;

namespace katio.Business;

public static class Utilities
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
}

