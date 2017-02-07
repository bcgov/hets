using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS83271 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JOB_DESC1",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "JOB_DESC2",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "ARCHIVE_CD",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "CONTACT_PERSON",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "LOCAL_TO_AREA",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "ACCEPTED_OFFER",
                table: "HET_HIRE_OFFER");

            migrationBuilder.DropColumn(
                name: "SECOND_BLK",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "APPROVAL",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "ARCHIVE_CD",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "BELLY_DUMP",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "HILIFT_GATE",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "PUP",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "ROCK_BOX",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "SEAL_COAT_HITCH",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "SINGLE_AXLE",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "TANDEM_AXLE",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "TRIDEM",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "WATER_TRUCK",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.RenameColumn(
                name: "YTD3",
                table: "HET_SENIORITY_AUDIT",
                newName: "SERVICE_HOURS_TWO_YEARS_AGO");

            migrationBuilder.RenameColumn(
                name: "YTD2",
                table: "HET_SENIORITY_AUDIT",
                newName: "SERVICE_HOURS_THREE_YEARS_AGO");

            migrationBuilder.RenameColumn(
                name: "YTD1",
                table: "HET_SENIORITY_AUDIT",
                newName: "SERVICE_HOURS_LAST_YEAR");

            migrationBuilder.RenameColumn(
                name: "BLOCK_NUM",
                table: "HET_SENIORITY_AUDIT",
                newName: "BLOCK_NUMBER");

            migrationBuilder.RenameColumn(
                name: "PROJECT_NUM",
                table: "HET_PROJECT",
                newName: "PROVINCIAL_PROJECT_NUMBER");

            migrationBuilder.RenameColumn(
                name: "STATUS_CD",
                table: "HET_OWNER",
                newName: "STATUS");

            migrationBuilder.RenameColumn(
                name: "OWNER_CD",
                table: "HET_OWNER",
                newName: "OWNER_CODE_PREFIX");

            migrationBuilder.RenameColumn(
                name: "MAINTENANCE_CONTRACTOR",
                table: "HET_OWNER",
                newName: "ARCHIVE_CODE");

            migrationBuilder.RenameColumn(
                name: "_NOTE",
                table: "HET_NOTE",
                newName: "TEXT");

            migrationBuilder.RenameColumn(
                name: "ASKED_DATE",
                table: "HET_HIRE_OFFER",
                newName: "ASKED_DATE_TIME");

            migrationBuilder.RenameColumn(
                name: "ASKED",
                table: "HET_HIRE_OFFER",
                newName: "WAS_ASKED");

            migrationBuilder.RenameColumn(
                name: "YTD3",
                table: "HET_EQUIPMENT",
                newName: "SERVICE_HOURS_TWO_YEARS_AGO");

            migrationBuilder.RenameColumn(
                name: "YTD2",
                table: "HET_EQUIPMENT",
                newName: "SERVICE_HOURS_THREE_YEARS_AGO");

            migrationBuilder.RenameColumn(
                name: "YTD1",
                table: "HET_EQUIPMENT",
                newName: "SERVICE_HOURS_LAST_YEAR");

            migrationBuilder.RenameColumn(
                name: "STATUS_CD",
                table: "HET_EQUIPMENT",
                newName: "STATUS");

            migrationBuilder.RenameColumn(
                name: "REG_DUMP_TRUCK",
                table: "HET_EQUIPMENT",
                newName: "LICENCE_PLATE");

            migrationBuilder.RenameColumn(
                name: "LICENCE",
                table: "HET_EQUIPMENT",
                newName: "EQUIP_CODE");

            migrationBuilder.RenameColumn(
                name: "EQUIP_CD",
                table: "HET_EQUIPMENT",
                newName: "ARCHIVE_CODE");

            migrationBuilder.AlterColumn<string>(
                name: "TYPE",
                table: "HET_USER_FAVOURITE",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SURNAME",
                table: "HET_USER",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "INITIALS",
                table: "HET_USER",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TIME_PERIOD",
                table: "HET_TIME_RECORD",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MINISTRY_SERVICE_AREA_ID",
                table: "HET_SERVICE_AREA",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MINISTRY_REGION_ID",
                table: "HET_REGION",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DESCRIPTION",
                table: "HET_PROJECT",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ARCHIVE_REASON",
                table: "HET_OWNER",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IS_MAINTENANCE_CONTRACTOR",
                table: "HET_OWNER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OFFER_RESPONSE",
                table: "HET_HIRE_OFFER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BLOCKS",
                table: "HET_EQUIPMENT_TYPE",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DUMP_TRUCK_REF_ID",
                table: "HET_EQUIPMENT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HAS_BELLY_DUMP",
                table: "HET_DUMP_TRUCK",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HAS_HILIFT_GATE",
                table: "HET_DUMP_TRUCK",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HAS_PUP",
                table: "HET_DUMP_TRUCK",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HAS_ROCK_BOX",
                table: "HET_DUMP_TRUCK",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HAS_SEALCOAT_HITCH",
                table: "HET_DUMP_TRUCK",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IS_SINGLE_AXLE",
                table: "HET_DUMP_TRUCK",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IS_TANDEM_AXLE",
                table: "HET_DUMP_TRUCK",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IS_TRIDEM",
                table: "HET_DUMP_TRUCK",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IS_WATER_TRUCK",
                table: "HET_DUMP_TRUCK",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MINISTRY_DISTRICT_ID",
                table: "HET_DISTRICT",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TYPE",
                table: "HET_CONTACT_PHONE",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PHONE_NUMBER",
                table: "HET_CONTACT_PHONE",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TYPE",
                table: "HET_CONTACT_ADDRESS",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PROVINCE",
                table: "HET_CONTACT_ADDRESS",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "POSTAL_CODE",
                table: "HET_CONTACT_ADDRESS",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CITY",
                table: "HET_CONTACT_ADDRESS",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SURNAME",
                table: "HET_CONTACT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ROLE",
                table: "HET_CONTACT",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NOTES",
                table: "HET_CONTACT",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GIVEN_NAME",
                table: "HET_CONTACT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "HET_ATTACHMENT",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_DUMP_TRUCK_REF_ID",
                table: "HET_EQUIPMENT",
                column: "DUMP_TRUCK_REF_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_HET_DUMP_TRUCK_DUMP_TRUCK_REF_ID",
                table: "HET_EQUIPMENT",
                column: "DUMP_TRUCK_REF_ID",
                principalTable: "HET_DUMP_TRUCK",
                principalColumn: "DUMP_TRUCK_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_HET_DUMP_TRUCK_DUMP_TRUCK_REF_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropIndex(
                name: "IX_HET_EQUIPMENT_DUMP_TRUCK_REF_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "TIME_PERIOD",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropColumn(
                name: "DESCRIPTION",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "IS_MAINTENANCE_CONTRACTOR",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "OFFER_RESPONSE",
                table: "HET_HIRE_OFFER");

            migrationBuilder.DropColumn(
                name: "BLOCKS",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "DUMP_TRUCK_REF_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "HAS_BELLY_DUMP",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "HAS_HILIFT_GATE",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "HAS_PUP",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "HAS_ROCK_BOX",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "HAS_SEALCOAT_HITCH",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "IS_SINGLE_AXLE",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "IS_TANDEM_AXLE",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "IS_TRIDEM",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.DropColumn(
                name: "IS_WATER_TRUCK",
                table: "HET_DUMP_TRUCK");

            migrationBuilder.RenameColumn(
                name: "SERVICE_HOURS_TWO_YEARS_AGO",
                table: "HET_SENIORITY_AUDIT",
                newName: "YTD3");

            migrationBuilder.RenameColumn(
                name: "SERVICE_HOURS_THREE_YEARS_AGO",
                table: "HET_SENIORITY_AUDIT",
                newName: "YTD2");

            migrationBuilder.RenameColumn(
                name: "SERVICE_HOURS_LAST_YEAR",
                table: "HET_SENIORITY_AUDIT",
                newName: "YTD1");

            migrationBuilder.RenameColumn(
                name: "BLOCK_NUMBER",
                table: "HET_SENIORITY_AUDIT",
                newName: "BLOCK_NUM");

            migrationBuilder.RenameColumn(
                name: "PROVINCIAL_PROJECT_NUMBER",
                table: "HET_PROJECT",
                newName: "PROJECT_NUM");

            migrationBuilder.RenameColumn(
                name: "STATUS",
                table: "HET_OWNER",
                newName: "STATUS_CD");

            migrationBuilder.RenameColumn(
                name: "OWNER_CODE_PREFIX",
                table: "HET_OWNER",
                newName: "OWNER_CD");

            migrationBuilder.RenameColumn(
                name: "ARCHIVE_CODE",
                table: "HET_OWNER",
                newName: "MAINTENANCE_CONTRACTOR");

            migrationBuilder.RenameColumn(
                name: "TEXT",
                table: "HET_NOTE",
                newName: "_NOTE");

            migrationBuilder.RenameColumn(
                name: "WAS_ASKED",
                table: "HET_HIRE_OFFER",
                newName: "ASKED");

            migrationBuilder.RenameColumn(
                name: "ASKED_DATE_TIME",
                table: "HET_HIRE_OFFER",
                newName: "ASKED_DATE");

            migrationBuilder.RenameColumn(
                name: "STATUS",
                table: "HET_EQUIPMENT",
                newName: "STATUS_CD");

            migrationBuilder.RenameColumn(
                name: "SERVICE_HOURS_TWO_YEARS_AGO",
                table: "HET_EQUIPMENT",
                newName: "YTD3");

            migrationBuilder.RenameColumn(
                name: "SERVICE_HOURS_THREE_YEARS_AGO",
                table: "HET_EQUIPMENT",
                newName: "YTD2");

            migrationBuilder.RenameColumn(
                name: "SERVICE_HOURS_LAST_YEAR",
                table: "HET_EQUIPMENT",
                newName: "YTD1");

            migrationBuilder.RenameColumn(
                name: "LICENCE_PLATE",
                table: "HET_EQUIPMENT",
                newName: "REG_DUMP_TRUCK");

            migrationBuilder.RenameColumn(
                name: "EQUIP_CODE",
                table: "HET_EQUIPMENT",
                newName: "LICENCE");

            migrationBuilder.RenameColumn(
                name: "ARCHIVE_CODE",
                table: "HET_EQUIPMENT",
                newName: "EQUIP_CD");

            migrationBuilder.AlterColumn<string>(
                name: "TYPE",
                table: "HET_USER_FAVOURITE",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SURNAME",
                table: "HET_USER",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "INITIALS",
                table: "HET_USER",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MINISTRY_SERVICE_AREA_ID",
                table: "HET_SERVICE_AREA",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MINISTRY_REGION_ID",
                table: "HET_REGION",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "JOB_DESC1",
                table: "HET_PROJECT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JOB_DESC2",
                table: "HET_PROJECT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ARCHIVE_REASON",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ARCHIVE_CD",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CONTACT_PERSON",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LOCAL_TO_AREA",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ACCEPTED_OFFER",
                table: "HET_HIRE_OFFER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SECOND_BLK",
                table: "HET_EQUIPMENT_TYPE",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "APPROVAL",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ARCHIVE_CD",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BELLY_DUMP",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HILIFT_GATE",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PUP",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ROCK_BOX",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SEAL_COAT_HITCH",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SINGLE_AXLE",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TANDEM_AXLE",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TRIDEM",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WATER_TRUCK",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MINISTRY_DISTRICT_ID",
                table: "HET_DISTRICT",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "TYPE",
                table: "HET_CONTACT_PHONE",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PHONE_NUMBER",
                table: "HET_CONTACT_PHONE",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TYPE",
                table: "HET_CONTACT_ADDRESS",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PROVINCE",
                table: "HET_CONTACT_ADDRESS",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "POSTAL_CODE",
                table: "HET_CONTACT_ADDRESS",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CITY",
                table: "HET_CONTACT_ADDRESS",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SURNAME",
                table: "HET_CONTACT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ROLE",
                table: "HET_CONTACT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NOTES",
                table: "HET_CONTACT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GIVEN_NAME",
                table: "HET_CONTACT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "HET_ATTACHMENT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2048,
                oldNullable: true);
        }
    }
}
