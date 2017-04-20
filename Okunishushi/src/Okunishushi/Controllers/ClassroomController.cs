using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Okunishushi.Models;

namespace Okunishushi.Controllers
{
    public class ClassroomController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Users()
        {
            List<User> allUsers = new List<Models.User>();
            using (var db = new UserContext())
            {
                allUsers = db.Users.ToList<User>();
            }
            return View(allUsers);
        }

        public IActionResult Roles()
        {
            List<Role> allRoles = new List<Models.Role>();
            using (var db = new UserContext())
            {
                allRoles = db.Roles.ToList<Role>();
            }
            return View(allRoles);
        }

        [HttpGet]
        public IActionResult NewRole(int? id)
        {
            if (id != null)
            {
                using (var db = new UserContext())
                {
                    return View(db.Roles.Single(r => r.Id == id));
                }
            }
            return View(new Role());
        }

        [HttpPost]
        public IActionResult NewRoleSave(int? id)
        {
            Role role = new Role();
            using (var db = new UserContext())
            {
                if (id != null)
                {
                    role = db.Roles.Single(r => r.Id == id);
                }
                else
                {
                    db.Roles.Add(role);
                }
                role.Name = Request.Form["role_name"];
                role.Description = Request.Form["description"];
                db.SaveChanges();
            }
            return Redirect("roles");
        }

        [HttpGet]
        public IActionResult NewUser(int? id)
        {
            if (id != null)
            {
                using (var db = new UserContext())
                {
                    return View(db.Users.Single(r => r.Id == id));
                }
            }
            return View(new User());
        }

        [HttpPost]
        public IActionResult NewUserSave(int? id)
        {
            User user = new User();
            using (var db = new UserContext())
            {
                if (id != null)
                {
                    user = db.Users.Single(r => r.Id == id);
                } else
                {
                    db.Users.Add(user);
                }
                user.Username = Request.Form["username"];
                user.email = Request.Form["email"];
                db.SaveChanges();
            }
            return Redirect("users");
        }
    }
}
