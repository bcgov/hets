using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HETSAPI.Models;

namespace HETSAPI.Migrations
{
    [DbContext(typeof(DbAppContext))]
    [Migration("20170201190010_HETS-72-2-1-1")]
    partial class HETS72211
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("HETSAPI.Models.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ATTACHMENT_ID");

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION")
                        .HasMaxLength(255);

                    b.Property<int?>("EquipmentId")
                        .HasColumnName("EQUIPMENT_ID");

                    b.Property<string>("ExternalFileName")
                        .HasColumnName("EXTERNAL_FILE_NAME")
                        .HasMaxLength(2048);

                    b.Property<string>("InternalFileName")
                        .HasColumnName("INTERNAL_FILE_NAME")
                        .HasMaxLength(2048);

                    b.Property<int?>("OwnerId")
                        .HasColumnName("OWNER_ID");

                    b.Property<int?>("ProjectId")
                        .HasColumnName("PROJECT_ID");

                    b.Property<int?>("RequestId")
                        .HasColumnName("REQUEST_ID");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("RequestId");

                    b.ToTable("HET_ATTACHMENT");
                });

            modelBuilder.Entity("HETSAPI.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CITY_ID");

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("HET_CITY");
                });

            modelBuilder.Entity("HETSAPI.Models.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CONTACT_ID");

                    b.Property<string>("GivenName")
                        .HasColumnName("GIVEN_NAME")
                        .HasMaxLength(255);

                    b.Property<string>("Notes")
                        .HasColumnName("NOTES")
                        .HasMaxLength(255);

                    b.Property<int?>("OwnerId")
                        .HasColumnName("OWNER_ID");

                    b.Property<int?>("ProjectId")
                        .HasColumnName("PROJECT_ID");

                    b.Property<string>("Role")
                        .HasColumnName("ROLE")
                        .HasMaxLength(255);

                    b.Property<string>("Surname")
                        .HasColumnName("SURNAME")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ProjectId");

                    b.ToTable("HET_CONTACT");
                });

            modelBuilder.Entity("HETSAPI.Models.ContactAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CONTACT_ADDRESS_ID");

                    b.Property<string>("AddressLine1")
                        .HasColumnName("ADDRESS_LINE1")
                        .HasMaxLength(255);

                    b.Property<string>("AddressLine2")
                        .HasColumnName("ADDRESS_LINE2")
                        .HasMaxLength(255);

                    b.Property<string>("City")
                        .HasColumnName("CITY")
                        .HasMaxLength(255);

                    b.Property<int?>("ContactId")
                        .HasColumnName("CONTACT_ID");

                    b.Property<string>("PostalCode")
                        .HasColumnName("POSTAL_CODE")
                        .HasMaxLength(255);

                    b.Property<string>("Province")
                        .HasColumnName("PROVINCE")
                        .HasMaxLength(255);

                    b.Property<string>("Type")
                        .HasColumnName("TYPE")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("HET_CONTACT_ADDRESS");
                });

            modelBuilder.Entity("HETSAPI.Models.ContactPhone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CONTACT_PHONE_ID");

                    b.Property<int?>("ContactId")
                        .HasColumnName("CONTACT_ID");

                    b.Property<string>("PhoneNumber")
                        .HasColumnName("PHONE_NUMBER")
                        .HasMaxLength(255);

                    b.Property<string>("Type")
                        .HasColumnName("TYPE")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("HET_CONTACT_PHONE");
                });

            modelBuilder.Entity("HETSAPI.Models.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("DISTRICT_ID");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnName("END_DATE");

                    b.Property<int?>("MinistryDistrictID")
                        .HasColumnName("MINISTRY_DISTRICT_ID");

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(255);

                    b.Property<int?>("RegionRefId")
                        .HasColumnName("REGION_REF_ID");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnName("START_DATE");

                    b.HasKey("Id");

                    b.HasIndex("RegionRefId");

                    b.ToTable("HET_DISTRICT");
                });

            modelBuilder.Entity("HETSAPI.Models.DumpTruck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("DUMP_TRUCK_ID");

                    b.Property<string>("BellyDump")
                        .HasColumnName("BELLY_DUMP")
                        .HasMaxLength(255);

                    b.Property<string>("BoxCapacity")
                        .HasColumnName("BOX_CAPACITY")
                        .HasMaxLength(255);

                    b.Property<string>("BoxHeight")
                        .HasColumnName("BOX_HEIGHT")
                        .HasMaxLength(255);

                    b.Property<string>("BoxLength")
                        .HasColumnName("BOX_LENGTH")
                        .HasMaxLength(255);

                    b.Property<string>("BoxWidth")
                        .HasColumnName("BOX_WIDTH")
                        .HasMaxLength(255);

                    b.Property<string>("FrontAxleCapacity")
                        .HasColumnName("FRONT_AXLE_CAPACITY")
                        .HasMaxLength(255);

                    b.Property<string>("FrontTireSize")
                        .HasColumnName("FRONT_TIRE_SIZE")
                        .HasMaxLength(255);

                    b.Property<string>("FrontTireUOM")
                        .HasColumnName("FRONT_TIRE_UOM")
                        .HasMaxLength(255);

                    b.Property<string>("HiliftGate")
                        .HasColumnName("HILIFT_GATE")
                        .HasMaxLength(255);

                    b.Property<string>("LegalCapacity")
                        .HasColumnName("LEGAL_CAPACITY")
                        .HasMaxLength(255);

                    b.Property<string>("LegalLoad")
                        .HasColumnName("LEGAL_LOAD")
                        .HasMaxLength(255);

                    b.Property<string>("LegalPUPTareWeight")
                        .HasColumnName("LEGAL_PUPTARE_WEIGHT")
                        .HasMaxLength(255);

                    b.Property<string>("LicencedCapacity")
                        .HasColumnName("LICENCED_CAPACITY")
                        .HasMaxLength(255);

                    b.Property<string>("LicencedGVW")
                        .HasColumnName("LICENCED_GVW")
                        .HasMaxLength(255);

                    b.Property<string>("LicencedGVWUOM")
                        .HasColumnName("LICENCED_GVWUOM")
                        .HasMaxLength(255);

                    b.Property<string>("LicencedLoad")
                        .HasColumnName("LICENCED_LOAD")
                        .HasMaxLength(255);

                    b.Property<string>("LicencedPUPTareWeight")
                        .HasColumnName("LICENCED_PUPTARE_WEIGHT")
                        .HasMaxLength(255);

                    b.Property<string>("LicencedTareWeight")
                        .HasColumnName("LICENCED_TARE_WEIGHT")
                        .HasMaxLength(255);

                    b.Property<string>("PUP")
                        .HasColumnName("PUP")
                        .HasMaxLength(255);

                    b.Property<string>("RearAxleCapacity")
                        .HasColumnName("REAR_AXLE_CAPACITY")
                        .HasMaxLength(255);

                    b.Property<string>("RearAxleSpacing")
                        .HasColumnName("REAR_AXLE_SPACING")
                        .HasMaxLength(255);

                    b.Property<string>("RockBox")
                        .HasColumnName("ROCK_BOX")
                        .HasMaxLength(255);

                    b.Property<string>("SealCoatHitch")
                        .HasColumnName("SEAL_COAT_HITCH")
                        .HasMaxLength(255);

                    b.Property<string>("SingleAxle")
                        .HasColumnName("SINGLE_AXLE")
                        .HasMaxLength(255);

                    b.Property<string>("TandemAxle")
                        .HasColumnName("TANDEM_AXLE")
                        .HasMaxLength(255);

                    b.Property<string>("TrailerBoxCapacity")
                        .HasColumnName("TRAILER_BOX_CAPACITY")
                        .HasMaxLength(255);

                    b.Property<string>("TrailerBoxHeight")
                        .HasColumnName("TRAILER_BOX_HEIGHT")
                        .HasMaxLength(255);

                    b.Property<string>("TrailerBoxLength")
                        .HasColumnName("TRAILER_BOX_LENGTH")
                        .HasMaxLength(255);

                    b.Property<string>("TrailerBoxWidth")
                        .HasColumnName("TRAILER_BOX_WIDTH")
                        .HasMaxLength(255);

                    b.Property<string>("Tridem")
                        .HasColumnName("TRIDEM")
                        .HasMaxLength(255);

                    b.Property<string>("WaterTruck")
                        .HasColumnName("WATER_TRUCK")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("HET_DUMP_TRUCK");
                });

            modelBuilder.Entity("HETSAPI.Models.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EQUIPMENT_ID");

                    b.Property<string>("AddressLine1")
                        .HasColumnName("ADDRESS_LINE1")
                        .HasMaxLength(255);

                    b.Property<string>("AddressLine2")
                        .HasColumnName("ADDRESS_LINE2")
                        .HasMaxLength(255);

                    b.Property<string>("AddressLine3")
                        .HasColumnName("ADDRESS_LINE3")
                        .HasMaxLength(255);

                    b.Property<string>("AddressLine4")
                        .HasColumnName("ADDRESS_LINE4")
                        .HasMaxLength(255);

                    b.Property<string>("Approval")
                        .HasColumnName("APPROVAL")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("ApprovedDate")
                        .HasColumnName("APPROVED_DATE");

                    b.Property<string>("ArchiveCd")
                        .HasColumnName("ARCHIVE_CD")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("ArchiveDate")
                        .HasColumnName("ARCHIVE_DATE");

                    b.Property<string>("ArchiveReason")
                        .HasColumnName("ARCHIVE_REASON")
                        .HasMaxLength(255);

                    b.Property<float?>("BlockNumber")
                        .HasColumnName("BLOCK_NUMBER");

                    b.Property<string>("City")
                        .HasColumnName("CITY")
                        .HasMaxLength(255);

                    b.Property<string>("Comment")
                        .HasColumnName("COMMENT")
                        .HasMaxLength(255);

                    b.Property<float?>("CycleHrsWrk")
                        .HasColumnName("CYCLE_HRS_WRK");

                    b.Property<float?>("DraftBlockNum")
                        .HasColumnName("DRAFT_BLOCK_NUM");

                    b.Property<int?>("DumpTruckDetailsRefId")
                        .HasColumnName("DUMP_TRUCK_DETAILS_REF_ID");

                    b.Property<string>("EquipCd")
                        .HasColumnName("EQUIP_CD")
                        .HasMaxLength(255);

                    b.Property<int?>("EquipmentTypeRefId")
                        .HasColumnName("EQUIPMENT_TYPE_REF_ID");

                    b.Property<string>("FrozenOut")
                        .HasColumnName("FROZEN_OUT")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("LastVerifiedDate")
                        .HasColumnName("LAST_VERIFIED_DATE");

                    b.Property<string>("Licence")
                        .HasColumnName("LICENCE")
                        .HasMaxLength(255);

                    b.Property<int?>("LocalAreaRefId")
                        .HasColumnName("LOCAL_AREA_REF_ID");

                    b.Property<string>("Make")
                        .HasColumnName("MAKE")
                        .HasMaxLength(255);

                    b.Property<string>("Model")
                        .HasColumnName("MODEL")
                        .HasMaxLength(255);

                    b.Property<float?>("NumYears")
                        .HasColumnName("NUM_YEARS");

                    b.Property<string>("Operator")
                        .HasColumnName("OPERATOR")
                        .HasMaxLength(255);

                    b.Property<int?>("OwnerRefId")
                        .HasColumnName("OWNER_REF_ID");

                    b.Property<float?>("PayRate")
                        .HasColumnName("PAY_RATE");

                    b.Property<string>("Postal")
                        .HasColumnName("POSTAL")
                        .HasMaxLength(255);

                    b.Property<string>("PrevRegArea")
                        .HasColumnName("PREV_REG_AREA")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("ReceivedDate")
                        .HasColumnName("RECEIVED_DATE");

                    b.Property<string>("RefuseRate")
                        .HasColumnName("REFUSE_RATE")
                        .HasMaxLength(255);

                    b.Property<string>("RegDumpTruck")
                        .HasColumnName("REG_DUMP_TRUCK")
                        .HasMaxLength(255);

                    b.Property<float?>("Seniority")
                        .HasColumnName("SENIORITY");

                    b.Property<string>("SerialNum")
                        .HasColumnName("SERIAL_NUM")
                        .HasMaxLength(255);

                    b.Property<string>("Size")
                        .HasColumnName("SIZE")
                        .HasMaxLength(255);

                    b.Property<string>("StatusCd")
                        .HasColumnName("STATUS_CD")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("ToDate")
                        .HasColumnName("TO_DATE");

                    b.Property<string>("Type")
                        .HasColumnName("TYPE")
                        .HasMaxLength(255);

                    b.Property<string>("Working")
                        .HasColumnName("WORKING")
                        .HasMaxLength(255);

                    b.Property<float?>("YTD")
                        .HasColumnName("YTD");

                    b.Property<float?>("YTD1")
                        .HasColumnName("YTD1");

                    b.Property<float?>("YTD2")
                        .HasColumnName("YTD2");

                    b.Property<float?>("YTD3")
                        .HasColumnName("YTD3");

                    b.Property<string>("Year")
                        .HasColumnName("YEAR")
                        .HasMaxLength(255);

                    b.Property<string>("YearEndReg")
                        .HasColumnName("YEAR_END_REG")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("DumpTruckDetailsRefId");

                    b.HasIndex("EquipmentTypeRefId");

                    b.HasIndex("LocalAreaRefId");

                    b.HasIndex("OwnerRefId");

                    b.ToTable("HET_EQUIPMENT");
                });

            modelBuilder.Entity("HETSAPI.Models.EquipmentAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EQUIPMENT_ATTACHMENT_ID");

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION")
                        .HasMaxLength(255);

                    b.Property<int?>("EquipmentRefId")
                        .HasColumnName("EQUIPMENT_REF_ID");

                    b.Property<int?>("SeqNum")
                        .HasColumnName("SEQ_NUM");

                    b.Property<int?>("TypeRefId")
                        .HasColumnName("TYPE_REF_ID");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentRefId");

                    b.HasIndex("TypeRefId");

                    b.ToTable("HET_EQUIPMENT_ATTACHMENT");
                });

            modelBuilder.Entity("HETSAPI.Models.EquipmentAttachmentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EQUIPMENT_ATTACHMENT_TYPE_ID");

                    b.Property<string>("Code")
                        .HasColumnName("CODE")
                        .HasMaxLength(255);

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("HET_EQUIPMENT_ATTACHMENT_TYPE");
                });

            modelBuilder.Entity("HETSAPI.Models.EquipmentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EQUIPMENT_TYPE_ID");

                    b.Property<string>("Code")
                        .HasColumnName("CODE")
                        .HasMaxLength(255);

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION")
                        .HasMaxLength(255);

                    b.Property<float?>("EquipRentalRateNo")
                        .HasColumnName("EQUIP_RENTAL_RATE_NO");

                    b.Property<float?>("EquipRentalRatePage")
                        .HasColumnName("EQUIP_RENTAL_RATE_PAGE");

                    b.Property<float?>("ExtendHours")
                        .HasColumnName("EXTEND_HOURS");

                    b.Property<int?>("LocalAreaRefId")
                        .HasColumnName("LOCAL_AREA_REF_ID");

                    b.Property<float?>("MaxHours")
                        .HasColumnName("MAX_HOURS");

                    b.Property<float?>("MaxHoursSub")
                        .HasColumnName("MAX_HOURS_SUB");

                    b.Property<string>("SecondBlk")
                        .HasColumnName("SECOND_BLK")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("LocalAreaRefId");

                    b.ToTable("HET_EQUIPMENT_TYPE");
                });

            modelBuilder.Entity("HETSAPI.Models.FavouriteContextType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("FAVOURITE_CONTEXT_TYPE_ID");

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("HET_FAVOURITE_CONTEXT_TYPE");
                });

            modelBuilder.Entity("HETSAPI.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("GROUP_ID");

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION")
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(255);

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

                    b.Property<int?>("GroupRefId")
                        .HasColumnName("GROUP_REF_ID");

                    b.Property<int?>("UserRefId")
                        .HasColumnName("USER_REF_ID");

                    b.HasKey("Id");

                    b.HasIndex("GroupRefId");

                    b.HasIndex("UserRefId");

                    b.ToTable("HET_GROUP_MEMBERSHIP");
                });

            modelBuilder.Entity("HETSAPI.Models.HireOffer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("HIRE_OFFER_ID");

                    b.Property<bool?>("AcceptedOffer")
                        .HasColumnName("ACCEPTED_OFFER");

                    b.Property<bool?>("Asked")
                        .HasColumnName("ASKED");

                    b.Property<DateTime?>("AskedDate")
                        .HasColumnName("ASKED_DATE");

                    b.Property<int?>("EquipmentRefId")
                        .HasColumnName("EQUIPMENT_REF_ID");

                    b.Property<string>("EquipmentUpdateReason")
                        .HasColumnName("EQUIPMENT_UPDATE_REASON")
                        .HasMaxLength(255);

                    b.Property<bool?>("EquipmentVerifiedActive")
                        .HasColumnName("EQUIPMENT_VERIFIED_ACTIVE");

                    b.Property<bool?>("FlagEquipmentUpdate")
                        .HasColumnName("FLAG_EQUIPMENT_UPDATE");

                    b.Property<bool?>("IsForceHire")
                        .HasColumnName("IS_FORCE_HIRE");

                    b.Property<string>("Note")
                        .HasColumnName("NOTE")
                        .HasMaxLength(255);

                    b.Property<string>("RefuseReason")
                        .HasColumnName("REFUSE_REASON")
                        .HasMaxLength(255);

                    b.Property<int?>("RentalAgreementRefId")
                        .HasColumnName("RENTAL_AGREEMENT_REF_ID");

                    b.Property<int?>("RequestRefId")
                        .HasColumnName("REQUEST_REF_ID");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentRefId");

                    b.HasIndex("RentalAgreementRefId");

                    b.HasIndex("RequestRefId");

                    b.ToTable("HET_HIRE_OFFER");
                });

            modelBuilder.Entity("HETSAPI.Models.History", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("HISTORY_ID");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnName("CREATED_DATE");

                    b.Property<int?>("EquipmentId")
                        .HasColumnName("EQUIPMENT_ID");

                    b.Property<string>("HistoryText")
                        .HasColumnName("HISTORY_TEXT")
                        .HasMaxLength(2048);

                    b.Property<int?>("OwnerId")
                        .HasColumnName("OWNER_ID");

                    b.Property<int?>("ProjectId")
                        .HasColumnName("PROJECT_ID");

                    b.Property<int?>("RequestId")
                        .HasColumnName("REQUEST_ID");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("RequestId");

                    b.ToTable("HET_HISTORY");
                });

            modelBuilder.Entity("HETSAPI.Models.LocalArea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("LOCAL_AREA_ID");

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(255);

                    b.Property<int?>("ServiceAreaRefId")
                        .HasColumnName("SERVICE_AREA_REF_ID");

                    b.HasKey("Id");

                    b.HasIndex("ServiceAreaRefId");

                    b.ToTable("HET_LOCAL_AREA");
                });

            modelBuilder.Entity("HETSAPI.Models.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("NOTE_ID");

                    b.Property<int?>("EquipmentId")
                        .HasColumnName("EQUIPMENT_ID");

                    b.Property<bool?>("IsNoLongerRelevant")
                        .HasColumnName("IS_NO_LONGER_RELEVANT");

                    b.Property<int?>("OwnerId")
                        .HasColumnName("OWNER_ID");

                    b.Property<int?>("ProjectId")
                        .HasColumnName("PROJECT_ID");

                    b.Property<int?>("RequestId")
                        .HasColumnName("REQUEST_ID");

                    b.Property<string>("_Note")
                        .HasColumnName("_NOTE")
                        .HasMaxLength(2048);

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("RequestId");

                    b.ToTable("HET_NOTE");
                });

            modelBuilder.Entity("HETSAPI.Models.Owner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("OWNER_ID");

                    b.Property<string>("ArchiveCd")
                        .HasColumnName("ARCHIVE_CD")
                        .HasMaxLength(255);

                    b.Property<string>("ArchiveReason")
                        .HasColumnName("ARCHIVE_REASON")
                        .HasMaxLength(255);

                    b.Property<string>("CGLCompany")
                        .HasColumnName("CGLCOMPANY")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("CGLEndDate")
                        .HasColumnName("CGLEND_DATE");

                    b.Property<string>("CGLPolicy")
                        .HasColumnName("CGLPOLICY")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("CGLStartDate")
                        .HasColumnName("CGLSTART_DATE");

                    b.Property<string>("Comment")
                        .HasColumnName("COMMENT")
                        .HasMaxLength(255);

                    b.Property<string>("ContactPerson")
                        .HasColumnName("CONTACT_PERSON")
                        .HasMaxLength(255);

                    b.Property<int?>("LocalAreaRefId")
                        .HasColumnName("LOCAL_AREA_REF_ID");

                    b.Property<string>("LocalToArea")
                        .HasColumnName("LOCAL_TO_AREA")
                        .HasMaxLength(255);

                    b.Property<string>("MaintenanceContractor")
                        .HasColumnName("MAINTENANCE_CONTRACTOR")
                        .HasMaxLength(255);

                    b.Property<string>("OwnerCd")
                        .HasColumnName("OWNER_CD")
                        .HasMaxLength(255);

                    b.Property<string>("OwnerFirstName")
                        .HasColumnName("OWNER_FIRST_NAME")
                        .HasMaxLength(255);

                    b.Property<string>("OwnerLastName")
                        .HasColumnName("OWNER_LAST_NAME")
                        .HasMaxLength(255);

                    b.Property<int?>("PrimaryContactRefId")
                        .HasColumnName("PRIMARY_CONTACT_REF_ID");

                    b.Property<string>("StatusCd")
                        .HasColumnName("STATUS_CD")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("WCBExpiryDate")
                        .HasColumnName("WCBEXPIRY_DATE");

                    b.Property<int?>("WCBNum")
                        .HasColumnName("WCBNUM");

                    b.HasKey("Id");

                    b.HasIndex("LocalAreaRefId");

                    b.HasIndex("PrimaryContactRefId");

                    b.ToTable("HET_OWNER");
                });

            modelBuilder.Entity("HETSAPI.Models.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PERMISSION_ID");

                    b.Property<string>("Code")
                        .HasColumnName("CODE")
                        .HasMaxLength(255);

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION")
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("HET_PERMISSION");
                });

            modelBuilder.Entity("HETSAPI.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PROJECT_ID");

                    b.Property<string>("JobDesc1")
                        .HasColumnName("JOB_DESC1")
                        .HasMaxLength(255);

                    b.Property<string>("JobDesc2")
                        .HasColumnName("JOB_DESC2")
                        .HasMaxLength(255);

                    b.Property<int?>("PrimaryContactRefId")
                        .HasColumnName("PRIMARY_CONTACT_REF_ID");

                    b.Property<string>("ProjectNum")
                        .HasColumnName("PROJECT_NUM")
                        .HasMaxLength(255);

                    b.Property<int?>("ServiceAreaRefId")
                        .HasColumnName("SERVICE_AREA_REF_ID");

                    b.HasKey("Id");

                    b.HasIndex("PrimaryContactRefId");

                    b.HasIndex("ServiceAreaRefId");

                    b.ToTable("HET_PROJECT");
                });

            modelBuilder.Entity("HETSAPI.Models.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("REGION_ID");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnName("END_DATE");

                    b.Property<int?>("MinistryRegionID")
                        .HasColumnName("MINISTRY_REGION_ID");

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("StartDate")
                        .HasColumnName("START_DATE");

                    b.HasKey("Id");

                    b.ToTable("HET_REGION");
                });

            modelBuilder.Entity("HETSAPI.Models.RentalAgreement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("RENTAL_AGREEMENT_ID");

                    b.Property<int?>("EquipmentRefId")
                        .HasColumnName("EQUIPMENT_REF_ID");

                    b.Property<int?>("ProjectRefId")
                        .HasColumnName("PROJECT_REF_ID");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentRefId");

                    b.HasIndex("ProjectRefId");

                    b.ToTable("HET_RENTAL_AGREEMENT");
                });

            modelBuilder.Entity("HETSAPI.Models.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("REQUEST_ID");

                    b.Property<int?>("EquipmentCount")
                        .HasColumnName("EQUIPMENT_COUNT");

                    b.Property<int?>("EquipmentTypeRefId")
                        .HasColumnName("EQUIPMENT_TYPE_REF_ID");

                    b.Property<DateTime?>("ExpectedEndDate")
                        .HasColumnName("EXPECTED_END_DATE");

                    b.Property<int?>("ExpectedHours")
                        .HasColumnName("EXPECTED_HOURS");

                    b.Property<DateTime?>("ExpectedStartDate")
                        .HasColumnName("EXPECTED_START_DATE");

                    b.Property<int?>("LocalAreaRefId")
                        .HasColumnName("LOCAL_AREA_REF_ID");

                    b.Property<int?>("ProjectRefId")
                        .HasColumnName("PROJECT_REF_ID");

                    b.Property<int?>("RotationListRefId")
                        .HasColumnName("ROTATION_LIST_REF_ID");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentTypeRefId");

                    b.HasIndex("LocalAreaRefId");

                    b.HasIndex("ProjectRefId");

                    b.HasIndex("RotationListRefId");

                    b.ToTable("HET_REQUEST");
                });

            modelBuilder.Entity("HETSAPI.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ROLE_ID");

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION")
                        .HasMaxLength(255);

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

                    b.Property<int?>("PermissionRefId")
                        .HasColumnName("PERMISSION_REF_ID");

                    b.Property<int?>("RoleRefId")
                        .HasColumnName("ROLE_REF_ID");

                    b.HasKey("Id");

                    b.HasIndex("PermissionRefId");

                    b.HasIndex("RoleRefId");

                    b.ToTable("HET_ROLE_PERMISSION");
                });

            modelBuilder.Entity("HETSAPI.Models.RotationList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ROTATION_LIST_ID");

                    b.Property<int?>("EquipmentTypeRefId")
                        .HasColumnName("EQUIPMENT_TYPE_REF_ID");

                    b.Property<int?>("LocalAreaRefId")
                        .HasColumnName("LOCAL_AREA_REF_ID");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentTypeRefId");

                    b.HasIndex("LocalAreaRefId");

                    b.ToTable("HET_ROTATION_LIST");
                });

            modelBuilder.Entity("HETSAPI.Models.RotationListBlock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ROTATION_LIST_BLOCK_ID");

                    b.Property<string>("BlockName")
                        .HasColumnName("BLOCK_NAME");

                    b.Property<int?>("BlockNum")
                        .HasColumnName("BLOCK_NUM");

                    b.Property<string>("Closed")
                        .HasColumnName("CLOSED")
                        .HasMaxLength(255);

                    b.Property<string>("ClosedBy")
                        .HasColumnName("CLOSED_BY");

                    b.Property<string>("ClosedComments")
                        .HasColumnName("CLOSED_COMMENTS")
                        .HasMaxLength(2048);

                    b.Property<DateTime?>("ClosedDate")
                        .HasColumnName("CLOSED_DATE");

                    b.Property<float?>("CycleNum")
                        .HasColumnName("CYCLE_NUM");

                    b.Property<int?>("LastHiredEquipmentRefId")
                        .HasColumnName("LAST_HIRED_EQUIPMENT_REF_ID");

                    b.Property<float?>("MaxCycle")
                        .HasColumnName("MAX_CYCLE");

                    b.Property<string>("Moved")
                        .HasColumnName("MOVED")
                        .HasMaxLength(255);

                    b.Property<string>("ReservedBy")
                        .HasColumnName("RESERVED_BY")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("ReservedDate")
                        .HasColumnName("RESERVED_DATE");

                    b.Property<int?>("RotatedBlock")
                        .HasColumnName("ROTATED_BLOCK");

                    b.Property<int?>("RotationListRefId")
                        .HasColumnName("ROTATION_LIST_REF_ID");

                    b.Property<int?>("StartCycleEquipmentRefId")
                        .HasColumnName("START_CYCLE_EQUIPMENT_REF_ID");

                    b.Property<string>("StartWasZero")
                        .HasColumnName("START_WAS_ZERO")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("LastHiredEquipmentRefId");

                    b.HasIndex("RotationListRefId");

                    b.HasIndex("StartCycleEquipmentRefId");

                    b.ToTable("HET_ROTATION_LIST_BLOCK");
                });

            modelBuilder.Entity("HETSAPI.Models.SeniorityAudit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("SENIORITY_AUDIT_ID");

                    b.Property<float?>("BlockNum")
                        .HasColumnName("BLOCK_NUM");

                    b.Property<float?>("CycleHrsWrk")
                        .HasColumnName("CYCLE_HRS_WRK");

                    b.Property<string>("EquipCd")
                        .HasColumnName("EQUIP_CD")
                        .HasMaxLength(255);

                    b.Property<int?>("EquipmentRefId")
                        .HasColumnName("EQUIPMENT_REF_ID");

                    b.Property<string>("FrozenOut")
                        .HasColumnName("FROZEN_OUT")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("GeneratedTime")
                        .HasColumnName("GENERATED_TIME");

                    b.Property<int?>("LocalAreaRefId")
                        .HasColumnName("LOCAL_AREA_REF_ID");

                    b.Property<string>("OwnerName")
                        .HasColumnName("OWNER_NAME")
                        .HasMaxLength(255);

                    b.Property<int?>("OwnerRefId")
                        .HasColumnName("OWNER_REF_ID");

                    b.Property<int?>("ProjectRefId")
                        .HasColumnName("PROJECT_REF_ID");

                    b.Property<float?>("Seniority")
                        .HasColumnName("SENIORITY");

                    b.Property<string>("Working")
                        .HasColumnName("WORKING")
                        .HasMaxLength(255);

                    b.Property<float?>("YTD")
                        .HasColumnName("YTD");

                    b.Property<float?>("YTD1")
                        .HasColumnName("YTD1");

                    b.Property<float?>("YTD2")
                        .HasColumnName("YTD2");

                    b.Property<float?>("YTD3")
                        .HasColumnName("YTD3");

                    b.Property<string>("YearEndReg")
                        .HasColumnName("YEAR_END_REG")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("EquipmentRefId");

                    b.HasIndex("LocalAreaRefId");

                    b.HasIndex("OwnerRefId");

                    b.HasIndex("ProjectRefId");

                    b.ToTable("HET_SENIORITY_AUDIT");
                });

            modelBuilder.Entity("HETSAPI.Models.ServiceArea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("SERVICE_AREA_ID");

                    b.Property<int?>("DistrictRefId")
                        .HasColumnName("DISTRICT_REF_ID");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnName("END_DATE");

                    b.Property<int?>("MinistryServiceAreaID")
                        .HasColumnName("MINISTRY_SERVICE_AREA_ID");

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("StartDate")
                        .HasColumnName("START_DATE");

                    b.HasKey("Id");

                    b.HasIndex("DistrictRefId");

                    b.ToTable("HET_SERVICE_AREA");
                });

            modelBuilder.Entity("HETSAPI.Models.TimeRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("TIME_RECORD_ID");

                    b.Property<DateTime?>("EnteredDate")
                        .HasColumnName("ENTERED_DATE");

                    b.Property<float?>("Hours")
                        .HasColumnName("HOURS");

                    b.Property<float?>("Hours2")
                        .HasColumnName("HOURS2");

                    b.Property<float?>("Hours3")
                        .HasColumnName("HOURS3");

                    b.Property<float?>("Rate")
                        .HasColumnName("RATE");

                    b.Property<float?>("Rate2")
                        .HasColumnName("RATE2");

                    b.Property<float?>("Rate3")
                        .HasColumnName("RATE3");

                    b.Property<int?>("RentalAgreementRefId")
                        .HasColumnName("RENTAL_AGREEMENT_REF_ID");

                    b.Property<DateTime?>("WorkedDate")
                        .HasColumnName("WORKED_DATE");

                    b.HasKey("Id");

                    b.HasIndex("RentalAgreementRefId");

                    b.ToTable("HET_TIME_RECORD");
                });

            modelBuilder.Entity("HETSAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("USER_ID");

                    b.Property<bool>("Active")
                        .HasColumnName("ACTIVE");

                    b.Property<string>("Email")
                        .HasColumnName("EMAIL")
                        .HasMaxLength(255);

                    b.Property<string>("GivenName")
                        .HasColumnName("GIVEN_NAME")
                        .HasMaxLength(255);

                    b.Property<string>("Guid")
                        .HasColumnName("GUID")
                        .HasMaxLength(255);

                    b.Property<string>("Initials")
                        .HasColumnName("INITIALS")
                        .HasMaxLength(255);

                    b.Property<string>("SmAuthorizationDirectory")
                        .HasColumnName("SM_AUTHORIZATION_DIRECTORY")
                        .HasMaxLength(255);

                    b.Property<string>("SmUserId")
                        .HasColumnName("SM_USER_ID")
                        .HasMaxLength(255);

                    b.Property<string>("Surname")
                        .HasColumnName("SURNAME")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("HET_USER");
                });

            modelBuilder.Entity("HETSAPI.Models.UserFavourite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("USER_FAVOURITE_ID");

                    b.Property<bool?>("IsDefault")
                        .HasColumnName("IS_DEFAULT");

                    b.Property<string>("Name")
                        .HasColumnName("NAME")
                        .HasMaxLength(255);

                    b.Property<string>("Type")
                        .HasColumnName("TYPE");

                    b.Property<int?>("UserRefId")
                        .HasColumnName("USER_REF_ID");

                    b.Property<string>("Value")
                        .HasColumnName("VALUE")
                        .HasMaxLength(2048);

                    b.HasKey("Id");

                    b.HasIndex("UserRefId");

                    b.ToTable("HET_USER_FAVOURITE");
                });

            modelBuilder.Entity("HETSAPI.Models.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("USER_ROLE_ID");

                    b.Property<DateTime>("EffectiveDate")
                        .HasColumnName("EFFECTIVE_DATE");

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnName("EXPIRY_DATE");

                    b.Property<int?>("RoleRefId")
                        .HasColumnName("ROLE_REF_ID");

                    b.Property<int?>("UserRefId")
                        .HasColumnName("USER_REF_ID");

                    b.HasKey("Id");

                    b.HasIndex("RoleRefId");

                    b.HasIndex("UserRefId");

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

                    b.HasOne("HETSAPI.Models.Request")
                        .WithMany("Attachments")
                        .HasForeignKey("RequestId");
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

            modelBuilder.Entity("HETSAPI.Models.ContactAddress", b =>
                {
                    b.HasOne("HETSAPI.Models.Contact")
                        .WithMany("Addresses")
                        .HasForeignKey("ContactId");
                });

            modelBuilder.Entity("HETSAPI.Models.ContactPhone", b =>
                {
                    b.HasOne("HETSAPI.Models.Contact")
                        .WithMany("Phones")
                        .HasForeignKey("ContactId");
                });

            modelBuilder.Entity("HETSAPI.Models.District", b =>
                {
                    b.HasOne("HETSAPI.Models.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.Equipment", b =>
                {
                    b.HasOne("HETSAPI.Models.DumpTruck", "DumpTruckDetails")
                        .WithMany()
                        .HasForeignKey("DumpTruckDetailsRefId");

                    b.HasOne("HETSAPI.Models.EquipmentType", "EquipmentType")
                        .WithMany()
                        .HasForeignKey("EquipmentTypeRefId");

                    b.HasOne("HETSAPI.Models.LocalArea", "LocalArea")
                        .WithMany()
                        .HasForeignKey("LocalAreaRefId");

                    b.HasOne("HETSAPI.Models.Owner", "Owner")
                        .WithMany("EquipmentList")
                        .HasForeignKey("OwnerRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.EquipmentAttachment", b =>
                {
                    b.HasOne("HETSAPI.Models.Equipment", "Equipment")
                        .WithMany("EquipmentAttachments")
                        .HasForeignKey("EquipmentRefId");

                    b.HasOne("HETSAPI.Models.EquipmentAttachmentType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.EquipmentType", b =>
                {
                    b.HasOne("HETSAPI.Models.LocalArea", "LocalArea")
                        .WithMany()
                        .HasForeignKey("LocalAreaRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.GroupMembership", b =>
                {
                    b.HasOne("HETSAPI.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupRefId");

                    b.HasOne("HETSAPI.Models.User", "User")
                        .WithMany("GroupMemberships")
                        .HasForeignKey("UserRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.HireOffer", b =>
                {
                    b.HasOne("HETSAPI.Models.Equipment", "Equipment")
                        .WithMany()
                        .HasForeignKey("EquipmentRefId");

                    b.HasOne("HETSAPI.Models.RentalAgreement", "RentalAgreement")
                        .WithMany()
                        .HasForeignKey("RentalAgreementRefId");

                    b.HasOne("HETSAPI.Models.Request", "Request")
                        .WithMany("HireOffers")
                        .HasForeignKey("RequestRefId");
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

                    b.HasOne("HETSAPI.Models.Request")
                        .WithMany("History")
                        .HasForeignKey("RequestId");
                });

            modelBuilder.Entity("HETSAPI.Models.LocalArea", b =>
                {
                    b.HasOne("HETSAPI.Models.ServiceArea", "ServiceArea")
                        .WithMany()
                        .HasForeignKey("ServiceAreaRefId");
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

                    b.HasOne("HETSAPI.Models.Request")
                        .WithMany("Notes")
                        .HasForeignKey("RequestId");
                });

            modelBuilder.Entity("HETSAPI.Models.Owner", b =>
                {
                    b.HasOne("HETSAPI.Models.LocalArea", "LocalArea")
                        .WithMany()
                        .HasForeignKey("LocalAreaRefId");

                    b.HasOne("HETSAPI.Models.Contact", "PrimaryContact")
                        .WithMany()
                        .HasForeignKey("PrimaryContactRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.Project", b =>
                {
                    b.HasOne("HETSAPI.Models.Contact", "PrimaryContact")
                        .WithMany()
                        .HasForeignKey("PrimaryContactRefId");

                    b.HasOne("HETSAPI.Models.ServiceArea", "ServiceArea")
                        .WithMany()
                        .HasForeignKey("ServiceAreaRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.RentalAgreement", b =>
                {
                    b.HasOne("HETSAPI.Models.Equipment", "Equipment")
                        .WithMany()
                        .HasForeignKey("EquipmentRefId");

                    b.HasOne("HETSAPI.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.Request", b =>
                {
                    b.HasOne("HETSAPI.Models.EquipmentType", "EquipmentType")
                        .WithMany()
                        .HasForeignKey("EquipmentTypeRefId");

                    b.HasOne("HETSAPI.Models.LocalArea", "LocalArea")
                        .WithMany()
                        .HasForeignKey("LocalAreaRefId");

                    b.HasOne("HETSAPI.Models.Project", "Project")
                        .WithMany("Requests")
                        .HasForeignKey("ProjectRefId");

                    b.HasOne("HETSAPI.Models.RotationList", "RotationList")
                        .WithMany()
                        .HasForeignKey("RotationListRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.RolePermission", b =>
                {
                    b.HasOne("HETSAPI.Models.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionRefId");

                    b.HasOne("HETSAPI.Models.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.RotationList", b =>
                {
                    b.HasOne("HETSAPI.Models.EquipmentType", "EquipmentType")
                        .WithMany()
                        .HasForeignKey("EquipmentTypeRefId");

                    b.HasOne("HETSAPI.Models.LocalArea", "LocalArea")
                        .WithMany()
                        .HasForeignKey("LocalAreaRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.RotationListBlock", b =>
                {
                    b.HasOne("HETSAPI.Models.Equipment", "LastHiredEquipment")
                        .WithMany()
                        .HasForeignKey("LastHiredEquipmentRefId");

                    b.HasOne("HETSAPI.Models.RotationList", "RotationList")
                        .WithMany("Blocks")
                        .HasForeignKey("RotationListRefId");

                    b.HasOne("HETSAPI.Models.Equipment", "StartCycleEquipment")
                        .WithMany()
                        .HasForeignKey("StartCycleEquipmentRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.SeniorityAudit", b =>
                {
                    b.HasOne("HETSAPI.Models.Equipment", "Equipment")
                        .WithMany("SeniorityAudit")
                        .HasForeignKey("EquipmentRefId");

                    b.HasOne("HETSAPI.Models.LocalArea", "LocalArea")
                        .WithMany()
                        .HasForeignKey("LocalAreaRefId");

                    b.HasOne("HETSAPI.Models.Owner", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerRefId");

                    b.HasOne("HETSAPI.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.ServiceArea", b =>
                {
                    b.HasOne("HETSAPI.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.TimeRecord", b =>
                {
                    b.HasOne("HETSAPI.Models.RentalAgreement", "RentalAgreement")
                        .WithMany("TimeRecords")
                        .HasForeignKey("RentalAgreementRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.UserFavourite", b =>
                {
                    b.HasOne("HETSAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.UserRole", b =>
                {
                    b.HasOne("HETSAPI.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleRefId");

                    b.HasOne("HETSAPI.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserRefId");
                });
        }
    }
}
