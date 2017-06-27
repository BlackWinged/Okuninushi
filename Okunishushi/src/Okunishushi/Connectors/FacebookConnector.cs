using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Okunishushi.Models;
using Microsoft.AspNetCore.Http;
using Okunishushi.Enums;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace Okunishushi.Connectors
{
    public class FacebookConnector
    {
        private static string connectionRoot = "https://graph.facebook.com/v2.9/";
        private FacebookAuth currentUser;

        public FacebookConnector(ISession session)
        {
            using (var db = new ClassroomContext())
            {
                int? currentId = session.GetInt32(EnumStrings.currentuser);
                if (currentId != null)
                {
                    currentUser = db.FacebookAuthSet.Where(x => x.UserId == currentId).FirstOrDefault();
                }
            }
        }

        public List<FacebookGroup> getGroups()
        {
            string requestString = currentUser.facebookUserId + "/groups";
            requestString += "?access_token=" + currentUser.accessToken;
            string resultRaw = fireGetRequest(requestString);
            JToken resultJson = JObject.Parse(resultRaw)["data"];
            List<FacebookGroup> result = JsonConvert.DeserializeObject<List<FacebookGroup>>(resultJson.ToString());
            result.ForEach(x => x.parentAuth = currentUser);
            return result;
        }

        public List<FacebookGroupPost> getGroupFeed(string groupId)
        {
            string requestString = groupId + "/feed?fields=permalink_url,description,message,from,comments{message,from}";
            requestString += "&access_token=" + currentUser.accessToken;
            FacebookGroup group = new FacebookGroup();
            using (var db = new ClassroomContext())
            {
                group = db.FacebookGroups.Where(x => x.facebookId == groupId).SingleOrDefault();
            }
            string resultRaw = fireGetRequest(requestString);
            JToken resultJson = JObject.Parse(resultRaw)["data"];
            List<FacebookGroupPost> result = JsonConvert.DeserializeObject<List<FacebookGroupPost>>(resultJson.ToString());
            result.ForEach(x =>
            {
                x.comments = new List<FacebookComment>();
                x.parentGroup = group;
            });
            foreach (JToken post in JObject.Parse(resultRaw)["data"])
            {
                if (post["comments"] != null)
                {
                    foreach (JToken comment in post["comments"]["data"])
                    {
                        FacebookComment newComment = JsonConvert.DeserializeObject<FacebookComment>(comment.ToString());
                        result.Where(x => x.facebookId == post["id"].ToString()).ToList().ForEach(x => x.comments.Add(newComment));
                    }
                }
            }

            return result;
        }


        public static List<FacebookGroupPost> getGroupFeed(string groupId, FacebookAuth givenAuth = null)
        {
            string requestString = groupId + "/feed?fields=permalink_url,description,message,from,comments{message,from}";
            requestString += "&access_token=" + givenAuth.accessToken;
            FacebookGroup group = new FacebookGroup();
            using (var db = new ClassroomContext())
            {
                group = db.FacebookGroups.Where(x => x.facebookId == groupId).SingleOrDefault();
            }
            string resultRaw = fireGetRequest(requestString);
            JToken resultJson = JObject.Parse(resultRaw)["data"];
            List<FacebookGroupPost> result = JsonConvert.DeserializeObject<List<FacebookGroupPost>>(resultJson.ToString());
            result.ForEach(x =>
            {
                x.comments = new List<FacebookComment>();
                x.parentGroup = group;
            });
            foreach (JToken post in JObject.Parse(resultRaw)["data"])
            {
                if (post["comments"] != null)
                {
                    foreach (JToken comment in post["comments"]["data"])
                    {
                        FacebookComment newComment = JsonConvert.DeserializeObject<FacebookComment>(comment.ToString());
                        result.Where(x => x.facebookId == post["id"].ToString()).ToList().ForEach(x => x.comments.Add(newComment));
                    }
                }
            }

            return result;
        }

        public static string fireGetRequest(string requestString)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(connectionRoot);
                HttpResponseMessage response = client.GetAsync(requestString).Result;
                response.EnsureSuccessStatusCode(); // Throw in not success

                var stringResponse = response.Content.ReadAsStringAsync().Result;
                return stringResponse;
            }
        }

        public static Document convertToDocument(FacebookGroupPost post)
        {
            if (!string.IsNullOrEmpty(post.message))
            {
                Document result = new Document();
                result.ExternalUrl = post.permalink_url;
                string sentence = post.from.name + ": " + post.message + Environment.NewLine;
                result.Content = sentence;
                foreach (FacebookComment comment in post.comments)
                {
                    sentence = comment.from.name + ": " + comment.message + Environment.NewLine;
                }
                return result;
            }
            return null;
        }

        public static void syncGroups()
        {
            using (var db = new ClassroomContext())
            {
                var em = new ElasticManager();
                var finishedGroups = new List<string>();
                List<FacebookGroup> groups = db.FacebookGroups
                                                    //.Include(x => x.posts)
                                                    //    .ThenInclude(post => post.comments)
                                                    //.Include(x => x.posts)
                                                    .Include(x => x.parentAuth)
                                                    .ToList();
                foreach (FacebookGroup group in groups)
                {
                    if (!finishedGroups.Contains(group.facebookId))
                    {
                        try
                        {
                            List<FacebookGroupPost> posts = getGroupFeed(group.facebookId, group.parentAuth);
                            //foreach (var post in posts)
                            //{

                            //        db.FacebookGroupPosts.Add(post);

                            //}
                            db.FacebookGroupPosts.RemoveRange( db.FacebookGroupPosts.ToList() );
                            db.SaveChanges();
                            db.FacebookGroupPosts.AddRange(posts);
                            db.SaveChanges();
                            finishedGroups.Add(group.facebookId);
                        }
                        catch (Exception ex)
                        {
                            //swallow
                        }
                    }

                }
                em.addManyDocuments(db.FacebookGroupPosts.Select(x => convertToDocument(x)).ToList());
            }
        }
    }
}
