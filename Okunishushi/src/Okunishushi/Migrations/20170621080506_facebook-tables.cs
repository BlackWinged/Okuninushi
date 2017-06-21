using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Okunishushi.Migrations
{
    public partial class facebooktables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "FacebookGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FacebookAuthId = table.Column<int>(nullable: false),
                    facebookId = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacebookGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacebookGroups_FacebookAuthSet_FacebookAuthId",
                        column: x => x.FacebookAuthId,
                        principalTable: "FacebookAuthSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacebookGroupPosts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FacebookGroupId = table.Column<int>(nullable: false),
                    facebookId = table.Column<string>(nullable: true),
                    message = table.Column<string>(nullable: true),
                    parentGroupId = table.Column<int>(nullable: true),
                    updated_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacebookGroupPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacebookGroupPosts_FacebookGroups_parentGroupId",
                        column: x => x.parentGroupId,
                        principalTable: "FacebookGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FacebookComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FacebookGroupPostId = table.Column<int>(nullable: false),
                    facebookId = table.Column<string>(nullable: true),
                    message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacebookComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacebookComments_FacebookGroupPosts_FacebookGroupPostId",
                        column: x => x.FacebookGroupPostId,
                        principalTable: "FacebookGroupPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacebookAuthSet_UserId",
                table: "FacebookAuthSet",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FacebookComments_FacebookGroupPostId",
                table: "FacebookComments",
                column: "FacebookGroupPostId");

            migrationBuilder.CreateIndex(
                name: "IX_FacebookGroupPosts_parentGroupId",
                table: "FacebookGroupPosts",
                column: "parentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FacebookGroups_FacebookAuthId",
                table: "FacebookGroups",
                column: "FacebookAuthId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacebookComments");

            migrationBuilder.DropTable(
                name: "FacebookGroupPosts");

            migrationBuilder.DropTable(
                name: "FacebookGroups");

            migrationBuilder.DropTable(
                name: "FacebookAuthSet");
        }
    }
}
