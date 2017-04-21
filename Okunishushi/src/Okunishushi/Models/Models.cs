﻿using System;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRole)
                .HasForeignKey(bc => bc.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(bc => bc.Role)
                .WithMany(c => c.UserRole)
                .HasForeignKey(bc => bc.RoleId);
        }
    }
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Schoolname { get; set; }
        public string Firstname { get; set; }
        public string Lestname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public string Description { get; set; }
        public List<UserRole> UserRole { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        private string name { get; set; }
        public string Name {
            get
            {
                return name;
            }
            set {
                name = value;
                Slug = System.Net.WebUtility.UrlEncode(value);
            }
        }
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