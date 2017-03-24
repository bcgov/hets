using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS1643242 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BLOCK_NUMBER",
                table: "HET_SENIORITY_AUDIT",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BLOCK_NUMBER",
                table: "HET_EQUIPMENT",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "BLOCK_NUMBER",
                table: "HET_SENIORITY_AUDIT",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "BLOCK_NUMBER",
                table: "HET_EQUIPMENT",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
