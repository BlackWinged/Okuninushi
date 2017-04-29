using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Okunishushi.Models
{
    public class ClassroomContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ClassroomDocuments > ClassroomDocuments { get; set; }
        public DbSet<UserClassrooms> StudentClassrooms { get; set; }
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



            modelBuilder.Entity<UserClassrooms>()
                .HasKey(ur => new { ur.UserId, ur.ClassroomId });

            modelBuilder.Entity<UserClassrooms>()
                .HasOne(sc => sc.Classroom)
                .WithMany(c => c.StudentClassrooms)
                .HasForeignKey(bc => bc.ClassroomId);

            modelBuilder.Entity<UserClassrooms>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.StudentClassrooms)
                .HasForeignKey(sc => sc.UserId);



            modelBuilder.Entity<ClassroomDocuments>()
                .HasKey(doc => new { doc.ClassroomId, doc.DocumentId });

            modelBuilder.Entity<ClassroomDocuments>()
                .HasOne(cd => cd.Classroom)
                .WithMany(c => c.ClassroomDocuments)
                .HasForeignKey(cd => cd.ClassroomId);

            modelBuilder.Entity<ClassroomDocuments>()
                .HasOne(cd => cd.Document)
                .WithMany(d => d.ClassroomsDocuments)
                .HasForeignKey(cd => cd.DocumentId);
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
        public List<UserClassrooms> StudentClassrooms { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        private string name { get; set; }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
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

    public class Classroom
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public List<UserClassrooms> StudentClassrooms { get; set; }
        public List<UserClassrooms> TeacherClassrooms { get; set; }
        public List<ClassroomDocuments> ClassroomDocuments { get; set; }
    }

    public class UserClassrooms
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
    }

    public class Document
    {
        public int Id { get; set; }
        public string GoogleId { get; set; }
        public string FileName { get; set; }
        public List<ClassroomDocuments> ClassroomsDocuments { get; set; }
    }

    public class ClassroomDocuments
    {
        public int Id { get; set; }

        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; }

        public int DocumentId { get; set; }
        public Document Document { get; set; }
    }
}
