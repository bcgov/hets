using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS2533271 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RENTAL_REQUEST_ID",
                table: "HET_ATTACHMENT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HET_ATTACHMENT_RENTAL_REQUEST_ID",
                table: "HET_ATTACHMENT",
                column: "RENTAL_REQUEST_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_ATTACHMENT_HET_RENTAL_REQUEST_RENTAL_REQUEST_ID",
                table: "HET_ATTACHMENT",
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

            migrationBuilder.DropIndex(
                name: "IX_HET_ATTACHMENT_RENTAL_REQUEST_ID",
                table: "HET_ATTACHMENT");

            migrationBuilder.DropColumn(
                name: "RENTAL_REQUEST_ID",
                table: "HET_ATTACHMENT");
        }
    }
}
