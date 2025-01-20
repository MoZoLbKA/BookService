using BookService.Domain.Entities.Orders.DTOs;
using BookService.Domain.Entities.Orders.Entity;
using BookService.Infrastructure.Persistence.Contexts;
using BookService.Infrastructure.Persistence.Repositories.Custom;
using BookService.Infrastructure.Persistence.UnitOfWorks.Default;
using BookService.Infrastructure.Persistence.UnitOfWorks.Custom;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookService.Persistance.UnitOfWorks.Custom;
using BookService.Persistance.Repositories.Custom;

namespace BookService.Persistance.UnitOfWorks.Custom;

public interface IOrderUnitOfWork
{
    IOrderRepository OrderRepository { get; set; }

    Task AddAsync(OrderEntity model);
    void Update(OrderEntity model);
}

public sealed class OrderUnitOfWork : UnitOfWork, IOrderUnitOfWork
{
    public IOrderRepository OrderRepository { get; set; }

    public OrderUnitOfWork(ApplicationDbContext database, ILoggerFactory factory) : base(database, factory)
    {
        OrderRepository = new OrderRepository(this);
    }

    public async Task AddAsync(OrderEntity model)
    {
        await OrderRepository.AddAsync(model);
    }

    public void Update(OrderEntity model)
    {
        OrderRepository.Update(model);
    }
}

