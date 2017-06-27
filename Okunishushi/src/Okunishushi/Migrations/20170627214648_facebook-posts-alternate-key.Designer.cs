using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Okunishushi.Models;

namespace Okunishushi.Migrations
{
    [DbContext(typeof(ClassroomContext))]
    [Migration("20170627214648_facebook-posts-alternate-key")]
    partial class facebookpostsalternatekey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Okunishushi.Models.Classroom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClassName");

                    b.Property<string>("Description");

                    b.Property<int>("OwnerId");

                    b.Property<string>("Tags");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Classrooms");
                });

            modelBuilder.Entity("Okunishushi.Models.ClassroomDocuments", b =>
                {
                    b.Property<int>("ClassroomId");

                    b.Property<int>("DocumentId");

                    b.Property<int>("Id");

                    b.HasKey("ClassroomId", "DocumentId");

                    b.HasIndex("DocumentId");

                    b.ToTable("ClassroomDocuments");
                });

            modelBuilder.Entity("Okunishushi.Models.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BucketName");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExternalUrl");

                    b.Property<string>("FileName");

                    b.Property<string>("GoogleTags");

                    b.Property<string>("KeyName");

                    b.Property<string>("Tags");

                    b.HasKey("Id");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("Okunishushi.Models.FacebookAuth", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("UserId");

                    b.Property<string>("accessToken");

                    b.Property<int>("expiresIn");

                    b.Property<string>("facebookUserId");

                    b.Property<string>("signedRequest");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("FacebookAuthSet");
                });

            modelBuilder.Entity("Okunishushi.Models.FacebookComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FacebookGroupPostId");

                    b.Property<string>("faceUserId");

                    b.Property<string>("faceUserName");

                    b.Property<string>("facebookId");

                    b.Property<string>("message");

                    b.HasKey("Id");

                    b.HasIndex("FacebookGroupPostId");

                    b.ToTable("FacebookComments");
                });

            modelBuilder.Entity("Okunishushi.Models.FacebookGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ClassroomId");

                    b.Property<int>("FacebookAuthId");

                    b.Property<string>("facebookId");

                    b.Property<string>("name");

                    b.HasKey("Id");

                    b.HasIndex("ClassroomId");

                    b.HasIndex("FacebookAuthId");

                    b.ToTable("FacebookGroups");
                });

            modelBuilder.Entity("Okunishushi.Models.FacebookGroupPost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FacebookGroupId");

                    b.Property<string>("faceUserId");

                    b.Property<string>("faceUserName");

                    b.Property<string>("facebookId")
                        .IsRequired();

                    b.Property<string>("message");

                    b.Property<string>("permalink_url");

                    b.Property<DateTime>("updated_time");

                    b.HasKey("Id");

                    b.HasAlternateKey("facebookId");

                    b.HasIndex("FacebookGroupId");

                    b.ToTable("FacebookGroupPosts");
                });

            modelBuilder.Entity("Okunishushi.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("Slug");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Okunishushi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<string>("Firstname");

                    b.Property<string>("Lastname");

                    b.Property<string>("Password");

                    b.Property<string>("Schoolname");

                    b.Property<string>("Username");

                    b.Property<string>("Zipcode");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Okunishushi.Models.UserClassrooms", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("ClassroomId");

                    b.Property<int>("Id");

                    b.HasKey("UserId", "ClassroomId");

                    b.HasIndex("ClassroomId");

                    b.ToTable("StudentClassrooms");
                });

            modelBuilder.Entity("Okunishushi.Models.UserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.Property<int>("Id");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Okunishushi.Models.Classroom", b =>
                {
                    b.HasOne("Okunishushi.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Okunishushi.Models.ClassroomDocuments", b =>
                {
                    b.HasOne("Okunishushi.Models.Classroom", "Classroom")
                        .WithMany("ClassroomDocuments")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Okunishushi.Models.Document", "Document")
                        .WithMany("ClassroomsDocuments")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Okunishushi.Models.FacebookAuth", b =>
                {
                    b.HasOne("Okunishushi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Okunishushi.Models.FacebookComment", b =>
                {
                    b.HasOne("Okunishushi.Models.FacebookGroupPost", "parentPost")
                        .WithMany("comments")
                        .HasForeignKey("FacebookGroupPostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Okunishushi.Models.FacebookGroup", b =>
                {
                    b.HasOne("Okunishushi.Models.Classroom", "attachedRoom")
                        .WithMany()
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Okunishushi.Models.FacebookAuth", "parentAuth")
                        .WithMany()
                        .HasForeignKey("FacebookAuthId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Okunishushi.Models.FacebookGroupPost", b =>
                {
                    b.HasOne("Okunishushi.Models.FacebookGroup", "parentGroup")
                        .WithMany("posts")
                        .HasForeignKey("FacebookGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Okunishushi.Models.UserClassrooms", b =>
                {
                    b.HasOne("Okunishushi.Models.Classroom", "Classroom")
                        .WithMany("StudentClassrooms")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Okunishushi.Models.User", "User")
                        .WithMany("StudentClassrooms")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Okunishushi.Models.UserRole", b =>
                {
                    b.HasOne("Okunishushi.Models.Role", "Role")
                        .WithMany("UserRole")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Okunishushi.Models.User", "User")
                        .WithMany("UserRole")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
