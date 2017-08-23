using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SPM.Data.Migrations
{
    public partial class UpdateModelRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plugin_Software_RelatedSoftwareId",
                table: "Plugin");

            migrationBuilder.DropForeignKey(
                name: "FK_EditorUser_CompanyUser_CompanyId",
                table: "EditorUser");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "EditorUser",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "CompanyUser",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RelatedSoftwareId",
                table: "Plugin",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Plugin_Software_RelatedSoftwareId",
                table: "Plugin",
                column: "RelatedSoftwareId",
                principalTable: "Software",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EditorUser_CompanyUser_CompanyId",
                table: "EditorUser",
                column: "CompanyId",
                principalTable: "CompanyUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plugin_Software_RelatedSoftwareId",
                table: "Plugin");

            migrationBuilder.DropForeignKey(
                name: "FK_EditorUser_CompanyUser_CompanyId",
                table: "EditorUser");

            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "CompanyUser");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "EditorUser",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "RelatedSoftwareId",
                table: "Plugin",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Plugin_Software_RelatedSoftwareId",
                table: "Plugin",
                column: "RelatedSoftwareId",
                principalTable: "Software",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EditorUser_CompanyUser_CompanyId",
                table: "EditorUser",
                column: "CompanyId",
                principalTable: "CompanyUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
