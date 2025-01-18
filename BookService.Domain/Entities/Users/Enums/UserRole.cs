using System.ComponentModel.DataAnnotations;

namespace BookService.Domain.Entiies.Users.Enums;
/// <summary>
/// Роль пользователя
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Админ
    /// </summary>
    [Display(Name = "Администратор")]
    Admin,
    /// <summary>
    /// Клиент
    /// </summary>
    [Display(Name = "Клиент")]
    Client,
}
