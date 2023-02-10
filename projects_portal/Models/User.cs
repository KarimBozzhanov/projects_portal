using System;

namespace projects_portal.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Curator { get; set; }
        public string Email { get; set; }
        public string Instagram { get; set; }
        public string teacherName { get; set; }
    }
}
