using System;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS20203211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HET_LOCAL_AREA_ROTATION_LIST",
                columns: table => new
                {
                    LOCAL_AREA_ROTATION_LIST_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ASK_NEXT_BLOCK1_ID = table.Column<int>(nullable: true),
                    ASK_NEXT_BLOCK1_SENIORITY = table.Column<float>(nullable: true),
                    ASK_NEXT_BLOCK2_ID = table.Column<int>(nullable: true),
                    ASK_NEXT_BLOCK2_SENIORITY = table.Column<float>(nullable: true),
                    ASK_NEXT_BLOCK_OPEN_ID = table.Column<int>(nullable: true),
                    CREATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    CREATE_USERID = table.Column<string>(maxLength: 50, nullable: true),
                    DISTRICT_EQUIPMENT_TYPE_ID = table.Column<int>(nullable: true),
                    LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    LAST_UPDATE_USERID = table.Column<string>(maxLength: 50, nullable: true),
                    LOCAL_AREA_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_LOCAL_AREA_ROTATION_LIST", x => x.LOCAL_AREA_ROTATION_LIST_ID);
                    table.ForeignKey(
                        name: "FK_HET_LOCAL_AREA_ROTATION_LIST_HET_EQUIPMENT_ASK_NEXT_BLOCK1_ID",
                        column: x => x.ASK_NEXT_BLOCK1_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_LOCAL_AREA_ROTATION_LIST_HET_EQUIPMENT_ASK_NEXT_BLOCK2_ID",
                        column: x => x.ASK_NEXT_BLOCK2_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_LOCAL_AREA_ROTATION_LIST_HET_EQUIPMENT_ASK_NEXT_BLOCK_OPEN_ID",
                        column: x => x.ASK_NEXT_BLOCK_OPEN_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_LOCAL_AREA_ROTATION_LIST_HET_DISTRICT_EQUIPMENT_TYPE_DISTRICT_EQUIPMENT_TYPE_ID",
                        column: x => x.DISTRICT_EQUIPMENT_TYPE_ID,
                        principalTable: "HET_DISTRICT_EQUIPMENT_TYPE",
                        principalColumn: "DISTRICT_EQUIPMENT_TYPE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_LOCAL_AREA_ROTATION_LIST_HET_LOCAL_AREA_LOCAL_AREA_ID",
                        column: x => x.LOCAL_AREA_ID,
                        principalTable: "HET_LOCAL_AREA",
                        principalColumn: "LOCAL_AREA_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_LOOKUP_LIST",
                columns: table => new
                {
                    LOOKUP_LIST_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CODE_NAME = table.Column<string>(maxLength: 30, nullable: true),
                    CONTEXT_NAME = table.Column<string>(maxLength: 100, nullable: true),
                    CREATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    CREATE_USERID = table.Column<string>(maxLength: 50, nullable: true),
                    DISPLAY_SORT_ORDER = table.Column<int>(nullable: true),
                    IS_DEFAULT = table.Column<bool>(nullable: true),
                    LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    LAST_UPDATE_USERID = table.Column<string>(maxLength: 50, nullable: true),
                    VALUE = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_LOOKUP_LIST", x => x.LOOKUP_LIST_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HET_LOCAL_AREA_ROTATION_LIST_ASK_NEXT_BLOCK1_ID",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                column: "ASK_NEXT_BLOCK1_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_LOCAL_AREA_ROTATION_LIST_ASK_NEXT_BLOCK2_ID",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                column: "ASK_NEXT_BLOCK2_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_LOCAL_AREA_ROTATION_LIST_ASK_NEXT_BLOCK_OPEN_ID",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                column: "ASK_NEXT_BLOCK_OPEN_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_LOCAL_AREA_ROTATION_LIST_DISTRICT_EQUIPMENT_TYPE_ID",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                column: "DISTRICT_EQUIPMENT_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_LOCAL_AREA_ROTATION_LIST_LOCAL_AREA_ID",
                table: "HET_LOCAL_AREA_ROTATION_LIST",
                column: "LOCAL_AREA_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HET_LOCAL_AREA_ROTATION_LIST");

            migrationBuilder.DropTable(
                name: "HET_LOOKUP_LIST");
        }
    }
}
