using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SPM.Data.Migrations
{
    public partial class AddUserPlugins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPlugin_NormalUser_UserId",
                table: "UserPlugin");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserPlugin",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoftwareId",
                table: "UserPlugin",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPlugin_SoftwareId",
                table: "UserPlugin",
                column: "SoftwareId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlugin_Software_SoftwareId",
                table: "UserPlugin",
                column: "SoftwareId",
                principalTable: "Software",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlugin_NormalUser_UserId",
                table: "UserPlugin",
                column: "UserId",
                principalTable: "NormalUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPlugin_Software_SoftwareId",
                table: "UserPlugin");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlugin_NormalUser_UserId",
                table: "UserPlugin");

            migrationBuilder.DropIndex(
                name: "IX_UserPlugin_SoftwareId",
                table: "UserPlugin");

            migrationBuilder.DropColumn(
                name: "SoftwareId",
                table: "UserPlugin");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserPlugin",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlugin_NormalUser_UserId",
                table: "UserPlugin",
                column: "UserId",
                principalTable: "NormalUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
