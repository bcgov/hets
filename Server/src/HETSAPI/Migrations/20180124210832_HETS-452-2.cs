using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HETSAPI.Migrations
{
    public partial class HETS4522 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OWNER_EQUIPMENT_CODE_PREFIX",
                table: "HET_OWNER",
                newName: "OWNER_CODE");

            migrationBuilder.AddColumn<string>(
                name: "GIVEN_NAME",
                table: "HET_OWNER",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SURNAME",
                table: "HET_OWNER",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GIVEN_NAME",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "SURNAME",
                table: "HET_OWNER");

            migrationBuilder.RenameColumn(
                name: "OWNER_CODE",
                table: "HET_OWNER",
                newName: "OWNER_EQUIPMENT_CODE_PREFIX");
        }
    }
}
