using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;
using BookService.Entiies.Users.DTOs;
using FinanceManager.WebApi.Infrastructure.Extensions;

namespace BookService.Entiies.Users.Entity
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public sealed class UserEntity : UserEntityBase
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Почта
        /// </summary>
        [DisplayName("Почта")]
        [Required(ErrorMessage = "Укажите почту!")]
        [MaxLength(500, ErrorMessage = "Максимальная длина - {0} символов!")]
        [EmailAddress(ErrorMessage = "Неверный формат почты!")]
        public string Login { get; set; } = null!;
        /// <summary>
        /// Хеш пароля
        /// </summary>
        [Required]
        [MaxLength(1000)]
        [JsonIgnore]
        public string PasswordHash { get; set; } = null!;
        public DateTime? RefreshExpiryTime { get; set; }
        public string? RefreshToken { get; set; }
        public UserEntity()
        {

        }
        public UserEntity(UserEntity user)
        {
            Id = user.Id;
            PasswordHash = user.PasswordHash;
        }
        public UserEntity(UserEditModel model) : base()
        {
            FirstName = model.FirstName;
            SecondName = model.SecondName;
        }
        public UserEntity(UserCreateModel model) : base(model)
        {
            Login = model.Login.ToLower();
            PasswordHash = HashingExtensions.ComputeHash(Login, model.Password);
            Role = Enums.UserRole.Client;
        }
        public List<Claim> GetClaims()
            =>
            [
                new Claim("id", Id.ToString()),
                new Claim("login", Login),
                new Claim("role", ((int)Role).ToString()),
            ];

        public IEnumerable<KeyValuePair<string, object>> GetGeneratorClaims()
            => GetClaims().Select(x => new KeyValuePair<string, object>(x.Type, x.Value));
    }
}
