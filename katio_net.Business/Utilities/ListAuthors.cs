using katio.Data.Models;

namespace katio.Business.Utilities;

public static class ListAuthors
{
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
