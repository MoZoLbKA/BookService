using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookService.Domain.Entities.Books;

namespace BookService.Domain.Entities.Orders.Entity
{
    public class BookOrderEntity
    {

        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }

        public int BookId { get; set; }
        public BookEntity Book { get; set; }

        public int Quantity { get; set; } // Количество книг в заказе


    }
}
