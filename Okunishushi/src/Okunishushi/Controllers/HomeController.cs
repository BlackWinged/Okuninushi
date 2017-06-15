using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentEmail.Mailgun;
using FluentEmail.Core;
using Xfinium.Pdf;
using Xfinium.Pdf.Content;
using System.IO;
using Okunishushi.Connectors;
using Google.Cloud.Vision.V1;

namespace Okunishushi.Controllers
{
    public class HomeController: Controller
    {
        public IActionResult Index()
        {
            //string cont = "";
            //Stream str = new FileStream(@"I:\SkyDrive\Nihongo\GENKI I.pdf", System.IO.FileMode.Open, System.IO.FileAccess.Read);
            //PdfFixedDocument doc = new PdfFixedDocument(str);
            //List<PdfVisualImageCollection> img = new List<PdfVisualImageCollection>();
            //foreach (PdfPage page in doc.Pages)
            //{
            //    PdfContentExtractor ce = new PdfContentExtractor(page);
            //    cont += ce.ExtractText();
            //    img.Add(ce.ExtractImages(true));
            //}

            return Redirect("classroom/homeroom");
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
