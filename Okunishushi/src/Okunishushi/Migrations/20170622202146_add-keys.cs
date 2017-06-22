using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Okunishushi.Migrations
{
    public partial class addkeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "facebookId",
                table: "FacebookGroupPosts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacebookUserId",
                table: "FacebookGroupPosts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "permalink_url",
                table: "FacebookGroupPosts",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "facebookId",
                table: "FacebookComments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacebookUserId",
                table: "FacebookComments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ExternalUrl",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_FacebookGroupPosts_facebookId",
                table: "FacebookGroupPosts",
                column: "facebookId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_FacebookComments_facebookId",
                table: "FacebookComments",
                column: "facebookId");

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
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_FacebookGroupPosts_FacebookUser_FacebookUserId",
                table: "FacebookGroupPosts",
                column: "FacebookUserId",
                principalTable: "FacebookUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacebookComments_FacebookUser_FacebookUserId",
                table: "FacebookComments");

            migrationBuilder.DropForeignKey(
                name: "FK_FacebookGroupPosts_FacebookUser_FacebookUserId",
                table: "FacebookGroupPosts");

            migrationBuilder.DropTable(
                name: "FacebookUser");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_FacebookGroupPosts_facebookId",
                table: "FacebookGroupPosts");

            migrationBuilder.DropIndex(
                name: "IX_FacebookGroupPosts_FacebookUserId",
                table: "FacebookGroupPosts");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_FacebookComments_facebookId",
                table: "FacebookComments");

            migrationBuilder.DropIndex(
                name: "IX_FacebookComments_FacebookUserId",
                table: "FacebookComments");

            migrationBuilder.DropColumn(
                name: "FacebookUserId",
                table: "FacebookGroupPosts");

            migrationBuilder.DropColumn(
                name: "permalink_url",
                table: "FacebookGroupPosts");

            migrationBuilder.DropColumn(
                name: "FacebookUserId",
                table: "FacebookComments");

            migrationBuilder.DropColumn(
                name: "ExternalUrl",
                table: "Documents");

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
    }
}
