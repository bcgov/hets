using System;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS1262221 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_ATTACHMENT_HET_EQUIPMENT_ATTACHMENT_TYPE_TYPE_REF_ID",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_HET_EQUIPMENT_ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropTable(
                name: "HET_CONTACT_ADDRESS");

            migrationBuilder.DropTable(
                name: "HET_CONTACT_PHONE");

            migrationBuilder.DropTable(
                name: "HET_EQUIPMENT_ATTACHMENT_TYPE");

            migrationBuilder.DropTable(
                name: "HET_FAVOURITE_CONTEXT_TYPE");

            migrationBuilder.DropIndex(
                name: "IX_HET_EQUIPMENT_TYPE_ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropIndex(
                name: "IX_HET_EQUIPMENT_ATTACHMENT_TYPE_REF_ID",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "HOURS2",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "HOURS3",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "RATE",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "RATE2",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "RATE3",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "SEQ_NUM",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "TYPE_REF_ID",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "EXTERNAL_FILE_NAME",
                table: "HET_ATTACHMENT");

            migrationBuilder.RenameColumn(
                name: "WCBEXPIRY_DATE",
                table: "HET_OWNER",
                newName: "WORK_SAFE_BCEXPIRY_DATE");

            migrationBuilder.RenameColumn(
                name: "INTERNAL_FILE_NAME",
                table: "HET_ATTACHMENT",
                newName: "FILE_NAME");

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_USER_ROLE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_USER_ROLE",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_USER_ROLE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_USER_ROLE",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_USER_FAVOURITE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_USER_FAVOURITE",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_USER_FAVOURITE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_USER_FAVOURITE",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_USER",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_USER",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_USER",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_USER",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_TIME_RECORD",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_TIME_RECORD",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_TIME_RECORD",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_TIME_RECORD",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RENTAL_AGREEMENT_RATE_REF_ID",
                table: "HET_TIME_RECORD",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_SERVICE_AREA",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_SERVICE_AREA",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_SERVICE_AREA",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_SERVICE_AREA",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_SENIORITY_AUDIT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_SENIORITY_AUDIT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_SENIORITY_AUDIT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_SENIORITY_AUDIT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_ROLE_PERMISSION",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_ROLE_PERMISSION",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_ROLE_PERMISSION",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_ROLE_PERMISSION",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_ROLE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_ROLE",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_ROLE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_ROLE",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_RENTAL_REQUEST",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_RENTAL_REQUEST",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_RENTAL_AGREEMENT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DATED_ON",
                table: "HET_RENTAL_AGREEMENT",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "EQUIPMENT_RATE",
                table: "HET_RENTAL_AGREEMENT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ESTIMATE_HOURS",
                table: "HET_RENTAL_AGREEMENT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ESTIMATE_START_WORK",
                table: "HET_RENTAL_AGREEMENT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_RENTAL_AGREEMENT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NOTE",
                table: "HET_RENTAL_AGREEMENT",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NUMBER",
                table: "HET_RENTAL_AGREEMENT",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RATE_COMMENT",
                table: "HET_RENTAL_AGREEMENT",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RATE_PERIOD",
                table: "HET_RENTAL_AGREEMENT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_REGION",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_REGION",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_REGION",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_REGION",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_PROJECT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_PROJECT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_PROJECT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_PROJECT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_PERMISSION",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_PERMISSION",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_PERMISSION",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_PERMISSION",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_OWNER",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_OWNER",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_OWNER",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_OWNER",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WORK_SAFE_BCPOLICY_NUMBER",
                table: "HET_OWNER",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_NOTE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_NOTE",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_NOTE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_NOTE",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_LOCAL_AREA",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_LOCAL_AREA",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_LOCAL_AREA",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_LOCAL_AREA",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_HISTORY",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_HISTORY",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_HISTORY",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_HISTORY",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_GROUP_MEMBERSHIP",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_GROUP_MEMBERSHIP",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_GROUP_MEMBERSHIP",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_GROUP_MEMBERSHIP",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_GROUP",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_GROUP",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_GROUP",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_GROUP",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT_TYPE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_EQUIPMENT_TYPE",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT_TYPE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_EQUIPMENT_TYPE",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ATTACHMENT",
                table: "HET_EQUIPMENT_ATTACHMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT_ATTACHMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT_ATTACHMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_EQUIPMENT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_EQUIPMENT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_DUMP_TRUCK",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_DUMP_TRUCK",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_DUMP_TRUCK",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_DUMP_TRUCK",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_DISTRICT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_DISTRICT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_DISTRICT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_DISTRICT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ADDRESS1",
                table: "HET_CONTACT",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ADDRESS2",
                table: "HET_CONTACT",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CITY",
                table: "HET_CONTACT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_CONTACT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_CONTACT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMAIL_ADDRESS",
                table: "HET_CONTACT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FAX_PHONE_NUMBER",
                table: "HET_CONTACT",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_CONTACT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_CONTACT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MOBILE_PHONE_NUMBER",
                table: "HET_CONTACT",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ORGANIZATION_NAME",
                table: "HET_CONTACT",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "POSTAL_CODE",
                table: "HET_CONTACT",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PROVINCE",
                table: "HET_CONTACT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WORK_PHONE_NUMBER",
                table: "HET_CONTACT",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_CITY",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_CITY",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_CITY",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_CITY",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_TIMESTAMP",
                table: "HET_ATTACHMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CREATE_USERID",
                table: "HET_ATTACHMENT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FILE_CONTENTS",
                table: "HET_ATTACHMENT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_ATTACHMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LAST_UPDATE_USERID",
                table: "HET_ATTACHMENT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TYPE",
                table: "HET_ATTACHMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                columns: table => new
                {
                    EQUIPMENT_TYPE_NEXT_RENTAL_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ASK_NEXT_BLOCK1_REF_ID = table.Column<int>(nullable: true),
                    ASK_NEXT_BLOCK2_REF_ID = table.Column<int>(nullable: true),
                    ASK_NEXT_BLOCK_OPEN_REF_ID = table.Column<int>(nullable: true),
                    CREATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    CREATE_USERID = table.Column<string>(maxLength: 50, nullable: true),
                    EQUIPMENT_TYPE_REF_ID = table.Column<int>(nullable: true),
                    LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    LAST_UPDATE_USERID = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_EQUIPMENT_TYPE_NEXT_RENTAL", x => x.EQUIPMENT_TYPE_NEXT_RENTAL_ID);
                    table.ForeignKey(
                        name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK1_REF_ID",
                        column: x => x.ASK_NEXT_BLOCK1_REF_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK2_REF_ID",
                        column: x => x.ASK_NEXT_BLOCK2_REF_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK_OPEN_REF_ID",
                        column: x => x.ASK_NEXT_BLOCK_OPEN_REF_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_REF_ID",
                        column: x => x.EQUIPMENT_TYPE_REF_ID,
                        principalTable: "HET_EQUIPMENT_TYPE",
                        principalColumn: "EQUIPMENT_TYPE_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_RENTAL_AGREEMENT_CONDITION",
                columns: table => new
                {
                    RENTAL_AGREEMENT_CONDITION_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    COMMENT = table.Column<string>(maxLength: 2048, nullable: true),
                    CONDITION_NAME = table.Column<string>(maxLength: 150, nullable: true),
                    CREATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    CREATE_USERID = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    LAST_UPDATE_USERID = table.Column<string>(maxLength: 50, nullable: true),
                    RENTAL_AGREEMENT_REF_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_RENTAL_AGREEMENT_CONDITION", x => x.RENTAL_AGREEMENT_CONDITION_ID);
                    table.ForeignKey(
                        name: "FK_HET_RENTAL_AGREEMENT_CONDITION_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_REF_ID",
                        column: x => x.RENTAL_AGREEMENT_REF_ID,
                        principalTable: "HET_RENTAL_AGREEMENT",
                        principalColumn: "RENTAL_AGREEMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_RENTAL_AGREEMENT_RATE",
                columns: table => new
                {
                    RENTAL_AGREEMENT_RATE_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    COMMENT = table.Column<string>(maxLength: 2048, nullable: true),
                    COMPONENT_NAME = table.Column<string>(maxLength: 150, nullable: true),
                    CREATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    CREATE_USERID = table.Column<string>(maxLength: 50, nullable: true),
                    IS_ATTACHMENT = table.Column<bool>(nullable: true),
                    LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    LAST_UPDATE_USERID = table.Column<string>(maxLength: 50, nullable: true),
                    PERCENT_OF_EQUIPMENT_RATE = table.Column<int>(nullable: true),
                    RATE = table.Column<float>(nullable: true),
                    RATE_PERIOD = table.Column<string>(maxLength: 50, nullable: true),
                    RENTAL_AGREEMENT_REF_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_RENTAL_AGREEMENT_RATE", x => x.RENTAL_AGREEMENT_RATE_ID);
                    table.ForeignKey(
                        name: "FK_HET_RENTAL_AGREEMENT_RATE_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_REF_ID",
                        column: x => x.RENTAL_AGREEMENT_REF_ID,
                        principalTable: "HET_RENTAL_AGREEMENT",
                        principalColumn: "RENTAL_AGREEMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HET_TIME_RECORD_RENTAL_AGREEMENT_RATE_REF_ID",
                table: "HET_TIME_RECORD",
                column: "RENTAL_AGREEMENT_RATE_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "ASK_NEXT_BLOCK1_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK2_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "ASK_NEXT_BLOCK2_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK_OPEN_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "ASK_NEXT_BLOCK_OPEN_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_EQUIPMENT_TYPE_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "EQUIPMENT_TYPE_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_RENTAL_AGREEMENT_CONDITION_RENTAL_AGREEMENT_REF_ID",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                column: "RENTAL_AGREEMENT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_RENTAL_AGREEMENT_RATE_RENTAL_AGREEMENT_REF_ID",
                table: "HET_RENTAL_AGREEMENT_RATE",
                column: "RENTAL_AGREEMENT_REF_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_TIME_RECORD_HET_RENTAL_AGREEMENT_RATE_RENTAL_AGREEMENT_RATE_REF_ID",
                table: "HET_TIME_RECORD",
                column: "RENTAL_AGREEMENT_RATE_REF_ID",
                principalTable: "HET_RENTAL_AGREEMENT_RATE",
                principalColumn: "RENTAL_AGREEMENT_RATE_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_TIME_RECORD_HET_RENTAL_AGREEMENT_RATE_RENTAL_AGREEMENT_RATE_REF_ID",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropTable(
                name: "HET_EQUIPMENT_TYPE_NEXT_RENTAL");

            migrationBuilder.DropTable(
                name: "HET_RENTAL_AGREEMENT_CONDITION");

            migrationBuilder.DropTable(
                name: "HET_RENTAL_AGREEMENT_RATE");

            migrationBuilder.DropIndex(
                name: "IX_HET_TIME_RECORD_RENTAL_AGREEMENT_RATE_REF_ID",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_USER_ROLE");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_USER_ROLE");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_USER_ROLE");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_USER_ROLE");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_USER");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_USER");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_USER");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_USER");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "RENTAL_AGREEMENT_RATE_REF_ID",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_ROLE");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_ROLE");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_ROLE");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_ROLE");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "DATED_ON",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "EQUIPMENT_RATE",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "ESTIMATE_HOURS",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "ESTIMATE_START_WORK",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "NOTE",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "NUMBER",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "RATE_COMMENT",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "RATE_PERIOD",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_REGION");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_REGION");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_REGION");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_REGION");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_PERMISSION");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_PERMISSION");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_PERMISSION");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_PERMISSION");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "WORK_SAFE_BCPOLICY_NUMBER",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_NOTE");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_NOTE");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_NOTE");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_NOTE");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_HISTORY");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_HISTORY");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_HISTORY");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_HISTORY");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_GROUP");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_GROUP");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_GROUP");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_GROUP");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "ATTACHMENT",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_DISTRICT");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_DISTRICT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_DISTRICT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_DISTRICT");

            migrationBuilder.DropColumn(
                name: "ADDRESS1",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "ADDRESS2",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "CITY",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "EMAIL_ADDRESS",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "FAX_PHONE_NUMBER",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "MOBILE_PHONE_NUMBER",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "ORGANIZATION_NAME",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "POSTAL_CODE",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "PROVINCE",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "WORK_PHONE_NUMBER",
                table: "HET_CONTACT");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_CITY");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_CITY");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_CITY");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_CITY");

            migrationBuilder.DropColumn(
                name: "CREATE_TIMESTAMP",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "FILE_CONTENTS",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_TIMESTAMP",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "LAST_UPDATE_USERID",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "TYPE",
                table: "HET_ATTACHMENT");

            migrationBuilder.RenameColumn(
                name: "WORK_SAFE_BCEXPIRY_DATE",
                table: "HET_OWNER",
                newName: "WCBEXPIRY_DATE");

            migrationBuilder.RenameColumn(
                name: "FILE_NAME",
                table: "HET_ATTACHMENT",
                newName: "INTERNAL_FILE_NAME");

            migrationBuilder.AddColumn<float>(
                name: "HOURS2",
                table: "HET_TIME_RECORD",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "HOURS3",
                table: "HET_TIME_RECORD",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "RATE",
                table: "HET_TIME_RECORD",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "RATE2",
                table: "HET_TIME_RECORD",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "RATE3",
                table: "HET_TIME_RECORD",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SEQ_NUM",
                table: "HET_EQUIPMENT_ATTACHMENT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TYPE_REF_ID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EXTERNAL_FILE_NAME",
                table: "HET_ATTACHMENT",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HET_CONTACT_ADDRESS",
                columns: table => new
                {
                    CONTACT_ADDRESS_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ADDRESS_LINE1 = table.Column<string>(maxLength: 150, nullable: true),
                    ADDRESS_LINE2 = table.Column<string>(maxLength: 150, nullable: true),
                    CITY = table.Column<string>(maxLength: 100, nullable: true),
                    CONTACT_ID = table.Column<int>(nullable: true),
                    POSTAL_CODE = table.Column<string>(maxLength: 15, nullable: true),
                    PROVINCE = table.Column<string>(maxLength: 50, nullable: true),
                    TYPE = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_CONTACT_ADDRESS", x => x.CONTACT_ADDRESS_ID);
                    table.ForeignKey(
                        name: "FK_HET_CONTACT_ADDRESS_HET_CONTACT_CONTACT_ID",
                        column: x => x.CONTACT_ID,
                        principalTable: "HET_CONTACT",
                        principalColumn: "CONTACT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_CONTACT_PHONE",
                columns: table => new
                {
                    CONTACT_PHONE_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CONTACT_ID = table.Column<int>(nullable: true),
                    PHONE_NUMBER = table.Column<string>(maxLength: 20, nullable: true),
                    TYPE = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_CONTACT_PHONE", x => x.CONTACT_PHONE_ID);
                    table.ForeignKey(
                        name: "FK_HET_CONTACT_PHONE_HET_CONTACT_CONTACT_ID",
                        column: x => x.CONTACT_ID,
                        principalTable: "HET_CONTACT",
                        principalColumn: "CONTACT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_EQUIPMENT_ATTACHMENT_TYPE",
                columns: table => new
                {
                    EQUIPMENT_ATTACHMENT_TYPE_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CODE = table.Column<string>(maxLength: 50, nullable: true),
                    DESCRIPTION = table.Column<string>(maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_EQUIPMENT_ATTACHMENT_TYPE", x => x.EQUIPMENT_ATTACHMENT_TYPE_ID);
                });

            migrationBuilder.CreateTable(
                name: "HET_FAVOURITE_CONTEXT_TYPE",
                columns: table => new
                {
                    FAVOURITE_CONTEXT_TYPE_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NAME = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_FAVOURITE_CONTEXT_TYPE", x => x.FAVOURITE_CONTEXT_TYPE_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_TYPE_ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE",
                column: "ASK_NEXT_BLOCK1_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_ATTACHMENT_TYPE_REF_ID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                column: "TYPE_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_CONTACT_ADDRESS_CONTACT_ID",
                table: "HET_CONTACT_ADDRESS",
                column: "CONTACT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_CONTACT_PHONE_CONTACT_ID",
                table: "HET_CONTACT_PHONE",
                column: "CONTACT_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_ATTACHMENT_HET_EQUIPMENT_ATTACHMENT_TYPE_TYPE_REF_ID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                column: "TYPE_REF_ID",
                principalTable: "HET_EQUIPMENT_ATTACHMENT_TYPE",
                principalColumn: "EQUIPMENT_ATTACHMENT_TYPE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_HET_EQUIPMENT_ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE",
                column: "ASK_NEXT_BLOCK1_REF_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
