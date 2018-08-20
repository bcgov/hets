using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HETSAPI.Models;

namespace HETSAPI.Migrations
{
    public partial class HETS2263221 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HET_EQUIPMENT_TYPE_NEXT_RENTAL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                columns: table => new
                {
                    EQUIPMENT_TYPE_NEXT_RENTAL_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ASK_NEXT_BLOCK1_ID = table.Column<int>(nullable: true),
                    ASK_NEXT_BLOCK1_SENIORITY = table.Column<float>(nullable: true),
                    ASK_NEXT_BLOCK2_ID = table.Column<int>(nullable: true),
                    ASK_NEXT_BLOCK2_SENIORITY = table.Column<float>(nullable: true),
                    ASK_NEXT_BLOCK_OPEN_ID = table.Column<int>(nullable: true),
                    CREATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    CREATE_USERID = table.Column<string>(maxLength: 50, nullable: true),
                    EQUIPMENT_TYPE_ID = table.Column<int>(nullable: true),
                    LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    LAST_UPDATE_USERID = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_EQUIPMENT_TYPE_NEXT_RENTAL", x => x.EQUIPMENT_TYPE_NEXT_RENTAL_ID);
                    table.ForeignKey(
                        name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK1_ID",
                        column: x => x.ASK_NEXT_BLOCK1_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK2_ID",
                        column: x => x.ASK_NEXT_BLOCK2_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK_OPEN_ID",
                        column: x => x.ASK_NEXT_BLOCK_OPEN_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_DISTRICT_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                        column: x => x.EQUIPMENT_TYPE_ID,
                        principalTable: "HET_DISTRICT_EQUIPMENT_TYPE",
                        principalColumn: "DISTRICT_EQUIPMENT_TYPE_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK1_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "ASK_NEXT_BLOCK1_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK2_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "ASK_NEXT_BLOCK2_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK_OPEN_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "ASK_NEXT_BLOCK_OPEN_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "EQUIPMENT_TYPE_ID");
        }
    }
}
