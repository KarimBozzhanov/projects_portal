using Microsoft.EntityFrameworkCore;

namespace projects_portal.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AddProject> addProject { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<favorite> favorite { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
