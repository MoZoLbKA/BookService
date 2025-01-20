using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookService.Domain.Entities.Orders.Enums
{
    public enum OrderStatus
    {
        /// <summary>
        /// В обработке
        /// </summary>
        [Display(Name = "В обработке")]
        InProgress,
        /// <summary>
        /// Отменен
        /// </summary>
        [Display(Name = "Отменен")]
        Cancelled,
        /// <summary>
        /// Выполнен
        /// </summary>
        [Display(Name = "Выполнен")]
        Done,
    }

}
