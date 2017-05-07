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
            using (var db = new ClassroomContext())
            {
                allUsers = db.Users.Include(u => u.UserRole)
                    .ToList<User>();
                allUsers.ForEach(
                    u => u.UserRole.ForEach(
                        ur => db.Entry(ur)
                        .Reference(r => r.Role).Load()));
            }
            return View(allUsers);
        }

        public IActionResult Roles()
        {
            List<Role> allRoles = new List<Models.Role>();
            using (var db = new ClassroomContext())
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
                using (var db = new ClassroomContext())
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
            using (var db = new ClassroomContext())
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
            using (var db = new ClassroomContext())
            {
                ViewData["roles"] = db.Roles.ToList();
            }
            if (id != null)
            {
                using (var db = new ClassroomContext())
                {
                    var user = db.Users.Include(u => u.UserRole).Single(r => r.Id == id);
                    user.UserRole.ForEach(u => db.Entry(u).Reference(ur => ur.Role).Load());
                    return View(user);
                }
            }
            return View(new User());
        }

        [HttpPost]
        public IActionResult NewUserSave(int? id)
        {
            User user = new User();
            using (var db = new ClassroomContext())
            {
                if (id != null)
                {
                    user = db.Users.Single(r => r.Id == id);
                }
                else
                {
                    db.Users.Add(user);
                }
                user.Username = Request.Form["username"];
                user.Email = Request.Form["email"];
                db.SaveChanges();
                var roles = db.Roles;

                List<string> roleCodes = new List<string>();
                roleCodes.AddRange(Request.Form["roles"].ToString().Split(',').ToList());

                List<Role> resultRoles = roles.Where(r => roleCodes.Where(x => x.ToLower() == r.Slug.ToLower()).Count() > 0).ToList();
                List<UserRole> existingRoles = db.UserRoles.Where(ur => ur.UserId == user.Id).ToList();
                db.UserRoles.RemoveRange(existingRoles);

                foreach (Role role in resultRoles)
                {
                    UserRole userRole = new UserRole();
                    userRole.User = user;
                    userRole.Role = role;

                    db.UserRoles.Add(userRole);
                }
                db.SaveChanges();
            }
            return Redirect("users");
        }

        public IActionResult Documents()
        {
            List<Document> files = GoogleDriveConnector.listFiles();
            return View(files);
        }

        public IActionResult uploadFiles()
        {
            //List<File> files = GoogleDriveConnector.listFiles();
            //return View(files);
            return View();
        }

        public IActionResult DownloadFiles()
        {
            ////GET api/download/12345abc
            //[HttpGet("{id}"]
            //    public async Task<IActionResult> Download(string id)
            //    {
            //        var stream = await { { __get_stream_here__} }
            //        var response = File(stream, "application/octet-stream"); // FileStreamResult
            //        return response;
            //    }  
            //        return Response;
            return null;
        }

        public IActionResult ClassRooms()
        {
            using (var db = new ClassroomContext())
            {
                return View(db.Classrooms.ToList());
            }
        }

        public IActionResult NewClassroom(int id)
        {
            {
                using (var db = new ClassroomContext())
                {
                    if (id != 0)
                    {
                        var classroom = db.Classrooms.Single(r => r.Id == id);
                        return View(classroom);

                    }
                }
            }
            return View(new Classroom());
        }

        public IActionResult SaveNewClassroom(Classroom newRoom)
        {
            var db = new ClassroomContext();
            db.Classrooms.Add(newRoom);
            db.SaveChanges();
            return Redirect("newclassroom/"+ newRoom.Id); 
        }
    }
}
