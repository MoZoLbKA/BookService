using BookService.Domain.Entities.Authors.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookService.Domain.Entities.Books.Entities;

public abstract class BookEntityBase
{
    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = null!;

    /// <summary>Автор</summary>
    [Required]
    public int AuthorId { get; set; }

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
    protected BookEntityBase()
    {
        
    }
    protected BookEntityBase(BookEntityBase model) : base()
    {
        Title = model.Title;
        AuthorId = model.AuthorId;
        Genre = model.Genre;
        Year = model.Year;
        Price = model.Price;
    }
}
