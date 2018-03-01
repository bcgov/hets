using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HETSAPI.Migrations
{
    public partial class HETS3541 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HET_CONDITION_TYPE",
                table: "HET_CONDITION_TYPE");

            migrationBuilder.AlterColumn<string>(
                name: "CONDITION_TYPE_CODE",
                table: "HET_CONDITION_TYPE",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "CONDITION_TYPE_ID",
                table: "HET_CONDITION_TYPE",
                nullable: false)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "DISTRICT_ID",
                table: "HET_CONDITION_TYPE",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HET_CONDITION_TYPE",
                table: "HET_CONDITION_TYPE",
                column: "CONDITION_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_CONDITION_TYPE_DISTRICT_ID",
                table: "HET_CONDITION_TYPE",
                column: "DISTRICT_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_CONDITION_TYPE_HET_DISTRICT_DISTRICT_ID",
                table: "HET_CONDITION_TYPE",
                column: "DISTRICT_ID",
                principalTable: "HET_DISTRICT",
                principalColumn: "DISTRICT_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_CONDITION_TYPE_HET_DISTRICT_DISTRICT_ID",
                table: "HET_CONDITION_TYPE");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HET_CONDITION_TYPE",
                table: "HET_CONDITION_TYPE");

            migrationBuilder.DropIndex(
                name: "IX_HET_CONDITION_TYPE_DISTRICT_ID",
                table: "HET_CONDITION_TYPE");

            migrationBuilder.DropColumn(
                name: "CONDITION_TYPE_ID",
                table: "HET_CONDITION_TYPE");

            migrationBuilder.DropColumn(
                name: "DISTRICT_ID",
                table: "HET_CONDITION_TYPE");

            migrationBuilder.AlterColumn<string>(
                name: "CONDITION_TYPE_CODE",
                table: "HET_CONDITION_TYPE",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HET_CONDITION_TYPE",
                table: "HET_CONDITION_TYPE",
                column: "CONDITION_TYPE_CODE");
        }
    }
}
