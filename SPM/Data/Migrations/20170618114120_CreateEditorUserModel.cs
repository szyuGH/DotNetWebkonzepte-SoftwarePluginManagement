using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SPM.Data.Migrations
{
    public partial class CreateEditorUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EditorUser",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CompanyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditorUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EditorUser_CompanyUser_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EditorUser_CompanyId",
                table: "EditorUser",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EditorUser");
        }
    }
}
