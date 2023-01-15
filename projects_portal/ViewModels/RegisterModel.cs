using System;
using System.ComponentModel.DataAnnotations;

namespace projects_portal.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Вы не указали ФИО")]
        [StringLength(20)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Вы не указали группу")]
        public string Group { get; set; }
        [Required(ErrorMessage = "Вы не указали пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
