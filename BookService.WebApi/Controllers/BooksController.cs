using BookService.Domain.Entiies.Books;
using BookService.Infrastructure.Persistence.UnitOfWorks.Custom;  // Для использования IBookUnitOfWork
using BookService.Persistance.UnitOfWorks.Custom;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookService.API.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly IBookUnitOfWork _bookUnitOfWork;

        public BookController(IBookUnitOfWork bookUnitOfWork)
        {
            _bookUnitOfWork = bookUnitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookUnitOfWork.BookRepository.FindAsync(id); // Используем IBookUnitOfWork
            return book is not null ? Ok(book) : NotFound();
        }

        [HttpGet("by-author/{authorId}")]
        public async Task<IActionResult> GetBooksByAuthor(int authorId)
        {
            var books = await _bookUnitOfWork.BookRepository.GetByAuthorIdAsync(authorId); // Используем IBookUnitOfWork
            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookEntity book)
        {
            await _bookUnitOfWork.BookRepository.AddAsync(book); // Используем IBookUnitOfWork
            await _bookUnitOfWork.SaveAsync(); // Сохраняем изменения через UnitOfWork
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }
    }
}
