using System;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS932153 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_ATTACHMENT_HET_REQUEST_REQUEST_ID",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_HISTORY_HET_REQUEST_REQUEST_ID",
                table: "HET_HISTORY");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_NOTE_HET_REQUEST_REQUEST_ID",
                table: "HET_NOTE");

            migrationBuilder.DropTable(
                name: "HET_HIRE_OFFER");

            migrationBuilder.DropTable(
                name: "HET_REQUEST_ROTATION_LIST");

            migrationBuilder.DropTable(
                name: "HET_ROTATION_LIST_BLOCK");

            migrationBuilder.DropTable(
                name: "HET_REQUEST");

            migrationBuilder.DropTable(
                name: "HET_ROTATION_LIST");

            migrationBuilder.DropIndex(
                name: "IX_HET_NOTE_REQUEST_ID",
                table: "HET_NOTE");

            migrationBuilder.DropIndex(
                name: "IX_HET_HISTORY_REQUEST_ID",
                table: "HET_HISTORY");

            migrationBuilder.DropIndex(
                name: "IX_HET_ATTACHMENT_REQUEST_ID",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "REQUEST_ID",
                table: "HET_NOTE");

            migrationBuilder.DropColumn(
                name: "REQUEST_ID",
                table: "HET_HISTORY");

            migrationBuilder.DropColumn(
                name: "REQUEST_ID",
                table: "HET_ATTACHMENT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "REQUEST_ID",
                table: "HET_NOTE",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "REQUEST_ID",
                table: "HET_HISTORY",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "REQUEST_ID",
                table: "HET_ATTACHMENT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HET_REQUEST",
                columns: table => new
                {
                    REQUEST_ID = table.Column<int>(nullable: false)
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
                    table.PrimaryKey("PK_HET_REQUEST", x => x.REQUEST_ID);
                    table.ForeignKey(
                        name: "FK_HET_REQUEST_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_REF_ID",
                        column: x => x.EQUIPMENT_TYPE_REF_ID,
                        principalTable: "HET_EQUIPMENT_TYPE",
                        principalColumn: "EQUIPMENT_TYPE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_REQUEST_HET_EQUIPMENT_FIRST_ON_ROTATION_LIST_REF_ID",
                        column: x => x.FIRST_ON_ROTATION_LIST_REF_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_REQUEST_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                        column: x => x.LOCAL_AREA_REF_ID,
                        principalTable: "HET_LOCAL_AREA",
                        principalColumn: "LOCAL_AREA_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_REQUEST_HET_PROJECT_PROJECT_REF_ID",
                        column: x => x.PROJECT_REF_ID,
                        principalTable: "HET_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_ROTATION_LIST",
                columns: table => new
                {
                    ROTATION_LIST_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EQUIPMENT_TYPE_REF_ID = table.Column<int>(nullable: true),
                    LOCAL_AREA_REF_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_ROTATION_LIST", x => x.ROTATION_LIST_ID);
                    table.ForeignKey(
                        name: "FK_HET_ROTATION_LIST_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_REF_ID",
                        column: x => x.EQUIPMENT_TYPE_REF_ID,
                        principalTable: "HET_EQUIPMENT_TYPE",
                        principalColumn: "EQUIPMENT_TYPE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_ROTATION_LIST_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                        column: x => x.LOCAL_AREA_REF_ID,
                        principalTable: "HET_LOCAL_AREA",
                        principalColumn: "LOCAL_AREA_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_HIRE_OFFER",
                columns: table => new
                {
                    HIRE_OFFER_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ASKED_DATE_TIME = table.Column<DateTime>(nullable: true),
                    EQUIPMENT_REF_ID = table.Column<int>(nullable: true),
                    EQUIPMENT_UPDATE_REASON = table.Column<string>(maxLength: 255, nullable: true),
                    EQUIPMENT_VERIFIED_ACTIVE = table.Column<bool>(nullable: true),
                    FLAG_EQUIPMENT_UPDATE = table.Column<bool>(nullable: true),
                    IS_FORCE_HIRE = table.Column<bool>(nullable: true),
                    NOTE = table.Column<string>(maxLength: 255, nullable: true),
                    OFFER_RESPONSE = table.Column<string>(nullable: true),
                    REFUSE_REASON = table.Column<string>(maxLength: 255, nullable: true),
                    RENTAL_AGREEMENT_REF_ID = table.Column<int>(nullable: true),
                    REQUEST_REF_ID = table.Column<int>(nullable: true),
                    WAS_ASKED = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_HIRE_OFFER", x => x.HIRE_OFFER_ID);
                    table.ForeignKey(
                        name: "FK_HET_HIRE_OFFER_HET_EQUIPMENT_EQUIPMENT_REF_ID",
                        column: x => x.EQUIPMENT_REF_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_HIRE_OFFER_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_REF_ID",
                        column: x => x.RENTAL_AGREEMENT_REF_ID,
                        principalTable: "HET_RENTAL_AGREEMENT",
                        principalColumn: "RENTAL_AGREEMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_HIRE_OFFER_HET_REQUEST_REQUEST_REF_ID",
                        column: x => x.REQUEST_REF_ID,
                        principalTable: "HET_REQUEST",
                        principalColumn: "REQUEST_ID",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "HET_ROTATION_LIST_BLOCK",
                columns: table => new
                {
                    ROTATION_LIST_BLOCK_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    BLOCK_NAME = table.Column<string>(nullable: true),
                    BLOCK_NUM = table.Column<int>(nullable: true),
                    CLOSED = table.Column<string>(maxLength: 255, nullable: true),
                    CLOSED_BY = table.Column<string>(nullable: true),
                    CLOSED_COMMENTS = table.Column<string>(maxLength: 2048, nullable: true),
                    CLOSED_DATE = table.Column<DateTime>(nullable: true),
                    CYCLE_NUM = table.Column<float>(nullable: true),
                    LAST_HIRED_EQUIPMENT_REF_ID = table.Column<int>(nullable: true),
                    MAX_CYCLE = table.Column<float>(nullable: true),
                    MOVED = table.Column<string>(maxLength: 255, nullable: true),
                    RESERVED_BY = table.Column<string>(maxLength: 255, nullable: true),
                    RESERVED_DATE = table.Column<DateTime>(nullable: true),
                    ROTATED_BLOCK = table.Column<int>(nullable: true),
                    ROTATION_LIST_REF_ID = table.Column<int>(nullable: true),
                    START_CYCLE_EQUIPMENT_REF_ID = table.Column<int>(nullable: true),
                    START_WAS_ZERO = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_ROTATION_LIST_BLOCK", x => x.ROTATION_LIST_BLOCK_ID);
                    table.ForeignKey(
                        name: "FK_HET_ROTATION_LIST_BLOCK_HET_EQUIPMENT_LAST_HIRED_EQUIPMENT_REF_ID",
                        column: x => x.LAST_HIRED_EQUIPMENT_REF_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_ROTATION_LIST_BLOCK_HET_ROTATION_LIST_ROTATION_LIST_REF_ID",
                        column: x => x.ROTATION_LIST_REF_ID,
                        principalTable: "HET_ROTATION_LIST",
                        principalColumn: "ROTATION_LIST_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_ROTATION_LIST_BLOCK_HET_EQUIPMENT_START_CYCLE_EQUIPMENT_REF_ID",
                        column: x => x.START_CYCLE_EQUIPMENT_REF_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HET_NOTE_REQUEST_ID",
                table: "HET_NOTE",
                column: "REQUEST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_HISTORY_REQUEST_ID",
                table: "HET_HISTORY",
                column: "REQUEST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_ATTACHMENT_REQUEST_ID",
                table: "HET_ATTACHMENT",
                column: "REQUEST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_HIRE_OFFER_EQUIPMENT_REF_ID",
                table: "HET_HIRE_OFFER",
                column: "EQUIPMENT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_HIRE_OFFER_RENTAL_AGREEMENT_REF_ID",
                table: "HET_HIRE_OFFER",
                column: "RENTAL_AGREEMENT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_HIRE_OFFER_REQUEST_REF_ID",
                table: "HET_HIRE_OFFER",
                column: "REQUEST_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_REQUEST_EQUIPMENT_TYPE_REF_ID",
                table: "HET_REQUEST",
                column: "EQUIPMENT_TYPE_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_REQUEST_FIRST_ON_ROTATION_LIST_REF_ID",
                table: "HET_REQUEST",
                column: "FIRST_ON_ROTATION_LIST_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_REQUEST_LOCAL_AREA_REF_ID",
                table: "HET_REQUEST",
                column: "LOCAL_AREA_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_REQUEST_PROJECT_REF_ID",
                table: "HET_REQUEST",
                column: "PROJECT_REF_ID");

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

            migrationBuilder.CreateIndex(
                name: "IX_HET_ROTATION_LIST_EQUIPMENT_TYPE_REF_ID",
                table: "HET_ROTATION_LIST",
                column: "EQUIPMENT_TYPE_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_ROTATION_LIST_LOCAL_AREA_REF_ID",
                table: "HET_ROTATION_LIST",
                column: "LOCAL_AREA_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_ROTATION_LIST_BLOCK_LAST_HIRED_EQUIPMENT_REF_ID",
                table: "HET_ROTATION_LIST_BLOCK",
                column: "LAST_HIRED_EQUIPMENT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_ROTATION_LIST_BLOCK_ROTATION_LIST_REF_ID",
                table: "HET_ROTATION_LIST_BLOCK",
                column: "ROTATION_LIST_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_ROTATION_LIST_BLOCK_START_CYCLE_EQUIPMENT_REF_ID",
                table: "HET_ROTATION_LIST_BLOCK",
                column: "START_CYCLE_EQUIPMENT_REF_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_ATTACHMENT_HET_REQUEST_REQUEST_ID",
                table: "HET_ATTACHMENT",
                column: "REQUEST_ID",
                principalTable: "HET_REQUEST",
                principalColumn: "REQUEST_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_HISTORY_HET_REQUEST_REQUEST_ID",
                table: "HET_HISTORY",
                column: "REQUEST_ID",
                principalTable: "HET_REQUEST",
                principalColumn: "REQUEST_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_NOTE_HET_REQUEST_REQUEST_ID",
                table: "HET_NOTE",
                column: "REQUEST_ID",
                principalTable: "HET_REQUEST",
                principalColumn: "REQUEST_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
