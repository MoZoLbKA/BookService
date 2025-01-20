using BookService.Domain.Entities.Authors.DTOs;
using BookService.Domain.Entities.Authors.Entities;
using BookService.Persistance.UnitOfWorks.Custom;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookService.API.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorController : ControllerBase
{
    public readonly IAuthorUnitOfWork _authorUnitOfWork;
    [HttpGet]
    public async Task<List<AuthorEntity>> List()
    {
        var list = await _authorUnitOfWork.GetListAsync();
        return list;
    }
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

    [HttpPost("create")]
    public async Task<IActionResult> CreateAuthor([FromBody] AuthorCreateModel model)
    {
        var author = new AuthorEntity(model);
        await _authorUnitOfWork.AddAsync(author);
        await _authorUnitOfWork.SaveAsync();
        return Ok(author);
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var author = await _authorUnitOfWork.FindAsync(id);
        if (author == null)
        {
            return NotFound();
        }
        _authorUnitOfWork.Delete(author);
        await _authorUnitOfWork.SaveAsync();
        return Ok(author);
    }
}
