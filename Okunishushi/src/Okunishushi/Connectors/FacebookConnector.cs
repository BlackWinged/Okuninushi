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

namespace Okunishushi.Connectors
{
    public class FacebookConnector
    {
        private string connectionRoot = "https://graph.facebook.com/v2.9/";
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

        public List<FacebookGroupPost> getGroupFeed(string groupId, FacebookAuth givenAuth = null)
        {
            string requestString = groupId + "/feed?fields=permalink_url,description,message,from,comments{message,from}";
            if (givenAuth != null)
            {
                requestString += "&access_token=" + givenAuth.accessToken;
            }
            else
            {
                requestString += "&access_token=" + currentUser.accessToken;
            }
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

        public string fireGetRequest(string requestString)
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

        public Document convertToDocument(FacebookGroupPost post)
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

        public void syncGroups()
        {
            using (var db = new ClassroomContext())
            {
                List<FacebookGroup> groups = db.FacebookGroups.ToList();
                foreach (FacebookGroup group in groups)
                {
                    try
                    {
                        List<FacebookGroupPost> posts = getGroupFeed(group.facebookId, group.parentAuth);
                    }
                    catch (Exception ex)
                    {
                        //swallow
                    }

                }

            }
        }
    }
}
