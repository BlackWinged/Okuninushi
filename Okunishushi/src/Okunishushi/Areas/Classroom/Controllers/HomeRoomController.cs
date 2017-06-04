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
    [AuthFilter]
    [Area("classroom")]
    public class HomeRoomController : Controller
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

      

        public IActionResult ClassRooms()
        {
            using (var db = new ClassroomContext())
            {
                return View(db.Classrooms.ToList());
            }
        }

    
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExecuteLogin(string username, string password)
        {
            if (SecurityHelper.login(username, password, HttpContext.Session))
            {
                return Redirect("/classroom/homeroom");
            }
            return View("login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExecuteRegister(User newUser, string confpassword)
        {
            if (SecurityHelper.register(newUser, confpassword) != 0)
            {
                return Redirect("/classroom/homeroom");
            }
            return View("register");
        }
    }
}
