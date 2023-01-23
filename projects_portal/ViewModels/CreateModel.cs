using System;
using System.ComponentModel.DataAnnotations;

namespace projects_portal.ViewModels
{
    public class CreateModel
    {
        [Required(ErrorMessage = "Вы не ввели имя проекта")]
        public string NameOfProject { get; set; }
        [Required(ErrorMessage = "Введите ФИО")]
        public string Name { get; set; }
        [Required (ErrorMessage = "Введите номер группы")]
        public string Group { get; set; }
        [Required (ErrorMessage = "Введите описание")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Выберете презентацию")]
        public string PresentationName { get; set; }
        public string PresentationPath { get; set; }
        public string apkFileName { get; set; }
        public string apkFilePath { get; set; }
        public string urlGit { get; set; }
        public string siteUrl { get; set; }
        [Required(ErrorMessage = "Выберете изображение")]
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
    }
}
