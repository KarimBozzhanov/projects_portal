using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using projects_portal.Models;
using projects_portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace projects_portal.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext db;
        IWebHostEnvironment app;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            app = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            User user = await db.User.FirstOrDefaultAsync(u => u.Name == User.Identity.Name);
            if (user != null)
            {
                ViewData["role"] = user.Role;
            }
            return View(await db.projects.OrderByDescending(a => a.TimeOfCreating).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create(string userName)
        {
           if (userName != null)
            {
                User user = await db.User.FirstOrDefaultAsync(u => u.Name == userName);
                if (user != null)
                {
                    ViewBag.Group = user.Group;
                    return View();
                }
            }
           return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateModel createModel, IFormFile uploadedPresentation, IFormFile uploadedApk, IFormFile uploadedImage)
        {
            if (uploadedApk != null)
            {
                string presentationPath = "/Presentations/" + uploadedPresentation.FileName;
                string apkPath = "/apk/" + uploadedApk.FileName;
                string imagePath = "/Images/" + uploadedImage.FileName;
                using (var fileStream = new FileStream(app.WebRootPath + presentationPath, FileMode.Create))
                {
                    await uploadedPresentation.CopyToAsync(fileStream);
                }
                using (var fileStream = new FileStream(app.WebRootPath + apkPath, FileMode.Create))
                {
                    await uploadedApk.CopyToAsync(fileStream);
                }
                using (var fileStream = new FileStream(app.WebRootPath + imagePath, FileMode.Create))
                {
                    await uploadedImage.CopyToAsync(fileStream);
                }
                User user = await db.User.FirstOrDefaultAsync(u => u.Name == User.Identity.Name);
                db.projects.Add(new Projects { NameOfProject = createModel.NameOfProject, Name = user.Name, TimeOfCreating = DateTime.Now, Group = user.Group, Description = createModel.Description, PresentationName = uploadedPresentation.FileName, PresentationPath = presentationPath, apkFileName = uploadedApk.FileName, apkFilePath = apkPath, urlGit = createModel.urlGit, siteUrl = createModel.siteUrl, ImageName = uploadedImage.FileName, ImagePath = imagePath });
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            } else
            {
                string presentationPath = "/Presentations/" + uploadedPresentation.FileName;
                string imagePath = "/Images/" + uploadedImage.FileName;
                using (var fileStream = new FileStream(app.WebRootPath + presentationPath, FileMode.Create))
                {
                    await uploadedPresentation.CopyToAsync(fileStream);
                }
                using (var fileStream = new FileStream(app.WebRootPath + imagePath, FileMode.Create))
                {
                    await uploadedImage.CopyToAsync(fileStream);
                }
                User user = await db.User.FirstOrDefaultAsync(u => u.Name == User.Identity.Name);
                db.projects.Add(new Projects { NameOfProject = createModel.NameOfProject, Name = user.Name, TimeOfCreating = DateTime.UtcNow, Group = user.Group, Description = createModel.Description, PresentationName = uploadedPresentation.FileName, PresentationPath = presentationPath, apkFileName = null, apkFilePath = null, urlGit = createModel.urlGit, siteUrl = createModel.siteUrl, ImageName = uploadedImage.FileName, ImagePath = imagePath });
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                User user = await db.User.FirstOrDefaultAsync(u => u.Name == loginModel.Name && u.Password == loginModel.Password);
                if (user != null)
                {
                    await Authenticate(loginModel.Name);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Некорректные логин или пароль");
            }
            return View(loginModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel, string role)
        {
            if (ModelState.IsValid)
            {
                User user = await db.User.FirstOrDefaultAsync(u => u.Name == registerModel.Name);
                if (user == null)
                {
                    db.User.Add(new User { Name = registerModel.Name, Group = registerModel.Group, Password = registerModel.Password, Role = role });
                    await db.SaveChangesAsync();
                    await Authenticate(registerModel.Name);
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин или пароль");
            }
            return View(registerModel);
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> editPost(int? id)
        {
            if (id != null)
            {
                Projects project = await db.projects.FirstOrDefaultAsync(p => p.Id == id);
                if (project != null)
                {
                    return View(project);
                }

            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> editPost(Projects project, IFormFile uploadedApk, IFormFile uploadedImage, IFormFile uploadedPresentation)
        {
            if (uploadedApk != null || uploadedImage != null || uploadedPresentation != null)
            {
                var projectUpdate = db.projects.Where(a => a.Id == project.Id).AsQueryable();
                if (uploadedApk != null)
                {
                    string apkPath = "/apk/" + uploadedApk.FileName;
                    using (var fileStream = new FileStream(app.WebRootPath + apkPath, FileMode.Create))
                    {
                        await uploadedApk.CopyToAsync(fileStream);
                    }
                    projectUpdate.FirstOrDefault().apkFileName = uploadedApk.FileName;
                    projectUpdate.FirstOrDefault().apkFilePath = apkPath;
                }
                if (uploadedImage != null)
                {
                    string imagePath = "/Images/" + uploadedImage.FileName;
                    using (var fileStream = new FileStream(app.WebRootPath + imagePath, FileMode.Create))
                    {
                        await uploadedImage.CopyToAsync(fileStream);
                    }
                    projectUpdate.FirstOrDefault().ImageName = uploadedImage.FileName;
                    projectUpdate.FirstOrDefault().ImagePath = imagePath;
                }
                if (uploadedPresentation != null)
                {
                    string presentationPath = "/Presentations/" + uploadedPresentation.FileName;
                    using (var fileStream = new FileStream(app.WebRootPath + presentationPath, FileMode.Create))
                    {
                        await uploadedPresentation.CopyToAsync(fileStream);
                    }
                    projectUpdate.FirstOrDefault().PresentationName = uploadedPresentation.FileName;
                    projectUpdate.FirstOrDefault().PresentationPath = presentationPath;
                }
                projectUpdate.FirstOrDefault().NameOfProject = project.NameOfProject;
                projectUpdate.FirstOrDefault().Description = project.Description;
                projectUpdate.FirstOrDefault().urlGit = project.urlGit;
                projectUpdate.FirstOrDefault().siteUrl = project.siteUrl;
                db.projects.UpdateRange(projectUpdate);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            } else
            {
                var projects = db.projects.Where(a => a.Id == project.Id).AsQueryable();
                projects.Where(a => a.Id == project.Id).FirstOrDefault().NameOfProject = project.NameOfProject;
                projects.Where(a => a.Id == project.Id).FirstOrDefault().Description = project.Description;
                projects.Where(a => a.Id == project.Id).FirstOrDefault().urlGit = project.urlGit;
                projects.Where(a => a.Id == project.Id).FirstOrDefault().siteUrl = project.siteUrl;
                db.projects.UpdateRange(projects);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [ActionName("projectDelete")]
        public async Task<IActionResult> ConfirmDelete (int? id)
        {
            if (id != null)
            {
                Projects project = await db.projects.FirstOrDefaultAsync(p => p.Id == id);
                if (project != null)
                {
                    return View(project);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> projectDelete (int? id)
        {
            if (id != null)
            {
                Projects project = new Projects { Id = id.Value };
                db.Entry(project).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> favoritePost (string userName, int postId)
        {
            User user = await db.User.FirstOrDefaultAsync(u => u.Name == userName);
            if (user != null)
            {
                db.favorite.Add(new favorite { userId = user.Id, userNameFavorite = userName, postFavoriteId = postId });
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            } 
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> deleteFavoritePost(string userNameFavorite, int? postFavoriteId)
        {
            if (postFavoriteId != null)
            {
                Projects project = await db.projects.FirstOrDefaultAsync(a => a.Id == postFavoriteId);
                if (project != null)
                {
                    User user = await db.User.FirstOrDefaultAsync(u => u.Name == userNameFavorite);
                    if (user != null)
                    {
                        favorite favoritePost = await db.favorite.FirstOrDefaultAsync(f => f.userNameFavorite == user.Name && f.postFavoriteId == project.Id);
                        if (favoritePost != null)
                        {
                            db.Entry(favoritePost).State = EntityState.Deleted;
                            await db.SaveChangesAsync();
                        }
                    }
                }
            }
            return NotFound();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
