﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication2.Data.Migrations
{
    public partial class AddFirstAndLastNameToNormalUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "NormalUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "NormalUser",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "NormalUser");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "NormalUser");
        }
    }
}
