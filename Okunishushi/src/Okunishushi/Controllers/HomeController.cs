using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentEmail.Mailgun;
using FluentEmail.Core;


namespace Okunishushi.Controllers
{
    public class HomeController: Controller
    {
        public IActionResult Index()
        {
            return View("Homepage");
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

        public IActionResult ContactMe(string name, string email, string phone, string message)
        {
            var sender = new MailgunSender(
                "sandbox80f23c652d1041e2b2ab7f16a1ec084b.mailgun.org", // Mailgun Domain
                "key-fffd1ed5730a7c8521ada4bf947be09a" // Mailgun API Key
            );
            Email.DefaultSender = sender;

            var emailSend = Email
                .From(email)
                .To("lovro.gamulin@gmail.com")
                .Subject("Poruka sa sajta")
                .Body(message + "<br/> Ime: " + name + " <br/> Telefonski broj: " + phone);

            var response = emailSend.SendAsync();
            return Content(response.ToString());
        }
    }
}
