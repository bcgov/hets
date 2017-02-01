using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS72211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_FAVOURITE_HET_FAVOURITE_CONTEXT_TYPE_FAVOURITE_CONTEXT_TYPE_REF_ID",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.RenameColumn(
                name: "FAVOURITE_CONTEXT_TYPE_REF_ID",
                table: "HET_USER_FAVOURITE",
                newName: "USER_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_USER_FAVOURITE_FAVOURITE_CONTEXT_TYPE_REF_ID",
                table: "HET_USER_FAVOURITE",
                newName: "IX_HET_USER_FAVOURITE_USER_REF_ID");

            migrationBuilder.AddColumn<string>(
                name: "TYPE",
                table: "HET_USER_FAVOURITE",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_FAVOURITE_HET_USER_USER_REF_ID",
                table: "HET_USER_FAVOURITE",
                column: "USER_REF_ID",
                principalTable: "HET_USER",
                principalColumn: "USER_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_FAVOURITE_HET_USER_USER_REF_ID",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropColumn(
                name: "TYPE",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.RenameColumn(
                name: "USER_REF_ID",
                table: "HET_USER_FAVOURITE",
                newName: "FAVOURITE_CONTEXT_TYPE_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_USER_FAVOURITE_USER_REF_ID",
                table: "HET_USER_FAVOURITE",
                newName: "IX_HET_USER_FAVOURITE_FAVOURITE_CONTEXT_TYPE_REF_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_FAVOURITE_HET_FAVOURITE_CONTEXT_TYPE_FAVOURITE_CONTEXT_TYPE_REF_ID",
                table: "HET_USER_FAVOURITE",
                column: "FAVOURITE_CONTEXT_TYPE_REF_ID",
                principalTable: "HET_FAVOURITE_CONTEXT_TYPE",
                principalColumn: "FAVOURITE_CONTEXT_TYPE_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
