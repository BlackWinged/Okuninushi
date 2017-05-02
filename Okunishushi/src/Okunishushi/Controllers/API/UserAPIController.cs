using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Okunishushi.Models;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Drive.v3;
using Okunishushi.Connectors;


namespace Okunishushi.Controllers
{
    public class UserAPIController : Controller
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

        public IActionResult Users(string slug)
        {
            List<User> allUsers = new List<Models.User>();
            using (var db = new ClassroomContext())
            {

                if (!string.IsNullOrEmpty(slug))
                {
                    allUsers = db.Users.Where(u => (u.Firstname.Contains(slug) || u.Lestname.Contains(slug) || u.Schoolname.Contains(slug))).ToList<User>();
                }
                allUsers = db.Users.Include(u => u.UserRole)
                    .ToList<User>();
                allUsers.ForEach(
                    u => u.UserRole.ForEach(
                        ur => db.Entry(ur)
                        .Reference(r => r.Role).Load()));
            }
            return Json(allUsers);
        }

    }
}
