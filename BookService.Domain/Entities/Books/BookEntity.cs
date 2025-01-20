using BookService.Domain.Entities.Authors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookService.Domain.Entities.Books
{
    /// <summary>
    /// Книга
    /// </summary>
    public class BookEntity
    {
        /// <summary>Идентификатор</summary>
        [Key]
        public int Id { get; set; }

        /// <summary>Название</summary>
        [Required]
        [MaxLength(500)]
        public string Title { get; set; } = null!;

        /// <summary>Автор</summary>
        [Required]
        public int AuthorId { get; set; }
        public AuthorEntity Author { get; set; } = null!;

        /// <summary>Жанр</summary>
        [Required]
        [MaxLength(100)]
        public string Genre { get; set; } = null!;

        /// <summary>Год издания</summary>
        [Required]
        public int Year { get; set; }

        /// <summary>Цена</summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>ID издательства</summary>
        [Required]
        public int PublisherId { get; set; }
    }
}
