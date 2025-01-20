using BookService.Domain.Entities.Users.DTOs;
using BookService.Domain.Entities.Users.Entity;
using BookService.Infrastructure.Persistence.Contexts;
using BookService.Infrastructure.Persistence.Repositories.Custom;
using BookService.Infrastructure.Persistence.UnitOfWorks.Default;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookService.Infrastructure.Persistence.UnitOfWorks.Custom;

public interface IUserUnitOfWork : IUnitOfWork
{
    IUserRepository UserRepository { get; set; }

    Task<List<UserEntity>> GetListAsync();
    Task<UserEntity?> FindAsync(int id, bool tracking = true);
    Task<UserEntity?> FindAsync(UserLoginModel model, bool tracking = true);
    Task AddAsync(UserEntity model);
    void Update(UserEntity model);
}

public sealed class UserUnitOfWork : UnitOfWork, IUserUnitOfWork
{
    public IUserRepository UserRepository { get; set; }
    public UserUnitOfWork(ApplicationDbContext database, ILoggerFactory factory) : base(database, factory)
    {
        UserRepository = new UserRepository(this);
    }

    public async Task<List<UserEntity>> GetListAsync()
    {
        SetNoTracking();
        var list = await UserRepository.GetListAsync();
        RestoreTracking();
        return list;
    }
    public async Task<UserEntity?> FindAsync(int id, bool tracking = true)
    {
        if (!tracking)
        {
            SetNoTracking();
        }
        var item = await UserRepository.FindAsync(id);
        if (!tracking)
        {
            RestoreTracking();
        }
        return item;
    }
    public async Task<UserEntity?> FindAsync(UserLoginModel model, bool tracking = true)
    {
        if (!tracking)
        {
            SetNoTracking();
        }
        var item = await UserRepository.FindWithLoginAndPasswordAsync(model);
        if (!tracking)
        {
            RestoreTracking();
        }
        return item;
    }
    public async Task AddAsync(UserEntity model)
        => await UserRepository.AddAsync(model);
    public void Update(UserEntity model)
        => UserRepository.Update(model);
}
