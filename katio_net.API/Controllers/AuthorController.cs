using Microsoft.AspNetCore.Mvc;
using katio.Business.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace katio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // Obtener todos los autores
        [HttpGet]
        public async Task<IEnumerable<string>> GetAllAuthors()
        {
            return await _authorService.GetAllAuthors();
        }

        // Obtener un autor por nombre
        // [HttpGet("{Name}")]
        // public async Task<string> GetAllAuthorByName(string Name)
        // {
        //    return await _authorService.GetAllAuthorsByName(Name);

        //}
    }
}
