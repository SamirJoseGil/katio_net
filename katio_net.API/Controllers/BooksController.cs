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
        public async Task<IActionResult> Index()
        {
            var response = await _bookService.GetAllBooks();
            return response.TotalElements > 0 ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }
        // Crea un libro
        [HttpPost]
        [Route("CreateBook")]
        public async Task<IActionResult> Create(Book book)
        {
            var response = await _bookService.CreateBook(book);
            return response.StatusCode == System.Net.HttpStatusCode.OK ? Ok(response) : StatusCode((int)response.StatusCode, response);
        }


        // TODO DE ACA PARA ABAJO NO FUNCIONA   
        // Trae los libros por nombre
        [HttpGet]
        [Route("GetBooksByName")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByName(string Name)
        {
            var books = await _bookService.GetAllBooksByName(Name);
            return Ok(books);
        }

        // Trae los libros por ISBN10
        [HttpGet]
        [Route("GetBooksByISBN10")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByISBN10(string ISBN10)
        {
            var books = await _bookService.GetAllBookByISBN10(ISBN10);
            return Ok(books);
        }

        // Trae los libros por ISBN13
        [HttpGet]
        [Route("GetBooksByISBN13")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByISBN13(string ISBN13)
        {
            var books = await _bookService.GetAllBookByISBN13(ISBN13);
            return Ok(books);
        }

        // Trae los libros por edición
        [HttpGet]
        [Route("GetBooksByEdition")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByEdition(string Edition)
        {
            var books = await _bookService.GetAllBookByEdition(Edition);
            return Ok(books);
        }

        // Trae los libros por índice Dewey
        [HttpGet]
        [Route("GetBooksByDeweyIndex")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByDeweyIndex(string DeweyIndex)
        {
            var books = await _bookService.GetAllBookByDeweyIndex(DeweyIndex);
            return Ok(books);
        }

        // Trae los libros por rango de publicación
        [HttpGet]
        [Route("GetBooksByPublished")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByPublished(DateTime StartDate, DateTime EndDate)
        {
            var books = await _bookService.GetAllBookByPublished(StartDate, EndDate);
            return Ok(books);
        }

        // Edita un libro
        [HttpPut]
        [Route("UpdateBook")]
        public async Task<ActionResult<Book>> UpdateBook(Book book)
        {
            var updatedBook = await _bookService.UpdateBook(book);
            return Ok(updatedBook);
        }

    }
}
