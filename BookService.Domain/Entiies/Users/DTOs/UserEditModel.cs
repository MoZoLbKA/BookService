using BookService.Domain.Entiies.Users.Entity;

namespace BookService.Domain.Entiies.Users.DTOs;

public class UserEditModel : UserEntityBase
{
    public int Id { get; set; }

    public UserEditModel() : base() { }
    public UserEditModel(UserEditModel model)
    {
        Id = model.Id;
    }
}
