using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SPM.Data.Migrations
{
    public partial class CreateSoftwareModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyUser",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Postcode = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Software",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CompanyId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Software", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Software_CompanyUser_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Software_CompanyId",
                table: "Software",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Software");

            migrationBuilder.DropTable(
                name: "CompanyUser");
        }
    }
}
