using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace projects_portal.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Вы не указали ФИО")]
        public string Name { get; set; }
        public string Group { get; set; }
        public string Curator { get; set; }
        public string Code { get; set; }
        [Required(ErrorMessage = "Вы не указали пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
