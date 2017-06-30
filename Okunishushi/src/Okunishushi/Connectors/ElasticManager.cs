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
        string elasticAdress = "http://52.59.35.80:9200";

        public ElasticManager()
        {
            if (Client == null)
            {
                var server = new Uri(elasticAdress);
                var settings = new ConnectionSettings(server).DefaultIndex("classroom.search");
                Client = new ElasticClient(settings);
            }
        }

        public void createIndex()
        {
            Client.CreateIndex("classroom.search", i => i
            .Settings(s => s
            .NumberOfShards(2)
            .NumberOfReplicas(0)
            .Analysis(analysis => analysis
                .CharFilters(cf => cf
                    .PatternReplace("dot-splitter", pr => pr
                        .Pattern("\\.")
                        .Replacement(" ")
                    )
                )
                .Tokenizers(tokenizers => tokenizers
                    .Pattern("class-tokenizer", p => p.Pattern(@"\W+"))
                       )
                .TokenFilters(tokenfilters => tokenfilters
                    .WordDelimiter("class-tokenfilter", w => w
                    .SplitOnCaseChange()
                    .PreserveOriginal()
                    .SplitOnNumerics()
                    .GenerateNumberParts(false)
                    .GenerateWordParts()
                        )
                    )
                .Analyzers(analyzers => analyzers
                    .Custom("filename-analyzer", c => c
                        .CharFilters("dot-splitter")
                        .Tokenizer("class-tokenizer")
                        .Filters("class-tokenfilter", "lowercase")
                        )
                        )
                    )
                )
                .Mappings(map => map
                    .Map<Document>(m => m
                        .AutoMap()
                        .Properties(ps => ps
                            .Text(t => t
                                .Name(d => d.FileName)
                                .Analyzer("filename-analyzer")
                                )
                            )
                        )
                    )
            );
        }

        public void fill()
        {
            using (var db = new ClassroomContext())
            {

                if (Client.IndexExists("classroom.search").Exists)
                {
                    Client.DeleteIndex("classroom.search");
                    createIndex();
                }

                var result = Client.IndexMany(db.Documents.ToList());
                result = Client.IndexMany(db.FacebookGroupPosts.ToList());

            }
        }

        public void addDocument(Document doc)
        {
            Client.Index(doc);
        }

        public void addManyDocuments(List<Document> doc)
        {
            Client.IndexMany(doc);
        }

        public ISearchResponse<Document> search(string query)
        {
            var result = Client.Search<Document>(s => s
                .Size(25)
                .Query(q => q
                    .MultiMatch(m => m
                        .Fields(f => f
                            .Field(d => d.FileName)
                            .Field(d => d.Content)
                            .Field(d => d.Tags)
                            .Field(d => d.GoogleTags)
                        )
                        .Query(query)
                    )
                )
               .Highlight(h => h
                .PreTags("<b>")
                .PostTags("</b>")
                .Fields(
                    fs => fs
                    .Field(p => p.Content)
                    .Type("plain")
                    .ForceSource()
                    .FragmentSize(150)
                    .NumberOfFragments(3)
                )
            )
            );
            return result;
        }


        public ISearchResponse<Document> tagSearch(string query)
        {
            var result = Client.Search<Document>(s => s
                .Size(25)
                .Query(q => q
                    .Match(m => m
                            .Field(d => d.Tags)
                            .Query(query)
                        )
                    )
               .Highlight(h => h
                .PreTags("<b>")
                .PostTags("</b>")
                .Fields(
                    fs => fs
                    .Field(p => p.Content)
                    .Type("plain")
                    .ForceSource()
                    .FragmentSize(150)
                    .NumberOfFragments(3)
                )
            )
            );
            return result;
        }
    }
}
