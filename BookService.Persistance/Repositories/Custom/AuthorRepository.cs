using BookService.Domain.Entities.Authors;
using BookService.Infrastructure.Persistence.Repositories.Default;
using BookService.Infrastructure.Persistence.UnitOfWorks.Default;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookService.Infrastructure.Persistence.Repositories.Custom
{
    public interface IAuthorRepository : IRepository<AuthorEntity>
    {
        Task<List<AuthorEntity>> GetByBookIdAsync(int bookId);
        Task<AuthorEntity?> FindByNameAsync(string name);
        Task<List<AuthorEntity>> GetListAsync(); // Добавлен метод для получения всех авторов
        Task<AuthorEntity?> FindAsync(int id);   // Добавлен метод для поиска по ID
    }

    public sealed class AuthorRepository : Repository<AuthorEntity>, IAuthorRepository
    {
        public AuthorRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<List<AuthorEntity>> GetByBookIdAsync(int bookId)
            => await GetSet()
                .Include(a => a.Books)
                .Where(a => a.Books.Any(b => b.Id == bookId))
                .ToListAsync();

        public async Task<AuthorEntity?> FindByNameAsync(string name)
            => await FirstOrDefaultAsync(a => a.Name.ToLower() == name.ToLower());

        public async Task<List<AuthorEntity>> GetListAsync()
            => await GetSet().ToListAsync(); // Реализация для получения всех авторов

        public async Task<AuthorEntity?> FindAsync(int id)
            => await GetSet().FindAsync(id); // Реализация для поиска автора по ID
    }
}
