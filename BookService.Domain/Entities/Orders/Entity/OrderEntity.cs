using BookService.Domain.Entities.Orders.Enums;
using BookService.Domain.Entities.Users.Entity;
using System.ComponentModel.DataAnnotations;

namespace BookService.Domain.Entities.Orders.Entity;

public class OrderEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public UserEntity User { get; set; }

    [Required]
    public ICollection<BookOrderEntity> Books { get; set; } = new List<BookOrderEntity>();

    [Required]
    public OrderStatus Status { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    [Required]
    [MaxLength(255)]
    public string? DeliveryAddress { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
