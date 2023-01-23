using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace projects_portal.Models
{
    public class AllModels
    {
        public IEnumerable<Projects> projects { get; set; }
        public IEnumerable<User> users { get; set; }
        public IEnumerable<favorite> favorite { get; set; }
    }
}
