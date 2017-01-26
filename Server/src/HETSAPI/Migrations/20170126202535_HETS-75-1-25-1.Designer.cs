using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HETSAPI.Models;

namespace HETSAPI.Migrations
{
    [DbContext(typeof(DbAppContext))]
    [Migration("20170126202535_HETS-75-1-25-1")]
    partial class HETS751251
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("HETSAPI.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CITY_ID");

                    b.Property<string>("Name")
                        .HasColumnName("NAME");

                    b.HasKey("Id");

                    b.ToTable("HET_CITY");
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
                        .HasColumnName("NAME");

                    b.Property<int?>("RegionRefId")
                        .HasColumnName("REGION_REF_ID");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnName("START_DATE");

                    b.HasKey("Id");

                    b.HasIndex("RegionRefId");

                    b.ToTable("HET_DISTRICT");
                });

            modelBuilder.Entity("HETSAPI.Models.FavouriteContextType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("FAVOURITE_CONTEXT_TYPE_ID");

                    b.Property<string>("Name")
                        .HasColumnName("NAME");

                    b.HasKey("Id");

                    b.ToTable("HET_FAVOURITE_CONTEXT_TYPE");
                });

            modelBuilder.Entity("HETSAPI.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("GROUP_ID");

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION");

                    b.Property<string>("Name")
                        .HasColumnName("NAME");

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

            modelBuilder.Entity("HETSAPI.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("NOTIFICATION_ID");

                    b.Property<int?>("Event2RefId")
                        .HasColumnName("EVENT2_REF_ID");

                    b.Property<int?>("EventRefId")
                        .HasColumnName("EVENT_REF_ID");

                    b.Property<bool?>("HasBeenViewed")
                        .HasColumnName("HAS_BEEN_VIEWED");

                    b.Property<bool?>("IsAllDay")
                        .HasColumnName("IS_ALL_DAY");

                    b.Property<bool?>("IsExpired")
                        .HasColumnName("IS_EXPIRED");

                    b.Property<bool?>("IsWatchNotification")
                        .HasColumnName("IS_WATCH_NOTIFICATION");

                    b.Property<string>("PriorityCode")
                        .HasColumnName("PRIORITY_CODE");

                    b.Property<int?>("UserRefId")
                        .HasColumnName("USER_REF_ID");

                    b.HasKey("Id");

                    b.HasIndex("Event2RefId");

                    b.HasIndex("EventRefId");

                    b.HasIndex("UserRefId");

                    b.ToTable("HET_NOTIFICATION");
                });

            modelBuilder.Entity("HETSAPI.Models.NotificationEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("NOTIFICATION_EVENT_ID");

                    b.Property<string>("EventSubTypeCode")
                        .HasColumnName("EVENT_SUB_TYPE_CODE");

                    b.Property<string>("EventTime")
                        .HasColumnName("EVENT_TIME");

                    b.Property<string>("EventTypeCode")
                        .HasColumnName("EVENT_TYPE_CODE");

                    b.Property<string>("Notes")
                        .HasColumnName("NOTES");

                    b.Property<bool?>("NotificationGenerated")
                        .HasColumnName("NOTIFICATION_GENERATED");

                    b.HasKey("Id");

                    b.ToTable("HET_NOTIFICATION_EVENT");
                });

            modelBuilder.Entity("HETSAPI.Models.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PERMISSION_ID");

                    b.Property<string>("Code")
                        .HasColumnName("CODE");

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION");

                    b.Property<string>("Name")
                        .HasColumnName("NAME");

                    b.HasKey("Id");

                    b.ToTable("HET_PERMISSION");
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
                        .HasColumnName("NAME");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnName("START_DATE");

                    b.HasKey("Id");

                    b.ToTable("HET_REGION");
                });

            modelBuilder.Entity("HETSAPI.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ROLE_ID");

                    b.Property<string>("Description")
                        .HasColumnName("DESCRIPTION");

                    b.Property<string>("Name")
                        .HasColumnName("NAME");

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
                        .HasColumnName("NAME");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnName("START_DATE");

                    b.HasKey("Id");

                    b.HasIndex("DistrictRefId");

                    b.ToTable("HET_SERVICE_AREA");
                });

            modelBuilder.Entity("HETSAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("USER_ID");

                    b.Property<bool>("Active")
                        .HasColumnName("ACTIVE");

                    b.Property<string>("Email")
                        .HasColumnName("EMAIL");

                    b.Property<string>("GivenName")
                        .HasColumnName("GIVEN_NAME");

                    b.Property<string>("Guid")
                        .HasColumnName("GUID");

                    b.Property<string>("Initials")
                        .HasColumnName("INITIALS");

                    b.Property<string>("SmAuthorizationDirectory")
                        .HasColumnName("SM_AUTHORIZATION_DIRECTORY");

                    b.Property<string>("SmUserId")
                        .HasColumnName("SM_USER_ID");

                    b.Property<string>("Surname")
                        .HasColumnName("SURNAME");

                    b.HasKey("Id");

                    b.ToTable("HET_USER");
                });

            modelBuilder.Entity("HETSAPI.Models.UserFavourite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("USER_FAVOURITE_ID");

                    b.Property<int?>("FavouriteContextTypeRefId")
                        .HasColumnName("FAVOURITE_CONTEXT_TYPE_REF_ID");

                    b.Property<bool?>("IsDefault")
                        .HasColumnName("IS_DEFAULT");

                    b.Property<string>("Name")
                        .HasColumnName("NAME");

                    b.Property<string>("Value")
                        .HasColumnName("VALUE");

                    b.HasKey("Id");

                    b.HasIndex("FavouriteContextTypeRefId");

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

            modelBuilder.Entity("HETSAPI.Models.District", b =>
                {
                    b.HasOne("HETSAPI.Models.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionRefId");
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

            modelBuilder.Entity("HETSAPI.Models.Notification", b =>
                {
                    b.HasOne("HETSAPI.Models.NotificationEvent", "Event2")
                        .WithMany()
                        .HasForeignKey("Event2RefId");

                    b.HasOne("HETSAPI.Models.NotificationEvent", "Event")
                        .WithMany()
                        .HasForeignKey("EventRefId");

                    b.HasOne("HETSAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserRefId");
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

            modelBuilder.Entity("HETSAPI.Models.ServiceArea", b =>
                {
                    b.HasOne("HETSAPI.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictRefId");
                });

            modelBuilder.Entity("HETSAPI.Models.UserFavourite", b =>
                {
                    b.HasOne("HETSAPI.Models.FavouriteContextType", "FavouriteContextType")
                        .WithMany()
                        .HasForeignKey("FavouriteContextTypeRefId");
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
