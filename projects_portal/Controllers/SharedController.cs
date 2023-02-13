using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projects_portal.Models;
using System.Linq;
using System.Threading.Tasks;

namespace projects_portal.Controllers
{
    public class SharedController : Controller
    {
        private ApplicationDbContext db;

        public SharedController(ApplicationDbContext context)
        {
            db = context;
        }
        public async Task<IActionResult> _Layout()
        {
            var allModels = new AllModels();
            allModels.users = await db.User.Where(u => u.Name == User.Identity.Name).ToListAsync();
            allModels.favorite = await db.favorite.ToListAsync();
            allModels.projects = await db.projects.ToListAsync();
            return View(allModels);
        }

        public async Task<IActionResult> _Layout1()
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = await db.User.FirstOrDefaultAsync(u => u.Name == User.Identity.Name);
                ViewData["role"] = user.Role;
                ViewData["userId"] = user.Id;
            }
            var allModels = new AllModels();
            allModels.users = await db.User.Where(u => u.Name == User.Identity.Name).ToListAsync();
            allModels.favorite = await db.favorite.ToListAsync();
            allModels.projects = await db.projects.ToListAsync();
            return View(allModels);
        }

        public async Task<IActionResult> _Layout2()
        {
            var allModels = new AllModels();
            allModels.users = await db.User.Where(u => u.Name == User.Identity.Name).ToListAsync();
            allModels.favorite = await db.favorite.ToListAsync();
            allModels.projects = await db.projects.ToListAsync();
            return View(allModels);
        }

        public async Task<IActionResult> _Layout3()
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = await db.User.FirstOrDefaultAsync(u => u.Name == User.Identity.Name);
                ViewData["role"] = user.Role;
                ViewData["userId"] = user.Id;
            }
            var allModels = new AllModels();
            allModels.users = await db.User.Where(u => u.Name == User.Identity.Name).ToListAsync();
            allModels.favorite = await db.favorite.ToListAsync();
            allModels.projects = await db.projects.ToListAsync();
            return View(allModels);
        }
    }
}
