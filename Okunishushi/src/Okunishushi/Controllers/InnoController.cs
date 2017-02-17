using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Okunishushi.Controllers
{
    public class InnoController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About1()
        {
            return View();
        }

        public IActionResult About2()
        {
            return View();
        }

        public IActionResult About3()
        {
            return View();
        }
        public IActionResult Contacts()
        {
            return View();
        }

    }
}
