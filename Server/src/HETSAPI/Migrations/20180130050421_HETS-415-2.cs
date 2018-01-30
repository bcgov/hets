using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HETSAPI.Migrations
{
    public partial class HETS4152 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HET_PROVINCIAL_RATE_TYPE",
                columns: table => new
                {
                    RATE_TYPE = table.Column<string>(maxLength: 20, nullable: false),
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
                    DESCRIPTION = table.Column<string>(maxLength: 200, nullable: true),
                    IS_INCLUDED_IN_TOTAL = table.Column<bool>(nullable: false),
                    IS_PERCENT_RATE = table.Column<bool>(nullable: false),
                    IS_RATE_EDITABLE = table.Column<bool>(nullable: false),
                    PERIOD_TYPE = table.Column<string>(maxLength: 20, nullable: true),
                    RATE = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_PROVINCIAL_RATE_TYPE", x => x.RATE_TYPE);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HET_PROVINCIAL_RATE_TYPE");
        }
    }
}
