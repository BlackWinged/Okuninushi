using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Okunishushi.Migrations
{
    public partial class removekeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_FacebookGroupPosts_facebookId",
                table: "FacebookGroupPosts");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_FacebookComments_facebookId",
                table: "FacebookComments");

            migrationBuilder.AlterColumn<string>(
                name: "facebookId",
                table: "FacebookGroupPosts",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "facebookId",
                table: "FacebookComments",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "facebookId",
                table: "FacebookGroupPosts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "facebookId",
                table: "FacebookComments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_FacebookGroupPosts_facebookId",
                table: "FacebookGroupPosts",
                column: "facebookId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_FacebookComments_facebookId",
                table: "FacebookComments",
                column: "facebookId");
        }
    }
}
