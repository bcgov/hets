using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HETSAPI.Migrations
{
    public partial class HETS4951 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropTable(
                name: "HET_LOOKUP_LIST");

            migrationBuilder.DropTable(
                name: "HET_GROUP");            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {            
            migrationBuilder.CreateTable(
                name: "HET_GROUP",
                columns: table => new
                {
                    GROUP_ID = table.Column<int>(nullable: false)
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
                    DESCRIPTION = table.Column<string>(maxLength: 2048, nullable: true),
                    NAME = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_GROUP", x => x.GROUP_ID);
                });

            migrationBuilder.CreateTable(
                name: "HET_LOOKUP_LIST",
                columns: table => new
                {
                    LOOKUP_LIST_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    APP_CREATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    APP_CREATE_USER_DIRECTORY = table.Column<string>(maxLength: 50, nullable: true),
                    APP_CREATE_USER_GUID = table.Column<string>(maxLength: 255, nullable: true),
                    APP_CREATE_USERID = table.Column<string>(maxLength: 255, nullable: true),
                    APP_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    APP_LAST_UPDATE_USER_DIRECTORY = table.Column<string>(maxLength: 50, nullable: true),
                    APP_LAST_UPDATE_USER_GUID = table.Column<string>(maxLength: 255, nullable: true),
                    APP_LAST_UPDATE_USERID = table.Column<string>(maxLength: 255, nullable: true),
                    CODE_NAME = table.Column<string>(maxLength: 30, nullable: true),
                    CONTEXT_NAME = table.Column<string>(maxLength: 100, nullable: true),
                    DB_CREATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    DB_CREATE_USER_ID = table.Column<string>(maxLength: 63, nullable: true),
                    DB_LAST_UPDATE_TIMESTAMP = table.Column<DateTime>(nullable: false),
                    DB_LAST_UPDATE_USER_ID = table.Column<string>(maxLength: 63, nullable: true),
                    DISPLAY_SORT_ORDER = table.Column<int>(nullable: true),
                    IS_DEFAULT = table.Column<bool>(nullable: false),
                    VALUE = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_LOOKUP_LIST", x => x.LOOKUP_LIST_ID);
                });

            migrationBuilder.CreateTable(
                name: "HET_GROUP_MEMBERSHIP",
                columns: table => new
                {
                    GROUP_MEMBERSHIP_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ACTIVE = table.Column<bool>(nullable: false),
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
                    GROUP_ID = table.Column<int>(nullable: true),
                    USER_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_GROUP_MEMBERSHIP", x => x.GROUP_MEMBERSHIP_ID);
                    table.ForeignKey(
                        name: "FK_HET_GROUP_MEMBERSHIP_HET_GROUP_GROUP_ID",
                        column: x => x.GROUP_ID,
                        principalTable: "HET_GROUP",
                        principalColumn: "GROUP_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_GROUP_MEMBERSHIP_HET_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "HET_USER",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HET_GROUP_MEMBERSHIP_GROUP_ID",
                table: "HET_GROUP_MEMBERSHIP",
                column: "GROUP_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_GROUP_MEMBERSHIP_USER_ID",
                table: "HET_GROUP_MEMBERSHIP",
                column: "USER_ID");
        }
    }
}
