using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Okunishushi.Migrations
{
    public partial class dbrebuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BucketName = table.Column<string>(nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<string>(nullable: true),
                    ExternalParentId = table.Column<string>(nullable: true),
                    ExternalUrl = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    GoogleTags = table.Column<string>(nullable: true),
                    KeyName = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Firstname = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Schoolname = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Zipcode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClassName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OwnerId = table.Column<int>(nullable: false),
                    Tags = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classrooms_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacebookAuthSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    accessToken = table.Column<string>(nullable: true),
                    expiresIn = table.Column<int>(nullable: false),
                    facebookUserId = table.Column<string>(nullable: true),
                    signedRequest = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacebookAuthSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacebookAuthSet_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassroomDocuments",
                columns: table => new
                {
                    ClassroomId = table.Column<int>(nullable: false),
                    DocumentId = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomDocuments", x => new { x.ClassroomId, x.DocumentId });
                    table.ForeignKey(
                        name: "FK_ClassroomDocuments_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassroomDocuments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentClassrooms",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ClassroomId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClassrooms", x => new { x.UserId, x.ClassroomId });
                    table.ForeignKey(
                        name: "FK_StudentClassrooms_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_StudentClassrooms_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FacebookGroups",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    ClassroomId = table.Column<int>(nullable: false),
                    FacebookAuthId = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacebookGroups", x => x.id);
                    table.ForeignKey(
                        name: "FK_FacebookGroups_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacebookGroups_FacebookAuthSet_FacebookAuthId",
                        column: x => x.FacebookAuthId,
                        principalTable: "FacebookAuthSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FacebookGroupPosts",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    FacebookGroupId = table.Column<int>(nullable: false),
                    message = table.Column<string>(nullable: true),
                    parentGroupid = table.Column<string>(nullable: true),
                    permalink_url = table.Column<string>(nullable: true),
                    updated_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacebookGroupPosts", x => x.id);
                    table.ForeignKey(
                        name: "FK_FacebookGroupPosts_FacebookGroups_parentGroupid",
                        column: x => x.parentGroupid,
                        principalTable: "FacebookGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FacebookComments",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    FacebookGroupPostId = table.Column<int>(nullable: false),
                    message = table.Column<string>(nullable: true),
                    parentPostid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacebookComments", x => x.id);
                    table.ForeignKey(
                        name: "FK_FacebookComments_FacebookGroupPosts_parentPostid",
                        column: x => x.parentPostid,
                        principalTable: "FacebookGroupPosts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_OwnerId",
                table: "Classrooms",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomDocuments_DocumentId",
                table: "ClassroomDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_FacebookAuthSet_UserId",
                table: "FacebookAuthSet",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FacebookComments_parentPostid",
                table: "FacebookComments",
                column: "parentPostid");

            migrationBuilder.CreateIndex(
                name: "IX_FacebookGroups_ClassroomId",
                table: "FacebookGroups",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_FacebookGroups_FacebookAuthId",
                table: "FacebookGroups",
                column: "FacebookAuthId");

            migrationBuilder.CreateIndex(
                name: "IX_FacebookGroupPosts_parentGroupid",
                table: "FacebookGroupPosts",
                column: "parentGroupid");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClassrooms_ClassroomId",
                table: "StudentClassrooms",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassroomDocuments");

            migrationBuilder.DropTable(
                name: "FacebookComments");

            migrationBuilder.DropTable(
                name: "StudentClassrooms");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "FacebookGroupPosts");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "FacebookGroups");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "FacebookAuthSet");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
