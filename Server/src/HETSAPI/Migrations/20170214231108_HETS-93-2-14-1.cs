using System;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS932141 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_HET_DUMP_TRUCK_DUMP_TRUCK_DETAILS_REF_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_REQUEST_HET_ROTATION_LIST_ROTATION_LIST_REF_ID",
                table: "HET_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_SENIORITY_AUDIT_HET_PROJECT_PROJECT_REF_ID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropIndex(
                name: "IX_HET_SENIORITY_AUDIT_PROJECT_REF_ID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropIndex(
                name: "IX_HET_EQUIPMENT_DUMP_TRUCK_DETAILS_REF_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "CYCLE_HRS_WRK",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "EQUIP_CD",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "FROZEN_OUT",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "OWNER_NAME",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "PROJECT_REF_ID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "WORKING",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "YEAR_END_REG",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "CGLCOMPANY",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "CGLPOLICY",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "COMMENT",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "OWNER_CODE_PREFIX",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "OWNER_FIRST_NAME",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "OWNER_LAST_NAME",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "WCBNUM",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "ADDRESS_LINE1",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "ADDRESS_LINE2",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "ADDRESS_LINE3",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "ADDRESS_LINE4",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "CITY",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "COMMENT",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "CYCLE_HRS_WRK",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "DRAFT_BLOCK_NUM",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "DUMP_TRUCK_DETAILS_REF_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "FROZEN_OUT",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "NUM_YEARS",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "POSTAL",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "PREV_REG_AREA",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "WORKING",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "YEAR_END_REG",
                table: "HET_EQUIPMENT");

            migrationBuilder.RenameColumn(
                name: "YTD",
                table: "HET_SENIORITY_AUDIT",
                newName: "SERVICE_HOURS_CURRENT_YEAR_TO_DATE");

            migrationBuilder.RenameColumn(
                name: "GENERATED_TIME",
                table: "HET_SENIORITY_AUDIT",
                newName: "START_DATE");

            migrationBuilder.RenameColumn(
                name: "ROTATION_LIST_REF_ID",
                table: "HET_REQUEST",
                newName: "FIRST_ON_ROTATION_LIST_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_REQUEST_ROTATION_LIST_REF_ID",
                table: "HET_REQUEST",
                newName: "IX_HET_REQUEST_FIRST_ON_ROTATION_LIST_REF_ID");

            migrationBuilder.RenameColumn(
                name: "DESCRIPTION",
                table: "HET_PROJECT",
                newName: "INFORMATION");

            migrationBuilder.RenameColumn(
                name: "CGLSTART_DATE",
                table: "HET_OWNER",
                newName: "ARCHIVE_DATE");

            migrationBuilder.RenameColumn(
                name: "YTD",
                table: "HET_EQUIPMENT",
                newName: "YEARS_OF_SERVICE");

            migrationBuilder.AlterColumn<string>(
                name: "TYPE",
                table: "HET_USER_FAVOURITE",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_USER_FAVOURITE",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GIVEN_NAME",
                table: "HET_USER",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "START_DATE",
                table: "HET_SERVICE_AREA",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_SERVICE_AREA",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AREA_NUMBER",
                table: "HET_SERVICE_AREA",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "END_DATE",
                table: "HET_SENIORITY_AUDIT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OWNER_ORGANIZATION_NAME",
                table: "HET_SENIORITY_AUDIT",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "HET_ROLE",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "START_DATE",
                table: "HET_REGION",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_REGION",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "REGION_NUMBER",
                table: "HET_REGION",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PROVINCIAL_PROJECT_NUMBER",
                table: "HET_PROJECT",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NAME",
                table: "HET_PROJECT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_PERMISSION",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "HET_PERMISSION",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CODE",
                table: "HET_PERMISSION",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "STATUS",
                table: "HET_OWNER",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ARCHIVE_CODE",
                table: "HET_OWNER",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ORGANIZATION_NAME",
                table: "HET_OWNER",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OWNER_EQUIPMENT_CODE_PREFIX",
                table: "HET_OWNER",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_LOCAL_AREA",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "END_DATE",
                table: "HET_LOCAL_AREA",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LOCAL_AREA_NUMBER",
                table: "HET_LOCAL_AREA",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "START_DATE",
                table: "HET_LOCAL_AREA",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_GROUP",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "HET_GROUP",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_FAVOURITE_CONTEXT_TYPE",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "HET_EQUIPMENT_TYPE",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CODE",
                table: "HET_EQUIPMENT_TYPE",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "HET_EQUIPMENT_ATTACHMENT_TYPE",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CODE",
                table: "HET_EQUIPMENT_ATTACHMENT_TYPE",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "HET_EQUIPMENT_ATTACHMENT",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YEAR",
                table: "HET_EQUIPMENT",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "STATUS",
                table: "HET_EQUIPMENT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SIZE",
                table: "HET_EQUIPMENT",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SERIAL_NUM",
                table: "HET_EQUIPMENT",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MODEL",
                table: "HET_EQUIPMENT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MAKE",
                table: "HET_EQUIPMENT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LICENCE_PLATE",
                table: "HET_EQUIPMENT",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EQUIP_CODE",
                table: "HET_EQUIPMENT",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ARCHIVE_REASON",
                table: "HET_EQUIPMENT",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ARCHIVE_CODE",
                table: "HET_EQUIPMENT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "INFORMATION_UPDATE_NEEDED_REASON",
                table: "HET_EQUIPMENT",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IS_INFORMATION_UPDATE_NEEDED",
                table: "HET_EQUIPMENT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SENIORITY_EFFECTIVE_DATE",
                table: "HET_EQUIPMENT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TRAILER_BOX_WIDTH",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TRAILER_BOX_LENGTH",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TRAILER_BOX_HEIGHT",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TRAILER_BOX_CAPACITY",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "REAR_AXLE_SPACING",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "REAR_AXLE_CAPACITY",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LICENCED_TARE_WEIGHT",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LICENCED_PUPTARE_WEIGHT",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LICENCED_LOAD",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LICENCED_GVWUOM",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LICENCED_GVW",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LICENCED_CAPACITY",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LEGAL_PUPTARE_WEIGHT",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LEGAL_LOAD",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LEGAL_CAPACITY",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FRONT_TIRE_UOM",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FRONT_TIRE_SIZE",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FRONT_AXLE_CAPACITY",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BOX_WIDTH",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BOX_LENGTH",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BOX_HEIGHT",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BOX_CAPACITY",
                table: "HET_DUMP_TRUCK",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "START_DATE",
                table: "HET_DISTRICT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_DISTRICT",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DISTRICT_NUMBER",
                table: "HET_DISTRICT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ADDRESS_LINE2",
                table: "HET_CONTACT_ADDRESS",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ADDRESS_LINE1",
                table: "HET_CONTACT_ADDRESS",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_CITY",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "HET_REQUEST_ROTATION_LIST",
                columns: table => new
                {
                    REQUEST_ROTATION_LIST_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ASKED_DATE_TIME = table.Column<DateTime>(nullable: true),
                    EQUIPMENT_REF_ID = table.Column<int>(nullable: true),
                    IS_FORCE_HIRE = table.Column<bool>(nullable: true),
                    NOTE = table.Column<string>(maxLength: 2048, nullable: true),
                    OFFER_RESPONSE = table.Column<string>(nullable: true),
                    REFUSE_REASON = table.Column<string>(maxLength: 2048, nullable: true),
                    RENTAL_AGREEMENT_REF_ID = table.Column<int>(nullable: true),
                    REQUEST_REF_ID = table.Column<int>(nullable: true),
                    ROTATION_LIST_SORT_ORDER = table.Column<int>(nullable: true),
                    WAS_ASKED = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_REQUEST_ROTATION_LIST", x => x.REQUEST_ROTATION_LIST_ID);
                    table.ForeignKey(
                        name: "FK_HET_REQUEST_ROTATION_LIST_HET_EQUIPMENT_EQUIPMENT_REF_ID",
                        column: x => x.EQUIPMENT_REF_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_REQUEST_ROTATION_LIST_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_REF_ID",
                        column: x => x.RENTAL_AGREEMENT_REF_ID,
                        principalTable: "HET_RENTAL_AGREEMENT",
                        principalColumn: "RENTAL_AGREEMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_REQUEST_ROTATION_LIST_HET_REQUEST_REQUEST_REF_ID",
                        column: x => x.REQUEST_REF_ID,
                        principalTable: "HET_REQUEST",
                        principalColumn: "REQUEST_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HET_REQUEST_ROTATION_LIST_EQUIPMENT_REF_ID",
                table: "HET_REQUEST_ROTATION_LIST",
                column: "EQUIPMENT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_REQUEST_ROTATION_LIST_RENTAL_AGREEMENT_REF_ID",
                table: "HET_REQUEST_ROTATION_LIST",
                column: "RENTAL_AGREEMENT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_REQUEST_ROTATION_LIST_REQUEST_REF_ID",
                table: "HET_REQUEST_ROTATION_LIST",
                column: "REQUEST_REF_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_REQUEST_HET_EQUIPMENT_FIRST_ON_ROTATION_LIST_REF_ID",
                table: "HET_REQUEST",
                column: "FIRST_ON_ROTATION_LIST_REF_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_REQUEST_HET_EQUIPMENT_FIRST_ON_ROTATION_LIST_REF_ID",
                table: "HET_REQUEST");

            migrationBuilder.DropTable(
                name: "HET_REQUEST_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "AREA_NUMBER",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropColumn(
                name: "END_DATE",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "OWNER_ORGANIZATION_NAME",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropColumn(
                name: "REGION_NUMBER",
                table: "HET_REGION");

            migrationBuilder.DropColumn(
                name: "NAME",
                table: "HET_PROJECT");

            migrationBuilder.DropColumn(
                name: "ORGANIZATION_NAME",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "OWNER_EQUIPMENT_CODE_PREFIX",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "END_DATE",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropColumn(
                name: "LOCAL_AREA_NUMBER",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropColumn(
                name: "START_DATE",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropColumn(
                name: "INFORMATION_UPDATE_NEEDED_REASON",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "IS_INFORMATION_UPDATE_NEEDED",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "SENIORITY_EFFECTIVE_DATE",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "DISTRICT_NUMBER",
                table: "HET_DISTRICT");

            migrationBuilder.RenameColumn(
                name: "START_DATE",
                table: "HET_SENIORITY_AUDIT",
                newName: "GENERATED_TIME");

            migrationBuilder.RenameColumn(
                name: "SERVICE_HOURS_CURRENT_YEAR_TO_DATE",
                table: "HET_SENIORITY_AUDIT",
                newName: "YTD");

            migrationBuilder.RenameColumn(
                name: "FIRST_ON_ROTATION_LIST_REF_ID",
                table: "HET_REQUEST",
                newName: "ROTATION_LIST_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_REQUEST_FIRST_ON_ROTATION_LIST_REF_ID",
                table: "HET_REQUEST",
                newName: "IX_HET_REQUEST_ROTATION_LIST_REF_ID");

            migrationBuilder.RenameColumn(
                name: "INFORMATION",
                table: "HET_PROJECT",
                newName: "DESCRIPTION");

            migrationBuilder.RenameColumn(
                name: "ARCHIVE_DATE",
                table: "HET_OWNER",
                newName: "CGLSTART_DATE");

            migrationBuilder.RenameColumn(
                name: "YEARS_OF_SERVICE",
                table: "HET_EQUIPMENT",
                newName: "YTD");

            migrationBuilder.AlterColumn<string>(
                name: "TYPE",
                table: "HET_USER_FAVOURITE",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_USER_FAVOURITE",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GIVEN_NAME",
                table: "HET_USER",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "START_DATE",
                table: "HET_SERVICE_AREA",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_SERVICE_AREA",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "CYCLE_HRS_WRK",
                table: "HET_SENIORITY_AUDIT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EQUIP_CD",
                table: "HET_SENIORITY_AUDIT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FROZEN_OUT",
                table: "HET_SENIORITY_AUDIT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OWNER_NAME",
                table: "HET_SENIORITY_AUDIT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PROJECT_REF_ID",
                table: "HET_SENIORITY_AUDIT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WORKING",
                table: "HET_SENIORITY_AUDIT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YEAR_END_REG",
                table: "HET_SENIORITY_AUDIT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "HET_ROLE",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "START_DATE",
                table: "HET_REGION",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_REGION",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PROVINCIAL_PROJECT_NUMBER",
                table: "HET_PROJECT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_PERMISSION",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "HET_PERMISSION",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CODE",
                table: "HET_PERMISSION",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "STATUS",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ARCHIVE_CODE",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CGLCOMPANY",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CGLPOLICY",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "COMMENT",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OWNER_CODE_PREFIX",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OWNER_FIRST_NAME",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OWNER_LAST_NAME",
                table: "HET_OWNER",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WCBNUM",
                table: "HET_OWNER",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_LOCAL_AREA",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_GROUP",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "HET_GROUP",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_FAVOURITE_CONTEXT_TYPE",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "HET_EQUIPMENT_TYPE",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CODE",
                table: "HET_EQUIPMENT_TYPE",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "HET_EQUIPMENT_ATTACHMENT_TYPE",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CODE",
                table: "HET_EQUIPMENT_ATTACHMENT_TYPE",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "HET_EQUIPMENT_ATTACHMENT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YEAR",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "STATUS",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SIZE",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SERIAL_NUM",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MODEL",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MAKE",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LICENCE_PLATE",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EQUIP_CODE",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ARCHIVE_REASON",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ARCHIVE_CODE",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ADDRESS_LINE1",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ADDRESS_LINE2",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ADDRESS_LINE3",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ADDRESS_LINE4",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CITY",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "COMMENT",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "CYCLE_HRS_WRK",
                table: "HET_EQUIPMENT",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DRAFT_BLOCK_NUM",
                table: "HET_EQUIPMENT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DUMP_TRUCK_DETAILS_REF_ID",
                table: "HET_EQUIPMENT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FROZEN_OUT",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "NUM_YEARS",
                table: "HET_EQUIPMENT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "POSTAL",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PREV_REG_AREA",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WORKING",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YEAR_END_REG",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TRAILER_BOX_WIDTH",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TRAILER_BOX_LENGTH",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TRAILER_BOX_HEIGHT",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TRAILER_BOX_CAPACITY",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "REAR_AXLE_SPACING",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "REAR_AXLE_CAPACITY",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LICENCED_TARE_WEIGHT",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LICENCED_PUPTARE_WEIGHT",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LICENCED_LOAD",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LICENCED_GVWUOM",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LICENCED_GVW",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LICENCED_CAPACITY",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LEGAL_PUPTARE_WEIGHT",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LEGAL_LOAD",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LEGAL_CAPACITY",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FRONT_TIRE_UOM",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FRONT_TIRE_SIZE",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FRONT_AXLE_CAPACITY",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BOX_WIDTH",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BOX_LENGTH",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BOX_HEIGHT",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BOX_CAPACITY",
                table: "HET_DUMP_TRUCK",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "START_DATE",
                table: "HET_DISTRICT",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_DISTRICT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ADDRESS_LINE2",
                table: "HET_CONTACT_ADDRESS",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ADDRESS_LINE1",
                table: "HET_CONTACT_ADDRESS",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NAME",
                table: "HET_CITY",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HET_SENIORITY_AUDIT_PROJECT_REF_ID",
                table: "HET_SENIORITY_AUDIT",
                column: "PROJECT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_DUMP_TRUCK_DETAILS_REF_ID",
                table: "HET_EQUIPMENT",
                column: "DUMP_TRUCK_DETAILS_REF_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_HET_DUMP_TRUCK_DUMP_TRUCK_DETAILS_REF_ID",
                table: "HET_EQUIPMENT",
                column: "DUMP_TRUCK_DETAILS_REF_ID",
                principalTable: "HET_DUMP_TRUCK",
                principalColumn: "DUMP_TRUCK_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_REQUEST_HET_ROTATION_LIST_ROTATION_LIST_REF_ID",
                table: "HET_REQUEST",
                column: "ROTATION_LIST_REF_ID",
                principalTable: "HET_ROTATION_LIST",
                principalColumn: "ROTATION_LIST_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_SENIORITY_AUDIT_HET_PROJECT_PROJECT_REF_ID",
                table: "HET_SENIORITY_AUDIT",
                column: "PROJECT_REF_ID",
                principalTable: "HET_PROJECT",
                principalColumn: "PROJECT_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
