using System;

namespace projects_portal.Models
{
    public class Projects
    {
        public int Id { get; set; }
        public string NameOfProject { get; set; }
        public string Name { get; set; }
        public DateTime TimeOfCreating { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public string PresentationName { get; set; }
        public string PresentationPath { get; set; }
        public string apkFileName { get; set; }
        public string apkFilePath { get; set; }
        public string urlGit { get; set; }
        public string siteUrl { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
    }
}
