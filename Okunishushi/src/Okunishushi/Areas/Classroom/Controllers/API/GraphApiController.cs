using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Okunishushi.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json;

namespace Okunishushi.Controllers
{
    public class GraphApiController : Controller
    {

        public IActionResult saveAuthObject (FacebookAuth auth)
        {

            int? userId = HttpContext.Session.GetInt32("currentuser");
            dynamic stringResponse;
            if (userId != null)
            {
                using (var db = new ClassroomContext())
                {

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://graph.facebook.com/v2.9/");
                        HttpResponseMessage response = client.GetAsync("oauth/access_token?grant_type=fb_exchange_token&client_id=" + Environment.GetEnvironmentVariable("FACEBOOK_APP_ID") + @"&client_secret=" + Environment.GetEnvironmentVariable("FACEBOOK_APP_SECRET") + @"&fb_exchange_token=" + auth.accessToken ).Result;
                        response.EnsureSuccessStatusCode(); // Throw in not success

                        stringResponse = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                    }

                    User currentUser = db.Users.Find(userId);
                    int? currAuth = db.FacebookAuthSet.Where(x => x.facebookUserId == auth.facebookUserId).Select(x => x.Id).SingleOrDefault();
                    auth.accessToken = stringResponse.access_token.ToString();
                    if (currAuth != 0)
                    {
                        auth.Id = (int)currAuth;
                        db.FacebookAuthSet.Update(auth);
                    } else
                    {
                        db.FacebookAuthSet.Add(auth);
                    }
                    auth.User = currentUser;
                    db.SaveChanges();
                }
            }
            return Content("success");
        }

    }
}
