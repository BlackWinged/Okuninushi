using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Okunishushi.Models;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Drive.v3;
using Okunishushi.Connectors;
using Okunishushi.Filters;
using Microsoft.AspNetCore.Http;
using Okunishushi.Helpers;

namespace Okunishushi.Controllers
{
    //[AuthFilter]
    [Area("classroom")]
    public class AdminController : Controller
    {



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
        public IActionResult NewUserSave(User user)
        {
            if (!SecurityHelper.isRegistrable(user.Username, user.Email, user.Password, Request.Form["confpassword"]) )
            {
                return Redirect("/classroom/admin/newuser");
            }
            using (var db = new ClassroomContext())
            {
                if (user.Id == 0)
                {
                    db.Users.Add(user);
                } else
                {
                    db.Users.Update(user);
                }
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

        public IActionResult Documents(string id)
        {

            return View("Documents", id);
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
            newRoom.OwnerId = SecurityHelper.currentUserId(HttpContext.Session);
            if (newRoom.Id == 0)
            {
                db.Classrooms.Add(newRoom);
            }
            else
            {
                db.Classrooms.Update(newRoom);
            }
            db.SaveChanges();
            return Redirect(Url.Action("NewClassroom", "Admin") + "/" + newRoom.Id);
        }

    }
}
