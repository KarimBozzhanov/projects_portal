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
        public string UserName { get; set; }
        [Required (ErrorMessage = "Введите номер группы")]
        public string Group { get; set; }
        [Required (ErrorMessage = "Введите описание")]
        public string Description { get; set; }
        public string PresentationName { get; set; }
        public string PresentationFile { get; set; }
        public string apkFileName { get; set; }
        public string apkFilePath { get; set; }
        public string urlGit { get; set; }
        public string siteUrl { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
    }
}
