using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS1032161 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DOING_BUSINESS_AS",
                table: "HET_OWNER",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MEETS_RESIDENCY",
                table: "HET_OWNER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "REGISTERED_COMPANY_NUMBER",
                table: "HET_OWNER",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IS_SENIORITY_OVERRIDDEN",
                table: "HET_EQUIPMENT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SENIORITY_OVERRIDE_REASON",
                table: "HET_EQUIPMENT",
                maxLength: 2048,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DOING_BUSINESS_AS",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "MEETS_RESIDENCY",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "REGISTERED_COMPANY_NUMBER",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "IS_SENIORITY_OVERRIDDEN",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "SENIORITY_OVERRIDE_REASON",
                table: "HET_EQUIPMENT");
        }
    }
}
