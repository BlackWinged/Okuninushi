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

namespace Okunishushi.Controllers
{
    public class DocumentAPIController : Controller
    {
        public IActionResult UploadDocuments(IFormFile file)
        {
            S3Connector.UploadObject(file.FileName, file.OpenReadStream(), WebUtility.UrlEncode(file.FileName)); 
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
