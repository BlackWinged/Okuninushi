using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Okunishushi.Models;
using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;
using System.Threading;
using Okunishushi.Connectors;
using System.Net;
using Microsoft.Net.Http.Headers;
using Amazon.S3.Model;

namespace Okunishushi.Controllers
{
    public class GraphApiController : Controller
    {

        public IActionResult saveAuthObject (FacebookAuth auth)
        {

            int? userId = HttpContext.Session.GetInt32("currentuser");
            if (userId != null)
            {
                using (var db = new ClassroomContext())
                {
                    User currentUser = db.Users.Find(userId);
                    auth.User = currentUser;
                    db.FacebookAuthSet.Add(auth);
                    db.SaveChanges();
                }
            }
            return Content("success");
        }

    }
}
