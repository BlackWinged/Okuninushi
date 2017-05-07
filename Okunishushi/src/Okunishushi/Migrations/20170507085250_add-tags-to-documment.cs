using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Okunishushi.Migrations
{
    public partial class addtagstodocumment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentClassrooms_Users_StudentId",
                table: "StudentClassrooms");

            migrationBuilder.RenameColumn(
                name: "Lestname",
                table: "Users",
                newName: "Lastname");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "StudentClassrooms",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassroomId1",
                table: "StudentClassrooms",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentClassrooms_ClassroomId1",
                table: "StudentClassrooms",
                column: "ClassroomId1");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentClassrooms_Classrooms_ClassroomId1",
                table: "StudentClassrooms",
                column: "ClassroomId1",
                principalTable: "Classrooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentClassrooms_Users_UserId",
                table: "StudentClassrooms",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentClassrooms_Classrooms_ClassroomId1",
                table: "StudentClassrooms");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentClassrooms_Users_UserId",
                table: "StudentClassrooms");

            migrationBuilder.DropIndex(
                name: "IX_StudentClassrooms_ClassroomId1",
                table: "StudentClassrooms");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ClassroomId1",
                table: "StudentClassrooms");

            migrationBuilder.RenameColumn(
                name: "Lastname",
                table: "Users",
                newName: "Lestname");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "StudentClassrooms",
                newName: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentClassrooms_Users_StudentId",
                table: "StudentClassrooms",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
