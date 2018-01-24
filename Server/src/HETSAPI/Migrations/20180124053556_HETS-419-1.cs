using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HETSAPI.Migrations
{
    public partial class HETS4191 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_USER_ROLE",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_USER_ROLE",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_USER_ROLE",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_USER_ROLE",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_USER_FAVOURITE",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_USER_FAVOURITE",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_USER_FAVOURITE",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_USER_FAVOURITE",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_USER",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_USER",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_USER",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_USER",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_TIME_RECORD",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_TIME_RECORD",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_TIME_RECORD",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_TIME_RECORD",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_SERVICE_AREA",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_SERVICE_AREA",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_SERVICE_AREA",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_SERVICE_AREA",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_SENIORITY_AUDIT",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_SENIORITY_AUDIT",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_SENIORITY_AUDIT",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_SENIORITY_AUDIT",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_ROLE_PERMISSION",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_ROLE_PERMISSION",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_ROLE_PERMISSION",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_ROLE_PERMISSION",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_ROLE",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_ROLE",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_ROLE",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_ROLE",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_RENTAL_REQUEST",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_RENTAL_REQUEST",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_RENTAL_AGREEMENT_RATE",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT_RATE",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_RENTAL_AGREEMENT_RATE",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT_RATE",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_RENTAL_AGREEMENT",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_RENTAL_AGREEMENT",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_REGION",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_REGION",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_REGION",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_REGION",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_PROJECT",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_PROJECT",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_PROJECT",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_PROJECT",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_PERMISSION",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_PERMISSION",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_PERMISSION",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_PERMISSION",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_OWNER",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_OWNER",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_OWNER",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_OWNER",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_NOTE",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_NOTE",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_NOTE",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_NOTE",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_LOOKUP_LIST",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_LOOKUP_LIST",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_LOOKUP_LIST",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_LOOKUP_LIST",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_LOCAL_AREA",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_LOCAL_AREA",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_LOCAL_AREA",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_LOCAL_AREA",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_IMPORT_MAP",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_IMPORT_MAP",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_IMPORT_MAP",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_IMPORT_MAP",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_HISTORY",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_HISTORY",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_HISTORY",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_HISTORY",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_GROUP_MEMBERSHIP",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_GROUP_MEMBERSHIP",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_GROUP_MEMBERSHIP",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_GROUP_MEMBERSHIP",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_GROUP",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_GROUP",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_GROUP",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_GROUP",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_EQUIPMENT_TYPE",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT_TYPE",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_EQUIPMENT_TYPE",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT_TYPE",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT_ATTACHMENT",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT_ATTACHMENT",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_EQUIPMENT",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_EQUIPMENT",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_DUMP_TRUCK",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_DUMP_TRUCK",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_DUMP_TRUCK",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_DUMP_TRUCK",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_DISTRICT",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_DISTRICT",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_DISTRICT",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_DISTRICT",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_CONTACT",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_CONTACT",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_CONTACT",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_CONTACT",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_CITY",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_CITY",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_CITY",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_CITY",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_ATTACHMENT",
                newName: "APP_LAST_UPDATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_ATTACHMENT",
                newName: "DB_LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "CREATE_USERID",
                table: "HET_ATTACHMENT",
                newName: "APP_CREATE_USER_DIRECTORY");

            migrationBuilder.RenameColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_ATTACHMENT",
                newName: "DB_CREATE_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_USER_ROLE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_USER_ROLE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_USER_ROLE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_USER_ROLE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_USER_ROLE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_USER_ROLE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_USER_ROLE",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_USER_ROLE",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_USER_FAVOURITE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_USER_FAVOURITE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_USER_FAVOURITE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_USER_FAVOURITE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_USER_FAVOURITE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_USER_FAVOURITE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_USER_FAVOURITE",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_USER_FAVOURITE",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_USER",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_USER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_USER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_USER",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_USER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_USER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_USER",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_USER",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_TIME_RECORD",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_TIME_RECORD",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_TIME_RECORD",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_TIME_RECORD",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_TIME_RECORD",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_TIME_RECORD",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_TIME_RECORD",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_TIME_RECORD",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_SERVICE_AREA",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_SERVICE_AREA",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_SERVICE_AREA",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_SERVICE_AREA",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_SERVICE_AREA",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_SERVICE_AREA",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_SERVICE_AREA",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_SERVICE_AREA",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_SENIORITY_AUDIT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_SENIORITY_AUDIT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_SENIORITY_AUDIT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_SENIORITY_AUDIT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_SENIORITY_AUDIT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_SENIORITY_AUDIT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_SENIORITY_AUDIT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_SENIORITY_AUDIT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_ROLE_PERMISSION",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_ROLE_PERMISSION",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_ROLE_PERMISSION",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_ROLE_PERMISSION",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_ROLE_PERMISSION",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_ROLE_PERMISSION",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_ROLE_PERMISSION",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_ROLE_PERMISSION",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_ROLE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_ROLE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_ROLE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_ROLE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_ROLE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_ROLE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_ROLE",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_ROLE",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_RENTAL_REQUEST",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_RENTAL_REQUEST",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_RENTAL_REQUEST",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_RENTAL_REQUEST",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_RENTAL_REQUEST",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_RENTAL_REQUEST",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT_RATE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_RENTAL_AGREEMENT_RATE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_RENTAL_AGREEMENT_RATE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT_RATE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_RENTAL_AGREEMENT_RATE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_RENTAL_AGREEMENT_RATE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_RENTAL_AGREEMENT_RATE",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_RENTAL_AGREEMENT_RATE",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_RENTAL_AGREEMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_RENTAL_AGREEMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_RENTAL_AGREEMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_RENTAL_AGREEMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_RENTAL_AGREEMENT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_RENTAL_AGREEMENT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_REGION",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_REGION",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_REGION",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_REGION",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_REGION",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_REGION",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_REGION",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_REGION",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_PROJECT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_PROJECT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_PROJECT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_PROJECT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_PROJECT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_PROJECT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_PROJECT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_PROJECT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_PERMISSION",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_PERMISSION",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_PERMISSION",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_PERMISSION",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_PERMISSION",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_PERMISSION",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_PERMISSION",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_PERMISSION",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_OWNER",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_OWNER",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_OWNER",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_OWNER",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_NOTE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_NOTE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_NOTE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_NOTE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_NOTE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_NOTE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_NOTE",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_NOTE",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_LOOKUP_LIST",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_LOOKUP_LIST",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_LOOKUP_LIST",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_LOOKUP_LIST",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_LOOKUP_LIST",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_LOOKUP_LIST",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_LOOKUP_LIST",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_LOOKUP_LIST",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_LOCAL_AREA",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_LOCAL_AREA",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_LOCAL_AREA",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_LOCAL_AREA",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_LOCAL_AREA",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_LOCAL_AREA",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_LOCAL_AREA",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_LOCAL_AREA",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_IMPORT_MAP",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_IMPORT_MAP",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_IMPORT_MAP",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_IMPORT_MAP",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_IMPORT_MAP",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_IMPORT_MAP",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_IMPORT_MAP",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_IMPORT_MAP",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_HISTORY",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_HISTORY",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_HISTORY",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_HISTORY",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_HISTORY",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_HISTORY",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_HISTORY",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_HISTORY",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_GROUP_MEMBERSHIP",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_GROUP_MEMBERSHIP",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_GROUP_MEMBERSHIP",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_GROUP_MEMBERSHIP",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_GROUP_MEMBERSHIP",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_GROUP_MEMBERSHIP",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_GROUP_MEMBERSHIP",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_GROUP_MEMBERSHIP",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_GROUP",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_GROUP",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_GROUP",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_GROUP",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_GROUP",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_GROUP",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_GROUP",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_GROUP",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT_TYPE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_EQUIPMENT_TYPE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_EQUIPMENT_TYPE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT_TYPE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_EQUIPMENT_TYPE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_EQUIPMENT_TYPE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_EQUIPMENT_TYPE",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_EQUIPMENT_TYPE",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT_ATTACHMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT_ATTACHMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_EQUIPMENT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_EQUIPMENT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_DUMP_TRUCK",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_DUMP_TRUCK",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_DUMP_TRUCK",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_DUMP_TRUCK",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_DISTRICT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_DISTRICT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_DISTRICT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_DISTRICT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_DISTRICT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_DISTRICT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_DISTRICT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_DISTRICT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_CONTACT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_CONTACT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_CONTACT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_CONTACT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_CONTACT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_CONTACT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_CONTACT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_CONTACT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_CITY",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_CITY",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_CITY",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_CITY",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_CITY",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_CITY",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_CITY",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_CITY",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_ATTACHMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USER_GUID",
                table: "HET_ATTACHMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_CREATE_USERID",
                table: "HET_ATTACHMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_ATTACHMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_ATTACHMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_ATTACHMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_CREATE_USER_ID",
                table: "HET_ATTACHMENT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_ATTACHMENT",
                maxLength: 63,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_USER_ROLE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_USER_ROLE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_USER_ROLE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_USER_ROLE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_USER_ROLE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_USER_ROLE");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_USER_ROLE");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_USER_ROLE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_USER");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_USER");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_USER");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_USER");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_USER");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_USER");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_USER");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_USER");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_ROLE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_ROLE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_ROLE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_ROLE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_ROLE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_ROLE");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_ROLE");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_ROLE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT_RATE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_RENTAL_AGREEMENT_RATE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_RENTAL_AGREEMENT_RATE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT_RATE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_RENTAL_AGREEMENT_RATE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_RENTAL_AGREEMENT_RATE");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_RENTAL_AGREEMENT_RATE");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_RENTAL_AGREEMENT_RATE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT_CONDITION");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_RENTAL_AGREEMENT_CONDITION");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_RENTAL_AGREEMENT_CONDITION");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT_CONDITION");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_RENTAL_AGREEMENT_CONDITION");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_RENTAL_AGREEMENT_CONDITION");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_RENTAL_AGREEMENT_CONDITION");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_RENTAL_AGREEMENT_CONDITION");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_REGION");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_REGION");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_REGION");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_REGION");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_REGION");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_REGION");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_REGION");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_REGION");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_PERMISSION");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_PERMISSION");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_PERMISSION");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_PERMISSION");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_PERMISSION");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_PERMISSION");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_PERMISSION");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_PERMISSION");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_NOTE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_NOTE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_NOTE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_NOTE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_NOTE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_NOTE");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_NOTE");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_NOTE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_LOOKUP_LIST");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_LOOKUP_LIST");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_LOOKUP_LIST");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_LOOKUP_LIST");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_LOOKUP_LIST");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_LOOKUP_LIST");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_LOOKUP_LIST");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_LOOKUP_LIST");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_LOCAL_AREA_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_LOCAL_AREA_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_LOCAL_AREA_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_LOCAL_AREA_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_LOCAL_AREA_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_LOCAL_AREA_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_LOCAL_AREA_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_LOCAL_AREA_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_IMPORT_MAP");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_IMPORT_MAP");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_IMPORT_MAP");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_IMPORT_MAP");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_IMPORT_MAP");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_IMPORT_MAP");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_IMPORT_MAP");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_IMPORT_MAP");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_HISTORY");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_HISTORY");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_HISTORY");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_HISTORY");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_HISTORY");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_HISTORY");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_HISTORY");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_HISTORY");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_GROUP");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_GROUP");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_GROUP");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_GROUP");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_GROUP");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_GROUP");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_GROUP");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_GROUP");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_DISTRICT_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_DISTRICT_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_DISTRICT_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_DISTRICT_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_DISTRICT_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_DISTRICT_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_DISTRICT_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_DISTRICT_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_DISTRICT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_DISTRICT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_DISTRICT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_DISTRICT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_DISTRICT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_DISTRICT");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_DISTRICT");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_DISTRICT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_CITY");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_CITY");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_CITY");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_CITY");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_CITY");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_CITY");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_CITY");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_CITY");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_TIMESTAMP",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USER_GUID",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_CREATE_USERID",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_TIMESTAMP",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USER_GUID",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "APP_LAST_UPDATE_USERID",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "DB_CREATE_USER_ID",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "DB_LAST_UPDATE_USER_ID",
                table: "HET_ATTACHMENT");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_USER_ROLE",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_USER_ROLE",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_USER_ROLE",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_USER_ROLE",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_USER_FAVOURITE",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_USER_FAVOURITE",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_USER_FAVOURITE",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_USER_FAVOURITE",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_USER",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_USER",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_USER",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_USER",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_TIME_RECORD",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_TIME_RECORD",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_TIME_RECORD",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_TIME_RECORD",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_SERVICE_AREA",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_SERVICE_AREA",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_SERVICE_AREA",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_SERVICE_AREA",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_SENIORITY_AUDIT",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_SENIORITY_AUDIT",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_SENIORITY_AUDIT",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_SENIORITY_AUDIT",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_ROLE_PERMISSION",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_ROLE_PERMISSION",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_ROLE_PERMISSION",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_ROLE_PERMISSION",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_ROLE",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_ROLE",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_ROLE",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_ROLE",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_RENTAL_REQUEST",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_RENTAL_REQUEST",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT_RATE",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT_RATE",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_RENTAL_AGREEMENT_RATE",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_RENTAL_AGREEMENT_RATE",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_RENTAL_AGREEMENT",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_RENTAL_AGREEMENT",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_REGION",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_REGION",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_REGION",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_REGION",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_PROJECT",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_PROJECT",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_PROJECT",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_PROJECT",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_PERMISSION",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_PERMISSION",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_PERMISSION",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_PERMISSION",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_OWNER",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_OWNER",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_OWNER",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_OWNER",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_NOTE",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_NOTE",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_NOTE",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_NOTE",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_LOOKUP_LIST",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_LOOKUP_LIST",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_LOOKUP_LIST",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_LOOKUP_LIST",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_LOCAL_AREA",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_LOCAL_AREA",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_LOCAL_AREA",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_LOCAL_AREA",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_IMPORT_MAP",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_IMPORT_MAP",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_IMPORT_MAP",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_IMPORT_MAP",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_HISTORY",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_HISTORY",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_HISTORY",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_HISTORY",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_GROUP_MEMBERSHIP",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_GROUP_MEMBERSHIP",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_GROUP_MEMBERSHIP",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_GROUP_MEMBERSHIP",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_GROUP",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_GROUP",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_GROUP",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_GROUP",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT_TYPE",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT_TYPE",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_EQUIPMENT_TYPE",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_EQUIPMENT_TYPE",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT_ATTACHMENT",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT_ATTACHMENT",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_EQUIPMENT_ATTACHMENT",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_EQUIPMENT_ATTACHMENT",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_EQUIPMENT",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_EQUIPMENT",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_DUMP_TRUCK",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_DUMP_TRUCK",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_DUMP_TRUCK",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_DUMP_TRUCK",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_DISTRICT",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_DISTRICT",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_DISTRICT",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_DISTRICT",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_CONTACT",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_CONTACT",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_CONTACT",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_CONTACT",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_CITY",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_CITY",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_CITY",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_CITY",
                newName: "CREATE_USERID");

            migrationBuilder.RenameColumn(
                name: "DB_LAST_UPDATE_TIMESTAMP",
                table: "HET_ATTACHMENT",
                newName: "LAST_UPDATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "DB_CREATE_TIMESTAMP",
                table: "HET_ATTACHMENT",
                newName: "CREATE_TIMESTAMP");

            migrationBuilder.RenameColumn(
                name: "APP_LAST_UPDATE_USER_DIRECTORY",
                table: "HET_ATTACHMENT",
                newName: "LAST_UPDATE_USERID");

            migrationBuilder.RenameColumn(
                name: "APP_CREATE_USER_DIRECTORY",
                table: "HET_ATTACHMENT",
                newName: "CREATE_USERID");
        }
    }
}
