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
        public IActionResult NewUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewUserSave()
        {
            User user = new User();
            user.Username = Request.Form["username"];
            user.email = Request.Form["email"];
            using (var db = new UserContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            return Redirect("users");
        }
    }
}
