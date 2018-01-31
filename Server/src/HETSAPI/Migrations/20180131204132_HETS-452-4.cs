using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HETSAPI.Migrations
{
    public partial class HETS4524 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IS_INCLUDED_IN_TOTAL",
                table: "HET_RENTAL_AGREEMENT_RATE",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IS_IN_TOTAL_EDITABLE",
                table: "HET_PROVINCIAL_RATE_TYPE",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IS_INCLUDED_IN_TOTAL",
                table: "HET_RENTAL_AGREEMENT_RATE");

            migrationBuilder.DropColumn(
                name: "IS_IN_TOTAL_EDITABLE",
                table: "HET_PROVINCIAL_RATE_TYPE");
        }
    }
}
