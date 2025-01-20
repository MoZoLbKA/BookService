using BookService.Domain.Entities.Orders.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookService.Domain.Entities.Orders.DTOs;

public class OrderCreateModel : OrderEntityBase
{
    [DisplayName("Телефон для связи")]
    [Required(ErrorMessage = "Укажите телефон!")]
    [MaxLength(11, ErrorMessage = "Введите номер телефона (не более {0} цифр!)")]
    public string PhoneNomber { get; set; } = null;
}
