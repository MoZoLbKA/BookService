using BookService.Domain.Entities.Authors.Entities;
using BookService.Domain.Entities.Books.DTOs;
using System.ComponentModel.DataAnnotations;

namespace BookService.Domain.Entities.Books.Entities;

/// <summary>
/// Книга
/// </summary>
public class BookEntity : BookEntityBase
{
    /// <summary>Идентификатор</summary>
    [Key]
    public int Id { get; set; }
    public AuthorEntity Author { get; set; } = null!;
    public BookEntity()
    {
        
    }
    public BookEntity(BookCreateModel model) : base(model)
    {
        
    }
}
