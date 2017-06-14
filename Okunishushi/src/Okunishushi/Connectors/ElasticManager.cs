using Nest;
using Okunishushi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Okunishushi.Connectors
{
    public class ElasticManager
    {
        private static ElasticClient Client { get; set; }
        string elasticAdress = "http://54.93.227.141:9200";

        public ElasticManager()
        {
            if (Client != null)
            {
                var server = new Uri(elasticAdress);
                var settings = new ConnectionSettings(server).DefaultIndex("classroom.search");
                Client = new ElasticClient(settings);
            }
        }

        public void fill()
        {
            using (var db = new ClassroomContext())
            {

                if (Client.IndexExists("classroom.search").Exists)
                    Client.DeleteIndex("classroom.search");

                var result = Client.IndexMany(db.Documents.ToList());
                Client.Index<Document>(null);

                if (!result.IsValid)
                {
                    foreach (var item in result.ItemsWithErrors)
                        Console.WriteLine("Failed to index document {0}: {1}", item.Id, item.Error);

                    Console.WriteLine(result.DebugInformation);
                    Console.Read();
                    Environment.Exit(1);
                }

            }
        }

        public void addDocument(Document doc)
        {
            Client.Index(doc);
        }

        public ISearchResponse<Document> search(string query)
        {
            var result = Client.Search<Document>(s => s
                .Size(25)
                .Query(q => q
                    .QueryString(qs => qs
                        .Query(query)
                    )
                )
            );
            return result;
        }

    }
}
