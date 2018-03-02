using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HETSAPI.Migrations
{
    public partial class HETS3522 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HET_USER_DISTRICT",
                columns: table => new
                {
                    USER_DISTRICT_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(maxLength: 50, nullable: true),
                    APP_CREATE_USER_GUID = table.Column<string>(maxLength: 255, nullable: true),
                    APP_CREATE_USERID = table.Column<string>(maxLength: 255, nullable: true),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(maxLength: 50, nullable: true),
                    APP_LAST_UPDATE_USER_GUID = table.Column<string>(maxLength: 255, nullable: true),
                    APP_LAST_UPDATE_USERID = table.Column<string>(maxLength: 255, nullable: true),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    DB_CREATE_USER_ID = table.Column<string>(maxLength: 63, nullable: true),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    DB_LAST_UPDATE_USER_ID = table.Column<string>(maxLength: 63, nullable: true),
                    DISTRICT_ID = table.Column<int>(nullable: true),
                    IS_PRIMARY = table.Column<bool>(nullable: false),
                    USER_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_USER_DISTRICT", x => x.USER_DISTRICT_ID);
                    table.ForeignKey(
                        name: "FK_HET_USER_DISTRICT_HET_DISTRICT_DISTRICT_ID",
                        column: x => x.DISTRICT_ID,
                        principalTable: "HET_DISTRICT",
                        principalColumn: "DISTRICT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_USER_DISTRICT_HET_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "HET_USER",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HET_USER_DISTRICT_DISTRICT_ID",
                table: "HET_USER_DISTRICT",
                column: "DISTRICT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_USER_DISTRICT_USER_ID",
                table: "HET_USER_DISTRICT",
                column: "USER_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HET_USER_DISTRICT");
        }
    }
}
