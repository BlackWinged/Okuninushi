using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Okunishushi.Migrations
{
    public partial class modelmodification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentClassrooms_Classrooms_ClassroomId1",
                table: "StudentClassrooms");

            migrationBuilder.DropIndex(
                name: "IX_StudentClassrooms_ClassroomId1",
                table: "StudentClassrooms");

            migrationBuilder.DropColumn(
                name: "ClassroomId1",
                table: "StudentClassrooms");

            migrationBuilder.RenameColumn(
                name: "GoogleId",
                table: "Documents",
                newName: "KeyName");

            migrationBuilder.AddColumn<string>(
                name: "BucketName",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Documents",
                type: "Text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleTags",
                table: "Documents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BucketName",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "GoogleTags",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "KeyName",
                table: "Documents",
                newName: "GoogleId");

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
        }
    }
}
