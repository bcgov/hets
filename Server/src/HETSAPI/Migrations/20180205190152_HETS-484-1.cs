using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HETSAPI.Migrations
{
    public partial class HETS4841 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "PERCENT_OF_EQUIPMENT_RATE",
                table: "HET_RENTAL_AGREEMENT_RATE",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PERCENT_OF_EQUIPMENT_RATE",
                table: "HET_RENTAL_AGREEMENT_RATE",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);
        }
    }
}
