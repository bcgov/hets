using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HETSAPI.Migrations
{
    public partial class HETS781 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_HET_DUMP_TRUCK_DUMP_TRUCK_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropTable(
                name: "HET_DUMP_TRUCK");

            migrationBuilder.DropIndex(
                name: "IX_HET_EQUIPMENT_DUMP_TRUCK_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "DUMP_TRUCK_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.AddColumn<string>(
                name: "CGL_POLICY_NUMBER",
                table: "HET_OWNER",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LEGAL_CAPACITY",
                table: "HET_EQUIPMENT",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LICENCED_GVW",
                table: "HET_EQUIPMENT",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PUP_LEGAL_CAPACITY",
                table: "HET_EQUIPMENT",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CGL_POLICY_NUMBER",
                table: "HET_OWNER");

            migrationBuilder.DropColumn(
                name: "LEGAL_CAPACITY",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "LICENCED_GVW",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "PUP_LEGAL_CAPACITY",
                table: "HET_EQUIPMENT");

            migrationBuilder.AddColumn<int>(
                name: "DUMP_TRUCK_ID",
                table: "HET_EQUIPMENT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HET_DUMP_TRUCK",
                columns: table => new
                {
                    DUMP_TRUCK_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(maxLength: 50, nullable: true),
                    APP_CREATE_USER_GUID = table.Column<string>(maxLength: 255, nullable: true),
                    APP_CREATE_USERID = table.Column<string>(maxLength: 255, nullable: true),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(maxLength: 50, nullable: true),
                    APP_LAST_UPDATE_USER_GUID = table.Column<string>(maxLength: 255, nullable: true),
                    APP_LAST_UPDATE_USERID = table.Column<string>(maxLength: 255, nullable: true),
                    BOX_CAPACITY = table.Column<string>(maxLength: 150, nullable: true),
                    BOX_HEIGHT = table.Column<string>(maxLength: 150, nullable: true),
                    BOX_LENGTH = table.Column<string>(maxLength: 150, nullable: true),
                    BOX_WIDTH = table.Column<string>(maxLength: 150, nullable: true),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    DB_CREATE_USER_ID = table.Column<string>(maxLength: 63, nullable: true),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    DB_LAST_UPDATE_USER_ID = table.Column<string>(maxLength: 63, nullable: true),
                    FRONT_AXLE_CAPACITY = table.Column<string>(maxLength: 150, nullable: true),
                    FRONT_TIRE_SIZE = table.Column<string>(maxLength: 150, nullable: true),
                    FRONT_TIRE_UOM = table.Column<string>(maxLength: 150, nullable: true),
                    HAS_BELLY_DUMP = table.Column<bool>(nullable: true),
                    HAS_HILIFT_GATE = table.Column<bool>(nullable: true),
                    HAS_PUP = table.Column<bool>(nullable: true),
                    HAS_ROCK_BOX = table.Column<bool>(nullable: true),
                    HAS_SEALCOAT_HITCH = table.Column<bool>(nullable: true),
                    IS_SINGLE_AXLE = table.Column<bool>(nullable: true),
                    IS_TANDEM_AXLE = table.Column<bool>(nullable: true),
                    IS_TRIDEM = table.Column<bool>(nullable: true),
                    IS_WATER_TRUCK = table.Column<bool>(nullable: true),
                    LEGAL_CAPACITY = table.Column<string>(maxLength: 150, nullable: true),
                    LEGAL_LOAD = table.Column<string>(maxLength: 150, nullable: true),
                    LEGAL_PUPTARE_WEIGHT = table.Column<string>(maxLength: 150, nullable: true),
                    LICENCED_CAPACITY = table.Column<string>(maxLength: 150, nullable: true),
                    LICENCED_GVW = table.Column<string>(maxLength: 150, nullable: true),
                    LICENCED_GVWUOM = table.Column<string>(maxLength: 150, nullable: true),
                    LICENCED_LOAD = table.Column<string>(maxLength: 150, nullable: true),
                    LICENCED_PUPTARE_WEIGHT = table.Column<string>(maxLength: 150, nullable: true),
                    LICENCED_TARE_WEIGHT = table.Column<string>(maxLength: 150, nullable: true),
                    REAR_AXLE_CAPACITY = table.Column<string>(maxLength: 150, nullable: true),
                    REAR_AXLE_SPACING = table.Column<string>(maxLength: 150, nullable: true),
                    TRAILER_BOX_CAPACITY = table.Column<string>(maxLength: 150, nullable: true),
                    TRAILER_BOX_HEIGHT = table.Column<string>(maxLength: 150, nullable: true),
                    TRAILER_BOX_LENGTH = table.Column<string>(maxLength: 150, nullable: true),
                    TRAILER_BOX_WIDTH = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_DUMP_TRUCK", x => x.DUMP_TRUCK_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_DUMP_TRUCK_ID",
                table: "HET_EQUIPMENT",
                column: "DUMP_TRUCK_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_HET_DUMP_TRUCK_DUMP_TRUCK_ID",
                table: "HET_EQUIPMENT",
                column: "DUMP_TRUCK_ID",
                principalTable: "HET_DUMP_TRUCK",
                principalColumn: "DUMP_TRUCK_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
