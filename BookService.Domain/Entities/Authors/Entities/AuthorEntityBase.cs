using System.ComponentModel.DataAnnotations;

namespace BookService.Domain.Entities.Authors.Entities;

public abstract class AuthorEntityBase
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>Биография</summary>
    [MaxLength(2000)]
    public string? Biography { get; set; }
    protected AuthorEntityBase()
    {
        
    }
    protected AuthorEntityBase(AuthorEntityBase model)
    {
        Biography = model.Biography;
        Name = model.Name;
    }

}
