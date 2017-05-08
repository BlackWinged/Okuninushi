using Okunishushi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Okunishushi.Helpers
{
    public class SecurityHelper
    {
        public static bool login(string username, string password, ISession session)
        {
            using (var db = new ClassroomContext())
            {
                User checkingUser = db.Users.Where(u => (u.Username == username || u.Email == username) && u.Password == password).SingleOrDefault();
                if (checkingUser != null)
                {
                    session.SetInt32("currentuser", checkingUser.Id);
                    return true;
                }
            }
            return false;
        }

        public static int register(string username, string email, string password, string confpassword)
        {
            if (isRegistrable(username, email, password, confpassword))
            {
                using (var db = new ClassroomContext())
                {
                    User newUser = new User();
                    newUser.Username = username;
                    newUser.Email = email;
                    newUser.Password = password;
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    return newUser.Id;
                }

            }
            return 0;
        }

        public static bool isRegistrable(string username, string email, string password, string confpassword)
        {
            if (password != confpassword)
            {
                return false;
            }
            using (var db = new ClassroomContext())
            {
                if (db.Users.Where(u => (u.Username == username || u.Email == username)).Count() > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static int currentUserId(ISession session)
        {
            if (session.GetInt32("currentuser") != null)
            {
                return (int)session.GetInt32("currentuser");
            } else
            {
                return 0;
            }
        }
    }

}