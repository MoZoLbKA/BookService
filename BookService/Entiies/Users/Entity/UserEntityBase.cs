using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BookService.Entiies.Users.Enums;

namespace BookService.Entiies.Users.Entity
{
    public abstract class UserEntityBase
    {
        /// <summary>
        /// Имя
        /// </summary>
        [DisplayName("Имя")]
        [Required(ErrorMessage = "Укажите имя!")]
        [MaxLength(500, ErrorMessage = "Максимальная длина - {0} символов!")]
        public string FirstName { get; set; } = null!;
        /// <summary>
        /// Фамилия
        /// </summary>
        [DisplayName("Фамилия")]
        [Required(ErrorMessage = "Укажите фамилию!")]
        [MaxLength(500, ErrorMessage = "Максимальная длина - {0} символов!")]
        public string SecondName { get; set; } = null!;
        /// <summary>
        /// Отчество
        /// </summary>
        [DisplayName("Отчество")]
        [MaxLength(500, ErrorMessage = "Максимальная длина - {0} символов!")]
        public string? ThirdName { get; set; }
        /// <summary>
        /// Роль
        /// </summary>
        [DisplayName("Роль")]
        [Required(ErrorMessage = "Укажите роль!")]
        [EnumDataType(typeof(UserRole), ErrorMessage = "Укажите роль!")]
        public UserRole Role { get; set; }
        protected UserEntityBase()
        {

        }
        protected UserEntityBase(UserEntityBase model)
        {
            FirstName = model.FirstName;
            SecondName = model.SecondName;
            ThirdName = model.ThirdName;
            Role = model.Role;
        }
    }
}
