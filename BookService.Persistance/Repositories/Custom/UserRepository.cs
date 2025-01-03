using BookService.Infrastructure.Persistence.UnitOfWorks.Default;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using BookService.Infrastructure.Persistence.Repositories.Default;
using BookService.Infrastructure.Persistence.Contexts;
using System.Linq;
using BookService.Domain.Entiies.Users.Entity;
using BookService.Domain.Entiies.Users.Enums;
using BookService.Domain.Entiies.Users.DTOs;

namespace BookService.Infrastructure.Persistence.Repositories.Custom;

public interface IUserRepository : IRepository<UserEntity>
{
    Task<List<UserEntity>> GetListAsync();
    Task<List<UserEntity>> GetListAsync(UserRole role);
    Task<UserEntity?> FindWithLoginAsync(string login);
    Task<UserEntity?> FindAsync(int id);
    Task<UserEntity?> FindWithRoleAsync(int id, UserRole role);
    Task<UserEntity?> FindWithLoginAndPasswordAsync(UserLoginModel model);
}

public sealed class UserRepository : Repository<UserEntity>, IUserRepository
{
    public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    public async Task<List<UserEntity>> GetListAsync()
        => await GetSet().ToListAsync();
    public async Task<List<UserEntity>> GetListAsync(UserRole role)
        => await GetSet().Where(x => x.Role == role).ToListAsync();
    public async Task<UserEntity?> FindAsync(int id)
        => await FirstOrDefaultAsync(x => x.Id == id);
    public async Task<UserEntity?> FindWithRoleAsync(int id, UserRole role)
        => await FirstOrDefaultAsync(x => x.Id == id && x.Role == role);
    public async Task<UserEntity?> FindWithLoginAndPasswordAsync(UserLoginModel model)
        => await FirstOrDefaultAsync(x => x.Login == model.Login.ToLower() && x.PasswordHash == model.GetPasswordHash());
    public async Task<UserEntity?> FindWithLoginAsync(string login)
        => await FirstOrDefaultAsync(x => x.Login == login.ToLower());
}
