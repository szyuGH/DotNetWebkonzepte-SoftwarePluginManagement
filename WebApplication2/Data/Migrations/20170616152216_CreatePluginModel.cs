using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication2.Data.Migrations
{
    public partial class CreatePluginModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Software",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Software",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Plugin",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CompanyId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    RelatedSoftwareId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plugin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plugin_CompanyUser_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plugin_Software_RelatedSoftwareId",
                        column: x => x.RelatedSoftwareId,
                        principalTable: "Software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plugin_CompanyId",
                table: "Plugin",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Plugin_RelatedSoftwareId",
                table: "Plugin",
                column: "RelatedSoftwareId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plugin");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Software",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Software",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
