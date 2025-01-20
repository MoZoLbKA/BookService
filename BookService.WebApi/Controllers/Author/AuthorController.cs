using BookService.Domain.Entities.Authors;
using BookService.Persistance.UnitOfWorks.Custom;
using BookService.Infrastructure.Persistence.UnitOfWorks.Custom;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookService.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorController : ControllerBase
    {
        public readonly IAuthorUnitOfWork _authorUnitOfWork;

        public AuthorController(IAuthorUnitOfWork authorUnitOfWork)
        {
            _authorUnitOfWork = authorUnitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _authorUnitOfWork.FindAsync(id);
            return author is not null ? Ok(author) : NotFound();
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetAuthorByName(string name)
        {
            var author = await _authorUnitOfWork.FindByNameAsync(name);
            return author is not null ? Ok(author) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorEntity author)
        {
            await _authorUnitOfWork.AddAsync(author);
            await _authorUnitOfWork.SaveAsync();
            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
        }
    }

}
