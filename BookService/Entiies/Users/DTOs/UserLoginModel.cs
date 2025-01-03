using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FinanceManager.WebApi.Infrastructure.Extensions;

namespace BookService.Entiies.Users.DTOs
{
    /// <summary>
    /// DTO для входа
    /// </summary>
    public class UserLoginModel
    {
        /// <summary>
        /// Логин
        /// </summary>
        [DisplayName("Почта")]
        [Required(ErrorMessage = "Укажите почту!")]
        [MaxLength(500, ErrorMessage = "Максимальная длина - {0} символов!")]
        [EmailAddress(ErrorMessage = "Неверный формат почты!")]
        public string Login { set; get; } = null!;
        /// <summary>
        /// Пароль
        /// </summary>
        [DisplayName("Пароль")]
        [MaxLength(100, ErrorMessage = "Максимальная длина пароля - 100 символов!")]
        [MinLength(6, ErrorMessage = "Минимальная длина пароля - 6 символов!")]
        [Required(ErrorMessage = "Укажите пароль!")]
        public string Password { set; get; } = null!;
        public string GetPasswordHash()
            => HashingExtensions.ComputeHash(Login, Password);
    }
}
