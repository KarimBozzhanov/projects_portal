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
using PagedList.Mvc;
using PagedList;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis;

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

        public async Task<IActionResult> Index(string searchText, string category)
        {
            User user = await db.User.FirstOrDefaultAsync(u => u.Name == User.Identity.Name);
            if (user != null)
            {
                ViewData["role"] = user.Role;
            }
            var allModels = new AllModels();
            if (!String.IsNullOrEmpty(category))
            {
                if (!String.IsNullOrEmpty(searchText))
                {
                    allModels.projects = db.projects.OrderByDescending(p => p.TimeOfCreating).Where(p => (p.Name.Contains(searchText) || p.NameOfProject.Contains(searchText) || p.Group.Contains(searchText)) && p.categoryType == category).ToList();
                } else
                {
                    allModels.projects = db.projects.OrderByDescending(a => a.TimeOfCreating).Where(p => p.categoryType == category).ToList();
                }
            } else
            {
                if (!String.IsNullOrEmpty(searchText))
                {
                    allModels.projects = db.projects.OrderByDescending(p => p.TimeOfCreating).Where(p => p.Name.Contains(searchText) || p.NameOfProject.Contains(searchText) || p.Group.Contains(searchText)).ToList();
                } else
                {
                    allModels.projects = db.projects.OrderByDescending(a => a.TimeOfCreating).ToList();
                }
            }
            allModels.favorite = db.favorite.Where(f => f.userNameFavorite == User.Identity.Name).ToList();
            allModels.users = db.User.ToList();
            return View(allModels);
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
        public async Task<IActionResult> Create(CreateModel createModel, IFormFile uploadedPresentation, IFormFile uploadedApk, IFormFile uploadedImage, String createCategory)
        {
            if (uploadedApk != null)
            {
                if (uploadedImage != null)
                {
                    if (uploadedPresentation != null)
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
                        db.projects.Add(new Projects { 
                            userId = user.Id, NameOfProject = createModel.NameOfProject, Name = user.Name, 
                            TimeOfCreating = DateTime.Now, Group = user.Group, 
                            Description = createModel.Description, 
                            PresentationName = uploadedPresentation.FileName, 
                            PresentationPath = presentationPath, apkFileName = uploadedApk.FileName, 
                            apkFilePath = apkPath, urlGit = createModel.urlGit, 
                            siteUrl = createModel.siteUrl, ImageName = uploadedImage.FileName, 
                            ImagePath = imagePath, categoryType = createCategory });
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    } else
                    {
                        string apkPath = "/apk/" + uploadedApk.FileName;
                        string imagePath = "/Images/" + uploadedImage.FileName;
                        using (var fileStream = new FileStream(app.WebRootPath + apkPath, FileMode.Create))
                        {
                            await uploadedApk.CopyToAsync(fileStream);
                        }
                        using (var fileStream = new FileStream(app.WebRootPath + imagePath, FileMode.Create))
                        {
                            await uploadedImage.CopyToAsync(fileStream);
                        }
                        User user = await db.User.FirstOrDefaultAsync(u => u.Name == User.Identity.Name);
                        db.projects.Add(new Projects { 
                            userId = user.Id, NameOfProject = createModel.NameOfProject, Name = user.Name, 
                            TimeOfCreating = DateTime.Now, Group = user.Group, 
                            Description = createModel.Description, PresentationName = null, 
                            PresentationPath = null, apkFileName = uploadedApk.FileName, 
                            apkFilePath = apkPath, urlGit = createModel.urlGit, 
                            siteUrl = createModel.siteUrl, ImageName = uploadedImage.FileName, 
                            ImagePath = imagePath, categoryType = createCategory
                        });
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                } else
                {
                    if (uploadedPresentation != null)
                    {
                        string presentationPath = "/Presentations/" + uploadedPresentation.FileName;
                        string apkPath = "/apk/" + uploadedApk.FileName;
                        using (var fileStream = new FileStream(app.WebRootPath + presentationPath, FileMode.Create))
                        {
                            await uploadedPresentation.CopyToAsync(fileStream);
                        }
                        using (var fileStream = new FileStream(app.WebRootPath + apkPath, FileMode.Create))
                        {
                            await uploadedApk.CopyToAsync(fileStream);
                        }
                        User user = await db.User.FirstOrDefaultAsync(u => u.Name == User.Identity.Name);
                        db.projects.Add(new Projects { 
                            userId = user.Id, NameOfProject = createModel.NameOfProject, Name = user.Name, 
                            TimeOfCreating = DateTime.Now, Group = user.Group, 
                            Description = createModel.Description, 
                            PresentationName = uploadedPresentation.FileName, 
                            PresentationPath = presentationPath, apkFileName = uploadedApk.FileName, 
                            apkFilePath = apkPath, urlGit = createModel.urlGit, 
                            siteUrl = createModel.siteUrl, ImageName = null, ImagePath = null, categoryType = createCategory
                        });
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    } else
                    {
                        string apkPath = "/apk/" + uploadedApk.FileName;
                        using (var fileStream = new FileStream(app.WebRootPath + apkPath, FileMode.Create))
                        {
                            await uploadedApk.CopyToAsync(fileStream);
                        }
                        User user = await db.User.FirstOrDefaultAsync(u => u.Name == User.Identity.Name);
                        db.projects.Add(new Projects {
                            userId = user.Id, NameOfProject = createModel.NameOfProject, Name = user.Name, 
                            TimeOfCreating = DateTime.Now, Group = user.Group, 
                            Description = createModel.Description, PresentationName = null, 
                            PresentationPath = null, apkFileName = uploadedApk.FileName, 
                            apkFilePath = apkPath, urlGit = createModel.urlGit, 
                            siteUrl = createModel.siteUrl, ImageName = null, ImagePath = null, categoryType = createCategory
                        });
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
            } else
            {
                if (uploadedImage != null)
                {
                    if (uploadedPresentation != null)
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
                        db.projects.Add(new Projects 
                        { 
                            userId = user.Id, NameOfProject = createModel.NameOfProject, Name = user.Name, 
                            TimeOfCreating = DateTime.Now, Group = user.Group, 
                            Description = createModel.Description, 
                            PresentationName = uploadedPresentation.FileName, 
                            PresentationPath = presentationPath, apkFileName = null, 
                            apkFilePath = null, urlGit = createModel.urlGit, 
                            siteUrl = createModel.siteUrl, ImageName = uploadedImage.FileName, 
                            ImagePath = imagePath, categoryType = createCategory
                        });
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    } else
                    {
                        string imagePath = "/Images/" + uploadedImage.FileName;
                        using (var fileStream = new FileStream(app.WebRootPath + imagePath, FileMode.Create))
                        {
                            await uploadedImage.CopyToAsync(fileStream);
                        }
                        User user = await db.User.FirstOrDefaultAsync(u => u.Name == User.Identity.Name);
                        db.projects.Add(new Projects { 
                            userId = user.Id, NameOfProject = createModel.NameOfProject, 
                            Name = user.Name, TimeOfCreating = DateTime.Now, 
                            Group = user.Group, Description = createModel.Description, 
                            PresentationName = null, PresentationPath = null, apkFileName = null, 
                            apkFilePath = null, urlGit = createModel.urlGit, 
                            siteUrl = createModel.siteUrl, ImageName = uploadedImage.FileName, 
                            ImagePath = imagePath, categoryType = createCategory
                        });
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                } else
                {
                    if (uploadedPresentation != null)
                    {
                        string presentationPath = "/Presentations/" + uploadedPresentation.FileName;
                        using (var fileStream = new FileStream(app.WebRootPath + presentationPath, FileMode.Create))
                        {
                            await uploadedPresentation.CopyToAsync(fileStream);
                        }
                        User user = await db.User.FirstOrDefaultAsync(u => u.Name == User.Identity.Name);
                        db.projects.Add(new Projects { 
                            userId = user.Id, NameOfProject = createModel.NameOfProject, 
                            Name = user.Name, TimeOfCreating = DateTime.Now, 
                            Group = user.Group, Description = createModel.Description, 
                            PresentationName = uploadedPresentation.FileName, 
                            PresentationPath = presentationPath, apkFileName = null, 
                            apkFilePath = null, urlGit = createModel.urlGit, 
                            siteUrl = createModel.siteUrl, ImageName = null, ImagePath = null, categoryType = createCategory
                        });
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    } else
                    {
                        User user = await db.User.FirstOrDefaultAsync(u => u.Name == User.Identity.Name);
                        db.projects.Add(new Projects { 
                            userId = user.Id, NameOfProject = createModel.NameOfProject, 
                            Name = user.Name, TimeOfCreating = DateTime.Now, 
                            Group = user.Group, Description = createModel.Description, 
                            PresentationName = null, PresentationPath = null, apkFileName = null, 
                            apkFilePath = null, urlGit = createModel.urlGit, 
                            siteUrl = createModel.siteUrl, ImageName = null, ImagePath = null, categoryType = createCategory
                        });
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
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
                User user = await db.User.FirstOrDefaultAsync(u => u.Name.Equals(loginModel.Name) && u.Password.Equals(loginModel.Password));
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
                    if (registerModel.Password == registerModel.ConfirmPassword)
                    {
                        if (role.Equals("teacher"))
                        {
                            if (registerModel.Code.Equals("Te@chers1NTC@2015+-"))
                            {
                                db.User.Add(new User { Name = registerModel.Name, Group = null, Curator = null, Password = registerModel.Password, Role = role });
                                await db.SaveChangesAsync();
                                await Authenticate(registerModel.Name);
                                return RedirectToAction("Index");
                            } else
                            {
                                ModelState.AddModelError("", "Проверочный код неправильный");
                            }
                        }
                        else
                        {
                            if (registerModel.Code == null)
                            {
                                db.User.Add(new User { Name = registerModel.Name, Group = registerModel.Group, Curator = registerModel.Curator, Password = registerModel.Password, Role = role });
                                await db.SaveChangesAsync();
                                await Authenticate(registerModel.Name);
                                return RedirectToAction("Index");
                            }
                        }
                    } else
                    {
                        ModelState.AddModelError("", "Пароли не совпадают");
                    }
                }
                else
                {
                    ModelState.AddModelError("","Пользователь с данным именем уже зарегистрирован");
                }
            } else
            {
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

        [HttpGet]
        public async Task<IActionResult> changeUser(int? id) 
        {
            if (id != null)
            {
                User user = await db.User.FirstOrDefaultAsync(u => u.Id == id);
                if (user != null)
                {
                    ViewData["role"] = user.Role;
                    return View(user);
                } 
            }
            return NotFound();
;        }

        [HttpPost]
        public async Task<IActionResult> changeUser(User user)
        {
            db.User.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }   

        public JsonResult userInfo(int? id)
        {
            Projects project = db.projects.FirstOrDefault(p => p.Id == id);
            if (project != null)
            {
                User user = db.User.FirstOrDefault(u => u.Id == project.userId);
                Console.WriteLine(user.Group);
                var userJson = JsonConvert.SerializeObject(user);
                return Json(userJson);
            }
            return Json(NotFound());
        }

        public JsonResult getMyInfo(int? id)
        {
            if (id != null)
            {
                User user = db.User.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    var userJson = JsonConvert.SerializeObject(user);
                    return Json(userJson);
                }
            }
            return Json(NotFound());
        }

        public async Task<IActionResult> studentsPage (string teacherName, string studentSearch)
        {
            var allModels = new AllModels();
            if (!String.IsNullOrEmpty(teacherName))
            {
                if (studentSearch != null)
                {
                    allModels.users = await db.User.Where(p => (p.Name.Contains(studentSearch) || p.Group.Contains(studentSearch)) && p.teacherName == teacherName).ToListAsync();
                }
                else
                {
                    allModels.users = await db.User.Where(s => s.Role == "student" && s.teacherName == teacherName).ToListAsync();
                }
                allModels.projects = db.projects.ToList();
                allModels.favorite = db.favorite.ToList();
                return View(allModels);
            } else
            {
                if (studentSearch != null)
                {
                    allModels.users = await db.User.Where(p => (p.Name.Contains(studentSearch) || p.Group.Contains(studentSearch))).ToListAsync();
                }
                else
                {
                    allModels.users = await db.User.Where(s => s.Role == "student").ToListAsync();
                }
                allModels.projects = db.projects.ToList();
                allModels.favorite = db.favorite.ToList();
                return View(allModels);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> addStudent(int? id)
        {
            if (id != null)
            {
                var userUpdate = db.User.Where(u => u.Id == id).AsQueryable();
                if (userUpdate != null)
                {
                    userUpdate.FirstOrDefault().teacherName = User.Identity.Name;
                    db.User.UpdateRange(userUpdate);
                    await db.SaveChangesAsync();
                    return RedirectToAction("studentsPage");
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> deleteStudent(int? id)
        {
            if (id != null)
            {
                var userUpdate = db.User.Where(u => u.Id == id).AsQueryable();
                if (userUpdate != null)
                {
                    userUpdate.FirstOrDefault().teacherName = null;
                    db.User.UpdateRange(userUpdate);
                    await db.SaveChangesAsync();
                    return RedirectToAction("studentsPage");
                }
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> studentProjects(int studentId, string projectSearch)
        {
            var allModels = new AllModels();
            User student = await db.User.FirstOrDefaultAsync(s => s.Id == studentId);
            User user = await db.User.FirstOrDefaultAsync(u => u.Name == User.Identity.Name);
            ViewData["role"] = user.Role;
            if (!String.IsNullOrEmpty(projectSearch))
            {
                if (student != null)
                {
                    allModels.projects = await db.projects.Where(p => p.Name == student.Name && (p.Name.Contains(projectSearch) || p.Group.Contains(projectSearch) || p.NameOfProject.Contains(projectSearch)) ).ToListAsync();
                    allModels.favorite = await db.favorite.ToListAsync();
                    allModels.users = await db.User.ToListAsync();
                    return View(allModels);
                }
            } else
            {
                if (student != null)
                {
                    allModels.projects = await db.projects.Where(p => p.Name == student.Name).ToListAsync();
                    allModels.favorite = await db.favorite.ToListAsync();
                    allModels.users = await db.User.ToListAsync();
                    return View(allModels);
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
