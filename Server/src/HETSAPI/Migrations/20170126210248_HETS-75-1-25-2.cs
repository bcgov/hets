using System;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HETSAPI.Migrations
{
    public partial class HETS751252 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HET_NOTIFICATION");

            migrationBuilder.DropTable(
                name: "HET_NOTIFICATION_EVENT");

            migrationBuilder.CreateTable(
                name: "HET_DUMP_TRUCK",
                columns: table => new
                {
                    DUMP_TRUCK_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    BELLY_DUMP = table.Column<string>(nullable: true),
                    BOX_CAPACITY = table.Column<string>(nullable: true),
                    BOX_HEIGHT = table.Column<string>(nullable: true),
                    BOX_LENGTH = table.Column<string>(nullable: true),
                    BOX_WIDTH = table.Column<string>(nullable: true),
                    FRONT_AXLE_CAPACITY = table.Column<string>(nullable: true),
                    FRONT_TIRE_SIZE = table.Column<string>(nullable: true),
                    FRONT_TIRE_UOM = table.Column<string>(nullable: true),
                    HILIFT_GATE = table.Column<string>(nullable: true),
                    LEGAL_CAPACITY = table.Column<string>(nullable: true),
                    LEGAL_LOAD = table.Column<string>(nullable: true),
                    LEGAL_PUPTARE_WEIGHT = table.Column<string>(nullable: true),
                    LICENCED_CAPACITY = table.Column<string>(nullable: true),
                    LICENCED_GVW = table.Column<string>(nullable: true),
                    LICENCED_GVWUOM = table.Column<string>(nullable: true),
                    LICENCED_LOAD = table.Column<string>(nullable: true),
                    LICENCED_PUPTARE_WEIGHT = table.Column<string>(nullable: true),
                    LICENCED_TARE_WEIGHT = table.Column<string>(nullable: true),
                    PUP = table.Column<string>(nullable: true),
                    REAR_AXLE_CAPACITY = table.Column<string>(nullable: true),
                    REAR_AXLE_SPACING = table.Column<string>(nullable: true),
                    ROCK_BOX = table.Column<string>(nullable: true),
                    SEAL_COAT_HITCH = table.Column<string>(nullable: true),
                    SINGLE_AXLE = table.Column<string>(nullable: true),
                    TANDEM_AXLE = table.Column<string>(nullable: true),
                    TRAILER_BOX_CAPACITY = table.Column<string>(nullable: true),
                    TRAILER_BOX_HEIGHT = table.Column<string>(nullable: true),
                    TRAILER_BOX_LENGTH = table.Column<string>(nullable: true),
                    TRAILER_BOX_WIDTH = table.Column<string>(nullable: true),
                    TRIDEM = table.Column<string>(nullable: true),
                    WATER_TRUCK = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_DUMP_TRUCK", x => x.DUMP_TRUCK_ID);
                });

            migrationBuilder.CreateTable(
                name: "HET_EQUIPMENT_ATTACHMENT_TYPE",
                columns: table => new
                {
                    EQUIPMENT_ATTACHMENT_TYPE_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CODE = table.Column<string>(nullable: true),
                    DESCRIPTION = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_EQUIPMENT_ATTACHMENT_TYPE", x => x.EQUIPMENT_ATTACHMENT_TYPE_ID);
                });

            migrationBuilder.CreateTable(
                name: "HET_LOCAL_AREA",
                columns: table => new
                {
                    LOCAL_AREA_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NAME = table.Column<string>(nullable: true),
                    SERVICE_AREA_REF_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_LOCAL_AREA", x => x.LOCAL_AREA_ID);
                    table.ForeignKey(
                        name: "FK_HET_LOCAL_AREA_HET_SERVICE_AREA_SERVICE_AREA_REF_ID",
                        column: x => x.SERVICE_AREA_REF_ID,
                        principalTable: "HET_SERVICE_AREA",
                        principalColumn: "SERVICE_AREA_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_EQUIPMENT_TYPE",
                columns: table => new
                {
                    EQUIPMENT_TYPE_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CODE = table.Column<string>(nullable: true),
                    DESCRIPTION = table.Column<string>(nullable: true),
                    EQUIP_RENTAL_RATE_NO = table.Column<float>(nullable: true),
                    EQUIP_RENTAL_RATE_PAGE = table.Column<float>(nullable: true),
                    EXTEND_HOURS = table.Column<float>(nullable: true),
                    LOCAL_AREA_REF_ID = table.Column<int>(nullable: true),
                    MAX_HOURS = table.Column<float>(nullable: true),
                    MAX_HOURS_SUB = table.Column<float>(nullable: true),
                    SECOND_BLK = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_EQUIPMENT_TYPE", x => x.EQUIPMENT_TYPE_ID);
                    table.ForeignKey(
                        name: "FK_HET_EQUIPMENT_TYPE_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                        column: x => x.LOCAL_AREA_REF_ID,
                        principalTable: "HET_LOCAL_AREA",
                        principalColumn: "LOCAL_AREA_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_ROTATION_LIST",
                columns: table => new
                {
                    ROTATION_LIST_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EQUIPMENT_TYPE_REF_ID = table.Column<int>(nullable: true),
                    LOCAL_AREA_REF_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_ROTATION_LIST", x => x.ROTATION_LIST_ID);
                    table.ForeignKey(
                        name: "FK_HET_ROTATION_LIST_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_REF_ID",
                        column: x => x.EQUIPMENT_TYPE_REF_ID,
                        principalTable: "HET_EQUIPMENT_TYPE",
                        principalColumn: "EQUIPMENT_TYPE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_ROTATION_LIST_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                        column: x => x.LOCAL_AREA_REF_ID,
                        principalTable: "HET_LOCAL_AREA",
                        principalColumn: "LOCAL_AREA_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_CONTACT_ADDRESS",
                columns: table => new
                {
                    CONTACT_ADDRESS_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ADDRESS_LINE1 = table.Column<string>(nullable: true),
                    ADDRESS_LINE2 = table.Column<string>(nullable: true),
                    CITY = table.Column<string>(nullable: true),
                    CONTACT_ID = table.Column<int>(nullable: true),
                    POSTAL_CODE = table.Column<string>(nullable: true),
                    PROVINCE = table.Column<string>(nullable: true),
                    TYPE = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_CONTACT_ADDRESS", x => x.CONTACT_ADDRESS_ID);
                });

            migrationBuilder.CreateTable(
                name: "HET_CONTACT_PHONE",
                columns: table => new
                {
                    CONTACT_PHONE_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CONTACT_ID = table.Column<int>(nullable: true),
                    PHONE_NUMBER = table.Column<string>(nullable: true),
                    TYPE = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_CONTACT_PHONE", x => x.CONTACT_PHONE_ID);
                });

            migrationBuilder.CreateTable(
                name: "HET_OWNER",
                columns: table => new
                {
                    OWNER_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ARCHIVE_CD = table.Column<string>(nullable: true),
                    ARCHIVE_REASON = table.Column<string>(nullable: true),
                    CGLCOMPANY = table.Column<string>(nullable: true),
                    CGLEND_DATE = table.Column<DateTime>(nullable: true),
                    CGLPOLICY = table.Column<string>(nullable: true),
                    CGLSTART_DATE = table.Column<DateTime>(nullable: true),
                    COMMENT = table.Column<string>(nullable: true),
                    CONTACT_PERSON = table.Column<string>(nullable: true),
                    LOCAL_AREA_REF_ID = table.Column<int>(nullable: true),
                    LOCAL_TO_AREA = table.Column<string>(nullable: true),
                    MAINTENANCE_CONTRACTOR = table.Column<string>(nullable: true),
                    OWNER_CD = table.Column<string>(nullable: true),
                    OWNER_FIRST_NAME = table.Column<string>(nullable: true),
                    OWNER_LAST_NAME = table.Column<string>(nullable: true),
                    PRIMARY_CONTACT_REF_ID = table.Column<int>(nullable: true),
                    STATUS_CD = table.Column<string>(nullable: true),
                    WCBEXPIRY_DATE = table.Column<DateTime>(nullable: true),
                    WCBNUM = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_OWNER", x => x.OWNER_ID);
                    table.ForeignKey(
                        name: "FK_HET_OWNER_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                        column: x => x.LOCAL_AREA_REF_ID,
                        principalTable: "HET_LOCAL_AREA",
                        principalColumn: "LOCAL_AREA_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_EQUIPMENT",
                columns: table => new
                {
                    EQUIPMENT_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ADDRESS_LINE1 = table.Column<string>(nullable: true),
                    ADDRESS_LINE2 = table.Column<string>(nullable: true),
                    ADDRESS_LINE3 = table.Column<string>(nullable: true),
                    ADDRESS_LINE4 = table.Column<string>(nullable: true),
                    APPROVAL = table.Column<string>(nullable: true),
                    APPROVED_DATE = table.Column<DateTime>(nullable: true),
                    ARCHIVE_CD = table.Column<string>(nullable: true),
                    ARCHIVE_DATE = table.Column<DateTime>(nullable: true),
                    ARCHIVE_REASON = table.Column<string>(nullable: true),
                    BLOCK_NUMBER = table.Column<float>(nullable: true),
                    CITY = table.Column<string>(nullable: true),
                    COMMENT = table.Column<string>(nullable: true),
                    CYCLE_HRS_WRK = table.Column<float>(nullable: true),
                    DRAFT_BLOCK_NUM = table.Column<float>(nullable: true),
                    DUMP_TRUCK_DETAILS_REF_ID = table.Column<int>(nullable: true),
                    EQUIP_CD = table.Column<string>(nullable: true),
                    EQUIPMENT_TYPE_REF_ID = table.Column<int>(nullable: true),
                    FROZEN_OUT = table.Column<string>(nullable: true),
                    LAST_DATE = table.Column<string>(nullable: true),
                    LICENCE = table.Column<string>(nullable: true),
                    LOCAL_AREA_REF_ID = table.Column<int>(nullable: true),
                    MAKE = table.Column<string>(nullable: true),
                    MODEL = table.Column<string>(nullable: true),
                    NUM_YEARS = table.Column<float>(nullable: true),
                    OPERATOR = table.Column<string>(nullable: true),
                    OWNER_REF_ID = table.Column<int>(nullable: true),
                    PAY_RATE = table.Column<float>(nullable: true),
                    POSTAL = table.Column<string>(nullable: true),
                    PREV_REG_AREA = table.Column<string>(nullable: true),
                    RECEIVED_DATE = table.Column<DateTime>(nullable: true),
                    REFUSE_RATE = table.Column<string>(nullable: true),
                    REG_DUMP_TRUCK = table.Column<string>(nullable: true),
                    SENIORITY = table.Column<float>(nullable: true),
                    SERIAL_NUM = table.Column<string>(nullable: true),
                    SIZE = table.Column<string>(nullable: true),
                    STATUS_CD = table.Column<string>(nullable: true),
                    TO_DATE = table.Column<DateTime>(nullable: true),
                    TYPE = table.Column<string>(nullable: true),
                    WORKING = table.Column<string>(nullable: true),
                    YTD = table.Column<float>(nullable: true),
                    YTD1 = table.Column<float>(nullable: true),
                    YTD2 = table.Column<float>(nullable: true),
                    YTD3 = table.Column<float>(nullable: true),
                    YEAR = table.Column<string>(nullable: true),
                    YEAR_END_REG = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_EQUIPMENT", x => x.EQUIPMENT_ID);
                    table.ForeignKey(
                        name: "FK_HET_EQUIPMENT_HET_DUMP_TRUCK_DUMP_TRUCK_DETAILS_REF_ID",
                        column: x => x.DUMP_TRUCK_DETAILS_REF_ID,
                        principalTable: "HET_DUMP_TRUCK",
                        principalColumn: "DUMP_TRUCK_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_EQUIPMENT_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_REF_ID",
                        column: x => x.EQUIPMENT_TYPE_REF_ID,
                        principalTable: "HET_EQUIPMENT_TYPE",
                        principalColumn: "EQUIPMENT_TYPE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_EQUIPMENT_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                        column: x => x.LOCAL_AREA_REF_ID,
                        principalTable: "HET_LOCAL_AREA",
                        principalColumn: "LOCAL_AREA_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_EQUIPMENT_HET_OWNER_OWNER_REF_ID",
                        column: x => x.OWNER_REF_ID,
                        principalTable: "HET_OWNER",
                        principalColumn: "OWNER_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_EQUIPMENT_ATTACHMENT",
                columns: table => new
                {
                    EQUIPMENT_ATTACHMENT_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DESCRIPTION = table.Column<string>(nullable: true),
                    EQUIPMENT_REF_ID = table.Column<int>(nullable: true),
                    SEQ_NUM = table.Column<int>(nullable: true),
                    TYPE_REF_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_EQUIPMENT_ATTACHMENT", x => x.EQUIPMENT_ATTACHMENT_ID);
                    table.ForeignKey(
                        name: "FK_HET_EQUIPMENT_ATTACHMENT_HET_EQUIPMENT_EQUIPMENT_REF_ID",
                        column: x => x.EQUIPMENT_REF_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_EQUIPMENT_ATTACHMENT_HET_EQUIPMENT_ATTACHMENT_TYPE_TYPE_REF_ID",
                        column: x => x.TYPE_REF_ID,
                        principalTable: "HET_EQUIPMENT_ATTACHMENT_TYPE",
                        principalColumn: "EQUIPMENT_ATTACHMENT_TYPE_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_ROTATION_LIST_BLOCK",
                columns: table => new
                {
                    ROTATION_LIST_BLOCK_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    BLOCK_NAME = table.Column<string>(nullable: true),
                    BLOCK_NUM = table.Column<int>(nullable: true),
                    CLOSED = table.Column<string>(nullable: true),
                    CLOSED_BY = table.Column<string>(nullable: true),
                    CLOSED_COMMENTS = table.Column<string>(nullable: true),
                    CLOSED_DATE = table.Column<DateTime>(nullable: true),
                    CYCLE_NUM = table.Column<float>(nullable: true),
                    LAST_HIRED_EQUIPMENT_REF_ID = table.Column<int>(nullable: true),
                    MAX_CYCLE = table.Column<float>(nullable: true),
                    MOVED = table.Column<string>(nullable: true),
                    RESERVED_BY = table.Column<string>(nullable: true),
                    RESERVED_DATE = table.Column<DateTime>(nullable: true),
                    ROTATED_BLOCK = table.Column<int>(nullable: true),
                    ROTATION_LIST_REF_ID = table.Column<int>(nullable: true),
                    START_CYCLE_EQUIPMENT_REF_ID = table.Column<int>(nullable: true),
                    START_WAS_ZERO = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_ROTATION_LIST_BLOCK", x => x.ROTATION_LIST_BLOCK_ID);
                    table.ForeignKey(
                        name: "FK_HET_ROTATION_LIST_BLOCK_HET_EQUIPMENT_LAST_HIRED_EQUIPMENT_REF_ID",
                        column: x => x.LAST_HIRED_EQUIPMENT_REF_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_ROTATION_LIST_BLOCK_HET_ROTATION_LIST_ROTATION_LIST_REF_ID",
                        column: x => x.ROTATION_LIST_REF_ID,
                        principalTable: "HET_ROTATION_LIST",
                        principalColumn: "ROTATION_LIST_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_ROTATION_LIST_BLOCK_HET_EQUIPMENT_START_CYCLE_EQUIPMENT_REF_ID",
                        column: x => x.START_CYCLE_EQUIPMENT_REF_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_PROJECT",
                columns: table => new
                {
                    PROJECT_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    JOB_DESC1 = table.Column<string>(nullable: true),
                    JOB_DESC2 = table.Column<string>(nullable: true),
                    PRIMARY_CONTACT_REF_ID = table.Column<int>(nullable: true),
                    PROJECT_NUM = table.Column<string>(nullable: true),
                    SERVICE_AREA_REF_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_PROJECT", x => x.PROJECT_ID);
                    table.ForeignKey(
                        name: "FK_HET_PROJECT_HET_SERVICE_AREA_SERVICE_AREA_REF_ID",
                        column: x => x.SERVICE_AREA_REF_ID,
                        principalTable: "HET_SERVICE_AREA",
                        principalColumn: "SERVICE_AREA_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_CONTACT",
                columns: table => new
                {
                    CONTACT_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    GIVEN_NAME = table.Column<string>(nullable: true),
                    NOTES = table.Column<string>(nullable: true),
                    OWNER_ID = table.Column<int>(nullable: true),
                    PROJECT_ID = table.Column<int>(nullable: true),
                    ROLE = table.Column<string>(nullable: true),
                    SURNAME = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_CONTACT", x => x.CONTACT_ID);
                    table.ForeignKey(
                        name: "FK_HET_CONTACT_HET_OWNER_OWNER_ID",
                        column: x => x.OWNER_ID,
                        principalTable: "HET_OWNER",
                        principalColumn: "OWNER_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_CONTACT_HET_PROJECT_PROJECT_ID",
                        column: x => x.PROJECT_ID,
                        principalTable: "HET_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_RENTAL_AGREEMENT",
                columns: table => new
                {
                    RENTAL_AGREEMENT_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EQUIPMENT_REF_ID = table.Column<int>(nullable: true),
                    PROJECT_REF_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_RENTAL_AGREEMENT", x => x.RENTAL_AGREEMENT_ID);
                    table.ForeignKey(
                        name: "FK_HET_RENTAL_AGREEMENT_HET_EQUIPMENT_EQUIPMENT_REF_ID",
                        column: x => x.EQUIPMENT_REF_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_RENTAL_AGREEMENT_HET_PROJECT_PROJECT_REF_ID",
                        column: x => x.PROJECT_REF_ID,
                        principalTable: "HET_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_REQUEST",
                columns: table => new
                {
                    REQUEST_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EQUIPMENT_COUNT = table.Column<int>(nullable: true),
                    EQUIPMENT_TYPE_REF_ID = table.Column<int>(nullable: true),
                    EXPECTED_END_DATE = table.Column<DateTime>(nullable: true),
                    EXPECTED_HOURS = table.Column<int>(nullable: true),
                    EXPECTED_START_DATE = table.Column<DateTime>(nullable: true),
                    LOCAL_AREA_REF_ID = table.Column<int>(nullable: true),
                    PROJECT_REF_ID = table.Column<int>(nullable: true),
                    ROTATION_LIST_REF_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_REQUEST", x => x.REQUEST_ID);
                    table.ForeignKey(
                        name: "FK_HET_REQUEST_HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_REF_ID",
                        column: x => x.EQUIPMENT_TYPE_REF_ID,
                        principalTable: "HET_EQUIPMENT_TYPE",
                        principalColumn: "EQUIPMENT_TYPE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_REQUEST_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                        column: x => x.LOCAL_AREA_REF_ID,
                        principalTable: "HET_LOCAL_AREA",
                        principalColumn: "LOCAL_AREA_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_REQUEST_HET_PROJECT_PROJECT_REF_ID",
                        column: x => x.PROJECT_REF_ID,
                        principalTable: "HET_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_REQUEST_HET_ROTATION_LIST_ROTATION_LIST_REF_ID",
                        column: x => x.ROTATION_LIST_REF_ID,
                        principalTable: "HET_ROTATION_LIST",
                        principalColumn: "ROTATION_LIST_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_SENIORITY_AUDIT",
                columns: table => new
                {
                    SENIORITY_AUDIT_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    BLOCK_NUM = table.Column<float>(nullable: true),
                    CYCLE_HRS_WRK = table.Column<float>(nullable: true),
                    EQUIP_CD = table.Column<string>(nullable: true),
                    EQUIPMENT_REF_ID = table.Column<int>(nullable: true),
                    FROZEN_OUT = table.Column<string>(nullable: true),
                    GENERATED_TIME = table.Column<DateTime>(nullable: true),
                    LOCAL_AREA_REF_ID = table.Column<int>(nullable: true),
                    OWNER_NAME = table.Column<string>(nullable: true),
                    OWNER_REF_ID = table.Column<int>(nullable: true),
                    PROJECT_REF_ID = table.Column<int>(nullable: true),
                    SENIORITY = table.Column<float>(nullable: true),
                    WORKING = table.Column<string>(nullable: true),
                    YTD = table.Column<float>(nullable: true),
                    YTD1 = table.Column<float>(nullable: true),
                    YTD2 = table.Column<float>(nullable: true),
                    YTD3 = table.Column<float>(nullable: true),
                    YEAR_END_REG = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_SENIORITY_AUDIT", x => x.SENIORITY_AUDIT_ID);
                    table.ForeignKey(
                        name: "FK_HET_SENIORITY_AUDIT_HET_EQUIPMENT_EQUIPMENT_REF_ID",
                        column: x => x.EQUIPMENT_REF_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_SENIORITY_AUDIT_HET_LOCAL_AREA_LOCAL_AREA_REF_ID",
                        column: x => x.LOCAL_AREA_REF_ID,
                        principalTable: "HET_LOCAL_AREA",
                        principalColumn: "LOCAL_AREA_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_SENIORITY_AUDIT_HET_OWNER_OWNER_REF_ID",
                        column: x => x.OWNER_REF_ID,
                        principalTable: "HET_OWNER",
                        principalColumn: "OWNER_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_SENIORITY_AUDIT_HET_PROJECT_PROJECT_REF_ID",
                        column: x => x.PROJECT_REF_ID,
                        principalTable: "HET_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_TIME_RECORD",
                columns: table => new
                {
                    TIME_RECORD_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ENTERED_DATE = table.Column<DateTime>(nullable: true),
                    HOURS = table.Column<float>(nullable: true),
                    HOURS2 = table.Column<float>(nullable: true),
                    HOURS3 = table.Column<float>(nullable: true),
                    RATE = table.Column<float>(nullable: true),
                    RATE2 = table.Column<float>(nullable: true),
                    RATE3 = table.Column<float>(nullable: true),
                    RENTAL_AGREEMENT_REF_ID = table.Column<int>(nullable: true),
                    WORKED_DATE = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_TIME_RECORD", x => x.TIME_RECORD_ID);
                    table.ForeignKey(
                        name: "FK_HET_TIME_RECORD_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_REF_ID",
                        column: x => x.RENTAL_AGREEMENT_REF_ID,
                        principalTable: "HET_RENTAL_AGREEMENT",
                        principalColumn: "RENTAL_AGREEMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_ATTACHMENT",
                columns: table => new
                {
                    ATTACHMENT_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DESCRIPTION = table.Column<string>(nullable: true),
                    EQUIPMENT_ID = table.Column<int>(nullable: true),
                    EXTERNAL_FILE_NAME = table.Column<string>(nullable: true),
                    INTERNAL_FILE_NAME = table.Column<string>(nullable: true),
                    OWNER_ID = table.Column<int>(nullable: true),
                    PROJECT_ID = table.Column<int>(nullable: true),
                    REQUEST_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_ATTACHMENT", x => x.ATTACHMENT_ID);
                    table.ForeignKey(
                        name: "FK_HET_ATTACHMENT_HET_EQUIPMENT_EQUIPMENT_ID",
                        column: x => x.EQUIPMENT_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_ATTACHMENT_HET_OWNER_OWNER_ID",
                        column: x => x.OWNER_ID,
                        principalTable: "HET_OWNER",
                        principalColumn: "OWNER_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_ATTACHMENT_HET_PROJECT_PROJECT_ID",
                        column: x => x.PROJECT_ID,
                        principalTable: "HET_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_ATTACHMENT_HET_REQUEST_REQUEST_ID",
                        column: x => x.REQUEST_ID,
                        principalTable: "HET_REQUEST",
                        principalColumn: "REQUEST_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_HIRE_OFFER",
                columns: table => new
                {
                    HIRE_OFFER_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ACCEPTED_OFFER = table.Column<bool>(nullable: true),
                    ASKED = table.Column<bool>(nullable: true),
                    ASKED_DATE = table.Column<DateTime>(nullable: true),
                    EQUIPMENT_REF_ID = table.Column<int>(nullable: true),
                    EQUIPMENT_UPDATE_REASON = table.Column<string>(nullable: true),
                    EQUIPMENT_VERIFIED_ACTIVE = table.Column<bool>(nullable: true),
                    FLAG_EQUIPMENT_UPDATE = table.Column<bool>(nullable: true),
                    IS_FORCE_HIRE = table.Column<bool>(nullable: true),
                    NOTE = table.Column<string>(nullable: true),
                    REFUSE_REASON = table.Column<string>(nullable: true),
                    RENTAL_AGREEMENT_REF_ID = table.Column<int>(nullable: true),
                    REQUEST_REF_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_HIRE_OFFER", x => x.HIRE_OFFER_ID);
                    table.ForeignKey(
                        name: "FK_HET_HIRE_OFFER_HET_EQUIPMENT_EQUIPMENT_REF_ID",
                        column: x => x.EQUIPMENT_REF_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_HIRE_OFFER_HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_REF_ID",
                        column: x => x.RENTAL_AGREEMENT_REF_ID,
                        principalTable: "HET_RENTAL_AGREEMENT",
                        principalColumn: "RENTAL_AGREEMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_HIRE_OFFER_HET_REQUEST_REQUEST_REF_ID",
                        column: x => x.REQUEST_REF_ID,
                        principalTable: "HET_REQUEST",
                        principalColumn: "REQUEST_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_HISTORY",
                columns: table => new
                {
                    HISTORY_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CREATED_DATE = table.Column<DateTime>(nullable: true),
                    EQUIPMENT_ID = table.Column<int>(nullable: true),
                    HISTORY_TEXT = table.Column<string>(nullable: true),
                    OWNER_ID = table.Column<int>(nullable: true),
                    PROJECT_ID = table.Column<int>(nullable: true),
                    REQUEST_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_HISTORY", x => x.HISTORY_ID);
                    table.ForeignKey(
                        name: "FK_HET_HISTORY_HET_EQUIPMENT_EQUIPMENT_ID",
                        column: x => x.EQUIPMENT_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_HISTORY_HET_OWNER_OWNER_ID",
                        column: x => x.OWNER_ID,
                        principalTable: "HET_OWNER",
                        principalColumn: "OWNER_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_HISTORY_HET_PROJECT_PROJECT_ID",
                        column: x => x.PROJECT_ID,
                        principalTable: "HET_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_HISTORY_HET_REQUEST_REQUEST_ID",
                        column: x => x.REQUEST_ID,
                        principalTable: "HET_REQUEST",
                        principalColumn: "REQUEST_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_NOTE",
                columns: table => new
                {
                    NOTE_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EQUIPMENT_ID = table.Column<int>(nullable: true),
                    IS_NO_LONGER_RELEVANT = table.Column<bool>(nullable: true),
                    OWNER_ID = table.Column<int>(nullable: true),
                    PROJECT_ID = table.Column<int>(nullable: true),
                    REQUEST_ID = table.Column<int>(nullable: true),
                    _NOTE = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_NOTE", x => x.NOTE_ID);
                    table.ForeignKey(
                        name: "FK_HET_NOTE_HET_EQUIPMENT_EQUIPMENT_ID",
                        column: x => x.EQUIPMENT_ID,
                        principalTable: "HET_EQUIPMENT",
                        principalColumn: "EQUIPMENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_NOTE_HET_OWNER_OWNER_ID",
                        column: x => x.OWNER_ID,
                        principalTable: "HET_OWNER",
                        principalColumn: "OWNER_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_NOTE_HET_PROJECT_PROJECT_ID",
                        column: x => x.PROJECT_ID,
                        principalTable: "HET_PROJECT",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_NOTE_HET_REQUEST_REQUEST_ID",
                        column: x => x.REQUEST_ID,
                        principalTable: "HET_REQUEST",
                        principalColumn: "REQUEST_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HET_ATTACHMENT_EQUIPMENT_ID",
                table: "HET_ATTACHMENT",
                column: "EQUIPMENT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_ATTACHMENT_OWNER_ID",
                table: "HET_ATTACHMENT",
                column: "OWNER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_ATTACHMENT_PROJECT_ID",
                table: "HET_ATTACHMENT",
                column: "PROJECT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_ATTACHMENT_REQUEST_ID",
                table: "HET_ATTACHMENT",
                column: "REQUEST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_CONTACT_OWNER_ID",
                table: "HET_CONTACT",
                column: "OWNER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_CONTACT_PROJECT_ID",
                table: "HET_CONTACT",
                column: "PROJECT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_CONTACT_ADDRESS_CONTACT_ID",
                table: "HET_CONTACT_ADDRESS",
                column: "CONTACT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_CONTACT_PHONE_CONTACT_ID",
                table: "HET_CONTACT_PHONE",
                column: "CONTACT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_DUMP_TRUCK_DETAILS_REF_ID",
                table: "HET_EQUIPMENT",
                column: "DUMP_TRUCK_DETAILS_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_EQUIPMENT_TYPE_REF_ID",
                table: "HET_EQUIPMENT",
                column: "EQUIPMENT_TYPE_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_LOCAL_AREA_REF_ID",
                table: "HET_EQUIPMENT",
                column: "LOCAL_AREA_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_OWNER_REF_ID",
                table: "HET_EQUIPMENT",
                column: "OWNER_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_ATTACHMENT_EQUIPMENT_REF_ID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                column: "EQUIPMENT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_ATTACHMENT_TYPE_REF_ID",
                table: "HET_EQUIPMENT_ATTACHMENT",
                column: "TYPE_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_EQUIPMENT_TYPE_LOCAL_AREA_REF_ID",
                table: "HET_EQUIPMENT_TYPE",
                column: "LOCAL_AREA_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_HIRE_OFFER_EQUIPMENT_REF_ID",
                table: "HET_HIRE_OFFER",
                column: "EQUIPMENT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_HIRE_OFFER_RENTAL_AGREEMENT_REF_ID",
                table: "HET_HIRE_OFFER",
                column: "RENTAL_AGREEMENT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_HIRE_OFFER_REQUEST_REF_ID",
                table: "HET_HIRE_OFFER",
                column: "REQUEST_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_HISTORY_EQUIPMENT_ID",
                table: "HET_HISTORY",
                column: "EQUIPMENT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_HISTORY_OWNER_ID",
                table: "HET_HISTORY",
                column: "OWNER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_HISTORY_PROJECT_ID",
                table: "HET_HISTORY",
                column: "PROJECT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_HISTORY_REQUEST_ID",
                table: "HET_HISTORY",
                column: "REQUEST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_LOCAL_AREA_SERVICE_AREA_REF_ID",
                table: "HET_LOCAL_AREA",
                column: "SERVICE_AREA_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_NOTE_EQUIPMENT_ID",
                table: "HET_NOTE",
                column: "EQUIPMENT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_NOTE_OWNER_ID",
                table: "HET_NOTE",
                column: "OWNER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_NOTE_PROJECT_ID",
                table: "HET_NOTE",
                column: "PROJECT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_NOTE_REQUEST_ID",
                table: "HET_NOTE",
                column: "REQUEST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_OWNER_LOCAL_AREA_REF_ID",
                table: "HET_OWNER",
                column: "LOCAL_AREA_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_OWNER_PRIMARY_CONTACT_REF_ID",
                table: "HET_OWNER",
                column: "PRIMARY_CONTACT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_PROJECT_PRIMARY_CONTACT_REF_ID",
                table: "HET_PROJECT",
                column: "PRIMARY_CONTACT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_PROJECT_SERVICE_AREA_REF_ID",
                table: "HET_PROJECT",
                column: "SERVICE_AREA_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_RENTAL_AGREEMENT_EQUIPMENT_REF_ID",
                table: "HET_RENTAL_AGREEMENT",
                column: "EQUIPMENT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_RENTAL_AGREEMENT_PROJECT_REF_ID",
                table: "HET_RENTAL_AGREEMENT",
                column: "PROJECT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_REQUEST_EQUIPMENT_TYPE_REF_ID",
                table: "HET_REQUEST",
                column: "EQUIPMENT_TYPE_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_REQUEST_LOCAL_AREA_REF_ID",
                table: "HET_REQUEST",
                column: "LOCAL_AREA_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_REQUEST_PROJECT_REF_ID",
                table: "HET_REQUEST",
                column: "PROJECT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_REQUEST_ROTATION_LIST_REF_ID",
                table: "HET_REQUEST",
                column: "ROTATION_LIST_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_ROTATION_LIST_EQUIPMENT_TYPE_REF_ID",
                table: "HET_ROTATION_LIST",
                column: "EQUIPMENT_TYPE_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_ROTATION_LIST_LOCAL_AREA_REF_ID",
                table: "HET_ROTATION_LIST",
                column: "LOCAL_AREA_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_ROTATION_LIST_BLOCK_LAST_HIRED_EQUIPMENT_REF_ID",
                table: "HET_ROTATION_LIST_BLOCK",
                column: "LAST_HIRED_EQUIPMENT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_ROTATION_LIST_BLOCK_ROTATION_LIST_REF_ID",
                table: "HET_ROTATION_LIST_BLOCK",
                column: "ROTATION_LIST_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_ROTATION_LIST_BLOCK_START_CYCLE_EQUIPMENT_REF_ID",
                table: "HET_ROTATION_LIST_BLOCK",
                column: "START_CYCLE_EQUIPMENT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_SENIORITY_AUDIT_EQUIPMENT_REF_ID",
                table: "HET_SENIORITY_AUDIT",
                column: "EQUIPMENT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_SENIORITY_AUDIT_LOCAL_AREA_REF_ID",
                table: "HET_SENIORITY_AUDIT",
                column: "LOCAL_AREA_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_SENIORITY_AUDIT_OWNER_REF_ID",
                table: "HET_SENIORITY_AUDIT",
                column: "OWNER_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_SENIORITY_AUDIT_PROJECT_REF_ID",
                table: "HET_SENIORITY_AUDIT",
                column: "PROJECT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_TIME_RECORD_RENTAL_AGREEMENT_REF_ID",
                table: "HET_TIME_RECORD",
                column: "RENTAL_AGREEMENT_REF_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HET_CONTACT_ADDRESS_HET_CONTACT_CONTACT_ID",
                table: "HET_CONTACT_ADDRESS",
                column: "CONTACT_ID",
                principalTable: "HET_CONTACT",
                principalColumn: "CONTACT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_CONTACT_PHONE_HET_CONTACT_CONTACT_ID",
                table: "HET_CONTACT_PHONE",
                column: "CONTACT_ID",
                principalTable: "HET_CONTACT",
                principalColumn: "CONTACT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_OWNER_HET_CONTACT_PRIMARY_CONTACT_REF_ID",
                table: "HET_OWNER",
                column: "PRIMARY_CONTACT_REF_ID",
                principalTable: "HET_CONTACT",
                principalColumn: "CONTACT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HET_PROJECT_HET_CONTACT_PRIMARY_CONTACT_REF_ID",
                table: "HET_PROJECT",
                column: "PRIMARY_CONTACT_REF_ID",
                principalTable: "HET_CONTACT",
                principalColumn: "CONTACT_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HET_CONTACT_HET_OWNER_OWNER_ID",
                table: "HET_CONTACT");

            migrationBuilder.DropForeignKey(
                name: "FK_HET_CONTACT_HET_PROJECT_PROJECT_ID",
                table: "HET_CONTACT");

            migrationBuilder.DropTable(
                name: "HET_ATTACHMENT");

            migrationBuilder.DropTable(
                name: "HET_CONTACT_ADDRESS");

            migrationBuilder.DropTable(
                name: "HET_CONTACT_PHONE");

            migrationBuilder.DropTable(
                name: "HET_EQUIPMENT_ATTACHMENT");

            migrationBuilder.DropTable(
                name: "HET_HIRE_OFFER");

            migrationBuilder.DropTable(
                name: "HET_HISTORY");

            migrationBuilder.DropTable(
                name: "HET_NOTE");

            migrationBuilder.DropTable(
                name: "HET_ROTATION_LIST_BLOCK");

            migrationBuilder.DropTable(
                name: "HET_SENIORITY_AUDIT");

            migrationBuilder.DropTable(
                name: "HET_TIME_RECORD");

            migrationBuilder.DropTable(
                name: "HET_EQUIPMENT_ATTACHMENT_TYPE");

            migrationBuilder.DropTable(
                name: "HET_REQUEST");

            migrationBuilder.DropTable(
                name: "HET_RENTAL_AGREEMENT");

            migrationBuilder.DropTable(
                name: "HET_ROTATION_LIST");

            migrationBuilder.DropTable(
                name: "HET_EQUIPMENT");

            migrationBuilder.DropTable(
                name: "HET_DUMP_TRUCK");

            migrationBuilder.DropTable(
                name: "HET_EQUIPMENT_TYPE");

            migrationBuilder.DropTable(
                name: "HET_OWNER");

            migrationBuilder.DropTable(
                name: "HET_LOCAL_AREA");

            migrationBuilder.DropTable(
                name: "HET_PROJECT");

            migrationBuilder.DropTable(
                name: "HET_CONTACT");

            migrationBuilder.CreateTable(
                name: "HET_NOTIFICATION_EVENT",
                columns: table => new
                {
                    NOTIFICATION_EVENT_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EVENT_SUB_TYPE_CODE = table.Column<string>(nullable: true),
                    EVENT_TIME = table.Column<string>(nullable: true),
                    EVENT_TYPE_CODE = table.Column<string>(nullable: true),
                    NOTES = table.Column<string>(nullable: true),
                    NOTIFICATION_GENERATED = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_NOTIFICATION_EVENT", x => x.NOTIFICATION_EVENT_ID);
                });

            migrationBuilder.CreateTable(
                name: "HET_NOTIFICATION",
                columns: table => new
                {
                    NOTIFICATION_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EVENT2_REF_ID = table.Column<int>(nullable: true),
                    EVENT_REF_ID = table.Column<int>(nullable: true),
                    HAS_BEEN_VIEWED = table.Column<bool>(nullable: true),
                    IS_ALL_DAY = table.Column<bool>(nullable: true),
                    IS_EXPIRED = table.Column<bool>(nullable: true),
                    IS_WATCH_NOTIFICATION = table.Column<bool>(nullable: true),
                    PRIORITY_CODE = table.Column<string>(nullable: true),
                    USER_REF_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_NOTIFICATION", x => x.NOTIFICATION_ID);
                    table.ForeignKey(
                        name: "FK_HET_NOTIFICATION_HET_NOTIFICATION_EVENT_EVENT2_REF_ID",
                        column: x => x.EVENT2_REF_ID,
                        principalTable: "HET_NOTIFICATION_EVENT",
                        principalColumn: "NOTIFICATION_EVENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_NOTIFICATION_HET_NOTIFICATION_EVENT_EVENT_REF_ID",
                        column: x => x.EVENT_REF_ID,
                        principalTable: "HET_NOTIFICATION_EVENT",
                        principalColumn: "NOTIFICATION_EVENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_NOTIFICATION_HET_USER_USER_REF_ID",
                        column: x => x.USER_REF_ID,
                        principalTable: "HET_USER",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HET_NOTIFICATION_EVENT2_REF_ID",
                table: "HET_NOTIFICATION",
                column: "EVENT2_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_NOTIFICATION_EVENT_REF_ID",
                table: "HET_NOTIFICATION",
                column: "EVENT_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_NOTIFICATION_USER_REF_ID",
                table: "HET_NOTIFICATION",
                column: "USER_REF_ID");
        }
    }
}
