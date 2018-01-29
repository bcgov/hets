using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HETSAPI.Migrations
{
    public partial class HETS4521 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ADDRESS1",
                table: "HET_OWNER",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ADDRESS2",
                table: "HET_OWNER",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CITY",
                table: "HET_OWNER",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "POSTAL_CODE",
                table: "HET_OWNER",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PROVINCE",
                table: "HET_OWNER",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ADDRESS1",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "ADDRESS2",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "CITY",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "POSTAL_CODE",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "PROVINCE",
                table: "HET_OWNER");
        }
    }
}
