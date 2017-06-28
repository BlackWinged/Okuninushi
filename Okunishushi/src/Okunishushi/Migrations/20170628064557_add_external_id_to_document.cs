using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Okunishushi.Migrations
{
    public partial class add_external_id_to_document : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "faceUserId",
                table: "FacebookGroupPosts");

            migrationBuilder.DropColumn(
                name: "faceUserName",
                table: "FacebookGroupPosts");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Documents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Documents");

            migrationBuilder.AddColumn<string>(
                name: "faceUserId",
                table: "FacebookGroupPosts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "faceUserName",
                table: "FacebookGroupPosts",
                nullable: true);
        }
    }
}
