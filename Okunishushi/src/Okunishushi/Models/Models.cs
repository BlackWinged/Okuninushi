using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Okunishushi.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        //public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING"));
        }
    }
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string email { get; set; }
        public List<UserRole> UserRole { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<UserRole> UserRole { get; set; }
    }

    public class UserRole
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
