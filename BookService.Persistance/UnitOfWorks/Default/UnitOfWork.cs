using BookService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookService.Infrastructure.Persistence.UnitOfWorks.Default;

public interface IUnitOfWork : IDisposable
{
    ApplicationDbContext DataBase { get; set; }
    ILoggerFactory LoggerFactory { get; set; }
    ILogger Logger { get; set; }
    Task MigrateAsync();
    Task SaveAsync(CancellationToken token = default);
    void SetNoTracking();
    void RestoreTracking();
}

public abstract class UnitOfWork : IUnitOfWork
{
    public ApplicationDbContext DataBase { get; set; }
    public ILoggerFactory LoggerFactory { get; set; }
    public ILogger Logger { get; set; }

    public UnitOfWork(ApplicationDbContext database, ILoggerFactory loggerFactory)
    {
        Logger = loggerFactory.CreateLogger(GetType().Name);
        DataBase = database;
        LoggerFactory = loggerFactory;
    }

    public async Task MigrateAsync()
        => await DataBase.Database.MigrateAsync();

    public async Task SaveAsync(CancellationToken token = default)
        => await DataBase.SaveChangesAsync(token);

    public void SetNoTracking()
        => DataBase.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

    public void RestoreTracking()
        => DataBase.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

    public void Dispose()
        => DataBase.Dispose();
}
