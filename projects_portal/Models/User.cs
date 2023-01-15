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
    }
}
