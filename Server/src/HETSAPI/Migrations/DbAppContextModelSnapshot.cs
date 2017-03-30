﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HETSAPI.Models;

namespace HETSAPI.Migrations
{
    [DbContext(typeof(DbAppContext))]
    partial class DbAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("HETSAPI.Models.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ATTACHMENT_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION")
                        .HasMaxLength(2048);

                    b.Property<int?>("EquipmentId")
                        .HasColumnName("EQUIPMENT_ID");

                    b.Property<byte[]>("FileContents")
                        .HasColumnName("FILE_CONTENTS");

                    b.Property<string>("FileName")
                        .HasColumnName("FILE_NAME")
                        .HasMaxLength(2048);

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("OwnerId")
                        .HasColumnName("OWNER_ID");

                    b.Property<int?>("ProjectId")
                        .HasColumnName("PROJECT_ID");

                    b.Property<int?>("RentalRequestId")
                        .HasColumnName("RENTAL_REQUEST_ID");

                    b.Property<string>("Type")
                        .HasColumnName("TYPE")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("RentalRequestId");

                    b.ToTable("HET_ATTACHMENT");
                });

            modelBuilder.Entity("HETSAPI.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CITY_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("HET_CITY");
                });

            modelBuilder.Entity("HETSAPI.Models.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CONTACT_ID");

                    b.Property<string>("Address1")
                        .HasColumnName("ADDRESS1")
                        .HasMaxLength(80);

                    b.Property<string>("Address2")
                        .HasColumnName("ADDRESS2")
                        .HasMaxLength(80);

                    b.Property<string>("City")
                        .HasColumnName("CITY")
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("EmailAddress")
                        .HasColumnName("EMAIL_ADDRESS")
                        .HasMaxLength(255);

                    b.Property<string>("FaxPhoneNumber")
                        .HasColumnName("FAX_PHONE_NUMBER")
                        .HasMaxLength(20);

                    b.Property<string>("GivenName")
                        .HasColumnName("GIVEN_NAME")
                        .HasMaxLength(50);

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("MobilePhoneNumber")
                        .HasColumnName("MOBILE_PHONE_NUMBER")
                        .HasMaxLength(20);

                    b.Property<string>("Notes")
                        .HasColumnName("NOTES")
                        .HasMaxLength(150);

                    b.Property<string>("OrganizationName")
                        .HasColumnName("ORGANIZATION_NAME")
                        .HasMaxLength(150);

                    b.Property<int?>("OwnerId")
                        .HasColumnName("OWNER_ID");

                    b.Property<string>("PostalCode")
                        .HasColumnName("POSTAL_CODE")
                        .HasMaxLength(15);

                    b.Property<int?>("ProjectId")
                        .HasColumnName("PROJECT_ID");

                    b.Property<string>("Province")
                        .HasColumnName("PROVINCE")
                        .HasMaxLength(50);

                    b.Property<string>("Role")
                        .HasColumnName("ROLE")
                        .HasMaxLength(100);

                    b.Property<string>("Surname")
                        .HasColumnName("SURNAME")
                        .HasMaxLength(50);

                    b.Property<string>("WorkPhoneNumber")
                        .HasColumnName("WORK_PHONE_NUMBER")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ProjectId");

                    b.ToTable("HET_CONTACT");
                });

            modelBuilder.Entity("HETSAPI.Models.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("DISTRICT_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("DistrictNumber")
                        .HasColumnName("DISTRICT_NUMBER");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnName("END_DATE");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int>("MinistryDistrictID")
                        .HasColumnName("MINISTRY_DISTRICT_ID");

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(150);

                    b.Property<int?>("RegionId")
                        .HasColumnName("REGION_ID");

                    b.Property<DateTime>("StartDate")
                        .HasColumnName("START_DATE");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("HET_DISTRICT");
                });

            modelBuilder.Entity("HETSAPI.Models.DistrictEquipmentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("DistrictEquipmentName")
                        .HasColumnName("DISTRICT_EQUIPMENT_NAME")
                        .HasMaxLength(50);

                    b.Property<int?>("DistrictId")
                        .HasColumnName("DISTRICT_ID");

                    b.Property<int?>("EquipmentTypeId")
                        .HasColumnName("EQUIPMENT_TYPE_ID");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.HasIndex("EquipmentTypeId");

                    b.ToTable("HET_DISTRICT_EQUIPMENT_TYPE");
                });

            modelBuilder.Entity("HETSAPI.Models.DumpTruck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("DUMP_TRUCK_ID");

                    b.Property<string>("BoxCapacity")
                        .HasColumnName("BOX_CAPACITY")
                        .HasMaxLength(150);

                    b.Property<string>("BoxHeight")
                        .HasColumnName("BOX_HEIGHT")
                        .HasMaxLength(150);

                    b.Property<string>("BoxLength")
                        .HasColumnName("BOX_LENGTH")
                        .HasMaxLength(150);

                    b.Property<string>("BoxWidth")
                        .HasColumnName("BOX_WIDTH")
                        .HasMaxLength(150);

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("FrontAxleCapacity")
                        .HasColumnName("FRONT_AXLE_CAPACITY")
                        .HasMaxLength(150);

                    b.Property<string>("FrontTireSize")
                        .HasColumnName("FRONT_TIRE_SIZE")
                        .HasMaxLength(150);

                    b.Property<string>("FrontTireUOM")
                        .HasColumnName("FRONT_TIRE_UOM")
                        .HasMaxLength(150);

                    b.Property<bool?>("HasBellyDump")
                        .HasColumnName("HAS_BELLY_DUMP");

                    b.Property<bool?>("HasHiliftGate")
                        .HasColumnName("HAS_HILIFT_GATE");

                    b.Property<bool?>("HasPUP")
                        .HasColumnName("HAS_PUP");

                    b.Property<bool?>("HasRockBox")
                        .HasColumnName("HAS_ROCK_BOX");

                    b.Property<bool?>("HasSealcoatHitch")
                        .HasColumnName("HAS_SEALCOAT_HITCH");

                    b.Property<bool?>("IsSingleAxle")
                        .HasColumnName("IS_SINGLE_AXLE");

                    b.Property<bool?>("IsTandemAxle")
                        .HasColumnName("IS_TANDEM_AXLE");

                    b.Property<bool?>("IsTridem")
                        .HasColumnName("IS_TRIDEM");

                    b.Property<bool?>("IsWaterTruck")
                        .HasColumnName("IS_WATER_TRUCK");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("LegalCapacity")
                        .HasColumnName("LEGAL_CAPACITY")
                        .HasMaxLength(150);

                    b.Property<string>("LegalLoad")
                        .HasColumnName("LEGAL_LOAD")
                        .HasMaxLength(150);

                    b.Property<string>("LegalPUPTareWeight")
                        .HasColumnName("LEGAL_PUPTARE_WEIGHT")
                        .HasMaxLength(150);

                    b.Property<string>("LicencedCapacity")
                        .HasColumnName("LICENCED_CAPACITY")
                        .HasMaxLength(150);

                    b.Property<string>("LicencedGVW")
                        .HasColumnName("LICENCED_GVW")
                        .HasMaxLength(150);

                    b.Property<string>("LicencedGVWUOM")
                        .HasColumnName("LICENCED_GVWUOM")
                        .HasMaxLength(150);

                    b.Property<string>("LicencedLoad")
                        .HasColumnName("LICENCED_LOAD")
                        .HasMaxLength(150);

                    b.Property<string>("LicencedPUPTareWeight")
                        .HasColumnName("LICENCED_PUPTARE_WEIGHT")
                        .HasMaxLength(150);

                    b.Property<string>("LicencedTareWeight")
                        .HasColumnName("LICENCED_TARE_WEIGHT")
                        .HasMaxLength(150);

                    b.Property<string>("RearAxleCapacity")
                        .HasColumnName("REAR_AXLE_CAPACITY")
                        .HasMaxLength(150);

                    b.Property<string>("RearAxleSpacing")
                        .HasColumnName("REAR_AXLE_SPACING")
                        .HasMaxLength(150);

                    b.Property<string>("TrailerBoxCapacity")
                        .HasColumnName("TRAILER_BOX_CAPACITY")
                        .HasMaxLength(150);

                    b.Property<string>("TrailerBoxHeight")
                        .HasColumnName("TRAILER_BOX_HEIGHT")
                        .HasMaxLength(150);

                    b.Property<string>("TrailerBoxLength")
                        .HasColumnName("TRAILER_BOX_LENGTH")
                        .HasMaxLength(150);

                    b.Property<string>("TrailerBoxWidth")
                        .HasColumnName("TRAILER_BOX_WIDTH")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("HET_DUMP_TRUCK");
                });

            modelBuilder.Entity("HETSAPI.Models.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EQUIPMENT_ID");

                    b.Property<DateTime?>("ApprovedDate")
                        .HasColumnName("APPROVED_DATE");

                    b.Property<string>("ArchiveCode")
                        .HasColumnName("ARCHIVE_CODE")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ArchiveDate")
                        .HasColumnName("ARCHIVE_DATE");

                    b.Property<string>("ArchiveReason")
                        .HasColumnName("ARCHIVE_REASON")
                        .HasMaxLength(2048);

                    b.Property<int?>("BlockNumber")
                        .HasColumnName("BLOCK_NUMBER");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("DistrictEquipmentTypeId")
                        .HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID");

                    b.Property<int?>("DumpTruckId")
                        .HasColumnName("DUMP_TRUCK_ID");

                    b.Property<string>("EquipmentCode")
                        .HasColumnName("EQUIPMENT_CODE")
                        .HasMaxLength(25);

                    b.Property<string>("InformationUpdateNeededReason")
                        .HasColumnName("INFORMATION_UPDATE_NEEDED_REASON")
                        .HasMaxLength(2048);

                    b.Property<bool?>("IsInformationUpdateNeeded")
                        .HasColumnName("IS_INFORMATION_UPDATE_NEEDED");

                    b.Property<bool?>("IsSeniorityOverridden")
                        .HasColumnName("IS_SENIORITY_OVERRIDDEN");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<DateTime>("LastVerifiedDate")
                        .HasColumnName("LAST_VERIFIED_DATE");

                    b.Property<string>("LicencePlate")
                        .HasColumnName("LICENCE_PLATE")
                        .HasMaxLength(20);

                    b.Property<int?>("LocalAreaId")
                        .HasColumnName("LOCAL_AREA_ID");

                    b.Property<string>("Make")
                        .HasColumnName("MAKE")
                        .HasMaxLength(50);

                    b.Property<string>("Model")
                        .HasColumnName("MODEL")
                        .HasMaxLength(50);

                    b.Property<int?>("NumberInBlock")
                        .HasColumnName("NUMBER_IN_BLOCK");

                    b.Property<string>("Operator")
                        .HasColumnName("OPERATOR")
                        .HasMaxLength(255);

                    b.Property<int?>("OwnerId")
                        .HasColumnName("OWNER_ID");

                    b.Property<float?>("PayRate")
                        .HasColumnName("PAY_RATE");

                    b.Property<DateTime>("ReceivedDate")
                        .HasColumnName("RECEIVED_DATE");

                    b.Property<string>("RefuseRate")
                        .HasColumnName("REFUSE_RATE")
                        .HasMaxLength(255);

                    b.Property<float?>("Seniority")
                        .HasColumnName("SENIORITY");

                    b.Property<DateTime?>("SeniorityEffectiveDate")
                        .HasColumnName("SENIORITY_EFFECTIVE_DATE");

                    b.Property<string>("SeniorityOverrideReason")
                        .HasColumnName("SENIORITY_OVERRIDE_REASON")
                        .HasMaxLength(2048);

                    b.Property<string>("SerialNumber")
                        .HasColumnName("SERIAL_NUMBER")
                        .HasMaxLength(100);

                    b.Property<float?>("ServiceHoursLastYear")
                        .HasColumnName("SERVICE_HOURS_LAST_YEAR");

                    b.Property<float?>("ServiceHoursThreeYearsAgo")
                        .HasColumnName("SERVICE_HOURS_THREE_YEARS_AGO");

                    b.Property<float?>("ServiceHoursTwoYearsAgo")
                        .HasColumnName("SERVICE_HOURS_TWO_YEARS_AGO");

                    b.Property<string>("Size")
                        .HasColumnName("SIZE")
                        .HasMaxLength(128);

                    b.Property<string>("Status")
                        .HasColumnName("STATUS")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ToDate")
                        .HasColumnName("TO_DATE");

                    b.Property<string>("Year")
                        .HasColumnName("YEAR")
                        .HasMaxLength(15);

                    b.Property<float?>("YearsOfService")
                        .HasColumnName("YEARS_OF_SERVICE");

                    b.HasKey("Id");

                    b.HasIndex("DistrictEquipmentTypeId");

                    b.HasIndex("DumpTruckId");

                    b.HasIndex("LocalAreaId");

                    b.HasIndex("OwnerId");

                    b.ToTable("HET_EQUIPMENT");
                });

            modelBuilder.Entity("HETSAPI.Models.EquipmentAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EQUIPMENT_ATTACHMENT_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION")
                        .HasMaxLength(2048);

                    b.Property<int?>("EquipmentId")
                        .HasColumnName("EQUIPMENT_ID");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("TypeName")
                        .HasColumnName("TYPE_NAME")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.ToTable("HET_EQUIPMENT_ATTACHMENT");
                });

            modelBuilder.Entity("HETSAPI.Models.EquipmentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EQUIPMENT_TYPE_ID");

                    b.Property<float?>("BlueBookRateNumber")
                        .HasColumnName("BLUE_BOOK_RATE_NUMBER");

                    b.Property<float?>("BlueBookSection")
                        .HasColumnName("BLUE_BOOK_SECTION");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<float?>("ExtendHours")
                        .HasColumnName("EXTEND_HOURS");

                    b.Property<bool>("IsDumpTruck")
                        .HasColumnName("IS_DUMP_TRUCK");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<float?>("MaxHoursSub")
                        .HasColumnName("MAX_HOURS_SUB");

                    b.Property<float?>("MaximumHours")
                        .HasColumnName("MAXIMUM_HOURS");

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(50);

                    b.Property<int>("NumberOfBlocks")
                        .HasColumnName("NUMBER_OF_BLOCKS");

                    b.HasKey("Id");

                    b.ToTable("HET_EQUIPMENT_TYPE");
                });

            modelBuilder.Entity("HETSAPI.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("GROUP_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION")
                        .HasMaxLength(2048);

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("HET_GROUP");
                });

            modelBuilder.Entity("HETSAPI.Models.GroupMembership", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("GROUP_MEMBERSHIP_ID");

                    b.Property<bool>("Active")
                        .HasColumnName("ACTIVE");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("GroupId")
                        .HasColumnName("GROUP_ID");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("UserId")
                        .HasColumnName("USER_ID");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("HET_GROUP_MEMBERSHIP");
                });

            modelBuilder.Entity("HETSAPI.Models.History", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("HISTORY_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnName("CREATED_DATE");

                    b.Property<int?>("EquipmentId")
                        .HasColumnName("EQUIPMENT_ID");

                    b.Property<string>("HistoryText")
                        .HasColumnName("HISTORY_TEXT")
                        .HasMaxLength(2048);

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("OwnerId")
                        .HasColumnName("OWNER_ID");

                    b.Property<int?>("ProjectId")
                        .HasColumnName("PROJECT_ID");

                    b.Property<int?>("RentalRequestId")
                        .HasColumnName("RENTAL_REQUEST_ID");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("RentalRequestId");

                    b.ToTable("HET_HISTORY");
                });

            modelBuilder.Entity("HETSAPI.Models.ImportMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("IMPORT_MAP_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int>("NewKey")
                        .HasColumnName("NEW_KEY");

                    b.Property<string>("NewTable")
                        .HasColumnName("NEW_TABLE");

                    b.Property<int>("OldKey")
                        .HasColumnName("OLD_KEY");

                    b.Property<string>("OldTable")
                        .HasColumnName("OLD_TABLE");

                    b.HasKey("Id");

                    b.ToTable("HET_IMPORT_MAP");
                });

            modelBuilder.Entity("HETSAPI.Models.LocalArea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("LOCAL_AREA_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("EndDate")
                        .HasColumnName("END_DATE");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int>("LocalAreaNumber")
                        .HasColumnName("LOCAL_AREA_NUMBER");

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(150);

                    b.Property<int?>("ServiceAreaId")
                        .HasColumnName("SERVICE_AREA_ID");

                    b.Property<DateTime>("StartDate")
                        .HasColumnName("START_DATE");

                    b.HasKey("Id");

                    b.HasIndex("ServiceAreaId");

                    b.ToTable("HET_LOCAL_AREA");
                });

            modelBuilder.Entity("HETSAPI.Models.LocalAreaRotationList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("LOCAL_AREA_ROTATION_LIST_ID");

                    b.Property<int?>("AskNextBlock1Id")
                        .HasColumnName("ASK_NEXT_BLOCK1_ID");

                    b.Property<float?>("AskNextBlock1Seniority")
                        .HasColumnName("ASK_NEXT_BLOCK1_SENIORITY");

                    b.Property<int?>("AskNextBlock2Id")
                        .HasColumnName("ASK_NEXT_BLOCK2_ID");

                    b.Property<float?>("AskNextBlock2Seniority")
                        .HasColumnName("ASK_NEXT_BLOCK2_SENIORITY");

                    b.Property<int?>("AskNextBlockOpenId")
                        .HasColumnName("ASK_NEXT_BLOCK_OPEN_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("DistrictEquipmentTypeId")
                        .HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("LocalAreaId")
                        .HasColumnName("LOCAL_AREA_ID");

                    b.HasKey("Id");

                    b.HasIndex("AskNextBlock1Id");

                    b.HasIndex("AskNextBlock2Id");

                    b.HasIndex("AskNextBlockOpenId");

                    b.HasIndex("DistrictEquipmentTypeId");

                    b.HasIndex("LocalAreaId");

                    b.ToTable("HET_LOCAL_AREA_ROTATION_LIST");
                });

            modelBuilder.Entity("HETSAPI.Models.LookupList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("LOOKUP_LIST_ID");

                    b.Property<string>("CodeName")
                        .HasColumnName("CODE_NAME")
                        .HasMaxLength(30);

                    b.Property<string>("ContextName")
                        .HasColumnName("CONTEXT_NAME")
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("DisplaySortOrder")
                        .HasColumnName("DISPLAY_SORT_ORDER");

                    b.Property<bool>("IsDefault")
                        .HasColumnName("IS_DEFAULT");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("Value")
                        .HasColumnName("VALUE")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("HET_LOOKUP_LIST");
                });

            modelBuilder.Entity("HETSAPI.Models.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("NOTE_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("EquipmentId")
                        .HasColumnName("EQUIPMENT_ID");

                    b.Property<bool?>("IsNoLongerRelevant")
                        .HasColumnName("IS_NO_LONGER_RELEVANT");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("OwnerId")
                        .HasColumnName("OWNER_ID");

                    b.Property<int?>("ProjectId")
                        .HasColumnName("PROJECT_ID");

                    b.Property<int?>("RentalRequestId")
                        .HasColumnName("RENTAL_REQUEST_ID");

                    b.Property<string>("Text")
                        .HasColumnName("TEXT")
                        .HasMaxLength(2048);

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("RentalRequestId");

                    b.ToTable("HET_NOTE");
                });

            modelBuilder.Entity("HETSAPI.Models.Owner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("OWNER_ID");

                    b.Property<string>("ArchiveCode")
                        .HasColumnName("ARCHIVE_CODE")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ArchiveDate")
                        .HasColumnName("ARCHIVE_DATE");

                    b.Property<string>("ArchiveReason")
                        .HasColumnName("ARCHIVE_REASON")
                        .HasMaxLength(2048);

                    b.Property<DateTime?>("CGLEndDate")
                        .HasColumnName("CGLEND_DATE");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("DoingBusinessAs")
                        .HasColumnName("DOING_BUSINESS_AS")
                        .HasMaxLength(150);

                    b.Property<bool?>("IsMaintenanceContractor")
                        .HasColumnName("IS_MAINTENANCE_CONTRACTOR");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("LocalAreaId")
                        .HasColumnName("LOCAL_AREA_ID");

                    b.Property<bool>("MeetsResidency")
                        .HasColumnName("MEETS_RESIDENCY");

                    b.Property<string>("OrganizationName")
                        .HasColumnName("ORGANIZATION_NAME")
                        .HasMaxLength(150);

                    b.Property<string>("OwnerEquipmentCodePrefix")
                        .HasColumnName("OWNER_EQUIPMENT_CODE_PREFIX")
                        .HasMaxLength(20);

                    b.Property<int?>("PrimaryContactId")
                        .HasColumnName("PRIMARY_CONTACT_ID");

                    b.Property<string>("RegisteredCompanyNumber")
                        .HasColumnName("REGISTERED_COMPANY_NUMBER")
                        .HasMaxLength(150);

                    b.Property<string>("Status")
                        .HasColumnName("STATUS")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("WorkSafeBCExpiryDate")
                        .HasColumnName("WORK_SAFE_BCEXPIRY_DATE");

                    b.Property<string>("WorkSafeBCPolicyNumber")
                        .HasColumnName("WORK_SAFE_BCPOLICY_NUMBER")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("LocalAreaId");

                    b.HasIndex("PrimaryContactId");

                    b.ToTable("HET_OWNER");
                });

            modelBuilder.Entity("HETSAPI.Models.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PERMISSION_ID");

                    b.Property<string>("Code")
                        .HasColumnName("CODE")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION")
                        .HasMaxLength(2048);

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("HET_PERMISSION");
                });

            modelBuilder.Entity("HETSAPI.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PROJECT_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("DistrictId")
                        .HasColumnName("DISTRICT_ID");

                    b.Property<string>("Information")
                        .HasColumnName("INFORMATION")
                        .HasMaxLength(2048);

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(100);

                    b.Property<int?>("PrimaryContactId")
                        .HasColumnName("PRIMARY_CONTACT_ID");

                    b.Property<string>("ProvincialProjectNumber")
                        .HasColumnName("PROVINCIAL_PROJECT_NUMBER")
                        .HasMaxLength(150);

                    b.Property<string>("Status")
                        .HasColumnName("STATUS")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.HasIndex("PrimaryContactId");

                    b.ToTable("HET_PROJECT");
                });

            modelBuilder.Entity("HETSAPI.Models.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("REGION_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("EndDate")
                        .HasColumnName("END_DATE");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int>("MinistryRegionID")
                        .HasColumnName("MINISTRY_REGION_ID");

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(150);

                    b.Property<int?>("RegionNumber")
                        .HasColumnName("REGION_NUMBER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnName("START_DATE");

                    b.HasKey("Id");

                    b.ToTable("HET_REGION");
                });

            modelBuilder.Entity("HETSAPI.Models.RentalAgreement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("RENTAL_AGREEMENT_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("DatedOn")
                        .HasColumnName("DATED_ON");

                    b.Property<int?>("EquipmentId")
                        .HasColumnName("EQUIPMENT_ID");

                    b.Property<float?>("EquipmentRate")
                        .HasColumnName("EQUIPMENT_RATE");

                    b.Property<int?>("EstimateHours")
                        .HasColumnName("ESTIMATE_HOURS");

                    b.Property<DateTime?>("EstimateStartWork")
                        .HasColumnName("ESTIMATE_START_WORK");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("Note")
                        .HasColumnName("NOTE")
                        .HasMaxLength(2048);

                    b.Property<string>("Number")
                        .HasColumnName("NUMBER")
                        .HasMaxLength(30);

                    b.Property<int?>("ProjectId")
                        .HasColumnName("PROJECT_ID");

                    b.Property<string>("RateComment")
                        .HasColumnName("RATE_COMMENT")
                        .HasMaxLength(2048);

                    b.Property<string>("RatePeriod")
                        .HasColumnName("RATE_PERIOD")
                        .HasMaxLength(50);

                    b.Property<string>("Status")
                        .HasColumnName("STATUS")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("ProjectId");

                    b.ToTable("HET_RENTAL_AGREEMENT");
                });

            modelBuilder.Entity("HETSAPI.Models.RentalAgreementCondition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("RENTAL_AGREEMENT_CONDITION_ID");

                    b.Property<string>("Comment")
                        .HasColumnName("COMMENT")
                        .HasMaxLength(2048);

                    b.Property<string>("ConditionName")
                        .HasColumnName("CONDITION_NAME")
                        .HasMaxLength(150);

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("RentalAgreementId")
                        .HasColumnName("RENTAL_AGREEMENT_ID");

                    b.HasKey("Id");

                    b.HasIndex("RentalAgreementId");

                    b.ToTable("HET_RENTAL_AGREEMENT_CONDITION");
                });

            modelBuilder.Entity("HETSAPI.Models.RentalAgreementRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("RENTAL_AGREEMENT_RATE_ID");

                    b.Property<string>("Comment")
                        .HasColumnName("COMMENT")
                        .HasMaxLength(2048);

                    b.Property<string>("ComponentName")
                        .HasColumnName("COMPONENT_NAME")
                        .HasMaxLength(150);

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<bool?>("IsAttachment")
                        .HasColumnName("IS_ATTACHMENT");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("PercentOfEquipmentRate")
                        .HasColumnName("PERCENT_OF_EQUIPMENT_RATE");

                    b.Property<float?>("Rate")
                        .HasColumnName("RATE");

                    b.Property<string>("RatePeriod")
                        .HasColumnName("RATE_PERIOD")
                        .HasMaxLength(50);

                    b.Property<int?>("RentalAgreementId")
                        .HasColumnName("RENTAL_AGREEMENT_ID");

                    b.HasKey("Id");

                    b.HasIndex("RentalAgreementId");

                    b.ToTable("HET_RENTAL_AGREEMENT_RATE");
                });

            modelBuilder.Entity("HETSAPI.Models.RentalRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("RENTAL_REQUEST_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("DistrictEquipmentTypeId")
                        .HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID");

                    b.Property<int>("EquipmentCount")
                        .HasColumnName("EQUIPMENT_COUNT");

                    b.Property<DateTime?>("ExpectedEndDate")
                        .HasColumnName("EXPECTED_END_DATE");

                    b.Property<int?>("ExpectedHours")
                        .HasColumnName("EXPECTED_HOURS");

                    b.Property<DateTime?>("ExpectedStartDate")
                        .HasColumnName("EXPECTED_START_DATE");

                    b.Property<int?>("FirstOnRotationListId")
                        .HasColumnName("FIRST_ON_ROTATION_LIST_ID");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("LocalAreaId")
                        .HasColumnName("LOCAL_AREA_ID");

                    b.Property<int?>("ProjectId")
                        .HasColumnName("PROJECT_ID");

                    b.Property<string>("Status")
                        .HasColumnName("STATUS")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("DistrictEquipmentTypeId");

                    b.HasIndex("FirstOnRotationListId");

                    b.HasIndex("LocalAreaId");

                    b.HasIndex("ProjectId");

                    b.ToTable("HET_RENTAL_REQUEST");
                });

            modelBuilder.Entity("HETSAPI.Models.RentalRequestAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("RENTAL_REQUEST_ATTACHMENT_ID");

                    b.Property<string>("Attachment")
                        .HasColumnName("ATTACHMENT")
                        .HasMaxLength(150);

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("RentalRequestId")
                        .HasColumnName("RENTAL_REQUEST_ID");

                    b.HasKey("Id");

                    b.HasIndex("RentalRequestId");

                    b.ToTable("HET_RENTAL_REQUEST_ATTACHMENT");
                });

            modelBuilder.Entity("HETSAPI.Models.RentalRequestRotationList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("RENTAL_REQUEST_ROTATION_LIST_ID");

                    b.Property<DateTime?>("AskedDateTime")
                        .HasColumnName("ASKED_DATE_TIME");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("EquipmentId")
                        .HasColumnName("EQUIPMENT_ID");

                    b.Property<bool?>("IsForceHire")
                        .HasColumnName("IS_FORCE_HIRE");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("Note")
                        .HasColumnName("NOTE")
                        .HasMaxLength(2048);

                    b.Property<string>("OfferRefusalReason")
                        .HasColumnName("OFFER_REFUSAL_REASON")
                        .HasMaxLength(50);

                    b.Property<string>("OfferResponse")
                        .HasColumnName("OFFER_RESPONSE");

                    b.Property<DateTime?>("OfferResponseDatetime")
                        .HasColumnName("OFFER_RESPONSE_DATETIME");

                    b.Property<string>("OfferResponseNote")
                        .HasColumnName("OFFER_RESPONSE_NOTE")
                        .HasMaxLength(2048);

                    b.Property<int?>("RentalAgreementId")
                        .HasColumnName("RENTAL_AGREEMENT_ID");

                    b.Property<int?>("RentalRequestId")
                        .HasColumnName("RENTAL_REQUEST_ID");

                    b.Property<int>("RotationListSortOrder")
                        .HasColumnName("ROTATION_LIST_SORT_ORDER");

                    b.Property<bool?>("WasAsked")
                        .HasColumnName("WAS_ASKED");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("RentalAgreementId");

                    b.HasIndex("RentalRequestId");

                    b.ToTable("HET_RENTAL_REQUEST_ROTATION_LIST");
                });

            modelBuilder.Entity("HETSAPI.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ROLE_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION")
                        .HasMaxLength(2048);

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("HET_ROLE");
                });

            modelBuilder.Entity("HETSAPI.Models.RolePermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ROLE_PERMISSION_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("PermissionId")
                        .HasColumnName("PERMISSION_ID");

                    b.Property<int?>("RoleId")
                        .HasColumnName("ROLE_ID");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("HET_ROLE_PERMISSION");
                });

            modelBuilder.Entity("HETSAPI.Models.SeniorityAudit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("SENIORITY_AUDIT_ID");

                    b.Property<int?>("BlockNumber")
                        .HasColumnName("BLOCK_NUMBER");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EndDate")
                        .HasColumnName("END_DATE");

                    b.Property<int?>("EquipmentId")
                        .HasColumnName("EQUIPMENT_ID");

                    b.Property<bool?>("IsSeniorityOverridden")
                        .HasColumnName("IS_SENIORITY_OVERRIDDEN");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("LocalAreaId")
                        .HasColumnName("LOCAL_AREA_ID");

                    b.Property<int?>("OwnerId")
                        .HasColumnName("OWNER_ID");

                    b.Property<string>("OwnerOrganizationName")
                        .HasColumnName("OWNER_ORGANIZATION_NAME")
                        .HasMaxLength(150);

                    b.Property<float?>("Seniority")
                        .HasColumnName("SENIORITY");

                    b.Property<string>("SeniorityOverrideReason")
                        .HasColumnName("SENIORITY_OVERRIDE_REASON")
                        .HasMaxLength(2048);

                    b.Property<float?>("ServiceHoursLastYear")
                        .HasColumnName("SERVICE_HOURS_LAST_YEAR");

                    b.Property<float?>("ServiceHoursThreeYearsAgo")
                        .HasColumnName("SERVICE_HOURS_THREE_YEARS_AGO");

                    b.Property<float?>("ServiceHoursTwoYearsAgo")
                        .HasColumnName("SERVICE_HOURS_TWO_YEARS_AGO");

                    b.Property<DateTime>("StartDate")
                        .HasColumnName("START_DATE");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("LocalAreaId");

                    b.HasIndex("OwnerId");

                    b.ToTable("HET_SENIORITY_AUDIT");
                });

            modelBuilder.Entity("HETSAPI.Models.ServiceArea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("SERVICE_AREA_ID");

                    b.Property<int?>("AreaNumber")
                        .HasColumnName("AREA_NUMBER");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("DistrictId")
                        .HasColumnName("DISTRICT_ID");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnName("END_DATE");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int>("MinistryServiceAreaID")
                        .HasColumnName("MINISTRY_SERVICE_AREA_ID");

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(150);

                    b.Property<DateTime>("StartDate")
                        .HasColumnName("START_DATE");

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.ToTable("HET_SERVICE_AREA");
                });

            modelBuilder.Entity("HETSAPI.Models.TimeRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("TIME_RECORD_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("EnteredDate")
                        .HasColumnName("ENTERED_DATE");

                    b.Property<float?>("Hours")
                        .HasColumnName("HOURS");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("RentalAgreementId")
                        .HasColumnName("RENTAL_AGREEMENT_ID");

                    b.Property<int?>("RentalAgreementRateId")
                        .HasColumnName("RENTAL_AGREEMENT_RATE_ID");

                    b.Property<string>("TimePeriod")
                        .HasColumnName("TIME_PERIOD")
                        .HasMaxLength(20);

                    b.Property<DateTime>("WorkedDate")
                        .HasColumnName("WORKED_DATE");

                    b.HasKey("Id");

                    b.HasIndex("RentalAgreementId");

                    b.HasIndex("RentalAgreementRateId");

                    b.ToTable("HET_TIME_RECORD");
                });

            modelBuilder.Entity("HETSAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("USER_ID");

                    b.Property<bool>("Active")
                        .HasColumnName("ACTIVE");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("DistrictId")
                        .HasColumnName("DISTRICT_ID");

                    b.Property<string>("Email")
                        .HasColumnName("EMAIL")
                        .HasMaxLength(255);

                    b.Property<string>("GivenName")
                        .HasColumnName("GIVEN_NAME")
                        .HasMaxLength(50);

                    b.Property<string>("Guid")
                        .HasColumnName("GUID")
                        .HasMaxLength(255);

                    b.Property<string>("Initials")
                        .HasColumnName("INITIALS")
                        .HasMaxLength(10);

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("SmAuthorizationDirectory")
                        .HasColumnName("SM_AUTHORIZATION_DIRECTORY")
                        .HasMaxLength(255);

                    b.Property<string>("SmUserId")
                        .HasColumnName("SM_USER_ID")
                        .HasMaxLength(255);

                    b.Property<string>("Surname")
                        .HasColumnName("SURNAME")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.ToTable("HET_USER");
                });

            modelBuilder.Entity("HETSAPI.Models.UserFavourite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("USER_FAVOURITE_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<bool?>("IsDefault")
                        .HasColumnName("IS_DEFAULT");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(150);

                    b.Property<string>("Type")
                        .HasColumnName("TYPE")
                        .HasMaxLength(150);

                    b.Property<int?>("UserId")
                        .HasColumnName("USER_ID");

                    b.Property<string>("Value")
                        .HasColumnName("VALUE")
                        .HasMaxLength(2048);

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("HET_USER_FAVOURITE");
                });

            modelBuilder.Entity("HETSAPI.Models.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("USER_ROLE_ID");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnName("CREATE_TIMESTAMP");

                    b.Property<string>("CreateUserid")
                        .HasColumnName("CREATE_USERID")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EffectiveDate")
                        .HasColumnName("EFFECTIVE_DATE");

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnName("EXPIRY_DATE");

                    b.Property<DateTime>("LastUpdateTimestamp")
                        .HasColumnName("LAST_UPDATE_TIMESTAMP");

                    b.Property<string>("LastUpdateUserid")
                        .HasColumnName("LAST_UPDATE_USERID")
                        .HasMaxLength(50);

                    b.Property<int?>("RoleId")
                        .HasColumnName("ROLE_ID");

                    b.Property<int?>("UserId")
                        .HasColumnName("USER_ID");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("HET_USER_ROLE");
                });

            modelBuilder.Entity("HETSAPI.Models.Attachment", b =>
                {
                    b.HasOne("HETSAPI.Models.Equipment")
                        .WithMany("Attachments")
                        .HasForeignKey("EquipmentId");

                    b.HasOne("HETSAPI.Models.Owner")
                        .WithMany("Attachments")
                        .HasForeignKey("OwnerId");

                    b.HasOne("HETSAPI.Models.Project")
                        .WithMany("Attachments")
                        .HasForeignKey("ProjectId");

                    b.HasOne("HETSAPI.Models.RentalRequest")
                        .WithMany("Attachments")
                        .HasForeignKey("RentalRequestId");
                });

            modelBuilder.Entity("HETSAPI.Models.Contact", b =>
                {
                    b.HasOne("HETSAPI.Models.Owner")
                        .WithMany("Contacts")
                        .HasForeignKey("OwnerId");

                    b.HasOne("HETSAPI.Models.Project")
                        .WithMany("Contacts")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("HETSAPI.Models.District", b =>
                {
                    b.HasOne("HETSAPI.Models.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId");
                });

            modelBuilder.Entity("HETSAPI.Models.DistrictEquipmentType", b =>
                {
                    b.HasOne("HETSAPI.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId");

                    b.HasOne("HETSAPI.Models.EquipmentType", "EquipmentType")
                        .WithMany()
                        .HasForeignKey("EquipmentTypeId");
                });

            modelBuilder.Entity("HETSAPI.Models.Equipment", b =>
                {
                    b.HasOne("HETSAPI.Models.DistrictEquipmentType", "DistrictEquipmentType")
                        .WithMany()
                        .HasForeignKey("DistrictEquipmentTypeId");

                    b.HasOne("HETSAPI.Models.DumpTruck", "DumpTruck")
                        .WithMany()
                        .HasForeignKey("DumpTruckId");

                    b.HasOne("HETSAPI.Models.LocalArea", "LocalArea")
                        .WithMany()
                        .HasForeignKey("LocalAreaId");

                    b.HasOne("HETSAPI.Models.Owner", "Owner")
                        .WithMany("EquipmentList")
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("HETSAPI.Models.EquipmentAttachment", b =>
                {
                    b.HasOne("HETSAPI.Models.Equipment", "Equipment")
                        .WithMany("EquipmentAttachments")
                        .HasForeignKey("EquipmentId");
                });

            modelBuilder.Entity("HETSAPI.Models.GroupMembership", b =>
                {
                    b.HasOne("HETSAPI.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("HETSAPI.Models.User", "User")
                        .WithMany("GroupMemberships")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("HETSAPI.Models.History", b =>
                {
                    b.HasOne("HETSAPI.Models.Equipment")
                        .WithMany("History")
                        .HasForeignKey("EquipmentId");

                    b.HasOne("HETSAPI.Models.Owner")
                        .WithMany("History")
                        .HasForeignKey("OwnerId");

                    b.HasOne("HETSAPI.Models.Project")
                        .WithMany("History")
                        .HasForeignKey("ProjectId");

                    b.HasOne("HETSAPI.Models.RentalRequest")
                        .WithMany("History")
                        .HasForeignKey("RentalRequestId");
                });

            modelBuilder.Entity("HETSAPI.Models.LocalArea", b =>
                {
                    b.HasOne("HETSAPI.Models.ServiceArea", "ServiceArea")
                        .WithMany()
                        .HasForeignKey("ServiceAreaId");
                });

            modelBuilder.Entity("HETSAPI.Models.LocalAreaRotationList", b =>
                {
                    b.HasOne("HETSAPI.Models.Equipment", "AskNextBlock1")
                        .WithMany()
                        .HasForeignKey("AskNextBlock1Id");

                    b.HasOne("HETSAPI.Models.Equipment", "AskNextBlock2")
                        .WithMany()
                        .HasForeignKey("AskNextBlock2Id");

                    b.HasOne("HETSAPI.Models.Equipment", "AskNextBlockOpen")
                        .WithMany()
                        .HasForeignKey("AskNextBlockOpenId");

                    b.HasOne("HETSAPI.Models.DistrictEquipmentType", "DistrictEquipmentType")
                        .WithMany()
                        .HasForeignKey("DistrictEquipmentTypeId");

                    b.HasOne("HETSAPI.Models.LocalArea", "LocalArea")
                        .WithMany()
                        .HasForeignKey("LocalAreaId");
                });

            modelBuilder.Entity("HETSAPI.Models.Note", b =>
                {
                    b.HasOne("HETSAPI.Models.Equipment")
                        .WithMany("Notes")
                        .HasForeignKey("EquipmentId");

                    b.HasOne("HETSAPI.Models.Owner")
                        .WithMany("Notes")
                        .HasForeignKey("OwnerId");

                    b.HasOne("HETSAPI.Models.Project")
                        .WithMany("Notes")
                        .HasForeignKey("ProjectId");

                    b.HasOne("HETSAPI.Models.RentalRequest")
                        .WithMany("Notes")
                        .HasForeignKey("RentalRequestId");
                });

            modelBuilder.Entity("HETSAPI.Models.Owner", b =>
                {
                    b.HasOne("HETSAPI.Models.LocalArea", "LocalArea")
                        .WithMany()
                        .HasForeignKey("LocalAreaId");

                    b.HasOne("HETSAPI.Models.Contact", "PrimaryContact")
                        .WithMany()
                        .HasForeignKey("PrimaryContactId");
                });

            modelBuilder.Entity("HETSAPI.Models.Project", b =>
                {
                    b.HasOne("HETSAPI.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId");

                    b.HasOne("HETSAPI.Models.Contact", "PrimaryContact")
                        .WithMany()
                        .HasForeignKey("PrimaryContactId");
                });

            modelBuilder.Entity("HETSAPI.Models.RentalAgreement", b =>
                {
                    b.HasOne("HETSAPI.Models.Equipment", "Equipment")
                        .WithMany()
                        .HasForeignKey("EquipmentId");

                    b.HasOne("HETSAPI.Models.Project", "Project")
                        .WithMany("RentalAgreements")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("HETSAPI.Models.RentalAgreementCondition", b =>
                {
                    b.HasOne("HETSAPI.Models.RentalAgreement", "RentalAgreement")
                        .WithMany("RentalAgreementConditions")
                        .HasForeignKey("RentalAgreementId");
                });

            modelBuilder.Entity("HETSAPI.Models.RentalAgreementRate", b =>
                {
                    b.HasOne("HETSAPI.Models.RentalAgreement", "RentalAgreement")
                        .WithMany("RentalAgreementRates")
                        .HasForeignKey("RentalAgreementId");
                });

            modelBuilder.Entity("HETSAPI.Models.RentalRequest", b =>
                {
                    b.HasOne("HETSAPI.Models.DistrictEquipmentType", "DistrictEquipmentType")
                        .WithMany()
                        .HasForeignKey("DistrictEquipmentTypeId");

                    b.HasOne("HETSAPI.Models.Equipment", "FirstOnRotationList")
                        .WithMany()
                        .HasForeignKey("FirstOnRotationListId");

                    b.HasOne("HETSAPI.Models.LocalArea", "LocalArea")
                        .WithMany()
                        .HasForeignKey("LocalAreaId");

                    b.HasOne("HETSAPI.Models.Project", "Project")
                        .WithMany("RentalRequests")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("HETSAPI.Models.RentalRequestAttachment", b =>
                {
                    b.HasOne("HETSAPI.Models.RentalRequest", "RentalRequest")
                        .WithMany("RentalRequestAttachments")
                        .HasForeignKey("RentalRequestId");
                });

            modelBuilder.Entity("HETSAPI.Models.RentalRequestRotationList", b =>
                {
                    b.HasOne("HETSAPI.Models.Equipment", "Equipment")
                        .WithMany()
                        .HasForeignKey("EquipmentId");

                    b.HasOne("HETSAPI.Models.RentalAgreement", "RentalAgreement")
                        .WithMany()
                        .HasForeignKey("RentalAgreementId");

                    b.HasOne("HETSAPI.Models.RentalRequest")
                        .WithMany("RentalRequestRotationList")
                        .HasForeignKey("RentalRequestId");
                });

            modelBuilder.Entity("HETSAPI.Models.RolePermission", b =>
                {
                    b.HasOne("HETSAPI.Models.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId");

                    b.HasOne("HETSAPI.Models.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("HETSAPI.Models.SeniorityAudit", b =>
                {
                    b.HasOne("HETSAPI.Models.Equipment", "Equipment")
                        .WithMany()
                        .HasForeignKey("EquipmentId");

                    b.HasOne("HETSAPI.Models.LocalArea", "LocalArea")
                        .WithMany()
                        .HasForeignKey("LocalAreaId");

                    b.HasOne("HETSAPI.Models.Owner", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("HETSAPI.Models.ServiceArea", b =>
                {
                    b.HasOne("HETSAPI.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId");
                });

            modelBuilder.Entity("HETSAPI.Models.TimeRecord", b =>
                {
                    b.HasOne("HETSAPI.Models.RentalAgreement", "RentalAgreement")
                        .WithMany("TimeRecords")
                        .HasForeignKey("RentalAgreementId");

                    b.HasOne("HETSAPI.Models.RentalAgreementRate", "RentalAgreementRate")
                        .WithMany("TimeRecords")
                        .HasForeignKey("RentalAgreementRateId");
                });

            modelBuilder.Entity("HETSAPI.Models.User", b =>
                {
                    b.HasOne("HETSAPI.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId");
                });

            modelBuilder.Entity("HETSAPI.Models.UserFavourite", b =>
                {
                    b.HasOne("HETSAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("HETSAPI.Models.UserRole", b =>
                {
                    b.HasOne("HETSAPI.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId");

                    b.HasOne("HETSAPI.Models.User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId");
                });
        }
    }
}
