using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Okunishushi.Migrations
{
    public partial class classroomadditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "Classrooms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Classrooms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Classrooms");
        }
    }
}
