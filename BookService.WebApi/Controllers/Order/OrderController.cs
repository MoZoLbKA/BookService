using BookService.Domain.Entities.Orders.DTOs;
using BookService.Domain.Entities.Orders.Entity;
using BookService.JwtAuth;
using BookService.Persistance.UnitOfWorks.Custom;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OrderService.WebApi.Controllers.Order;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderUnitOfWork _OrderUnitOfWork;
    private readonly IJwtUserManager _userManager;

    public OrderController(IOrderUnitOfWork OrderUnitOfWork, IJwtUserManager userManager)
    {
        _OrderUnitOfWork = OrderUnitOfWork;
        _userManager = userManager;
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

    [HttpPost("create")]
    [AuthorizeJWT]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateModel model)
    {
        var order = new OrderEntity(model);
        order.UserId = _userManager.GetUserId();
        await _OrderUnitOfWork.OrderRepository.AddAsync(order);
        await _OrderUnitOfWork.OrderRepository.SaveAsync();
        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }
}

