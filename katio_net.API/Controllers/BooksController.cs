using Microsoft.AspNetCore.Mvc;
using katio.Business.Interfaces;
using katio.Data.Models;

namespace katio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        // Servicio de libros
        private readonly IBookService _bookService;
        // Constructor
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }
        // Crea un libro
        [HttpPost]
        [Route("CreateBook")]
        public async Task<IActionResult> CreateBook(Book book)
        {
            var response = await _bookService.CreateBook(book);
            return response.StatusCode == System.Net.HttpStatusCode.OK ? Ok(response) : StatusCode((int)response.StatusCode, response);
        }
        // Actualiza un libro
        [HttpPut]
        [Route("UpdateBook")]
        public async Task<IActionResult> UpdateBook(Book book)
        {
            var response = await _bookService.UpdateBook(book);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }
        //Elimina un libro
        [HttpDelete]
        [Route("DeleteBook")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var response = await _bookService.DeleteBook(id);
            return response.StatusCode == System.Net.HttpStatusCode.OK ? Ok(response) : StatusCode((int)response.StatusCode, response);
        }
        // Trae todos los libros
        [HttpGet]
        [Route("GetBooks")]
        public async Task<IActionResult> Index()
        {
            var response = await _bookService.Index();
            return response.TotalElements > 0 ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }
        //Trae un libro por su nombre
        [HttpGet]
        [Route("GetBookByName")]
        public async Task<IActionResult> GetBookByName(string name)
        {
            var response = await _bookService.GetBooksByName(name);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }
        // Trae un libro por su ISBN10
        [HttpGet]
        [Route("GetBookByISBN10")]
        public async Task<IActionResult> GetBookByISBN10(string isbn10)
        {
            var response = await _bookService.GetBooksByISBN10(isbn10);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }
        // Trae un libro por su ISBN13
        [HttpGet]
        [Route("GetBookByISBN13")]
        public async Task<IActionResult> GetBookByISBN13(string isbn13)
        {
            var response = await _bookService.GetBooksByISBN13(isbn13);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }
        // Trae libros por rango de fecha de publicación
        [HttpGet]
        [Route("GetBooksByPublished")]
        public async Task<IActionResult> GetBookByPublished(DateTime StartDate, DateTime EndDate)
        {
            var response = await _bookService.GetBooksByPublished(StartDate, EndDate);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }
        // Trae un libro por su edición
        [HttpGet]
        [Route("GetBookByEdition")]
        public async Task<IActionResult> GetBookByEdition(string edition)
        {
            var response = await _bookService.GetBooksByEdition(edition);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }
        // Trae un libro por su índice Dewey
        [HttpGet]
        [Route("GetBookByDeweyIndex")]
        public async Task<IActionResult> GetBookByDeweyIndex(string deweyIndex)
        {
            var response = await _bookService.GetBooksByDeweyIndex(deweyIndex);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }
    }
}
