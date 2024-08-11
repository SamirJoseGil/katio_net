using Microsoft.AspNetCore.Mvc;
using katio.Business.Interfaces;
using katio.Data.Models;

namespace katio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // Trae todos los libros
        [HttpGet]
        [Route("GetBooks")]
        public async Task<IActionResult> GetBooks()
        {
            var response = await _bookService.GetAllBooks();
            return Ok(response);
        }
        // Trae los libros por nombre
        [HttpGet]
        [Route("GetBooksByName")]
        public async Task<ActionResult<IEnumerable<Books>>> GetBooksByName(string Name)
        {
            var books = await _bookService.GetAllBooksByName(Name);
            return Ok(books);
        }
        // Trae los libros por ISBN10
        [HttpGet]
        [Route("GetBooksByISBN10")]
        public async Task<ActionResult<IEnumerable<Books>>> GetBooksByISBN10(string ISBN10)
        {
            var books = await _bookService.GetAllBookByISBN10(ISBN10);
            return Ok(books);
        }
        // Trae los libros por ISBN13
        [HttpGet]
        [Route("GetBooksByISBN13")]
        public async Task<ActionResult<IEnumerable<Books>>> GetBooksByISBN13(string ISBN13)
        {
            var books = await _bookService.GetAllBookByISBN13(ISBN13);
            return Ok(books);
        }
        // Trae los libros por edición
        [HttpGet]
        [Route("GetBooksByEdition")]
        public async Task<ActionResult<IEnumerable<Books>>> GetBooksByEdition(string Edition)
        {
            var books = await _bookService.GetAllBookByEdition(Edition);
            return Ok(books);
        }
        // Trae los libros por índice Dewey
        [HttpGet]
        [Route("GetBooksByDeweyIndex")]
        public async Task<ActionResult<IEnumerable<Books>>> GetBooksByDeweyIndex(string DeweyIndex)
        {
            var books = await _bookService.GetAllBookByDeweyIndex(DeweyIndex);
            return Ok(books);
        }
    }
}
