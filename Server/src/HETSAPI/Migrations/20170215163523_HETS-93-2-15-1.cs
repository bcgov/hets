using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS932151 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DISTRICT_REF_ID",
                table: "HET_USER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HET_USER_DISTRICT_REF_ID",
                table: "HET_USER",
                column: "DISTRICT_REF_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_HET_DISTRICT_DISTRICT_REF_ID",
                table: "HET_USER",
                column: "DISTRICT_REF_ID",
                principalTable: "HET_DISTRICT",
                principalColumn: "DISTRICT_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_HET_DISTRICT_DISTRICT_REF_ID",
                table: "HET_USER");

            migrationBuilder.DropIndex(
                name: "IX_HET_USER_DISTRICT_REF_ID",
                table: "HET_USER");

            migrationBuilder.DropColumn(
                name: "DISTRICT_REF_ID",
                table: "HET_USER");
        }
    }
}
