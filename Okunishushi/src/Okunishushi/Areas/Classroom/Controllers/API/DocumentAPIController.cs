using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Okunishushi.Models;
using Microsoft.AspNetCore.Http;

namespace Okunishushi.Controllers
{
    public class DocumentAPIController : Controller
    {
        public IActionResult UploadDocuments(List<IFormFile> file)
        {
            var test = file;
            return Content("");
        }

    }
}
