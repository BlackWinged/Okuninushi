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

        public IActionResult Users(string role, int id, bool adding = true)
        {
            String search = Request.Query["search[value]"];
            List<User> allUsers = new List<Models.User>();
            int recordsTotal = 0;
            int recordsFiltered = 0;
            using (var db = new ClassroomContext())
            {

                if (!string.IsNullOrEmpty(search))
                {
                    allUsers = db.Users.Where(u => (u.Username.Contains(search) || u.Firstname.Contains(search) || u.Lastname.Contains(search) || u.Schoolname.Contains(search))).Include(u => u.UserRole).ToList<User>();
                }
                else
                {
                    allUsers = db.Users.Include(u => u.UserRole).Include(u=>u.StudentClassrooms)
                        .ToList<User>();
                }
                allUsers.ForEach(
                    u => u.UserRole.ForEach(
                        ur => db.Entry(ur)
                        .Reference(r => r.Role).Load()));

                if (!string.IsNullOrEmpty(role))
                {
                    allUsers.RemoveAll(u => u.UserRole.Where(ur => ur.Role.Slug.ToLower() == role).Count() == 0);
                }
                if (id != 0)
                {
                    if (adding == true)
                    {
                        allUsers.RemoveAll(u => u.StudentClassrooms.Where(sc => sc.ClassroomId == id).Count() > 0);
                    } else
                    {
                        allUsers.RemoveAll(u => u.StudentClassrooms.Where(sc => sc.ClassroomId == id).Count() == 0);
                    }
                }
             recordsTotal = db.Users.Count();
             recordsFiltered = allUsers.Count();
            }
            var data = new List<Object>();

            allUsers.ForEach(u =>
            {
                var user = new List<String>();

                user.Add(u.Id.ToString());
                user.Add(u.Username);
                user.Add(u.Address);
                user.Add(u.City);

                data.Add(user);
            });

            return Json(new { data, recordsTotal, recordsFiltered });
        }

        public IActionResult addUserToClassroom(int id, int userId)
        {
            using (var db = new ClassroomContext())
            {
                UserClassrooms uc = new UserClassrooms();
                uc.ClassroomId = id;
                uc.UserId = userId;
                db.StudentClassrooms.Add(uc);
                db.SaveChanges();
            }
            return Json("fakjea");
        }

    }
}
