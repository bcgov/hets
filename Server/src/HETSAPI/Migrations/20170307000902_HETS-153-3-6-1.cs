using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS153361 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SERVICE_HOURS_CURRENT_YEAR_TO_DATE",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.AddColumn<float>(
                name: "ASK_NEXT_BLOCK1_SENIORITY",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ASK_NEXT_BLOCK2_SENIORITY",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ASK_NEXT_BLOCK1_SENIORITY",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL");

            migrationBuilder.DropColumn(
                name: "ASK_NEXT_BLOCK2_SENIORITY",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL");

            migrationBuilder.AddColumn<float>(
                name: "SERVICE_HOURS_CURRENT_YEAR_TO_DATE",
                table: "HET_SENIORITY_AUDIT",
                nullable: true);
        }
    }
}
