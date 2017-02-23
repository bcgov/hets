using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS1432231 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_PROJECT_HET_SERVICE_AREA_SERVICE_AREA_REF_ID",
                table: "HET_PROJECT");

            migrationBuilder.RenameColumn(
                name: "SERVICE_AREA_REF_ID",
                table: "HET_PROJECT",
                newName: "LOCAL_AREA_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_PROJECT_SERVICE_AREA_REF_ID",
                table: "HET_PROJECT",
                newName: "IX_HET_PROJECT_LOCAL_AREA_REF_ID");

            migrationBuilder.AddColumn<string>(
                name: "STATUS",
                table: "HET_PROJECT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_PROJECT_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                table: "HET_PROJECT",
                column: "LOCAL_AREA_REF_ID",
                principalTable: "HET_LOCAL_AREA",
                principalColumn: "LOCAL_AREA_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_PROJECT_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "HET_PROJECT");

            migrationBuilder.RenameColumn(
                name: "LOCAL_AREA_REF_ID",
                table: "HET_PROJECT",
                newName: "SERVICE_AREA_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_PROJECT_LOCAL_AREA_REF_ID",
                table: "HET_PROJECT",
                newName: "IX_HET_PROJECT_SERVICE_AREA_REF_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_PROJECT_HET_SERVICE_AREA_SERVICE_AREA_REF_ID",
                table: "HET_PROJECT",
                column: "SERVICE_AREA_REF_ID",
                principalTable: "HET_SERVICE_AREA",
                principalColumn: "SERVICE_AREA_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
