using BookService.Domain.Entities.Authors.DTOs;
using BookService.Domain.Entities.Books.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookService.Domain.Entities.Authors.Entities;

/// <summary>
/// Автор книги
/// </summary>
public class AuthorEntity : AuthorEntityBase
{
    /// <summary>Идентификатор</summary>
    [Key]
    public int Id { get; set; }
    [JsonIgnore]
    public List<BookEntity> Books { get; set; } = new();
    public AuthorEntity()
    {
        
    }
    public AuthorEntity(AuthorCreateModel model) : base(model) { }
}
