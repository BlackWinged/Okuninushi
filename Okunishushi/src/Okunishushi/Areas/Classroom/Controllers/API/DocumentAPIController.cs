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
using Xfinium.Pdf;
using Xfinium.Pdf.Content;
using Amazon.S3.Model;

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
                fileBuffer.Seek(0, SeekOrigin.Begin);

                IReadOnlyList<EntityAnnotation> resultLabels = GoogleMLConnector.LabelImage(fileBuffer);
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
                content = "";

                foreach (var thing in resultLabels)
                {
                    content += thing.Description + ", ";
                }
                doc.GoogleTags = content;

                fileBuffer = fileBuffer2;

            }
            else if (textValues.Contains<string>(file.ContentType))
            {
                StreamReader reader = new StreamReader(fileBuffer);
                doc.Content = reader.ReadToEnd();
            }
            else if (file.ContentType.ToLower().Contains("pdf"))
            {
                string cont = "";
                PdfFixedDocument pdfdoc = new PdfFixedDocument(fileBuffer);
                List<PdfVisualImageCollection> img = new List<PdfVisualImageCollection>();
                foreach (PdfPage page in pdfdoc.Pages)
                {
                    PdfContentExtractor ce = new PdfContentExtractor(page);
                    cont += ce.ExtractText();
                    //img.Add(ce.ExtractImages(true));
                }
                doc.Content = cont;
            }
            else
            {
                StreamReader reader = new StreamReader(fileBuffer);
                doc.Content = reader.ReadToEnd();
            }
            var uploadResult = S3Connector.UploadObject(file.FileName, fileBuffer, WebUtility.UrlEncode(file.FileName));

            if (db.Documents.Where(x => x.KeyName == doc.KeyName).Count()  ==  0)
            {
                db.Documents.Add(doc);
                db.SaveChanges();
                ElasticManager em = new ElasticManager();
                em.addDocument(doc);
            }

            return Content(doc.Id.ToString());
        }

        public FileStreamResult downloadFile(string keyName)
        {
             
            GetObjectResponse result = S3Connector.ReadObjectData(WebUtility.UrlEncode(keyName));

            return new FileStreamResult(result.ResponseStream, new MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = keyName,
            };
        }

        public IActionResult docdata(string id)
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
                ElasticManager em = new ElasticManager();
                em.addDocument(data);
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
