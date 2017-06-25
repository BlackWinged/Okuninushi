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
                    if (db.FacebookGroups.Where(y => y.facebookId == x.facebookId).Count() != 0)
                    {
                        group.isAdded = true;
                    } else
                    {
                        group.isAdded = false;
                    }
                        facebookSet.Groups.Add(group);
                });
                result.Groups.Add(facebookSet);
            }
            return Json(result);
        }

        public IActionResult saveGroupToRoom(int roomId, string groupset, string groupid)
        {
            using (var db = new ClassroomContext())
            {
                if (groupset.ToLower() == "facebook")
                {
                    var faceGroup = new FacebookGroup();
                    var fb = new FacebookConnector(HttpContext.Session);
                    var fbGroups = fb.getGroups();
                    faceGroup = fbGroups.Where(x => x.facebookId == groupid).FirstOrDefault();
                    faceGroup.ClassroomId = roomId;
                    if (db.FacebookGroups.Where(x=>x.facebookId == groupid && x.ClassroomId == roomId).Count() == 0)
                    {
                        db.FacebookAuthSet.Update(faceGroup.parentAuth);
                        db.FacebookGroups.Add(faceGroup);
                    } else
                    {
                        
                        db.FacebookGroups.Remove(db.FacebookGroups.Where(x => x.facebookId == groupid && x.ClassroomId == roomId).First());
                    }
                }

                db.SaveChanges();
            }
            return Content("success");
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
            public Boolean isAdded { get; set; }
        }
    }
}
