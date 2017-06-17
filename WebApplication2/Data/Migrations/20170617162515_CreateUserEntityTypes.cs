using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication2.Data.Migrations
{
    public partial class CreateUserEntityTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EntityId",
                table: "AspNetUsers",
                nullable: true,
                defaultValue: null
            );
            migrationBuilder.AddColumn<int>(
                name: "EntityType",
                table: "AspNetUsers",
                defaultValue: -1
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "AspNetUsers"
            );
            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "AspNetUsers"
            );
        }
    }
}
