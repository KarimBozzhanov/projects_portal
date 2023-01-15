using System;
using System.ComponentModel.DataAnnotations;

namespace projects_portal.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Вы не указали ФИО")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не введен пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
