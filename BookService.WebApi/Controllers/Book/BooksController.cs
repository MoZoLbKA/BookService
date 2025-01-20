using BookService.Domain.Entities.Books;
using BookService.Domain.Entities.Books.DTOs;
using BookService.Domain.Entities.Books.Entities;
using BookService.Domain.Entities.Users.Enums;
using BookService.Infrastructure.Persistence.UnitOfWorks.Custom;  // Для использования IBookUnitOfWork
using BookService.JwtAuth;
using BookService.Persistance.UnitOfWorks.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookService.WebApi.Controllers.Book
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly IBookUnitOfWork _bookUnitOfWork;
        private readonly IJwtUserManager _userManager;

        public BookController(IBookUnitOfWork bookUnitOfWork, IJwtUserManager userManager)
        {
            _bookUnitOfWork = bookUnitOfWork;
            _userManager = userManager;
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

        [HttpPost("create")]
        [AuthorizeJWT(roles: UserRole.Admin)]
        public async Task<IActionResult> CreateBook([FromBody] BookCreateModel model)
        {
            var book = new BookEntity(model);
            var author = await _bookUnitOfWork.AuthorRepository.FindAsync(book.AuthorId);
            if(author == null)
            {
                return BadRequest("Такого автора не существует");
            }
            book.PublisherId = _userManager.GetUserId();
            await _bookUnitOfWork.BookRepository.AddAsync(book);
            await _bookUnitOfWork.SaveAsync();
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }
    }
}
