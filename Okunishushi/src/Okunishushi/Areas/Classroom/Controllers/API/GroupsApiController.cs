using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Okunishushi.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json;
using Okunishushi.Connectors;

namespace Okunishushi.Controllers
{
    public class GroupsApiController : Controller
    {

        public IActionResult listGroupsBySource()
        {
            var result = new SerializeableGroupList();
            using (var db = new ClassroomContext())
            {
                var fb = new FacebookConnector(HttpContext.Session);
                var facebookSet = new SerializableGroupSet();
                facebookSet.Name = "Facebook";
                var fbGroups = fb.getGroups();
                fbGroups.ForEach(x =>
                {
                    var group = new SerializeableSingleGroup();
                    group.Name = x.name;
                    group.Id = x.facebookId;
                    facebookSet.Groups.Add(group);
                });
                result.Groups.Add(facebookSet);
            }
            return Json(result);
        }

    }

    public class SerializeableGroupList
    {
        public SerializeableGroupList()
        {
            Groups = new List<SerializableGroupSet>();
        }
        public List<SerializableGroupSet> Groups = new List<SerializableGroupSet>();
    }
    public class SerializableGroupSet
    {
        public SerializableGroupSet()
        {
            Groups = new List<SerializeableSingleGroup>();
        }
        public string Name { get; set; }
        public List<SerializeableSingleGroup> Groups { get; set; }
    }
    public class SerializeableSingleGroup
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
