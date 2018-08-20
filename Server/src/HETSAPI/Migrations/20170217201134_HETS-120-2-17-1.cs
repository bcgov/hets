using System;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS1202171 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "REFUSE_REASON",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "OFFER_RESPONSE_NOTE");

            migrationBuilder.RenameColumn(
                name: "CODE",
                table: "HET_EQUIPMENT_TYPE",
                newName: "NAME");

            migrationBuilder.RenameColumn(
                name: "SERIAL_NUM",
                table: "HET_EQUIPMENT",
                newName: "SERIAL_NUMBER");

            migrationBuilder.RenameColumn(
                name: "EQUIP_CODE",
                table: "HET_EQUIPMENT",
                newName: "EQUIPMENT_CODE");

            migrationBuilder.AddColumn<string>(
                name: "OFFER_REFUSAL_REASON",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OFFER_RESPONSE_DATETIME",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "STATUS",
                table: "HET_RENTAL_REQUEST",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "STATUS",
                table: "HET_RENTAL_AGREEMENT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NUMBER_IN_BLOCK",
                table: "HET_EQUIPMENT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HET_RENTAL_REQUEST_ATTACHMENT",
                columns: table => new
                {
                    RENTAL_REQUEST_ATTACHMENT_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ATTACHMENT = table.Column<string>(maxLength: 150, nullable: true),
                    RENTAL_REQUEST_REF_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_RENTAL_REQUEST_ATTACHMENT", x => x.RENTAL_REQUEST_ATTACHMENT_ID);
                    table.ForeignKey(
                        name: "FK_HET_RENTAL_REQUEST_ATTACHMENT_HET_RENTAL_REQUEST_RENTAL_REQUEST_REF_ID",
                        column: x => x.RENTAL_REQUEST_REF_ID,
                        principalTable: "HET_RENTAL_REQUEST",
                        principalColumn: "RENTAL_REQUEST_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_TYPE_ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE",
                column: "ASK_NEXT_BLOCK1_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_RENTAL_REQUEST_ATTACHMENT_RENTAL_REQUEST_REF_ID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                column: "RENTAL_REQUEST_REF_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_HET_EQUIPMENT_ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE",
                column: "ASK_NEXT_BLOCK1_REF_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_HET_EQUIPMENT_ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropTable(
                name: "HET_RENTAL_REQUEST_ATTACHMENT");

            migrationBuilder.DropIndex(
                name: "IX_HET_EQUIPMENT_TYPE_ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "OFFER_REFUSAL_REASON",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "OFFER_RESPONSE_DATETIME",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropColumn(
                name: "ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropColumn(
                name: "NUMBER_IN_BLOCK",
                table: "HET_EQUIPMENT");

            migrationBuilder.RenameColumn(
                name: "OFFER_RESPONSE_NOTE",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "REFUSE_REASON");

            migrationBuilder.RenameColumn(
                name: "NAME",
                table: "HET_EQUIPMENT_TYPE",
                newName: "CODE");

            migrationBuilder.RenameColumn(
                name: "SERIAL_NUMBER",
                table: "HET_EQUIPMENT",
                newName: "SERIAL_NUM");

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_CODE",
                table: "HET_EQUIPMENT",
                newName: "EQUIP_CODE");

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
    }
}
