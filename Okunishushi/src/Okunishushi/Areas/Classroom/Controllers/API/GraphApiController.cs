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

        public FileStreamResult downloadFile(string keyName)
        {
             
            GetObjectResponse result = S3Connector.ReadObjectData(WebUtility.UrlEncode(keyName));

            return new FileStreamResult(result.ResponseStream, new MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = keyName,
            };
        }

    }
}
