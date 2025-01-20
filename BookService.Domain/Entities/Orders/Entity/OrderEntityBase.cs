using BookService.Domain.Entities.Orders.Enums;
using System.ComponentModel.DataAnnotations;

namespace BookService.Domain.Entities.Orders.Entity;

public abstract class OrderEntityBase
{
    [Required]
    public OrderStatus Status { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    [Required]
    [MaxLength(255)]
    public string? DeliveryAddress { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    protected OrderEntityBase()
    {
        
    }
    protected OrderEntityBase(OrderEntityBase model)
    {
        Status = model.Status;
        TotalPrice = model.TotalPrice;
        DeliveryAddress = model.DeliveryAddress;
        CreatedAt = model.CreatedAt;
        UpdatedAt = model.UpdatedAt;
    }
}
