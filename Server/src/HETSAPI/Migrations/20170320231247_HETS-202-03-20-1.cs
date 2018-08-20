using System;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS20203201 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_HET_LOCAL_AREA_LOCAL_AREA_ID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_PROJECT_HET_LOCAL_AREA_LOCAL_AREA_ID",
                table: "HET_PROJECT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropIndex(
                name: "IX_HET_EQUIPMENT_TYPE_LOCAL_AREA_ID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "BLOCKS",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "DESCRIPTION",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "ATTACHMENT",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "TYPE",
                table: "HET_EQUIPMENT");

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_TYPE_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "DISTRICT_EQUIPMENT_TYPE_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_EQUIPMENT_TYPE_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "IX_HET_RENTAL_REQUEST_DISTRICT_EQUIPMENT_TYPE_ID");

            migrationBuilder.RenameColumn(
                name: "LOCAL_AREA_ID",
                table: "HET_PROJECT",
                newName: "DISTRICT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_PROJECT_LOCAL_AREA_ID",
                table: "HET_PROJECT",
                newName: "IX_HET_PROJECT_DISTRICT_ID");

            migrationBuilder.RenameColumn(
                name: "MAX_HOURS",
                table: "HET_EQUIPMENT_TYPE",
                newName: "MAXIMUM_HOURS");

            migrationBuilder.RenameColumn(
                name: "LOCAL_AREA_ID",
                table: "HET_EQUIPMENT_TYPE",
                newName: "NUMBER_OF_BLOCKS");

            migrationBuilder.RenameColumn(
                name: "EQUIP_RENTAL_RATE_PAGE",
                table: "HET_EQUIPMENT_TYPE",
                newName: "BLUE_BOOK_SECTION");

            migrationBuilder.RenameColumn(
                name: "EQUIP_RENTAL_RATE_NO",
                table: "HET_EQUIPMENT_TYPE",
                newName: "BLUE_BOOK_RATE_NUMBER");

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT",
                newName: "DISTRICT_EQUIPMENT_TYPE_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT",
                newName: "IX_HET_EQUIPMENT_DISTRICT_EQUIPMENT_TYPE_ID");

            migrationBuilder.AddColumn<bool>(
                name: "IS_DUMP_TRUCK",
                table: "HET_EQUIPMENT_TYPE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TYPE_NAME",
                table: "HET_EQUIPMENT_ATTACHMENT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HET_DISTRICT_EQUIPMENT_TYPE",
                columns: table => new
                {
                    DISTRICT_EQUIPMENT_TYPE_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CREATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    CREATE_USERID = table.Column<string>(maxLength: 50, nullable: true),
                    DISTRICT_EQUIPMENT_NAME = table.Column<string>(maxLength: 50, nullable: true),
                    DISTRICT_ID = table.Column<int>(nullable: true),
                    EQUIPMENT_TYPE_ID = table.Column<int>(nullable: true),
                    LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    LAST_UPDATE_USERID = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_DISTRICT_EQUIPMENT_TYPE", x => x.DISTRICT_EQUIPMENT_TYPE_ID);
                    table.ForeignKey(
                        name: "FK_HET_DISTRICT_EQUIPMENT_TYPE_HET_DISTRICT_DISTRICT_ID",
                        column: x => x.DISTRICT_ID,
                        principalTable: "HET_DISTRICT",
                        principalColumn: "DISTRICT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_DISTRICT_EQUIPMENT_TYPE_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                        column: x => x.EQUIPMENT_TYPE_ID,
                        principalTable: "HET_EQUIPMENT_TYPE",
                        principalColumn: "EQUIPMENT_TYPE_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HET_DISTRICT_EQUIPMENT_TYPE_DISTRICT_ID",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                column: "DISTRICT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_DISTRICT_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                table: "HET_DISTRICT_EQUIPMENT_TYPE",
                column: "EQUIPMENT_TYPE_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_HET_DISTRICT_EQUIPMENT_TYPE_DISTRICT_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT",
                column: "DISTRICT_EQUIPMENT_TYPE_ID",
                principalTable: "HET_DISTRICT_EQUIPMENT_TYPE",
                principalColumn: "DISTRICT_EQUIPMENT_TYPE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_DISTRICT_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "EQUIPMENT_TYPE_ID",
                principalTable: "HET_DISTRICT_EQUIPMENT_TYPE",
                principalColumn: "DISTRICT_EQUIPMENT_TYPE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_PROJECT_HET_DISTRICT_DISTRICT_ID",
                table: "HET_PROJECT",
                column: "DISTRICT_ID",
                principalTable: "HET_DISTRICT",
                principalColumn: "DISTRICT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_DISTRICT_EQUIPMENT_TYPE_DISTRICT_EQUIPMENT_TYPE_ID",
                table: "HET_RENTAL_REQUEST",
                column: "DISTRICT_EQUIPMENT_TYPE_ID",
                principalTable: "HET_DISTRICT_EQUIPMENT_TYPE",
                principalColumn: "DISTRICT_EQUIPMENT_TYPE_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_HET_DISTRICT_EQUIPMENT_TYPE_DISTRICT_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_DISTRICT_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_PROJECT_HET_DISTRICT_DISTRICT_ID",
                table: "HET_PROJECT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_DISTRICT_EQUIPMENT_TYPE_DISTRICT_EQUIPMENT_TYPE_ID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropTable(
                name: "HET_DISTRICT_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "IS_DUMP_TRUCK",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "TYPE_NAME",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.RenameColumn(
                name: "DISTRICT_EQUIPMENT_TYPE_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "EQUIPMENT_TYPE_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_DISTRICT_EQUIPMENT_TYPE_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "IX_HET_RENTAL_REQUEST_EQUIPMENT_TYPE_ID");

            migrationBuilder.RenameColumn(
                name: "DISTRICT_ID",
                table: "HET_PROJECT",
                newName: "LOCAL_AREA_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_PROJECT_DISTRICT_ID",
                table: "HET_PROJECT",
                newName: "IX_HET_PROJECT_LOCAL_AREA_ID");

            migrationBuilder.RenameColumn(
                name: "NUMBER_OF_BLOCKS",
                table: "HET_EQUIPMENT_TYPE",
                newName: "LOCAL_AREA_ID");

            migrationBuilder.RenameColumn(
                name: "MAXIMUM_HOURS",
                table: "HET_EQUIPMENT_TYPE",
                newName: "MAX_HOURS");

            migrationBuilder.RenameColumn(
                name: "BLUE_BOOK_SECTION",
                table: "HET_EQUIPMENT_TYPE",
                newName: "EQUIP_RENTAL_RATE_PAGE");

            migrationBuilder.RenameColumn(
                name: "BLUE_BOOK_RATE_NUMBER",
                table: "HET_EQUIPMENT_TYPE",
                newName: "EQUIP_RENTAL_RATE_NO");

            migrationBuilder.RenameColumn(
                name: "DISTRICT_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT",
                newName: "EQUIPMENT_TYPE_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_DISTRICT_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT",
                newName: "IX_HET_EQUIPMENT_EQUIPMENT_TYPE_ID");

            migrationBuilder.AddColumn<int>(
                name: "BLOCKS",
                table: "HET_EQUIPMENT_TYPE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DESCRIPTION",
                table: "HET_EQUIPMENT_TYPE",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ATTACHMENT",
                table: "HET_EQUIPMENT_ATTACHMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TYPE",
                table: "HET_EQUIPMENT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_TYPE_LOCAL_AREA_ID",
                table: "HET_EQUIPMENT_TYPE",
                column: "LOCAL_AREA_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT",
                column: "EQUIPMENT_TYPE_ID",
                principalTable: "HET_EQUIPMENT_TYPE",
                principalColumn: "EQUIPMENT_TYPE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_HET_LOCAL_AREA_LOCAL_AREA_ID",
                table: "HET_EQUIPMENT_TYPE",
                column: "LOCAL_AREA_ID",
                principalTable: "HET_LOCAL_AREA",
                principalColumn: "LOCAL_AREA_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "EQUIPMENT_TYPE_ID",
                principalTable: "HET_EQUIPMENT_TYPE",
                principalColumn: "EQUIPMENT_TYPE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_PROJECT_HET_LOCAL_AREA_LOCAL_AREA_ID",
                table: "HET_PROJECT",
                column: "LOCAL_AREA_ID",
                principalTable: "HET_LOCAL_AREA",
                principalColumn: "LOCAL_AREA_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                table: "HET_RENTAL_REQUEST",
                column: "EQUIPMENT_TYPE_ID",
                principalTable: "HET_EQUIPMENT_TYPE",
                principalColumn: "EQUIPMENT_TYPE_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
