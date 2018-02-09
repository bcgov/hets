using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HETSAPI.Migrations
{
    public partial class HETS4961 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "STATUS_COMMENT",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "STATUS_COMMENT",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "STATUS_COMMENT",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "STATUS_COMMENT",
                table: "HET_EQUIPMENT");
        }
    }
}
