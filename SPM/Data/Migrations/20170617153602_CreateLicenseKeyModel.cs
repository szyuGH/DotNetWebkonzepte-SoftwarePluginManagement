using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SPM.Data.Migrations
{
    public partial class CreateLicenseKeyModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LicenseKey",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    SoftwareId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseKey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenseKey_Software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseKey_NormalUser_UserId",
                        column: x => x.UserId,
                        principalTable: "NormalUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseKey_SoftwareId",
                table: "LicenseKey",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseKey_UserId",
                table: "LicenseKey",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenseKey");
        }
    }
}
