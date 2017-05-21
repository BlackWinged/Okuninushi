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
using System.IO;
using Google.Cloud.Vision.V1;
using Microsoft.Net.Http.Headers;

namespace Okunishushi.Controllers
{
    public class DocumentAPIController : Controller
    {
        string[] textValues = new string[] { "text/plain", "application/json" };
        public IActionResult UploadDocuments(IFormFile file)
        {
            Stream fileBuffer = file.OpenReadStream();
            var db = new ClassroomContext();
            Document doc = new Document();

            doc.KeyName = WebUtility.UrlEncode(file.FileName);
            doc.FileName = file.FileName;

            if (file.ContentType.Contains("image"))
            {
                Stream fileBuffer2 = new MemoryStream();
                fileBuffer.CopyTo(fileBuffer2);
                fileBuffer.Seek(0, SeekOrigin.Begin);
                IReadOnlyList<EntityAnnotation> resultText = GoogleMLConnector.ReadImageText(fileBuffer);

                fileBuffer = fileBuffer2;
                FileStream fstream = (FileStream) fileBuffer;

                IReadOnlyList<EntityAnnotation> resultLabels = GoogleMLConnector.LabelImage(fstream);
                string lang = "";
                string content = "";
                foreach (var thing in resultText)
                {
                    if (thing.Locale != null)
                    {
                        lang = thing.Locale;
                    }
                    content += thing.Description;
                }
                doc.Content = content;

                fileBuffer = fileBuffer2;

            }
            else if (textValues.Contains<string>(file.ContentType))
            {
                StreamReader reader = new StreamReader(fileBuffer);
                doc.Content = reader.ReadToEnd();
            }
            else
            {
                StreamReader reader = new StreamReader(fileBuffer);
                doc.Content = reader.ReadToEnd();
            }
            var uploadResult = S3Connector.UploadObject(file.FileName, fileBuffer, WebUtility.UrlEncode(file.FileName));


            db.Documents.Add(doc);
            db.SaveChanges();

            return Content(doc.Id.ToString());
        }

        public IActionResult downloadFile()
        {
            return new FileStreamResult(null, new MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = "README.md"
            };
        }

        public IActionResult docdata(int id)
        {
            using (var db = new ClassroomContext())
            {
                var doc = db.Documents.Find(id);

                return Json(doc);
            }

        }

        public IActionResult docdatasave(Document data)
        {

            using (var db = new ClassroomContext())
            {
                db.Update(data);
                db.SaveChanges();
            }

            return Content("success");

        }
        public async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

    }
}
