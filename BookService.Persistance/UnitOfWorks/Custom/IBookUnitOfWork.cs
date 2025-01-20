using BookService.Domain.Entities.Books;
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

    public interface IBookUnitOfWork : IUnitOfWork
    {
        IBookRepository BookRepository { get; set; }

        Task<List<BookEntity>> GetListAsync();
        Task<BookEntity?> FindAsync(int id, bool tracking = true);
        Task AddAsync(BookEntity model);
        void Update(BookEntity model);
    }

public sealed class BookUnitOfWork : UnitOfWork, IBookUnitOfWork
{
    public IBookRepository BookRepository { get; set; }

    public BookUnitOfWork(ApplicationDbContext database, ILoggerFactory factory) : base(database, factory)
    {
        BookRepository = new BookRepository(this);
    }

    public async Task<List<BookEntity>> GetListAsync()
    {
        SetNoTracking();
        var list = await BookRepository.GetListAsync();
        RestoreTracking();
        return list;
    }

    public async Task<BookEntity?> FindAsync(int id, bool tracking = true)
    {
        if (!tracking)
        {
            SetNoTracking();
        }
        var item = await BookRepository.FindAsync(id);
        if (!tracking)
        {
            RestoreTracking();
        }
        return item;
    }

    public async Task AddAsync(BookEntity model)
        => await BookRepository.AddAsync(model);

    public void Update(BookEntity model)
        => BookRepository.Update(model);
}




