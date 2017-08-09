using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication2.Data.Migrations
{
    public partial class AddFirstAndLastNameToNormalUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LicenseKey_Software_SoftwareId",
                table: "LicenseKey");

            migrationBuilder.DropForeignKey(
                name: "FK_LicenseKey_NormalUser_UserId",
                table: "LicenseKey");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LicenseKey",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SoftwareId",
                table: "LicenseKey",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LicenseKey_Software_SoftwareId",
                table: "LicenseKey",
                column: "SoftwareId",
                principalTable: "Software",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LicenseKey_NormalUser_UserId",
                table: "LicenseKey",
                column: "UserId",
                principalTable: "NormalUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LicenseKey_Software_SoftwareId",
                table: "LicenseKey");

            migrationBuilder.DropForeignKey(
                name: "FK_LicenseKey_NormalUser_UserId",
                table: "LicenseKey");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LicenseKey",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "SoftwareId",
                table: "LicenseKey",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_LicenseKey_Software_SoftwareId",
                table: "LicenseKey",
                column: "SoftwareId",
                principalTable: "Software",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LicenseKey_NormalUser_UserId",
                table: "LicenseKey",
                column: "UserId",
                principalTable: "NormalUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
