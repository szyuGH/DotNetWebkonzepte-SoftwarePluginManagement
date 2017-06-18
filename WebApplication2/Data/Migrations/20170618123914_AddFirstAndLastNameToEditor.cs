using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication2.Data.Migrations
{
    public partial class AddFirstAndLastNameToEditor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "EditorUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "EditorUser",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "EditorUser");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "EditorUser");
        }
    }
}
