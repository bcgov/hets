using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS18212 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_ROLE_HET_USER_USER_REF_ID",
                table: "HET_USER_ROLE");

            migrationBuilder.RenameColumn(
                name: "USER_REF_ID",
                table: "HET_USER_ROLE",
                newName: "USER_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_USER_ROLE_USER_REF_ID",
                table: "HET_USER_ROLE",
                newName: "IX_HET_USER_ROLE_USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_ROLE_HET_USER_USER_ID",
                table: "HET_USER_ROLE",
                column: "USER_ID",
                principalTable: "HET_USER",
                principalColumn: "USER_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_ROLE_HET_USER_USER_ID",
                table: "HET_USER_ROLE");

            migrationBuilder.RenameColumn(
                name: "USER_ID",
                table: "HET_USER_ROLE",
                newName: "USER_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_USER_ROLE_USER_ID",
                table: "HET_USER_ROLE",
                newName: "IX_HET_USER_ROLE_USER_REF_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_ROLE_HET_USER_USER_REF_ID",
                table: "HET_USER_ROLE",
                column: "USER_REF_ID",
                principalTable: "HET_USER",
                principalColumn: "USER_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
