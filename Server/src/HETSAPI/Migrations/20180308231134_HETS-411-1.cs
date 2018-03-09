using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HETSAPI.Migrations
{
    public partial class HETS4111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_USER_ROLE",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_USER_FAVOURITE",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_USER_DISTRICT",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_USER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_TIME_RECORD",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_SERVICE_AREA",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_SENIORITY_AUDIT",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_ROLE_PERMISSION",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_ROLE",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_RENTAL_REQUEST",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_RENTAL_AGREEMENT_RATE",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_RENTAL_AGREEMENT",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_REGION",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_PROVINCIAL_RATE_TYPE",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_PROJECT",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_PERMISSION",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_OWNER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_NOTE",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_LOCAL_AREA",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_IMPORT_MAP",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_HISTORY",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_EQUIPMENT_TYPE",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_EQUIPMENT_ATTACHMENT",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_EQUIPMENT",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_DISTRICT",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_CONTACT",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_CONDITION_TYPE",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_CITY",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_ATTACHMENT",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_USER_ROLE");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_USER_DISTRICT");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_USER");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_ROLE");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_RENTAL_REQUEST_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_RENTAL_AGREEMENT_RATE");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_RENTAL_AGREEMENT_CONDITION");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_REGION");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_PROVINCIAL_RATE_TYPE");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_PERMISSION");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_NOTE");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_LOCAL_AREA_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_IMPORT_MAP");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_HISTORY");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_DISTRICT_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_DISTRICT");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_CONDITION_TYPE");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_CITY");

            migrationBuilder.DropColumn(
                name: "CONCURRENCY_CONTROL_NUMBER",
                table: "HET_ATTACHMENT");
        }
    }
}
