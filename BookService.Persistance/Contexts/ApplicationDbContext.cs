using BookService.Domain.Entities.Users.Entity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using BookService.Domain.Entities.Authors.Entities;
using BookService.Domain.Entities.Books.Entities;

namespace BookService.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<AuthorEntity> Authors { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }
    }
}