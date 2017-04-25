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
using Google.Apis.Requests;

namespace Okunishushi.Connectors
{
    public class GoogleDriveConnector
    {
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "My Project";
        static DriveService service;

        public static void setupService()
        {
            if (service == null)
            {
                var credential = GoogleCredential.FromStream(new System.IO.FileStream("clientsettings.json", System.IO.FileMode.Open, System.IO.FileAccess.Read)).CreateScoped(Scopes);
                // Create Drive API service.
                service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "My Project"
                });

            }

        }

        public static bool uploadFile(string filename, byte[] content, string folder)
        {
            bool result = true;

            return result;
        }

        public static List<File> listFiles()
        {
            setupService();
            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            //var test = "";
            // List files.
            //createDirectory(service, "etst", "testy", null);
            List<File> files = (List<File>)listRequest.Execute().Files;


            //FilesResource.ListRequest listRequest2 = service.Permissions.

            return files;
        }

        public static File createDirectory(DriveService _service, string _title, string _description, string _parent)
        {
            setupService();
            File NewDirectory = null;
            // Create metaData for a new Directory File body = new File();
            File body = new File();
            body.Name = _title;
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

        public static void shareFolder(string email, File file)
        {
            setupService();
            var batch = new BatchRequest(service);

            BatchRequest.OnResponse<Permission> callback = delegate (Permission permission, RequestError error, int index, System.Net.Http.HttpResponseMessage message)
            {
                if (error != null)
                {
                    // Handle error
                    Console.WriteLine(error.Message);
                }
                else
                {
                    Console.WriteLine("Permission ID: " + permission.Id);
                }
            };

            //foreach (File f in files)
            //{

            //    Permission userPermission = new Permission()
            //    {
            //        Type = "user",
            //        Role = "writer",
            //        EmailAddress = "learnrooms@gmail.com"
            //    };
            //    var request = service.Permissions.Create(userPermission, f.Id);
            //    request.Fields = "id";
            //    batch.Queue(request, callback);
            //}

            var task = batch.ExecuteAsync();
        }
    }
}
