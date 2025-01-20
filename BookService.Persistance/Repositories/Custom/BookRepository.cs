using BookService.Domain.Entities.Books;
using BookService.Infrastructure.Persistence.Repositories.Default;
using BookService.Infrastructure.Persistence.UnitOfWorks.Default;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookService.Infrastructure.Persistence.Repositories.Custom
{
    public interface IBookRepository : IRepository<BookEntity>
    {
        Task<List<BookEntity>> GetByAuthorIdAsync(int authorId);
        Task<BookEntity?> FindByTitleAsync(string title);
        Task<List<BookEntity>> GetListAsync(); // Метод для получения всех книг
        Task<BookEntity?> FindAsync(int id); // Метод для поиска книги по id
    }

    public sealed class BookRepository : Repository<BookEntity>, IBookRepository
    {
        public BookRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<List<BookEntity>> GetByAuthorIdAsync(int authorId)
            => await GetSet().Where(b => b.AuthorId == authorId).ToListAsync();

        public async Task<BookEntity?> FindByTitleAsync(string title)
            => await FirstOrDefaultAsync(b => b.Title.ToLower() == title.ToLower());

        public async Task<List<BookEntity>> GetListAsync()
            => await GetSet().ToListAsync(); // Реализация метода для получения всех книг

        public async Task<BookEntity?> FindAsync(int id)
            => await GetSet().FindAsync(id); // Реализация метода для поиска книги по id
    }
}
