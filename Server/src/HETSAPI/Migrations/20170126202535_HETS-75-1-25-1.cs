using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS751251 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_DISTRICT_HET_REGION_REGION_REF_ID",
                table: "HET_DISTRICT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_GROUP_MEMBERSHIP_HET_GROUP_GROUP_ID",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_GROUP_MEMBERSHIP_HET_USER_USER_ID",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_ROLE_PERMISSION_HET_PERMISSION_PERMISSION_ID",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_ROLE_PERMISSION_HET_ROLE_ROLE_ID",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_SERVICE_AREA_HET_DISTRICT_DISTRICT_ID",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_FAVOURITE_HET_FAVOURITE_CONTEXT_TYPE_FAVOURITE_CONTEXT_TYPE_ID",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_ROLE_HET_ROLE_ROLE_ID",
                table: "HET_USER_ROLE");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_ROLE_HET_USER_USER_ID",
                table: "HET_USER_ROLE");

            migrationBuilder.RenameColumn(
                name: "USER_ID",
                table: "HET_USER_ROLE",
                newName: "USER_REF_ID");

            migrationBuilder.RenameColumn(
                name: "ROLE_ID",
                table: "HET_USER_ROLE",
                newName: "ROLE_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_USER_ROLE_USER_ID",
                table: "HET_USER_ROLE",
                newName: "IX_HET_USER_ROLE_USER_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_USER_ROLE_ROLE_ID",
                table: "HET_USER_ROLE",
                newName: "IX_HET_USER_ROLE_ROLE_REF_ID");

            migrationBuilder.RenameColumn(
                name: "FAVOURITE_CONTEXT_TYPE_ID",
                table: "HET_USER_FAVOURITE",
                newName: "FAVOURITE_CONTEXT_TYPE_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_USER_FAVOURITE_FAVOURITE_CONTEXT_TYPE_ID",
                table: "HET_USER_FAVOURITE",
                newName: "IX_HET_USER_FAVOURITE_FAVOURITE_CONTEXT_TYPE_REF_ID");

            migrationBuilder.RenameColumn(
                name: "DISTRICT_ID",
                table: "HET_SERVICE_AREA",
                newName: "DISTRICT_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_SERVICE_AREA_DISTRICT_ID",
                table: "HET_SERVICE_AREA",
                newName: "IX_HET_SERVICE_AREA_DISTRICT_REF_ID");

            migrationBuilder.RenameColumn(
                name: "ROLE_ID",
                table: "HET_ROLE_PERMISSION",
                newName: "ROLE_REF_ID");

            migrationBuilder.RenameColumn(
                name: "PERMISSION_ID",
                table: "HET_ROLE_PERMISSION",
                newName: "PERMISSION_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_ROLE_PERMISSION_ROLE_ID",
                table: "HET_ROLE_PERMISSION",
                newName: "IX_HET_ROLE_PERMISSION_ROLE_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_ROLE_PERMISSION_PERMISSION_ID",
                table: "HET_ROLE_PERMISSION",
                newName: "IX_HET_ROLE_PERMISSION_PERMISSION_REF_ID");

            migrationBuilder.RenameColumn(
                name: "USER_ID",
                table: "HET_GROUP_MEMBERSHIP",
                newName: "USER_REF_ID");

            migrationBuilder.RenameColumn(
                name: "GROUP_ID",
                table: "HET_GROUP_MEMBERSHIP",
                newName: "GROUP_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_GROUP_MEMBERSHIP_USER_ID",
                table: "HET_GROUP_MEMBERSHIP",
                newName: "IX_HET_GROUP_MEMBERSHIP_USER_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_GROUP_MEMBERSHIP_GROUP_ID",
                table: "HET_GROUP_MEMBERSHIP",
                newName: "IX_HET_GROUP_MEMBERSHIP_GROUP_REF_ID");

            migrationBuilder.AlterColumn<int>(
                name: "REGION_REF_ID",
                table: "HET_DISTRICT",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_HET_DISTRICT_HET_REGION_REGION_REF_ID",
                table: "HET_DISTRICT",
                column: "REGION_REF_ID",
                principalTable: "HET_REGION",
                principalColumn: "REGION_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_GROUP_MEMBERSHIP_HET_GROUP_GROUP_REF_ID",
                table: "HET_GROUP_MEMBERSHIP",
                column: "GROUP_REF_ID",
                principalTable: "HET_GROUP",
                principalColumn: "GROUP_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_GROUP_MEMBERSHIP_HET_USER_USER_REF_ID",
                table: "HET_GROUP_MEMBERSHIP",
                column: "USER_REF_ID",
                principalTable: "HET_USER",
                principalColumn: "USER_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_ROLE_PERMISSION_HET_PERMISSION_PERMISSION_REF_ID",
                table: "HET_ROLE_PERMISSION",
                column: "PERMISSION_REF_ID",
                principalTable: "HET_PERMISSION",
                principalColumn: "PERMISSION_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_ROLE_PERMISSION_HET_ROLE_ROLE_REF_ID",
                table: "HET_ROLE_PERMISSION",
                column: "ROLE_REF_ID",
                principalTable: "HET_ROLE",
                principalColumn: "ROLE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_SERVICE_AREA_HET_DISTRICT_DISTRICT_REF_ID",
                table: "HET_SERVICE_AREA",
                column: "DISTRICT_REF_ID",
                principalTable: "HET_DISTRICT",
                principalColumn: "DISTRICT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_FAVOURITE_HET_FAVOURITE_CONTEXT_TYPE_FAVOURITE_CONTEXT_TYPE_REF_ID",
                table: "HET_USER_FAVOURITE",
                column: "FAVOURITE_CONTEXT_TYPE_REF_ID",
                principalTable: "HET_FAVOURITE_CONTEXT_TYPE",
                principalColumn: "FAVOURITE_CONTEXT_TYPE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_ROLE_HET_ROLE_ROLE_REF_ID",
                table: "HET_USER_ROLE",
                column: "ROLE_REF_ID",
                principalTable: "HET_ROLE",
                principalColumn: "ROLE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_ROLE_HET_USER_USER_REF_ID",
                table: "HET_USER_ROLE",
                column: "USER_REF_ID",
                principalTable: "HET_USER",
                principalColumn: "USER_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_DISTRICT_HET_REGION_REGION_REF_ID",
                table: "HET_DISTRICT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_GROUP_MEMBERSHIP_HET_GROUP_GROUP_REF_ID",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_GROUP_MEMBERSHIP_HET_USER_USER_REF_ID",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_ROLE_PERMISSION_HET_PERMISSION_PERMISSION_REF_ID",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_ROLE_PERMISSION_HET_ROLE_ROLE_REF_ID",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_SERVICE_AREA_HET_DISTRICT_DISTRICT_REF_ID",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_FAVOURITE_HET_FAVOURITE_CONTEXT_TYPE_FAVOURITE_CONTEXT_TYPE_REF_ID",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_ROLE_HET_ROLE_ROLE_REF_ID",
                table: "HET_USER_ROLE");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_ROLE_HET_USER_USER_REF_ID",
                table: "HET_USER_ROLE");

            migrationBuilder.RenameColumn(
                name: "USER_REF_ID",
                table: "HET_USER_ROLE",
                newName: "USER_ID");

            migrationBuilder.RenameColumn(
                name: "ROLE_REF_ID",
                table: "HET_USER_ROLE",
                newName: "ROLE_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_USER_ROLE_USER_REF_ID",
                table: "HET_USER_ROLE",
                newName: "IX_HET_USER_ROLE_USER_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_USER_ROLE_ROLE_REF_ID",
                table: "HET_USER_ROLE",
                newName: "IX_HET_USER_ROLE_ROLE_ID");

            migrationBuilder.RenameColumn(
                name: "FAVOURITE_CONTEXT_TYPE_REF_ID",
                table: "HET_USER_FAVOURITE",
                newName: "FAVOURITE_CONTEXT_TYPE_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_USER_FAVOURITE_FAVOURITE_CONTEXT_TYPE_REF_ID",
                table: "HET_USER_FAVOURITE",
                newName: "IX_HET_USER_FAVOURITE_FAVOURITE_CONTEXT_TYPE_ID");

            migrationBuilder.RenameColumn(
                name: "DISTRICT_REF_ID",
                table: "HET_SERVICE_AREA",
                newName: "DISTRICT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_SERVICE_AREA_DISTRICT_REF_ID",
                table: "HET_SERVICE_AREA",
                newName: "IX_HET_SERVICE_AREA_DISTRICT_ID");

            migrationBuilder.RenameColumn(
                name: "ROLE_REF_ID",
                table: "HET_ROLE_PERMISSION",
                newName: "ROLE_ID");

            migrationBuilder.RenameColumn(
                name: "PERMISSION_REF_ID",
                table: "HET_ROLE_PERMISSION",
                newName: "PERMISSION_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_ROLE_PERMISSION_ROLE_REF_ID",
                table: "HET_ROLE_PERMISSION",
                newName: "IX_HET_ROLE_PERMISSION_ROLE_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_ROLE_PERMISSION_PERMISSION_REF_ID",
                table: "HET_ROLE_PERMISSION",
                newName: "IX_HET_ROLE_PERMISSION_PERMISSION_ID");

            migrationBuilder.RenameColumn(
                name: "USER_REF_ID",
                table: "HET_GROUP_MEMBERSHIP",
                newName: "USER_ID");

            migrationBuilder.RenameColumn(
                name: "GROUP_REF_ID",
                table: "HET_GROUP_MEMBERSHIP",
                newName: "GROUP_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_GROUP_MEMBERSHIP_USER_REF_ID",
                table: "HET_GROUP_MEMBERSHIP",
                newName: "IX_HET_GROUP_MEMBERSHIP_USER_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_GROUP_MEMBERSHIP_GROUP_REF_ID",
                table: "HET_GROUP_MEMBERSHIP",
                newName: "IX_HET_GROUP_MEMBERSHIP_GROUP_ID");

            migrationBuilder.AlterColumn<int>(
                name: "REGION_REF_ID",
                table: "HET_DISTRICT",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_DISTRICT_HET_REGION_REGION_REF_ID",
                table: "HET_DISTRICT",
                column: "REGION_REF_ID",
                principalTable: "HET_REGION",
                principalColumn: "REGION_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_GROUP_MEMBERSHIP_HET_GROUP_GROUP_ID",
                table: "HET_GROUP_MEMBERSHIP",
                column: "GROUP_ID",
                principalTable: "HET_GROUP",
                principalColumn: "GROUP_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_GROUP_MEMBERSHIP_HET_USER_USER_ID",
                table: "HET_GROUP_MEMBERSHIP",
                column: "USER_ID",
                principalTable: "HET_USER",
                principalColumn: "USER_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_ROLE_PERMISSION_HET_PERMISSION_PERMISSION_ID",
                table: "HET_ROLE_PERMISSION",
                column: "PERMISSION_ID",
                principalTable: "HET_PERMISSION",
                principalColumn: "PERMISSION_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_ROLE_PERMISSION_HET_ROLE_ROLE_ID",
                table: "HET_ROLE_PERMISSION",
                column: "ROLE_ID",
                principalTable: "HET_ROLE",
                principalColumn: "ROLE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_SERVICE_AREA_HET_DISTRICT_DISTRICT_ID",
                table: "HET_SERVICE_AREA",
                column: "DISTRICT_ID",
                principalTable: "HET_DISTRICT",
                principalColumn: "DISTRICT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_FAVOURITE_HET_FAVOURITE_CONTEXT_TYPE_FAVOURITE_CONTEXT_TYPE_ID",
                table: "HET_USER_FAVOURITE",
                column: "FAVOURITE_CONTEXT_TYPE_ID",
                principalTable: "HET_FAVOURITE_CONTEXT_TYPE",
                principalColumn: "FAVOURITE_CONTEXT_TYPE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_ROLE_HET_ROLE_ROLE_ID",
                table: "HET_USER_ROLE",
                column: "ROLE_ID",
                principalTable: "HET_ROLE",
                principalColumn: "ROLE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_ROLE_HET_USER_USER_ID",
                table: "HET_USER_ROLE",
                column: "USER_ID",
                principalTable: "HET_USER",
                principalColumn: "USER_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
