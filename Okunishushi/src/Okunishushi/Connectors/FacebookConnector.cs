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

        public List<FaceboookGroup> getGroups()
        {
            string requestString = currentUser.facebookUserId + "/groups";
            requestString += "?access_token=" + currentUser.accessToken;
            string resultRaw = fireGetRequest(requestString);
            JToken resultJson = JObject.Parse(resultRaw)["data"];
            List<FaceboookGroup> result = JsonConvert.DeserializeObject<List<FaceboookGroup>>(resultJson.ToString());

            return result;
        }

        public List<FacebookGroupPost> getGroupFeed(string groupId)
        {
            string requestString = groupId + "/feed";
            requestString += "?access_token=" + currentUser.accessToken;
            string resultRaw = fireGetRequest(requestString);
            JToken resultJson = JObject.Parse(resultRaw)["data"];
            List<FacebookGroupPost> result = JsonConvert.DeserializeObject<List<FacebookGroupPost>>(resultJson.ToString());

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
            return null;
        }
    }
}
