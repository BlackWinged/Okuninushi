using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Okunishushi.Migrations
{
    public partial class facebookpostsalternatekey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "facebookId",
                table: "FacebookGroupPosts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_FacebookGroupPosts_facebookId",
                table: "FacebookGroupPosts",
                column: "facebookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_FacebookGroupPosts_facebookId",
                table: "FacebookGroupPosts");

            migrationBuilder.AlterColumn<string>(
                name: "facebookId",
                table: "FacebookGroupPosts",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
