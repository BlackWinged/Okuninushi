using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Okunishushi.Migrations
{
    public partial class addclassroomtofacebookgroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacebookGroupPosts_FacebookGroups_parentGroupId",
                table: "FacebookGroupPosts");

            migrationBuilder.DropIndex(
                name: "IX_FacebookGroupPosts_parentGroupId",
                table: "FacebookGroupPosts");

            migrationBuilder.DropColumn(
                name: "parentGroupId",
                table: "FacebookGroupPosts");

            migrationBuilder.AddColumn<int>(
                name: "ClassroomId",
                table: "FacebookGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FacebookGroupPosts_FacebookGroupId",
                table: "FacebookGroupPosts",
                column: "FacebookGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FacebookGroups_ClassroomId",
                table: "FacebookGroups",
                column: "ClassroomId");

            migrationBuilder.AddForeignKey(
                name: "FK_FacebookGroups_Classrooms_ClassroomId",
                table: "FacebookGroups",
                column: "ClassroomId",
                principalTable: "Classrooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacebookGroups_Classrooms_ClassroomId",
                table: "FacebookGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_FacebookGroupPosts_FacebookGroups_FacebookGroupId",
                table: "FacebookGroupPosts");

            migrationBuilder.DropIndex(
                name: "IX_FacebookGroupPosts_FacebookGroupId",
                table: "FacebookGroupPosts");

            migrationBuilder.DropIndex(
                name: "IX_FacebookGroups_ClassroomId",
                table: "FacebookGroups");

            migrationBuilder.DropColumn(
                name: "ClassroomId",
                table: "FacebookGroups");

            migrationBuilder.AddColumn<int>(
                name: "parentGroupId",
                table: "FacebookGroupPosts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FacebookGroupPosts_parentGroupId",
                table: "FacebookGroupPosts",
                column: "parentGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_FacebookGroupPosts_FacebookGroups_parentGroupId",
                table: "FacebookGroupPosts",
                column: "parentGroupId",
                principalTable: "FacebookGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
