using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS2263231 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "WORKED_DATE",
                table: "HET_TIME_RECORD",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "START_DATE",
                table: "HET_SENIORITY_AUDIT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "END_DATE",
                table: "HET_SENIORITY_AUDIT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IS_SENIORITY_OVERRIDDEN",
                table: "HET_SENIORITY_AUDIT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SENIORITY_OVERRIDE_REASON",
                table: "HET_SENIORITY_AUDIT",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ROTATION_LIST_SORT_ORDER",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EQUIPMENT_COUNT",
                table: "HET_RENTAL_REQUEST",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "MEETS_RESIDENCY",
                table: "HET_OWNER",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DEFAULT",
                table: "HET_LOOKUP_LIST",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LOCAL_AREA_NUMBER",
                table: "HET_LOCAL_AREA",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NUMBER_OF_BLOCKS",
                table: "HET_EQUIPMENT_TYPE",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DUMP_TRUCK",
                table: "HET_EQUIPMENT_TYPE",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RECEIVED_DATE",
                table: "HET_EQUIPMENT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LAST_VERIFIED_DATE",
                table: "HET_EQUIPMENT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IS_SENIORITY_OVERRIDDEN",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "SENIORITY_OVERRIDE_REASON",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "WORKED_DATE",
                table: "HET_TIME_RECORD",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "START_DATE",
                table: "HET_SENIORITY_AUDIT",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "END_DATE",
                table: "HET_SENIORITY_AUDIT",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<int>(
                name: "ROTATION_LIST_SORT_ORDER",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "EQUIPMENT_COUNT",
                table: "HET_RENTAL_REQUEST",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<bool>(
                name: "MEETS_RESIDENCY",
                table: "HET_OWNER",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DEFAULT",
                table: "HET_LOOKUP_LIST",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<int>(
                name: "LOCAL_AREA_NUMBER",
                table: "HET_LOCAL_AREA",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "NUMBER_OF_BLOCKS",
                table: "HET_EQUIPMENT_TYPE",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<bool>(
                name: "IS_DUMP_TRUCK",
                table: "HET_EQUIPMENT_TYPE",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<DateTime>(
                name: "RECEIVED_DATE",
                table: "HET_EQUIPMENT",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LAST_VERIFIED_DATE",
                table: "HET_EQUIPMENT",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
