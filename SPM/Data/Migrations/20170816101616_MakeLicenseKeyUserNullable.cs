using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SPM.Data.Migrations
{
    public partial class MakeLicenseKeyUserNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "LicenseKey",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LicenseKey",
                nullable: true,
                oldNullable: false
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "LicenseKey",
                nullable: true,
                oldClrType: typeof(string));
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LicenseKey",
                nullable: false
                );
        }
    }
}
