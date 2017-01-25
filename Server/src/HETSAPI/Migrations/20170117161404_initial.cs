using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HETSAPI.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HET_CITY",
                columns: table => new
                {
                    CITY_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NAME = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_CITY", x => x.CITY_ID);
                });

            migrationBuilder.CreateTable(
                name: "HET_FAVOURITE_CONTEXT_TYPE",
                columns: table => new
                {
                    FAVOURITE_CONTEXT_TYPE_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NAME = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_FAVOURITE_CONTEXT_TYPE", x => x.FAVOURITE_CONTEXT_TYPE_ID);
                });

            migrationBuilder.CreateTable(
                name: "HET_GROUP",
                columns: table => new
                {
                    GROUP_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DESCRIPTION = table.Column<string>(nullable: true),
                    NAME = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_GROUP", x => x.GROUP_ID);
                });

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
                name: "HET_PERMISSION",
                columns: table => new
                {
                    PERMISSION_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CODE = table.Column<string>(nullable: true),
                    DESCRIPTION = table.Column<string>(nullable: true),
                    NAME = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_PERMISSION", x => x.PERMISSION_ID);
                });

            migrationBuilder.CreateTable(
                name: "HET_REGION",
                columns: table => new
                {
                    REGION_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    END_DATE = table.Column<DateTime>(nullable: true),
                    MINISTRY_REGION_ID = table.Column<int>(nullable: true),
                    NAME = table.Column<string>(nullable: true),
                    START_DATE = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_REGION", x => x.REGION_ID);
                });

            migrationBuilder.CreateTable(
                name: "HET_ROLE",
                columns: table => new
                {
                    ROLE_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DESCRIPTION = table.Column<string>(nullable: true),
                    NAME = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_ROLE", x => x.ROLE_ID);
                });

            migrationBuilder.CreateTable(
                name: "HET_USER",
                columns: table => new
                {
                    USER_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ACTIVE = table.Column<bool>(nullable: false),
                    EMAIL = table.Column<string>(nullable: true),
                    GIVEN_NAME = table.Column<string>(nullable: true),
                    GUID = table.Column<string>(nullable: true),
                    INITIALS = table.Column<string>(nullable: true),
                    SM_AUTHORIZATION_DIRECTORY = table.Column<string>(nullable: true),
                    SM_USER_ID = table.Column<string>(nullable: true),
                    SURNAME = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_USER", x => x.USER_ID);
                });

            migrationBuilder.CreateTable(
                name: "HET_USER_FAVOURITE",
                columns: table => new
                {
                    USER_FAVOURITE_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FAVOURITE_CONTEXT_TYPE_ID = table.Column<int>(nullable: true),
                    IS_DEFAULT = table.Column<bool>(nullable: true),
                    NAME = table.Column<string>(nullable: true),
                    VALUE = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_USER_FAVOURITE", x => x.USER_FAVOURITE_ID);
                    table.ForeignKey(
                        name: "FK_HET_USER_FAVOURITE_HET_FAVOURITE_CONTEXT_TYPE_FAVOURITE_CONTEXT_TYPE_ID",
                        column: x => x.FAVOURITE_CONTEXT_TYPE_ID,
                        principalTable: "HET_FAVOURITE_CONTEXT_TYPE",
                        principalColumn: "FAVOURITE_CONTEXT_TYPE_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_DISTRICT",
                columns: table => new
                {
                    DISTRICT_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    END_DATE = table.Column<DateTime>(nullable: true),
                    MINISTRY_DISTRICT_ID = table.Column<int>(nullable: true),
                    NAME = table.Column<string>(nullable: true),
                    REGION_REF_ID = table.Column<int>(nullable: false),
                    START_DATE = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_DISTRICT", x => x.DISTRICT_ID);
                    table.ForeignKey(
                        name: "FK_HET_DISTRICT_HET_REGION_REGION_REF_ID",
                        column: x => x.REGION_REF_ID,
                        principalTable: "HET_REGION",
                        principalColumn: "REGION_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HET_ROLE_PERMISSION",
                columns: table => new
                {
                    ROLE_PERMISSION_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    PERMISSION_ID = table.Column<int>(nullable: true),
                    ROLE_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_ROLE_PERMISSION", x => x.ROLE_PERMISSION_ID);
                    table.ForeignKey(
                        name: "FK_HET_ROLE_PERMISSION_HET_PERMISSION_PERMISSION_ID",
                        column: x => x.PERMISSION_ID,
                        principalTable: "HET_PERMISSION",
                        principalColumn: "PERMISSION_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_ROLE_PERMISSION_HET_ROLE_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalTable: "HET_ROLE",
                        principalColumn: "ROLE_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_GROUP_MEMBERSHIP",
                columns: table => new
                {
                    GROUP_MEMBERSHIP_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ACTIVE = table.Column<bool>(nullable: false),
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

            migrationBuilder.CreateTable(
                name: "HET_USER_ROLE",
                columns: table => new
                {
                    USER_ROLE_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EFFECTIVE_DATE = table.Column<DateTime>(nullable: false),
                    EXPIRY_DATE = table.Column<DateTime>(nullable: true),
                    ROLE_ID = table.Column<int>(nullable: true),
                    USER_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_USER_ROLE", x => x.USER_ROLE_ID);
                    table.ForeignKey(
                        name: "FK_HET_USER_ROLE_HET_ROLE_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalTable: "HET_ROLE",
                        principalColumn: "ROLE_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HET_USER_ROLE_HET_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "HET_USER",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HET_SERVICE_AREA",
                columns: table => new
                {
                    SERVICE_AREA_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DISTRICT_ID = table.Column<int>(nullable: true),
                    END_DATE = table.Column<DateTime>(nullable: true),
                    MINISTRY_SERVICE_AREA_ID = table.Column<int>(nullable: true),
                    NAME = table.Column<string>(nullable: true),
                    START_DATE = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HET_SERVICE_AREA", x => x.SERVICE_AREA_ID);
                    table.ForeignKey(
                        name: "FK_HET_SERVICE_AREA_HET_DISTRICT_DISTRICT_ID",
                        column: x => x.DISTRICT_ID,
                        principalTable: "HET_DISTRICT",
                        principalColumn: "DISTRICT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HET_DISTRICT_REGION_REF_ID",
                table: "HET_DISTRICT",
                column: "REGION_REF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_GROUP_MEMBERSHIP_GROUP_ID",
                table: "HET_GROUP_MEMBERSHIP",
                column: "GROUP_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_GROUP_MEMBERSHIP_USER_ID",
                table: "HET_GROUP_MEMBERSHIP",
                column: "USER_ID");

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

            migrationBuilder.CreateIndex(
                name: "IX_HET_ROLE_PERMISSION_PERMISSION_ID",
                table: "HET_ROLE_PERMISSION",
                column: "PERMISSION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_ROLE_PERMISSION_ROLE_ID",
                table: "HET_ROLE_PERMISSION",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_SERVICE_AREA_DISTRICT_ID",
                table: "HET_SERVICE_AREA",
                column: "DISTRICT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_USER_FAVOURITE_FAVOURITE_CONTEXT_TYPE_ID",
                table: "HET_USER_FAVOURITE",
                column: "FAVOURITE_CONTEXT_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_USER_ROLE_ROLE_ID",
                table: "HET_USER_ROLE",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HET_USER_ROLE_USER_ID",
                table: "HET_USER_ROLE",
                column: "USER_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HET_CITY");

            migrationBuilder.DropTable(
                name: "HET_GROUP_MEMBERSHIP");

            migrationBuilder.DropTable(
                name: "HET_NOTIFICATION");

            migrationBuilder.DropTable(
                name: "HET_ROLE_PERMISSION");

            migrationBuilder.DropTable(
                name: "HET_SERVICE_AREA");

            migrationBuilder.DropTable(
                name: "HET_USER_FAVOURITE");

            migrationBuilder.DropTable(
                name: "HET_USER_ROLE");

            migrationBuilder.DropTable(
                name: "HET_GROUP");

            migrationBuilder.DropTable(
                name: "HET_NOTIFICATION_EVENT");

            migrationBuilder.DropTable(
                name: "HET_PERMISSION");

            migrationBuilder.DropTable(
                name: "HET_DISTRICT");

            migrationBuilder.DropTable(
                name: "HET_FAVOURITE_CONTEXT_TYPE");

            migrationBuilder.DropTable(
                name: "HET_ROLE");

            migrationBuilder.DropTable(
                name: "HET_USER");

            migrationBuilder.DropTable(
                name: "HET_REGION");
        }
    }
}
