using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication2.Data.Migrations
{
    public partial class CreateUserPluginMergeCollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NormalUser",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NormalUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPlugin",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PluginId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlugin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPlugin_Plugin_PluginId",
                        column: x => x.PluginId,
                        principalTable: "Plugin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPlugin_NormalUser_UserId",
                        column: x => x.UserId,
                        principalTable: "NormalUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPlugin_PluginId",
                table: "UserPlugin",
                column: "PluginId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlugin_UserId",
                table: "UserPlugin",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPlugin");

            migrationBuilder.DropTable(
                name: "NormalUser");
        }
    }
}
