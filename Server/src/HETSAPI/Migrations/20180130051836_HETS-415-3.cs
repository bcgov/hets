using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HETSAPI.Migrations
{
    public partial class HETS4153 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HET_CONDITION_TYPE",
                columns: table => new
                {
                    CONDITION_TYPE_CODE = table.Column<string>(maxLength: 20, nullable: false),
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
                    DESCRIPTION = table.Column<string>(maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_CONDITION_TYPE", x => x.CONDITION_TYPE_CODE);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HET_CONDITION_TYPE");
        }
    }
}
