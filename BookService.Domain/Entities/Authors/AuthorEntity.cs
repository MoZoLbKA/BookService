using BookService.Domain.Entities.Books;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookService.Domain.Entities.Authors
{
    /// <summary>
    /// Автор книги
    /// </summary>
    public class AuthorEntity
    {
        /// <summary>Идентификатор</summary>
        [Key]
        public int Id { get; set; }

        /// <summary>Имя</summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        /// <summary>Биография</summary>
        [MaxLength(2000)]
        public string? Biography { get; set; }

        /// <summary>Список книг</summary>
        public List<BookEntity> Books { get; set; } = new();
    }
}
