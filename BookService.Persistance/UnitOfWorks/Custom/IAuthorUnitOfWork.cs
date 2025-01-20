using BookService.Domain.Entities.Authors.Entities;
using BookService.Infrastructure.Persistence.Contexts;
using BookService.Infrastructure.Persistence.Repositories.Custom;
using BookService.Infrastructure.Persistence.UnitOfWorks.Default;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookService.Persistance.UnitOfWorks.Custom;

public interface IAuthorUnitOfWork : IUnitOfWork
{
    IAuthorRepository AuthorRepository { get; set; }

    Task<List<AuthorEntity>> GetListAsync();
    Task<AuthorEntity?> FindAsync(int id, bool tracking = true);
    Task<AuthorEntity?> FindByNameAsync(string name, bool tracking = true);
    Task AddAsync(AuthorEntity author);
    void Update(AuthorEntity author);
    void Delete(AuthorEntity author);
}

public sealed class AuthorUnitOfWork : UnitOfWork, IAuthorUnitOfWork
{
    public IAuthorRepository AuthorRepository { get; set; }

    public AuthorUnitOfWork(ApplicationDbContext database, ILoggerFactory factory)
        : base(database, factory)
    {
        AuthorRepository = new AuthorRepository(this);
    }

    public async Task<List<AuthorEntity>> GetListAsync()
    {
        SetNoTracking();
        var list = await AuthorRepository.GetListAsync();
        RestoreTracking();
        return list;
    }

    public async Task<AuthorEntity?> FindAsync(int id, bool tracking = true)
    {
        if (!tracking)
        {
            SetNoTracking();
        }
        var item = await AuthorRepository.FindAsync(id);
        if (!tracking)
        {
            RestoreTracking();
        }
        return item;
    }

    public async Task<AuthorEntity?> FindByNameAsync(string name, bool tracking = true)
    {
        if (!tracking)
        {
            SetNoTracking();
        }
        var item = await AuthorRepository.FindByNameAsync(name);
        if (!tracking)
        {
            RestoreTracking();
        }
        return item;
    }

    public async Task AddAsync(AuthorEntity author)
    {
        await AuthorRepository.AddAsync(author);
    }

    public void Update(AuthorEntity author)
    {
        AuthorRepository.Update(author);
    }
    public void Delete(AuthorEntity author)
    {
        AuthorRepository.Delete(author);
    }
}



