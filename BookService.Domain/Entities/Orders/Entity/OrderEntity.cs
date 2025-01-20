using BookService.Domain.Entities.Orders.DTOs;
using BookService.Domain.Entities.Users.Entity;
using System.ComponentModel.DataAnnotations;

namespace BookService.Domain.Entities.Orders.Entity;

public class OrderEntity : OrderEntityBase
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public UserEntity User { get; set; }

    [Required]
    public ICollection<BookOrderEntity> Books { get; set; } = new List<BookOrderEntity>();
    public OrderEntity()
    {
        
    }
    public OrderEntity(OrderCreateModel model) : base(model)
    {
        Status = Enums.OrderStatus.InProgress;
    }
}
