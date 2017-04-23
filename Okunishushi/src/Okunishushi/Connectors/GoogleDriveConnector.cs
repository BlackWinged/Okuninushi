using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Okunishushi.Connectors
{
    public class GoogleDriveConnector
    {
        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "My Project";

        public static bool uploadFile(string filename, byte[] content, string folder)
        {
            bool result = true;

            return result;
        }

        public static string listFiles()
        {

            var credential = GoogleCredential.FromStream(new System.IO.FileStream("My Project-047741070e6c.json", System.IO.FileMode.Open, System.IO.FileAccess.Read))
        .CreateScoped(Scopes);

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "My Project"
            });
            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            //var test = "";
            // List files.
            createDirectory(service, "etst", "testy", null);
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
            Console.WriteLine("Files:");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }
            Console.Read();
            return "";
        }

        public static Google.Apis.Drive.v3.Data.File createDirectory(DriveService _service, string _title, string _description, string _parent)
        {
            File NewDirectory = null;
            // Create metaData for a new Directory File body = new File();
            File body = new File();
            body.Description = _description;
            body.MimeType = "application/vnd.google-apps.folder";
            try
            {
                FilesResource.CreateRequest request = _service.Files.Create(body);
                NewDirectory = request.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
            return NewDirectory;



        }
    }
}
