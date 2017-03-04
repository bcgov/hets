using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS1512271 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_DISTRICT_HET_REGION_REGION_REF_ID",
                table: "HET_DISTRICT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_HET_DUMP_TRUCK_DUMP_TRUCK_REF_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_REF_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_HET_OWNER_OWNER_REF_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_ATTACHMENT_HET_EQUIPMENT_EQUIPMENT_REF_ID",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK2_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK_OPEN_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_GROUP_MEMBERSHIP_HET_GROUP_GROUP_REF_ID",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_GROUP_MEMBERSHIP_HET_USER_USER_REF_ID",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_LOCAL_AREA_HET_SERVICE_AREA_SERVICE_AREA_REF_ID",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_OWNER_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                table: "HET_OWNER");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_OWNER_HET_CONTACT_PRIMARY_CONTACT_REF_ID",
                table: "HET_OWNER");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_PROJECT_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                table: "HET_PROJECT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_PROJECT_HET_CONTACT_PRIMARY_CONTACT_REF_ID",
                table: "HET_PROJECT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_AGREEMENT_HET_EQUIPMENT_EQUIPMENT_REF_ID",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_AGREEMENT_HET_PROJECT_PROJECT_REF_ID",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_AGREEMENT_CONDITION_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_REF_ID",
                table: "HET_RENTAL_AGREEMENT_CONDITION");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_AGREEMENT_RATE_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_REF_ID",
                table: "HET_RENTAL_AGREEMENT_RATE");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_REF_ID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_EQUIPMENT_FIRST_ON_ROTATION_LIST_REF_ID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_PROJECT_PROJECT_REF_ID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_ATTACHMENT_HET_RENTAL_REQUEST_RENTAL_REQUEST_REF_ID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_EQUIPMENT_EQUIPMENT_REF_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_REF_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_RENTAL_REQUEST_RENTAL_REQUEST_REF_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_ROLE_PERMISSION_HET_PERMISSION_PERMISSION_REF_ID",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_ROLE_PERMISSION_HET_ROLE_ROLE_REF_ID",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_SENIORITY_AUDIT_HET_EQUIPMENT_EQUIPMENT_REF_ID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_SENIORITY_AUDIT_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_SENIORITY_AUDIT_HET_OWNER_OWNER_REF_ID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_SERVICE_AREA_HET_DISTRICT_DISTRICT_REF_ID",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_TIME_RECORD_HET_RENTAL_AGREEMENT_RATE_RENTAL_AGREEMENT_RATE_REF_ID",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_TIME_RECORD_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_REF_ID",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_HET_DISTRICT_DISTRICT_REF_ID",
                table: "HET_USER");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_FAVOURITE_HET_USER_USER_REF_ID",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_ROLE_HET_ROLE_ROLE_REF_ID",
                table: "HET_USER_ROLE");

            migrationBuilder.RenameColumn(
                name: "ROLE_REF_ID",
                table: "HET_USER_ROLE",
                newName: "ROLE_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_USER_ROLE_ROLE_REF_ID",
                table: "HET_USER_ROLE",
                newName: "IX_HET_USER_ROLE_ROLE_ID");

            migrationBuilder.RenameColumn(
                name: "USER_REF_ID",
                table: "HET_USER_FAVOURITE",
                newName: "USER_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_USER_FAVOURITE_USER_REF_ID",
                table: "HET_USER_FAVOURITE",
                newName: "IX_HET_USER_FAVOURITE_USER_ID");

            migrationBuilder.RenameColumn(
                name: "DISTRICT_REF_ID",
                table: "HET_USER",
                newName: "DISTRICT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_USER_DISTRICT_REF_ID",
                table: "HET_USER",
                newName: "IX_HET_USER_DISTRICT_ID");

            migrationBuilder.RenameColumn(
                name: "RENTAL_AGREEMENT_REF_ID",
                table: "HET_TIME_RECORD",
                newName: "RENTAL_AGREEMENT_RATE_ID");

            migrationBuilder.RenameColumn(
                name: "RENTAL_AGREEMENT_RATE_REF_ID",
                table: "HET_TIME_RECORD",
                newName: "RENTAL_AGREEMENT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_TIME_RECORD_RENTAL_AGREEMENT_REF_ID",
                table: "HET_TIME_RECORD",
                newName: "IX_HET_TIME_RECORD_RENTAL_AGREEMENT_RATE_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_TIME_RECORD_RENTAL_AGREEMENT_RATE_REF_ID",
                table: "HET_TIME_RECORD",
                newName: "IX_HET_TIME_RECORD_RENTAL_AGREEMENT_ID");

            migrationBuilder.RenameColumn(
                name: "DISTRICT_REF_ID",
                table: "HET_SERVICE_AREA",
                newName: "DISTRICT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_SERVICE_AREA_DISTRICT_REF_ID",
                table: "HET_SERVICE_AREA",
                newName: "IX_HET_SERVICE_AREA_DISTRICT_ID");

            migrationBuilder.RenameColumn(
                name: "OWNER_REF_ID",
                table: "HET_SENIORITY_AUDIT",
                newName: "OWNER_ID");

            migrationBuilder.RenameColumn(
                name: "LOCAL_AREA_REF_ID",
                table: "HET_SENIORITY_AUDIT",
                newName: "LOCAL_AREA_ID");

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_REF_ID",
                table: "HET_SENIORITY_AUDIT",
                newName: "EQUIPMENT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_SENIORITY_AUDIT_OWNER_REF_ID",
                table: "HET_SENIORITY_AUDIT",
                newName: "IX_HET_SENIORITY_AUDIT_OWNER_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_SENIORITY_AUDIT_LOCAL_AREA_REF_ID",
                table: "HET_SENIORITY_AUDIT",
                newName: "IX_HET_SENIORITY_AUDIT_LOCAL_AREA_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_SENIORITY_AUDIT_EQUIPMENT_REF_ID",
                table: "HET_SENIORITY_AUDIT",
                newName: "IX_HET_SENIORITY_AUDIT_EQUIPMENT_ID");

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
                name: "RENTAL_REQUEST_REF_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "RENTAL_REQUEST_ID");

            migrationBuilder.RenameColumn(
                name: "RENTAL_AGREEMENT_REF_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "RENTAL_AGREEMENT_ID");

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_REF_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "EQUIPMENT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_ROTATION_LIST_RENTAL_REQUEST_REF_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "IX_HET_RENTAL_REQUEST_ROTATION_LIST_RENTAL_REQUEST_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_ROTATION_LIST_RENTAL_AGREEMENT_REF_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "IX_HET_RENTAL_REQUEST_ROTATION_LIST_RENTAL_AGREEMENT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_ROTATION_LIST_EQUIPMENT_REF_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "IX_HET_RENTAL_REQUEST_ROTATION_LIST_EQUIPMENT_ID");

            migrationBuilder.RenameColumn(
                name: "RENTAL_REQUEST_REF_ID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                newName: "RENTAL_REQUEST_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_ATTACHMENT_RENTAL_REQUEST_REF_ID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                newName: "IX_HET_RENTAL_REQUEST_ATTACHMENT_RENTAL_REQUEST_ID");

            migrationBuilder.RenameColumn(
                name: "PROJECT_REF_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "PROJECT_ID");

            migrationBuilder.RenameColumn(
                name: "LOCAL_AREA_REF_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "LOCAL_AREA_ID");

            migrationBuilder.RenameColumn(
                name: "FIRST_ON_ROTATION_LIST_REF_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "FIRST_ON_ROTATION_LIST_ID");

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_TYPE_REF_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "EQUIPMENT_TYPE_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_PROJECT_REF_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "IX_HET_RENTAL_REQUEST_PROJECT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_LOCAL_AREA_REF_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "IX_HET_RENTAL_REQUEST_LOCAL_AREA_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_FIRST_ON_ROTATION_LIST_REF_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "IX_HET_RENTAL_REQUEST_FIRST_ON_ROTATION_LIST_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_EQUIPMENT_TYPE_REF_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "IX_HET_RENTAL_REQUEST_EQUIPMENT_TYPE_ID");

            migrationBuilder.RenameColumn(
                name: "RENTAL_AGREEMENT_REF_ID",
                table: "HET_RENTAL_AGREEMENT_RATE",
                newName: "RENTAL_AGREEMENT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_AGREEMENT_RATE_RENTAL_AGREEMENT_REF_ID",
                table: "HET_RENTAL_AGREEMENT_RATE",
                newName: "IX_HET_RENTAL_AGREEMENT_RATE_RENTAL_AGREEMENT_ID");

            migrationBuilder.RenameColumn(
                name: "RENTAL_AGREEMENT_REF_ID",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                newName: "RENTAL_AGREEMENT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_AGREEMENT_CONDITION_RENTAL_AGREEMENT_REF_ID",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                newName: "IX_HET_RENTAL_AGREEMENT_CONDITION_RENTAL_AGREEMENT_ID");

            migrationBuilder.RenameColumn(
                name: "PROJECT_REF_ID",
                table: "HET_RENTAL_AGREEMENT",
                newName: "PROJECT_ID");

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_REF_ID",
                table: "HET_RENTAL_AGREEMENT",
                newName: "EQUIPMENT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_AGREEMENT_PROJECT_REF_ID",
                table: "HET_RENTAL_AGREEMENT",
                newName: "IX_HET_RENTAL_AGREEMENT_PROJECT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_AGREEMENT_EQUIPMENT_REF_ID",
                table: "HET_RENTAL_AGREEMENT",
                newName: "IX_HET_RENTAL_AGREEMENT_EQUIPMENT_ID");

            migrationBuilder.RenameColumn(
                name: "PRIMARY_CONTACT_REF_ID",
                table: "HET_PROJECT",
                newName: "PRIMARY_CONTACT_ID");

            migrationBuilder.RenameColumn(
                name: "LOCAL_AREA_REF_ID",
                table: "HET_PROJECT",
                newName: "LOCAL_AREA_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_PROJECT_PRIMARY_CONTACT_REF_ID",
                table: "HET_PROJECT",
                newName: "IX_HET_PROJECT_PRIMARY_CONTACT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_PROJECT_LOCAL_AREA_REF_ID",
                table: "HET_PROJECT",
                newName: "IX_HET_PROJECT_LOCAL_AREA_ID");

            migrationBuilder.RenameColumn(
                name: "PRIMARY_CONTACT_REF_ID",
                table: "HET_OWNER",
                newName: "PRIMARY_CONTACT_ID");

            migrationBuilder.RenameColumn(
                name: "LOCAL_AREA_REF_ID",
                table: "HET_OWNER",
                newName: "LOCAL_AREA_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_OWNER_PRIMARY_CONTACT_REF_ID",
                table: "HET_OWNER",
                newName: "IX_HET_OWNER_PRIMARY_CONTACT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_OWNER_LOCAL_AREA_REF_ID",
                table: "HET_OWNER",
                newName: "IX_HET_OWNER_LOCAL_AREA_ID");

            migrationBuilder.RenameColumn(
                name: "SERVICE_AREA_REF_ID",
                table: "HET_LOCAL_AREA",
                newName: "SERVICE_AREA_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_LOCAL_AREA_SERVICE_AREA_REF_ID",
                table: "HET_LOCAL_AREA",
                newName: "IX_HET_LOCAL_AREA_SERVICE_AREA_ID");

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

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_TYPE_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                newName: "EQUIPMENT_TYPE_ID");

            migrationBuilder.RenameColumn(
                name: "ASK_NEXT_BLOCK_OPEN_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                newName: "ASK_NEXT_BLOCK_OPEN_ID");

            migrationBuilder.RenameColumn(
                name: "ASK_NEXT_BLOCK2_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                newName: "ASK_NEXT_BLOCK2_ID");

            migrationBuilder.RenameColumn(
                name: "ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                newName: "ASK_NEXT_BLOCK1_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_EQUIPMENT_TYPE_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                newName: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_EQUIPMENT_TYPE_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK_OPEN_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                newName: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK_OPEN_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK2_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                newName: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK2_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                newName: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK1_ID");

            migrationBuilder.RenameColumn(
                name: "LOCAL_AREA_REF_ID",
                table: "HET_EQUIPMENT_TYPE",
                newName: "LOCAL_AREA_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_TYPE_LOCAL_AREA_REF_ID",
                table: "HET_EQUIPMENT_TYPE",
                newName: "IX_HET_EQUIPMENT_TYPE_LOCAL_AREA_ID");

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_REF_ID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                newName: "EQUIPMENT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_ATTACHMENT_EQUIPMENT_REF_ID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                newName: "IX_HET_EQUIPMENT_ATTACHMENT_EQUIPMENT_ID");

            migrationBuilder.RenameColumn(
                name: "OWNER_REF_ID",
                table: "HET_EQUIPMENT",
                newName: "OWNER_ID");

            migrationBuilder.RenameColumn(
                name: "LOCAL_AREA_REF_ID",
                table: "HET_EQUIPMENT",
                newName: "LOCAL_AREA_ID");

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_TYPE_REF_ID",
                table: "HET_EQUIPMENT",
                newName: "EQUIPMENT_TYPE_ID");

            migrationBuilder.RenameColumn(
                name: "DUMP_TRUCK_REF_ID",
                table: "HET_EQUIPMENT",
                newName: "DUMP_TRUCK_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_OWNER_REF_ID",
                table: "HET_EQUIPMENT",
                newName: "IX_HET_EQUIPMENT_OWNER_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_LOCAL_AREA_REF_ID",
                table: "HET_EQUIPMENT",
                newName: "IX_HET_EQUIPMENT_LOCAL_AREA_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_EQUIPMENT_TYPE_REF_ID",
                table: "HET_EQUIPMENT",
                newName: "IX_HET_EQUIPMENT_EQUIPMENT_TYPE_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_DUMP_TRUCK_REF_ID",
                table: "HET_EQUIPMENT",
                newName: "IX_HET_EQUIPMENT_DUMP_TRUCK_ID");

            migrationBuilder.RenameColumn(
                name: "REGION_REF_ID",
                table: "HET_DISTRICT",
                newName: "REGION_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_DISTRICT_REGION_REF_ID",
                table: "HET_DISTRICT",
                newName: "IX_HET_DISTRICT_REGION_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_DISTRICT_HET_REGION_REGION_ID",
                table: "HET_DISTRICT",
                column: "REGION_ID",
                principalTable: "HET_REGION",
                principalColumn: "REGION_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_HET_DUMP_TRUCK_DUMP_TRUCK_ID",
                table: "HET_EQUIPMENT",
                column: "DUMP_TRUCK_ID",
                principalTable: "HET_DUMP_TRUCK",
                principalColumn: "DUMP_TRUCK_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT",
                column: "EQUIPMENT_TYPE_ID",
                principalTable: "HET_EQUIPMENT_TYPE",
                principalColumn: "EQUIPMENT_TYPE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_HET_LOCAL_AREA_LOCAL_AREA_ID",
                table: "HET_EQUIPMENT",
                column: "LOCAL_AREA_ID",
                principalTable: "HET_LOCAL_AREA",
                principalColumn: "LOCAL_AREA_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_HET_OWNER_OWNER_ID",
                table: "HET_EQUIPMENT",
                column: "OWNER_ID",
                principalTable: "HET_OWNER",
                principalColumn: "OWNER_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_ATTACHMENT_HET_EQUIPMENT_EQUIPMENT_ID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                column: "EQUIPMENT_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_HET_LOCAL_AREA_LOCAL_AREA_ID",
                table: "HET_EQUIPMENT_TYPE",
                column: "LOCAL_AREA_ID",
                principalTable: "HET_LOCAL_AREA",
                principalColumn: "LOCAL_AREA_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK1_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "ASK_NEXT_BLOCK1_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK2_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "ASK_NEXT_BLOCK2_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK_OPEN_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "ASK_NEXT_BLOCK_OPEN_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "EQUIPMENT_TYPE_ID",
                principalTable: "HET_EQUIPMENT_TYPE",
                principalColumn: "EQUIPMENT_TYPE_ID",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_HET_LOCAL_AREA_HET_SERVICE_AREA_SERVICE_AREA_ID",
                table: "HET_LOCAL_AREA",
                column: "SERVICE_AREA_ID",
                principalTable: "HET_SERVICE_AREA",
                principalColumn: "SERVICE_AREA_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_OWNER_HET_LOCAL_AREA_LOCAL_AREA_ID",
                table: "HET_OWNER",
                column: "LOCAL_AREA_ID",
                principalTable: "HET_LOCAL_AREA",
                principalColumn: "LOCAL_AREA_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_OWNER_HET_CONTACT_PRIMARY_CONTACT_ID",
                table: "HET_OWNER",
                column: "PRIMARY_CONTACT_ID",
                principalTable: "HET_CONTACT",
                principalColumn: "CONTACT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_PROJECT_HET_LOCAL_AREA_LOCAL_AREA_ID",
                table: "HET_PROJECT",
                column: "LOCAL_AREA_ID",
                principalTable: "HET_LOCAL_AREA",
                principalColumn: "LOCAL_AREA_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_PROJECT_HET_CONTACT_PRIMARY_CONTACT_ID",
                table: "HET_PROJECT",
                column: "PRIMARY_CONTACT_ID",
                principalTable: "HET_CONTACT",
                principalColumn: "CONTACT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_AGREEMENT_HET_EQUIPMENT_EQUIPMENT_ID",
                table: "HET_RENTAL_AGREEMENT",
                column: "EQUIPMENT_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_AGREEMENT_HET_PROJECT_PROJECT_ID",
                table: "HET_RENTAL_AGREEMENT",
                column: "PROJECT_ID",
                principalTable: "HET_PROJECT",
                principalColumn: "PROJECT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_AGREEMENT_CONDITION_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_ID",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                column: "RENTAL_AGREEMENT_ID",
                principalTable: "HET_RENTAL_AGREEMENT",
                principalColumn: "RENTAL_AGREEMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_AGREEMENT_RATE_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_ID",
                table: "HET_RENTAL_AGREEMENT_RATE",
                column: "RENTAL_AGREEMENT_ID",
                principalTable: "HET_RENTAL_AGREEMENT",
                principalColumn: "RENTAL_AGREEMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                table: "HET_RENTAL_REQUEST",
                column: "EQUIPMENT_TYPE_ID",
                principalTable: "HET_EQUIPMENT_TYPE",
                principalColumn: "EQUIPMENT_TYPE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_EQUIPMENT_FIRST_ON_ROTATION_LIST_ID",
                table: "HET_RENTAL_REQUEST",
                column: "FIRST_ON_ROTATION_LIST_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_LOCAL_AREA_LOCAL_AREA_ID",
                table: "HET_RENTAL_REQUEST",
                column: "LOCAL_AREA_ID",
                principalTable: "HET_LOCAL_AREA",
                principalColumn: "LOCAL_AREA_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_PROJECT_PROJECT_ID",
                table: "HET_RENTAL_REQUEST",
                column: "PROJECT_ID",
                principalTable: "HET_PROJECT",
                principalColumn: "PROJECT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_ATTACHMENT_HET_RENTAL_REQUEST_RENTAL_REQUEST_ID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                column: "RENTAL_REQUEST_ID",
                principalTable: "HET_RENTAL_REQUEST",
                principalColumn: "RENTAL_REQUEST_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_EQUIPMENT_EQUIPMENT_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                column: "EQUIPMENT_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                column: "RENTAL_AGREEMENT_ID",
                principalTable: "HET_RENTAL_AGREEMENT",
                principalColumn: "RENTAL_AGREEMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_RENTAL_REQUEST_RENTAL_REQUEST_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                column: "RENTAL_REQUEST_ID",
                principalTable: "HET_RENTAL_REQUEST",
                principalColumn: "RENTAL_REQUEST_ID",
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
                name: "FK_HET_SENIORITY_AUDIT_HET_EQUIPMENT_EQUIPMENT_ID",
                table: "HET_SENIORITY_AUDIT",
                column: "EQUIPMENT_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_SENIORITY_AUDIT_HET_LOCAL_AREA_LOCAL_AREA_ID",
                table: "HET_SENIORITY_AUDIT",
                column: "LOCAL_AREA_ID",
                principalTable: "HET_LOCAL_AREA",
                principalColumn: "LOCAL_AREA_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_SENIORITY_AUDIT_HET_OWNER_OWNER_ID",
                table: "HET_SENIORITY_AUDIT",
                column: "OWNER_ID",
                principalTable: "HET_OWNER",
                principalColumn: "OWNER_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_SERVICE_AREA_HET_DISTRICT_DISTRICT_ID",
                table: "HET_SERVICE_AREA",
                column: "DISTRICT_ID",
                principalTable: "HET_DISTRICT",
                principalColumn: "DISTRICT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_TIME_RECORD_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_ID",
                table: "HET_TIME_RECORD",
                column: "RENTAL_AGREEMENT_ID",
                principalTable: "HET_RENTAL_AGREEMENT",
                principalColumn: "RENTAL_AGREEMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_TIME_RECORD_HET_RENTAL_AGREEMENT_RATE_RENTAL_AGREEMENT_RATE_ID",
                table: "HET_TIME_RECORD",
                column: "RENTAL_AGREEMENT_RATE_ID",
                principalTable: "HET_RENTAL_AGREEMENT_RATE",
                principalColumn: "RENTAL_AGREEMENT_RATE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_HET_DISTRICT_DISTRICT_ID",
                table: "HET_USER",
                column: "DISTRICT_ID",
                principalTable: "HET_DISTRICT",
                principalColumn: "DISTRICT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_FAVOURITE_HET_USER_USER_ID",
                table: "HET_USER_FAVOURITE",
                column: "USER_ID",
                principalTable: "HET_USER",
                principalColumn: "USER_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_ROLE_HET_ROLE_ROLE_ID",
                table: "HET_USER_ROLE",
                column: "ROLE_ID",
                principalTable: "HET_ROLE",
                principalColumn: "ROLE_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_DISTRICT_HET_REGION_REGION_ID",
                table: "HET_DISTRICT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_HET_DUMP_TRUCK_DUMP_TRUCK_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_HET_LOCAL_AREA_LOCAL_AREA_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_HET_OWNER_OWNER_ID",
                table: "HET_EQUIPMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_ATTACHMENT_HET_EQUIPMENT_EQUIPMENT_ID",
                table: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_HET_LOCAL_AREA_LOCAL_AREA_ID",
                table: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK1_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK2_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK_OPEN_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_GROUP_MEMBERSHIP_HET_GROUP_GROUP_ID",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_GROUP_MEMBERSHIP_HET_USER_USER_ID",
                table: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_LOCAL_AREA_HET_SERVICE_AREA_SERVICE_AREA_ID",
                table: "HET_LOCAL_AREA");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_OWNER_HET_LOCAL_AREA_LOCAL_AREA_ID",
                table: "HET_OWNER");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_OWNER_HET_CONTACT_PRIMARY_CONTACT_ID",
                table: "HET_OWNER");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_PROJECT_HET_LOCAL_AREA_LOCAL_AREA_ID",
                table: "HET_PROJECT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_PROJECT_HET_CONTACT_PRIMARY_CONTACT_ID",
                table: "HET_PROJECT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_AGREEMENT_HET_EQUIPMENT_EQUIPMENT_ID",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_AGREEMENT_HET_PROJECT_PROJECT_ID",
                table: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_AGREEMENT_CONDITION_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_ID",
                table: "HET_RENTAL_AGREEMENT_CONDITION");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_AGREEMENT_RATE_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_ID",
                table: "HET_RENTAL_AGREEMENT_RATE");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_EQUIPMENT_FIRST_ON_ROTATION_LIST_ID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_LOCAL_AREA_LOCAL_AREA_ID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_PROJECT_PROJECT_ID",
                table: "HET_RENTAL_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_ATTACHMENT_HET_RENTAL_REQUEST_RENTAL_REQUEST_ID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_EQUIPMENT_EQUIPMENT_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_RENTAL_REQUEST_RENTAL_REQUEST_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_ROLE_PERMISSION_HET_PERMISSION_PERMISSION_ID",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_ROLE_PERMISSION_HET_ROLE_ROLE_ID",
                table: "HET_ROLE_PERMISSION");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_SENIORITY_AUDIT_HET_EQUIPMENT_EQUIPMENT_ID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_SENIORITY_AUDIT_HET_LOCAL_AREA_LOCAL_AREA_ID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_SENIORITY_AUDIT_HET_OWNER_OWNER_ID",
                table: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_SERVICE_AREA_HET_DISTRICT_DISTRICT_ID",
                table: "HET_SERVICE_AREA");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_TIME_RECORD_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_ID",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_TIME_RECORD_HET_RENTAL_AGREEMENT_RATE_RENTAL_AGREEMENT_RATE_ID",
                table: "HET_TIME_RECORD");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_HET_DISTRICT_DISTRICT_ID",
                table: "HET_USER");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_FAVOURITE_HET_USER_USER_ID",
                table: "HET_USER_FAVOURITE");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_USER_ROLE_HET_ROLE_ROLE_ID",
                table: "HET_USER_ROLE");

            migrationBuilder.RenameColumn(
                name: "ROLE_ID",
                table: "HET_USER_ROLE",
                newName: "ROLE_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_USER_ROLE_ROLE_ID",
                table: "HET_USER_ROLE",
                newName: "IX_HET_USER_ROLE_ROLE_REF_ID");

            migrationBuilder.RenameColumn(
                name: "USER_ID",
                table: "HET_USER_FAVOURITE",
                newName: "USER_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_USER_FAVOURITE_USER_ID",
                table: "HET_USER_FAVOURITE",
                newName: "IX_HET_USER_FAVOURITE_USER_REF_ID");

            migrationBuilder.RenameColumn(
                name: "DISTRICT_ID",
                table: "HET_USER",
                newName: "DISTRICT_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_USER_DISTRICT_ID",
                table: "HET_USER",
                newName: "IX_HET_USER_DISTRICT_REF_ID");

            migrationBuilder.RenameColumn(
                name: "RENTAL_AGREEMENT_RATE_ID",
                table: "HET_TIME_RECORD",
                newName: "RENTAL_AGREEMENT_REF_ID");

            migrationBuilder.RenameColumn(
                name: "RENTAL_AGREEMENT_ID",
                table: "HET_TIME_RECORD",
                newName: "RENTAL_AGREEMENT_RATE_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_TIME_RECORD_RENTAL_AGREEMENT_RATE_ID",
                table: "HET_TIME_RECORD",
                newName: "IX_HET_TIME_RECORD_RENTAL_AGREEMENT_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_TIME_RECORD_RENTAL_AGREEMENT_ID",
                table: "HET_TIME_RECORD",
                newName: "IX_HET_TIME_RECORD_RENTAL_AGREEMENT_RATE_REF_ID");

            migrationBuilder.RenameColumn(
                name: "DISTRICT_ID",
                table: "HET_SERVICE_AREA",
                newName: "DISTRICT_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_SERVICE_AREA_DISTRICT_ID",
                table: "HET_SERVICE_AREA",
                newName: "IX_HET_SERVICE_AREA_DISTRICT_REF_ID");

            migrationBuilder.RenameColumn(
                name: "OWNER_ID",
                table: "HET_SENIORITY_AUDIT",
                newName: "OWNER_REF_ID");

            migrationBuilder.RenameColumn(
                name: "LOCAL_AREA_ID",
                table: "HET_SENIORITY_AUDIT",
                newName: "LOCAL_AREA_REF_ID");

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_ID",
                table: "HET_SENIORITY_AUDIT",
                newName: "EQUIPMENT_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_SENIORITY_AUDIT_OWNER_ID",
                table: "HET_SENIORITY_AUDIT",
                newName: "IX_HET_SENIORITY_AUDIT_OWNER_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_SENIORITY_AUDIT_LOCAL_AREA_ID",
                table: "HET_SENIORITY_AUDIT",
                newName: "IX_HET_SENIORITY_AUDIT_LOCAL_AREA_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_SENIORITY_AUDIT_EQUIPMENT_ID",
                table: "HET_SENIORITY_AUDIT",
                newName: "IX_HET_SENIORITY_AUDIT_EQUIPMENT_REF_ID");

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
                name: "RENTAL_REQUEST_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "RENTAL_REQUEST_REF_ID");

            migrationBuilder.RenameColumn(
                name: "RENTAL_AGREEMENT_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "RENTAL_AGREEMENT_REF_ID");

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "EQUIPMENT_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_ROTATION_LIST_RENTAL_REQUEST_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "IX_HET_RENTAL_REQUEST_ROTATION_LIST_RENTAL_REQUEST_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_ROTATION_LIST_RENTAL_AGREEMENT_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "IX_HET_RENTAL_REQUEST_ROTATION_LIST_RENTAL_AGREEMENT_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_ROTATION_LIST_EQUIPMENT_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                newName: "IX_HET_RENTAL_REQUEST_ROTATION_LIST_EQUIPMENT_REF_ID");

            migrationBuilder.RenameColumn(
                name: "RENTAL_REQUEST_ID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                newName: "RENTAL_REQUEST_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_ATTACHMENT_RENTAL_REQUEST_ID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                newName: "IX_HET_RENTAL_REQUEST_ATTACHMENT_RENTAL_REQUEST_REF_ID");

            migrationBuilder.RenameColumn(
                name: "PROJECT_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "PROJECT_REF_ID");

            migrationBuilder.RenameColumn(
                name: "LOCAL_AREA_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "LOCAL_AREA_REF_ID");

            migrationBuilder.RenameColumn(
                name: "FIRST_ON_ROTATION_LIST_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "FIRST_ON_ROTATION_LIST_REF_ID");

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_TYPE_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "EQUIPMENT_TYPE_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_PROJECT_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "IX_HET_RENTAL_REQUEST_PROJECT_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_LOCAL_AREA_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "IX_HET_RENTAL_REQUEST_LOCAL_AREA_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_FIRST_ON_ROTATION_LIST_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "IX_HET_RENTAL_REQUEST_FIRST_ON_ROTATION_LIST_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_REQUEST_EQUIPMENT_TYPE_ID",
                table: "HET_RENTAL_REQUEST",
                newName: "IX_HET_RENTAL_REQUEST_EQUIPMENT_TYPE_REF_ID");

            migrationBuilder.RenameColumn(
                name: "RENTAL_AGREEMENT_ID",
                table: "HET_RENTAL_AGREEMENT_RATE",
                newName: "RENTAL_AGREEMENT_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_AGREEMENT_RATE_RENTAL_AGREEMENT_ID",
                table: "HET_RENTAL_AGREEMENT_RATE",
                newName: "IX_HET_RENTAL_AGREEMENT_RATE_RENTAL_AGREEMENT_REF_ID");

            migrationBuilder.RenameColumn(
                name: "RENTAL_AGREEMENT_ID",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                newName: "RENTAL_AGREEMENT_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_AGREEMENT_CONDITION_RENTAL_AGREEMENT_ID",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                newName: "IX_HET_RENTAL_AGREEMENT_CONDITION_RENTAL_AGREEMENT_REF_ID");

            migrationBuilder.RenameColumn(
                name: "PROJECT_ID",
                table: "HET_RENTAL_AGREEMENT",
                newName: "PROJECT_REF_ID");

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_ID",
                table: "HET_RENTAL_AGREEMENT",
                newName: "EQUIPMENT_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_AGREEMENT_PROJECT_ID",
                table: "HET_RENTAL_AGREEMENT",
                newName: "IX_HET_RENTAL_AGREEMENT_PROJECT_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_RENTAL_AGREEMENT_EQUIPMENT_ID",
                table: "HET_RENTAL_AGREEMENT",
                newName: "IX_HET_RENTAL_AGREEMENT_EQUIPMENT_REF_ID");

            migrationBuilder.RenameColumn(
                name: "PRIMARY_CONTACT_ID",
                table: "HET_PROJECT",
                newName: "PRIMARY_CONTACT_REF_ID");

            migrationBuilder.RenameColumn(
                name: "LOCAL_AREA_ID",
                table: "HET_PROJECT",
                newName: "LOCAL_AREA_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_PROJECT_PRIMARY_CONTACT_ID",
                table: "HET_PROJECT",
                newName: "IX_HET_PROJECT_PRIMARY_CONTACT_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_PROJECT_LOCAL_AREA_ID",
                table: "HET_PROJECT",
                newName: "IX_HET_PROJECT_LOCAL_AREA_REF_ID");

            migrationBuilder.RenameColumn(
                name: "PRIMARY_CONTACT_ID",
                table: "HET_OWNER",
                newName: "PRIMARY_CONTACT_REF_ID");

            migrationBuilder.RenameColumn(
                name: "LOCAL_AREA_ID",
                table: "HET_OWNER",
                newName: "LOCAL_AREA_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_OWNER_PRIMARY_CONTACT_ID",
                table: "HET_OWNER",
                newName: "IX_HET_OWNER_PRIMARY_CONTACT_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_OWNER_LOCAL_AREA_ID",
                table: "HET_OWNER",
                newName: "IX_HET_OWNER_LOCAL_AREA_REF_ID");

            migrationBuilder.RenameColumn(
                name: "SERVICE_AREA_ID",
                table: "HET_LOCAL_AREA",
                newName: "SERVICE_AREA_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_LOCAL_AREA_SERVICE_AREA_ID",
                table: "HET_LOCAL_AREA",
                newName: "IX_HET_LOCAL_AREA_SERVICE_AREA_REF_ID");

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

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                newName: "EQUIPMENT_TYPE_REF_ID");

            migrationBuilder.RenameColumn(
                name: "ASK_NEXT_BLOCK_OPEN_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                newName: "ASK_NEXT_BLOCK_OPEN_REF_ID");

            migrationBuilder.RenameColumn(
                name: "ASK_NEXT_BLOCK2_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                newName: "ASK_NEXT_BLOCK2_REF_ID");

            migrationBuilder.RenameColumn(
                name: "ASK_NEXT_BLOCK1_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                newName: "ASK_NEXT_BLOCK1_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                newName: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_EQUIPMENT_TYPE_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK_OPEN_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                newName: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK_OPEN_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK2_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                newName: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK2_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK1_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                newName: "IX_HET_EQUIPMENT_TYPE_NEXT_RENTAL_ASK_NEXT_BLOCK1_REF_ID");

            migrationBuilder.RenameColumn(
                name: "LOCAL_AREA_ID",
                table: "HET_EQUIPMENT_TYPE",
                newName: "LOCAL_AREA_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_TYPE_LOCAL_AREA_ID",
                table: "HET_EQUIPMENT_TYPE",
                newName: "IX_HET_EQUIPMENT_TYPE_LOCAL_AREA_REF_ID");

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_ID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                newName: "EQUIPMENT_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_ATTACHMENT_EQUIPMENT_ID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                newName: "IX_HET_EQUIPMENT_ATTACHMENT_EQUIPMENT_REF_ID");

            migrationBuilder.RenameColumn(
                name: "OWNER_ID",
                table: "HET_EQUIPMENT",
                newName: "OWNER_REF_ID");

            migrationBuilder.RenameColumn(
                name: "LOCAL_AREA_ID",
                table: "HET_EQUIPMENT",
                newName: "LOCAL_AREA_REF_ID");

            migrationBuilder.RenameColumn(
                name: "EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT",
                newName: "EQUIPMENT_TYPE_REF_ID");

            migrationBuilder.RenameColumn(
                name: "DUMP_TRUCK_ID",
                table: "HET_EQUIPMENT",
                newName: "DUMP_TRUCK_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_OWNER_ID",
                table: "HET_EQUIPMENT",
                newName: "IX_HET_EQUIPMENT_OWNER_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_LOCAL_AREA_ID",
                table: "HET_EQUIPMENT",
                newName: "IX_HET_EQUIPMENT_LOCAL_AREA_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_EQUIPMENT_TYPE_ID",
                table: "HET_EQUIPMENT",
                newName: "IX_HET_EQUIPMENT_EQUIPMENT_TYPE_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_EQUIPMENT_DUMP_TRUCK_ID",
                table: "HET_EQUIPMENT",
                newName: "IX_HET_EQUIPMENT_DUMP_TRUCK_REF_ID");

            migrationBuilder.RenameColumn(
                name: "REGION_ID",
                table: "HET_DISTRICT",
                newName: "REGION_REF_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HET_DISTRICT_REGION_ID",
                table: "HET_DISTRICT",
                newName: "IX_HET_DISTRICT_REGION_REF_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_DISTRICT_HET_REGION_REGION_REF_ID",
                table: "HET_DISTRICT",
                column: "REGION_REF_ID",
                principalTable: "HET_REGION",
                principalColumn: "REGION_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_HET_DUMP_TRUCK_DUMP_TRUCK_REF_ID",
                table: "HET_EQUIPMENT",
                column: "DUMP_TRUCK_REF_ID",
                principalTable: "HET_DUMP_TRUCK",
                principalColumn: "DUMP_TRUCK_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_REF_ID",
                table: "HET_EQUIPMENT",
                column: "EQUIPMENT_TYPE_REF_ID",
                principalTable: "HET_EQUIPMENT_TYPE",
                principalColumn: "EQUIPMENT_TYPE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                table: "HET_EQUIPMENT",
                column: "LOCAL_AREA_REF_ID",
                principalTable: "HET_LOCAL_AREA",
                principalColumn: "LOCAL_AREA_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_HET_OWNER_OWNER_REF_ID",
                table: "HET_EQUIPMENT",
                column: "OWNER_REF_ID",
                principalTable: "HET_OWNER",
                principalColumn: "OWNER_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_ATTACHMENT_HET_EQUIPMENT_EQUIPMENT_REF_ID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                column: "EQUIPMENT_REF_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                table: "HET_EQUIPMENT_TYPE",
                column: "LOCAL_AREA_REF_ID",
                principalTable: "HET_LOCAL_AREA",
                principalColumn: "LOCAL_AREA_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK1_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "ASK_NEXT_BLOCK1_REF_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK2_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "ASK_NEXT_BLOCK2_REF_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_ASK_NEXT_BLOCK_OPEN_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "ASK_NEXT_BLOCK_OPEN_REF_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_EQUIPMENT_TYPE_NEXT_RENTAL_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_REF_ID",
                table: "HET_EQUIPMENT_TYPE_NEXT_RENTAL",
                column: "EQUIPMENT_TYPE_REF_ID",
                principalTable: "HET_EQUIPMENT_TYPE",
                principalColumn: "EQUIPMENT_TYPE_ID",
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
                name: "FK_HET_LOCAL_AREA_HET_SERVICE_AREA_SERVICE_AREA_REF_ID",
                table: "HET_LOCAL_AREA",
                column: "SERVICE_AREA_REF_ID",
                principalTable: "HET_SERVICE_AREA",
                principalColumn: "SERVICE_AREA_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_OWNER_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                table: "HET_OWNER",
                column: "LOCAL_AREA_REF_ID",
                principalTable: "HET_LOCAL_AREA",
                principalColumn: "LOCAL_AREA_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_OWNER_HET_CONTACT_PRIMARY_CONTACT_REF_ID",
                table: "HET_OWNER",
                column: "PRIMARY_CONTACT_REF_ID",
                principalTable: "HET_CONTACT",
                principalColumn: "CONTACT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_PROJECT_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                table: "HET_PROJECT",
                column: "LOCAL_AREA_REF_ID",
                principalTable: "HET_LOCAL_AREA",
                principalColumn: "LOCAL_AREA_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_PROJECT_HET_CONTACT_PRIMARY_CONTACT_REF_ID",
                table: "HET_PROJECT",
                column: "PRIMARY_CONTACT_REF_ID",
                principalTable: "HET_CONTACT",
                principalColumn: "CONTACT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_AGREEMENT_HET_EQUIPMENT_EQUIPMENT_REF_ID",
                table: "HET_RENTAL_AGREEMENT",
                column: "EQUIPMENT_REF_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_AGREEMENT_HET_PROJECT_PROJECT_REF_ID",
                table: "HET_RENTAL_AGREEMENT",
                column: "PROJECT_REF_ID",
                principalTable: "HET_PROJECT",
                principalColumn: "PROJECT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_AGREEMENT_CONDITION_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_REF_ID",
                table: "HET_RENTAL_AGREEMENT_CONDITION",
                column: "RENTAL_AGREEMENT_REF_ID",
                principalTable: "HET_RENTAL_AGREEMENT",
                principalColumn: "RENTAL_AGREEMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_AGREEMENT_RATE_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_REF_ID",
                table: "HET_RENTAL_AGREEMENT_RATE",
                column: "RENTAL_AGREEMENT_REF_ID",
                principalTable: "HET_RENTAL_AGREEMENT",
                principalColumn: "RENTAL_AGREEMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_REF_ID",
                table: "HET_RENTAL_REQUEST",
                column: "EQUIPMENT_TYPE_REF_ID",
                principalTable: "HET_EQUIPMENT_TYPE",
                principalColumn: "EQUIPMENT_TYPE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_EQUIPMENT_FIRST_ON_ROTATION_LIST_REF_ID",
                table: "HET_RENTAL_REQUEST",
                column: "FIRST_ON_ROTATION_LIST_REF_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                table: "HET_RENTAL_REQUEST",
                column: "LOCAL_AREA_REF_ID",
                principalTable: "HET_LOCAL_AREA",
                principalColumn: "LOCAL_AREA_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_HET_PROJECT_PROJECT_REF_ID",
                table: "HET_RENTAL_REQUEST",
                column: "PROJECT_REF_ID",
                principalTable: "HET_PROJECT",
                principalColumn: "PROJECT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_ATTACHMENT_HET_RENTAL_REQUEST_RENTAL_REQUEST_REF_ID",
                table: "HET_RENTAL_REQUEST_ATTACHMENT",
                column: "RENTAL_REQUEST_REF_ID",
                principalTable: "HET_RENTAL_REQUEST",
                principalColumn: "RENTAL_REQUEST_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_EQUIPMENT_EQUIPMENT_REF_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                column: "EQUIPMENT_REF_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_REF_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                column: "RENTAL_AGREEMENT_REF_ID",
                principalTable: "HET_RENTAL_AGREEMENT",
                principalColumn: "RENTAL_AGREEMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_RENTAL_REQUEST_RENTAL_REQUEST_REF_ID",
                table: "HET_RENTAL_REQUEST_ROTATION_LIST",
                column: "RENTAL_REQUEST_REF_ID",
                principalTable: "HET_RENTAL_REQUEST",
                principalColumn: "RENTAL_REQUEST_ID",
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
                name: "FK_HET_SENIORITY_AUDIT_HET_EQUIPMENT_EQUIPMENT_REF_ID",
                table: "HET_SENIORITY_AUDIT",
                column: "EQUIPMENT_REF_ID",
                principalTable: "HET_EQUIPMENT",
                principalColumn: "EQUIPMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_SENIORITY_AUDIT_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                table: "HET_SENIORITY_AUDIT",
                column: "LOCAL_AREA_REF_ID",
                principalTable: "HET_LOCAL_AREA",
                principalColumn: "LOCAL_AREA_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_SENIORITY_AUDIT_HET_OWNER_OWNER_REF_ID",
                table: "HET_SENIORITY_AUDIT",
                column: "OWNER_REF_ID",
                principalTable: "HET_OWNER",
                principalColumn: "OWNER_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_SERVICE_AREA_HET_DISTRICT_DISTRICT_REF_ID",
                table: "HET_SERVICE_AREA",
                column: "DISTRICT_REF_ID",
                principalTable: "HET_DISTRICT",
                principalColumn: "DISTRICT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_TIME_RECORD_HET_RENTAL_AGREEMENT_RATE_RENTAL_AGREEMENT_RATE_REF_ID",
                table: "HET_TIME_RECORD",
                column: "RENTAL_AGREEMENT_RATE_REF_ID",
                principalTable: "HET_RENTAL_AGREEMENT_RATE",
                principalColumn: "RENTAL_AGREEMENT_RATE_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_TIME_RECORD_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_REF_ID",
                table: "HET_TIME_RECORD",
                column: "RENTAL_AGREEMENT_REF_ID",
                principalTable: "HET_RENTAL_AGREEMENT",
                principalColumn: "RENTAL_AGREEMENT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_HET_DISTRICT_DISTRICT_REF_ID",
                table: "HET_USER",
                column: "DISTRICT_REF_ID",
                principalTable: "HET_DISTRICT",
                principalColumn: "DISTRICT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_FAVOURITE_HET_USER_USER_REF_ID",
                table: "HET_USER_FAVOURITE",
                column: "USER_REF_ID",
                principalTable: "HET_USER",
                principalColumn: "USER_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_USER_ROLE_HET_ROLE_ROLE_REF_ID",
                table: "HET_USER_ROLE",
                column: "ROLE_REF_ID",
                principalTable: "HET_ROLE",
                principalColumn: "ROLE_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
