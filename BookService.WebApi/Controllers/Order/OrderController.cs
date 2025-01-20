using BookService.Domain.Entities.Orders.Entity;
using BookService.Persistance.UnitOfWorks.Custom;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OrderService.WebApi.Controllers.Order;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderUnitOfWork _OrderUnitOfWork;

    public OrderController(IOrderUnitOfWork OrderUnitOfWork)
    {
        _OrderUnitOfWork = OrderUnitOfWork;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var Order = await _OrderUnitOfWork.OrderRepository.FindAsync(id); // Используем IOrderUnitOfWork
        return Order is not null ? Ok(Order) : NotFound();
    }

    [HttpGet("by-author/{authorId}")]
    public async Task<IActionResult> GetOrdersByUserId(int userId)
    {
        var Orders = await _OrderUnitOfWork.OrderRepository.FindByUserIdAsync(userId); // Используем IOrderUnitOfWork
        return Ok(Orders);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderEntity Order)
    {
        await _OrderUnitOfWork.OrderRepository.AddAsync(Order); // Используем IOrderUnitOfWork
        await _OrderUnitOfWork.OrderRepository.SaveAsync(); // Сохраняем изменения через UnitOfWork
        return CreatedAtAction(nameof(GetOrderById), new { id = Order.Id }, Order);
    }
}

