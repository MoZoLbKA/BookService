using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using BookService.Domain.Entities.Orders.Entity;

namespace BookService.Domain.Entities.Orders.DTOs
{
    public class OrderCreateModel : OrderEntity
    {
        [DisplayName("Телефон для связи")]
        [Required(ErrorMessage = "Укажите телефон!")]
        [MaxLength(11, ErrorMessage = "Введите номер телефона (не более {0} цифр!)")]
        public string PhoneNomber { get; set; } = null;
    }
}
