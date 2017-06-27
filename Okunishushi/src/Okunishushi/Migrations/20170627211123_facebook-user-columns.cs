using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Okunishushi.Migrations
{
    public partial class facebookusercolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacebookComments_FacebookUser_FacebookUserId",
                table: "FacebookComments");

            migrationBuilder.DropForeignKey(
                name: "FK_FacebookGroupPosts_FacebookUser_FacebookUserId",
                table: "FacebookGroupPosts");

            migrationBuilder.DropTable(
                name: "FacebookUser");

            migrationBuilder.DropIndex(
                name: "IX_FacebookGroupPosts_FacebookUserId",
                table: "FacebookGroupPosts");

            migrationBuilder.DropIndex(
                name: "IX_FacebookComments_FacebookUserId",
                table: "FacebookComments");

            migrationBuilder.DropColumn(
                name: "FacebookUserId",
                table: "FacebookGroupPosts");

            migrationBuilder.DropColumn(
                name: "FacebookUserId",
                table: "FacebookComments");

            migrationBuilder.AddColumn<string>(
                name: "faceUserId",
                table: "FacebookGroupPosts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "faceUserName",
                table: "FacebookGroupPosts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "faceUserId",
                table: "FacebookComments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "faceUserName",
                table: "FacebookComments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "faceUserId",
                table: "FacebookGroupPosts");

            migrationBuilder.DropColumn(
                name: "faceUserName",
                table: "FacebookGroupPosts");

            migrationBuilder.DropColumn(
                name: "faceUserId",
                table: "FacebookComments");

            migrationBuilder.DropColumn(
                name: "faceUserName",
                table: "FacebookComments");

            migrationBuilder.AddColumn<int>(
                name: "FacebookUserId",
                table: "FacebookGroupPosts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FacebookUserId",
                table: "FacebookComments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FacebookUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    facebookId = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacebookUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacebookGroupPosts_FacebookUserId",
                table: "FacebookGroupPosts",
                column: "FacebookUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FacebookComments_FacebookUserId",
                table: "FacebookComments",
                column: "FacebookUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FacebookComments_FacebookUser_FacebookUserId",
                table: "FacebookComments",
                column: "FacebookUserId",
                principalTable: "FacebookUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacebookGroupPosts_FacebookUser_FacebookUserId",
                table: "FacebookGroupPosts",
                column: "FacebookUserId",
                principalTable: "FacebookUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
