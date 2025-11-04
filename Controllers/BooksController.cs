using Microsoft.AspNetCore.Mvc;
using DZ5_Az.Models;
using DZ5_Az.Models.DTOs;
using DZ5_Az.Services.Abstract;

namespace DZ5_Az.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IAzureBookService _bookService;
        // GET
        public BooksController(IAzureBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        public async Task<IActionResult> PostBook(BookDTO bookData)
        {
            if (ModelState.IsValid)
            {
                Book book = new Book
                {
                    Title = bookData.Title,
                    Author = bookData.Author,
                    Pages = bookData.Pages,
                    PartitionKey = bookData.Author,
                    RowKey = Guid.NewGuid().ToString()
                };
                Book createdBook = await _bookService.UpsertEntityAsync(book);
                return Ok(new { message = "Book created", book = createdBook });
            }
            return BadRequest(new { error = "Validation error", ModelState });
        }

        [HttpGet("{author?}")]
        public async Task<IActionResult> GetBooks(string? author)
        {
            IEnumerable<Book> books = await _bookService.GetEntitiesAsync(author);
            return Ok(books);
        }

        [HttpGet]
        public async Task<IActionResult> GetBook(string id, string author)
        {
            Book book = await _bookService.GetEntityAsync(author, id);
            return Ok(book);
        }

        [HttpDelete("{author}/{id}")]
        public async Task<IActionResult> DeleteBook(string id, string author)
        {
            Book book = await _bookService.GetEntityAsync(author, id);
            if (book is null)
            {
                return NotFound();
            }
            await _bookService.DeleteEntityAsync(book);
            return Ok(new { message = "Book deleted" });
        }
    }
}
