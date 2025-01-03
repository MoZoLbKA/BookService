using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BookService.Entiies.Users.Entity;

namespace BookService.Entiies.Users.DTOs
{
    public sealed class UserCreateModel : UserEntityBase
    {
        [DisplayName("Почта")]
        [Required(ErrorMessage = "Укажите почту!")]
        [MaxLength(500, ErrorMessage = "Максимальная длина - {0} символов!")]
        [EmailAddress(ErrorMessage = "Неверный формат почты!")]
        public string Login { set; get; } = null!;

        [DisplayName("Пароль")]
        [MaxLength(100, ErrorMessage = "Максимальная длина пароля - {0} символов!")]
        [MinLength(6, ErrorMessage = "Минимальная длина пароля - 6 символов!")]
        [Required(ErrorMessage = "Укажите пароль!")]
        public string Password { set; get; } = null!;
    }
}
