using System;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS932152 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RENTAL_REQUEST_ID",
                table: "HET_NOTE",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RENTAL_REQUEST_ID",
                table: "HET_HISTORY",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RENTAL_REQUEST_ID",
                table: "HET_ATTACHMENT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HET_RENTAL_REQUEST",
                columns: table => new
                {
                    RENTAL_REQUEST_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EQUIPMENT_COUNT = table.Column<int>(nullable: true),
                    EQUIPMENT_TYPE_REF_ID = table.Column<int>(nullable: true),
                    EXPECTED_END_DATE = table.Column<DateTime>(nullable: true),
                    EXPECTED_HOURS = table.Column<int>(nullable: true),
                    EXPECTED_START_DATE = table.Column<DateTime>(nullable: true),
                    FIRST_ON_ROTATION_LIST_REF_ID = table.Column<int>(nullable: true),
                    LOCAL_AREA_REF_ID = table.Column<int>(nullable: true),
                    PROJECT_REF_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_RENTAL_REQUEST", x => x.RENTAL_REQUEST_ID);
                    table.ForeignKey(
                        name: "FK_HET_RENTAL_REQUEST_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_REF_ID",
                        column: x => x.EQUIPMENT_TYPE_REF_ID,
                        principalTable: "HET_EQUIPMENT_TYPE",
                        principalColumn: "EQUIPMENT_TYPE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_RENTAL_REQUEST_HET_EQUIPMENT_FIRST_ON_ROTATION_LIST_REF_ID",
                        column: x => x.FIRST_ON_ROTATION_LIST_REF_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_RENTAL_REQUEST_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                        column: x => x.LOCAL_AREA_REF_ID,
                        principalTable: "HET_LOCAL_AREA",
                        principalColumn: "LOCAL_AREA_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_RENTAL_REQUEST_HET_PROJECT_PROJECT_REF_ID",
                        column: x => x.PROJECT_REF_ID,
                        principalTable: "HET_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_RENTAL_REQUEST_ROTATION_LIST",
                columns: table => new
                {
                    RENTAL_REQUEST_ROTATION_LIST_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ASKED_DATE_TIME = table.Column<DateTime>(nullable: true),
                    EQUIPMENT_REF_ID = table.Column<int>(nullable: true),
                    IS_FORCE_HIRE = table.Column<bool>(nullable: true),
                    NOTE = table.Column<string>(maxLength: 2048, nullable: true),
                    OFFER_RESPONSE = table.Column<string>(nullable: true),
                    REFUSE_REASON = table.Column<string>(maxLength: 2048, nullable: true),
                    RENTAL_AGREEMENT_REF_ID = table.Column<int>(nullable: true),
                    RENTAL_REQUEST_REF_ID = table.Column<int>(nullable: true),
                    ROTATION_LIST_SORT_ORDER = table.Column<int>(nullable: true),
                    WAS_ASKED = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_RENTAL_REQUEST_ROTATION_LIST", x => x.RENTAL_REQUEST_ROTATION_LIST_ID);
                    table.ForeignKey(
                        name: "FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_EQUIPMENT_EQUIPMENT_REF_ID",
                        column: x => x.EQUIPMENT_REF_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_REF_ID",
                        column: x => x.RENTAL_AGREEMENT_REF_ID,
                        principalTable: "HET_RENTAL_AGREEMENT",
                        principalColumn: "RENTAL_AGREEMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_RENTAL_REQUEST_RENTAL_REQUEST_REF_ID",
                        column: x => x.RENTAL_REQUEST_REF_ID,
                        principalTable: "HET_RENTAL_REQUEST",
                        principalColumn: "RENTAL_REQUEST_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HET_NOTE_RENTAL_REQUEST_ID",
                table: "HET_NOTE",
                column: "RENTAL_REQUEST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_HISTORY_RENTAL_REQUEST_ID",
                table: "HET_HISTORY",
                column: "RENTAL_REQUEST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_ATTACHMENT_RENTAL_REQUEST_ID",
                table: "HET_ATTACHMENT",
                column: "RENTAL_REQUEST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_RENTAL_REQUEST_EQUIPMENT_TYPE_REF_ID",
                table: "HET_RENTAL_REQUEST",
                column: "EQUIPMENT_TYPE_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_RENTAL_REQUEST_FIRST_ON_ROTATION_LIST_REF_ID",
                table: "HET_RENTAL_REQUEST",
                column: "FIRST_ON_ROTATION_LIST_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_RENTAL_REQUEST_LOCAL_AREA_REF_ID",
                table: "HET_RENTAL_REQUEST",
                column: "LOCAL_AREA_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_RENTAL_REQUEST_PROJECT_REF_ID",
                table: "HET_RENTAL_REQUEST",
                column: "PROJECT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_RENTAL_REQUEST_ROTATION_LIST_EQUIPMENT_REF_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                column: "EQUIPMENT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_RENTAL_REQUEST_ROTATION_LIST_RENTAL_AGREEMENT_REF_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                column: "RENTAL_AGREEMENT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_RENTAL_REQUEST_ROTATION_LIST_RENTAL_REQUEST_REF_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                column: "RENTAL_REQUEST_REF_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_ATTACHMENT_HET_RENTAL_REQUEST_RENTAL_REQUEST_ID",
                table: "HET_ATTACHMENT",
                column: "RENTAL_REQUEST_ID",
                principalTable: "HET_RENTAL_REQUEST",
                principalColumn: "RENTAL_REQUEST_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_HISTORY_HET_RENTAL_REQUEST_RENTAL_REQUEST_ID",
                table: "HET_HISTORY",
                column: "RENTAL_REQUEST_ID",
                principalTable: "HET_RENTAL_REQUEST",
                principalColumn: "RENTAL_REQUEST_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_NOTE_HET_RENTAL_REQUEST_RENTAL_REQUEST_ID",
                table: "HET_NOTE",
                column: "RENTAL_REQUEST_ID",
                principalTable: "HET_RENTAL_REQUEST",
                principalColumn: "RENTAL_REQUEST_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_ATTACHMENT_HET_RENTAL_REQUEST_RENTAL_REQUEST_ID",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_HISTORY_HET_RENTAL_REQUEST_RENTAL_REQUEST_ID",
                table: "HET_HISTORY");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_NOTE_HET_RENTAL_REQUEST_RENTAL_REQUEST_ID",
                table: "HET_NOTE");

            migrationBuilder.DropTable(
                name: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropTable(
                name: "HET_RENTAL_REQUEST");

            migrationBuilder.DropIndex(
                name: "IX_HET_NOTE_RENTAL_REQUEST_ID",
                table: "HET_NOTE");

            migrationBuilder.DropIndex(
                name: "IX_HET_HISTORY_RENTAL_REQUEST_ID",
                table: "HET_HISTORY");

            migrationBuilder.DropIndex(
                name: "IX_HET_ATTACHMENT_RENTAL_REQUEST_ID",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "RENTAL_REQUEST_ID",
                table: "HET_NOTE");

            migrationBuilder.DropColumn(
                name: "RENTAL_REQUEST_ID",
                table: "HET_HISTORY");

            migrationBuilder.DropColumn(
                name: "RENTAL_REQUEST_ID",
                table: "HET_ATTACHMENT");
        }
    }
}
