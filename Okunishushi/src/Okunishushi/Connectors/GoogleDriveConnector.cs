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
using Okunishushi.Models;

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

        public static List<Document> listFiles()
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

            List<Document> documents = new List<Document>();

            using (var db = new ClassroomContext())
            {
                files.ForEach(x =>
                {
                    var doc = db.Documents.SingleOrDefault(d => d.GoogleId == x.Id);
                    if (doc != null)
                    {
                        documents.Add(doc);
                    }
                    else
                    {
                        doc = new Document();
                        doc.GoogleId = x.Id;
                        doc.FileName = x.Name;
                        documents.Add(doc);
                        db.Documents.Add(doc);
                    }
                });
                db.SaveChanges();
            }
            //FilesResource.ListRequest listRequest2 = service.Permissions.

            return documents;
        }

        public static System.IO.MemoryStream downloadFile(string id)
        {
            setupService();
            // Define parameters of request.
            FilesResource.GetRequest req = service.Files.Get(id);

            //var test = "";
            // List files.
            //createDirectory(service, "etst", "testy", null);
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            req.Download(stream);
            return stream;
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
