using BookService.Domain.Entities.Users.Entity;

namespace BookService.Domain.Entities.Users.DTOs;

public class UserEditModel : UserEntityBase
{
    public int Id { get; set; }

    public UserEditModel() : base() { }
    public UserEditModel(UserEditModel model)
    {
        Id = model.Id;
    }
}
