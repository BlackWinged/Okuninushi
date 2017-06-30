using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Okunishushi.Models
{
    public class ClassroomContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ClassroomDocuments> ClassroomDocuments { get; set; }
        public DbSet<UserClassrooms> StudentClassrooms { get; set; }
        public DbSet<FacebookAuth> FacebookAuthSet { get; set; }
        public DbSet<FacebookGroup> FacebookGroups { get; set; }
        public DbSet<FacebookGroupPost> FacebookGroupPosts { get; set; }
        public DbSet<FacebookComment> FacebookComments { get; set; }
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

            modelBuilder.Entity<FacebookGroupPost>()
                .HasAlternateKey(x => x.facebookId);


        }
    }
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Schoolname { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
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

        public string ClassName { get; set; }
        public string Tags { get; set; }
        public string Description { get; set; }

        public List<UserClassrooms> StudentClassrooms { get; set; }
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
        public string FileName { get; set; }
        public string ExternalUrl { get; set; }
        public string ExternalId { get; set; }
        public string ExternalParentId { get; set; }
        public string KeyName { get; set; }
        public string BucketName { get; set; }
        public string Tags { get; set; }
        public string GoogleTags { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Content { get; set; }

        [NotMapped]
        [JsonIgnore]
        public TagBuilder LinkTags
        {
            get
            {
                if (!string.IsNullOrEmpty(Tags))
                {
                    string[] tags = Tags.Split(',');
                    List<TagBuilder> cleanedTags = new List<TagBuilder>();
                    TagBuilder tagContainer = new TagBuilder("div");
                    tagContainer.AddCssClass("bootstrap-tagsinput");

                    foreach (string tag in tags)
                    {
                        TagBuilder container = new TagBuilder("span");
                        container.AddCssClass("tag");
                        container.AddCssClass("label");
                        container.AddCssClass("label-info");
                        TagBuilder cleanedTag = new TagBuilder("a");
                        cleanedTag.TagRenderMode = TagRenderMode.Normal;
                        cleanedTag.InnerHtml.Append(tag.Trim());
                        cleanedTag.MergeAttribute("href", "/classroom/homeroom/search?tagsOnly=true&search=" + tag.Trim().ToLower());
                        container.InnerHtml.AppendHtml(cleanedTag);
                        cleanedTags.Add(container);
                        tagContainer.InnerHtml.AppendHtml(container);
                    }
                    return tagContainer;

                }
                return null;
            }
        }

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

    public class FacebookAuth
    {
        public int Id { get; set; }
        public string accessToken { get; set; }
        [JsonProperty(propertyName : "userID")]
        public string facebookUserId { get; set; }
        public int expiresIn { get; set; }
        public string signedRequest { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }

    public class FacebookGroup
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonProperty(propertyName: "id")]
        public string facebookId { get; set; }
        public string name { get; set; }

        public int FacebookAuthId { get; set; }
        public FacebookAuth parentAuth { get; set; }

        public int ClassroomId { get; set; }
        public Classroom attachedRoom { get; set; }

        public List<FacebookGroupPost> posts { get; set; }
    }

    public class FacebookGroupPost
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonProperty(propertyName: "id")]
        public string facebookId { get; set; }
        public DateTime updated_time { get; set; }
        public string message { get; set; }

        public int FacebookGroupId { get; set; }
        public FacebookGroup parentGroup { get; set; }

        [NotMapped]
        public FacebookUser from { get; set; }

        [JsonIgnore]
        public string faceUserName {
            get
            {
                return from.name;
            }
        }

        [JsonIgnore]
        public string faceUserId {
            get
            {
                return from.facebookId;
            }
        }

        public string permalink_url { get; set; }
        [JsonIgnore]
        public List<FacebookComment> comments { get; set; }
    }

    public class FacebookComment
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonProperty(propertyName: "id")]
        public string facebookId { get; set; }
        public string message { get; set; }

        public int FacebookGroupPostId { get; set; }
        public FacebookGroupPost parentPost {get; set;}

        [NotMapped]
        public FacebookUser from { get; set; }

        [JsonIgnore]
        public string faceUserName
        {
            get
            {
                return from.name;
            }
        }

        [JsonIgnore]
        public string faceUserId
        {
            get
            {
                return from.facebookId;
            }
        }
    }

    public class FacebookUser
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonProperty(propertyName: "id")]
        public string facebookId { get; set; }
        public string name { get; set; }

    }

}
