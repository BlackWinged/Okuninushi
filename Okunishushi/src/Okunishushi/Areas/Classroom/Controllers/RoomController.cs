using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Okunishushi.Models;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Okunishushi.Connectors;
using Okunishushi.Filters;
using Microsoft.AspNetCore.Http;
using Okunishushi.Helpers;
using Okunishushi.Enums;

namespace Okunishushi.Controllers
{
    [AuthFilter]
    [Area("classroom")]
    public class RoomController : Controller
    {

        public IActionResult Index(int id)
        {
            
            return View();
        }

        public IActionResult Documents(int id)
        {

            return View();
        }

    }
}
