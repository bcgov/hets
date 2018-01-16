using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HETSAPI.Migrations
{
    public partial class HETS4171152 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ORGANIZATION_NAME",
                table: "HET_CONTACT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ORGANIZATION_NAME",
                table: "HET_CONTACT",
                maxLength: 150,
                nullable: true);
        }
    }
}
