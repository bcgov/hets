using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

#nullable disable

namespace HetsData.Entities
{
    public partial class DbAppContext : DbContext
    {
        private readonly ILogger<DbAppContext> _logger;

        public DbAppContext()
        {
        }

        public DbAppContext(DbContextOptions<DbAppContext> options, ILogger<DbAppContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        public virtual DbSet<HetBatchReport> HetBatchReports { get; set; }
        public virtual DbSet<HetBusiness> HetBusinesses { get; set; }
        public virtual DbSet<HetBusinessUser> HetBusinessUsers { get; set; }
        public virtual DbSet<HetBusinessUserRole> HetBusinessUserRoles { get; set; }
        public virtual DbSet<HetConditionType> HetConditionTypes { get; set; }
        public virtual DbSet<HetContact> HetContacts { get; set; }
        public virtual DbSet<HetDigitalFile> HetDigitalFiles { get; set; }
        public virtual DbSet<HetDistrict> HetDistricts { get; set; }
        public virtual DbSet<HetDistrictEquipmentType> HetDistrictEquipmentTypes { get; set; }
        public virtual DbSet<HetDistrictStatus> HetDistrictStatuses { get; set; }
        public virtual DbSet<HetEquipment> HetEquipments { get; set; }
        public virtual DbSet<HetEquipmentAttachment> HetEquipmentAttachments { get; set; }
        public virtual DbSet<HetEquipmentAttachmentHist> HetEquipmentAttachmentHists { get; set; }
        public virtual DbSet<HetEquipmentHist> HetEquipmentHists { get; set; }
        public virtual DbSet<HetEquipmentStatusType> HetEquipmentStatusTypes { get; set; }
        public virtual DbSet<HetEquipmentType> HetEquipmentTypes { get; set; }
        public virtual DbSet<HetHistory> HetHistories { get; set; }
        public virtual DbSet<HetLocalArea> HetLocalAreas { get; set; }
        public virtual DbSet<HetLog> HetLogs { get; set; }
        public virtual DbSet<HetMimeType> HetMimeTypes { get; set; }
        public virtual DbSet<HetNote> HetNotes { get; set; }
        public virtual DbSet<HetNoteHist> HetNoteHists { get; set; }
        public virtual DbSet<HetOwner> HetOwners { get; set; }
        public virtual DbSet<HetOwnerStatusType> HetOwnerStatusTypes { get; set; }
        public virtual DbSet<HetPermission> HetPermissions { get; set; }
        public virtual DbSet<HetProject> HetProjects { get; set; }
        public virtual DbSet<HetProjectStatusType> HetProjectStatusTypes { get; set; }
        public virtual DbSet<HetProvincialRateType> HetProvincialRateTypes { get; set; }
        public virtual DbSet<HetRatePeriodType> HetRatePeriodTypes { get; set; }
        public virtual DbSet<HetRegion> HetRegions { get; set; }
        public virtual DbSet<HetRentalAgreement> HetRentalAgreements { get; set; }
        public virtual DbSet<HetRentalAgreementCondition> HetRentalAgreementConditions { get; set; }
        public virtual DbSet<HetRentalAgreementConditionHist> HetRentalAgreementConditionHists { get; set; }
        public virtual DbSet<HetRentalAgreementHist> HetRentalAgreementHists { get; set; }
        public virtual DbSet<HetRentalAgreementRate> HetRentalAgreementRates { get; set; }
        public virtual DbSet<HetRentalAgreementRateHist> HetRentalAgreementRateHists { get; set; }
        public virtual DbSet<HetRentalAgreementStatusType> HetRentalAgreementStatusTypes { get; set; }
        public virtual DbSet<HetRentalRequest> HetRentalRequests { get; set; }
        public virtual DbSet<HetRentalRequestAttachment> HetRentalRequestAttachments { get; set; }
        public virtual DbSet<HetRentalRequestRotationList> HetRentalRequestRotationLists { get; set; }
        public virtual DbSet<HetRentalRequestRotationListHist> HetRentalRequestRotationListHists { get; set; }
        public virtual DbSet<HetRentalRequestSeniorityList> HetRentalRequestSeniorityLists { get; set; }
        public virtual DbSet<HetRentalRequestStatusType> HetRentalRequestStatusTypes { get; set; }
        public virtual DbSet<HetRole> HetRoles { get; set; }
        public virtual DbSet<HetRolePermission> HetRolePermissions { get; set; }
        public virtual DbSet<HetRolloverProgress> HetRolloverProgresses { get; set; }
        public virtual DbSet<HetSeniorityAudit> HetSeniorityAudits { get; set; }
        public virtual DbSet<HetServiceArea> HetServiceAreas { get; set; }
        public virtual DbSet<HetTimePeriodType> HetTimePeriodTypes { get; set; }
        public virtual DbSet<HetTimeRecord> HetTimeRecords { get; set; }
        public virtual DbSet<HetTimeRecordHist> HetTimeRecordHists { get; set; }
        public virtual DbSet<HetUser> HetUsers { get; set; }
        public virtual DbSet<HetUserDistrict> HetUserDistricts { get; set; }
        public virtual DbSet<HetUserFavourite> HetUserFavourites { get; set; }
        public virtual DbSet<HetUserRole> HetUserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<HetBatchReport>(entity =>
            {
                entity.HasKey(e => e.ReportId);

                entity.ToTable("HET_BATCH_REPORT");

                entity.HasIndex(e => e.DistrictId, "IX_HET_BATCH_REPORT_DISTRICT_ID");

                entity.Property(e => e.ReportId)
                    .HasColumnName("REPORT_ID")
                    .HasDefaultValueSql("nextval('\"HET_BATCH_REPORT_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.Complete).HasColumnName("COMPLETE");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.ReportLink)
                    .HasMaxLength(500)
                    .HasColumnName("REPORT_LINK");

                entity.Property(e => e.ReportName)
                    .HasMaxLength(100)
                    .HasColumnName("REPORT_NAME");

                entity.Property(e => e.StartDate).HasColumnName("START_DATE");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetBatchReports)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_BATCH_REPORT_DISTRICT_ID");
            });

            modelBuilder.Entity<HetBusiness>(entity =>
            {
                entity.HasKey(e => e.BusinessId);

                entity.ToTable("HET_BUSINESS");

                entity.HasIndex(e => e.BceidBusinessGuid, "HET_BUSINESS_GUID_UK")
                    .IsUnique();

                entity.HasIndex(e => e.BceidBusinessGuid, "IX_HET_BUSINESS_BUSINESS_GUID");

                entity.Property(e => e.BusinessId)
                    .HasColumnName("BUSINESS_ID")
                    .HasDefaultValueSql("nextval('\"HET_BUSINESS_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.BceidBusinessGuid)
                    .HasMaxLength(50)
                    .HasColumnName("BCEID_BUSINESS_GUID");

                entity.Property(e => e.BceidBusinessNumber)
                    .HasMaxLength(50)
                    .HasColumnName("BCEID_BUSINESS_NUMBER");

                entity.Property(e => e.BceidDoingBusinessAs)
                    .HasMaxLength(150)
                    .HasColumnName("BCEID_DOING_BUSINESS_AS");

                entity.Property(e => e.BceidLegalName)
                    .HasMaxLength(150)
                    .HasColumnName("BCEID_LEGAL_NAME");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");
            });

            modelBuilder.Entity<HetBusinessUser>(entity =>
            {
                entity.HasKey(e => e.BusinessUserId);

                entity.ToTable("HET_BUSINESS_USER");

                entity.HasIndex(e => e.BusinessId, "IX_HET_BUSINESS_USER_BUSINESS_ID");

                entity.HasIndex(e => e.BceidGuid, "IX_HET_BUSINESS_USER_GUID");

                entity.Property(e => e.BusinessUserId)
                    .HasColumnName("BUSINESS_USER_ID")
                    .HasDefaultValueSql("nextval('\"HET_BUSINESS_USER_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.BceidDisplayName)
                    .HasMaxLength(150)
                    .HasColumnName("BCEID_DISPLAY_NAME");

                entity.Property(e => e.BceidEmail)
                    .HasMaxLength(150)
                    .HasColumnName("BCEID_EMAIL");

                entity.Property(e => e.BceidFirstName)
                    .HasMaxLength(150)
                    .HasColumnName("BCEID_FIRST_NAME");

                entity.Property(e => e.BceidGuid)
                    .HasMaxLength(50)
                    .HasColumnName("BCEID_GUID");

                entity.Property(e => e.BceidLastName)
                    .HasMaxLength(150)
                    .HasColumnName("BCEID_LAST_NAME");

                entity.Property(e => e.BceidTelephone)
                    .HasMaxLength(150)
                    .HasColumnName("BCEID_TELEPHONE");

                entity.Property(e => e.BceidUserId)
                    .HasMaxLength(150)
                    .HasColumnName("BCEID_USER_ID");

                entity.Property(e => e.BusinessId).HasColumnName("BUSINESS_ID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.HetBusinessUsers)
                    .HasForeignKey(d => d.BusinessId)
                    .HasConstraintName("FK_HET_BUSINESS_USER_BUSINESS_ID");
            });

            modelBuilder.Entity<HetBusinessUserRole>(entity =>
            {
                entity.HasKey(e => e.BusinessUserRoleId)
                    .HasName("PK_PK_HET_BUSINESS_USER_ROLE");

                entity.ToTable("HET_BUSINESS_USER_ROLE");

                entity.HasIndex(e => e.RoleId, "IX_HET_BUSINESS_USER_ROLE_ROLE_ID");

                entity.HasIndex(e => e.BusinessUserId, "IX_HET_BUSINESS_USER_ROLE_USER_ID");

                entity.Property(e => e.BusinessUserRoleId)
                    .HasColumnName("BUSINESS_USER_ROLE_ID")
                    .HasDefaultValueSql("nextval('\"HET_BUSINESS_USER_ROLE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.BusinessUserId).HasColumnName("BUSINESS_USER_ID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.ExpiryDate).HasColumnName("EXPIRY_DATE");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.HasOne(d => d.BusinessUser)
                    .WithMany(p => p.HetBusinessUserRoles)
                    .HasForeignKey(d => d.BusinessUserId)
                    .HasConstraintName("FK_HET_BUSINESS_USER_ROLE_USER_ID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.HetBusinessUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_HET_BUSINESS_USER_ROLE_ROLE_ID");
            });

            modelBuilder.Entity<HetConditionType>(entity =>
            {
                entity.HasKey(e => e.ConditionTypeId);

                entity.ToTable("HET_CONDITION_TYPE");

                entity.HasIndex(e => e.DistrictId, "IX_HET_CONDITION_TYPE_DISTRICT_ID");

                entity.Property(e => e.ConditionTypeId)
                    .HasColumnName("CONDITION_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_CONDITION_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.Active).HasColumnName("ACTIVE");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.ConditionTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("CONDITION_TYPE_CODE");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(2048)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetConditionTypes)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_HET_CONDITION_TYPE_DISTRICT_ID");
            });

            modelBuilder.Entity<HetContact>(entity =>
            {
                entity.HasKey(e => e.ContactId);

                entity.ToTable("HET_CONTACT");

                entity.HasIndex(e => e.OwnerId, "IX_HET_CONTACT_OWNER_ID");

                entity.HasIndex(e => e.ProjectId, "IX_HET_CONTACT_PROJECT_ID");

                entity.Property(e => e.ContactId)
                    .HasColumnName("CONTACT_ID")
                    .HasDefaultValueSql("nextval('\"HET_CONTACT_ID_seq\"'::regclass)");

                entity.Property(e => e.Address1)
                    .HasMaxLength(80)
                    .HasColumnName("ADDRESS1");

                entity.Property(e => e.Address2)
                    .HasMaxLength(80)
                    .HasColumnName("ADDRESS2");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .HasColumnName("CITY");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(255)
                    .HasColumnName("EMAIL_ADDRESS");

                entity.Property(e => e.FaxPhoneNumber)
                    .HasMaxLength(20)
                    .HasColumnName("FAX_PHONE_NUMBER");

                entity.Property(e => e.GivenName)
                    .HasMaxLength(50)
                    .HasColumnName("GIVEN_NAME");

                entity.Property(e => e.MobilePhoneNumber)
                    .HasMaxLength(20)
                    .HasColumnName("MOBILE_PHONE_NUMBER");

                entity.Property(e => e.Notes)
                    .HasMaxLength(512)
                    .HasColumnName("NOTES");

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(15)
                    .HasColumnName("POSTAL_CODE");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.Province)
                    .HasMaxLength(50)
                    .HasColumnName("PROVINCE");

                entity.Property(e => e.Role)
                    .HasMaxLength(100)
                    .HasColumnName("ROLE");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .HasColumnName("SURNAME");

                entity.Property(e => e.WorkPhoneNumber)
                    .HasMaxLength(20)
                    .HasColumnName("WORK_PHONE_NUMBER");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetContacts)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_HET_CONTACT_OWNER_ID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetContacts)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_HET_CONTACT_PROJECT_ID");
            });

            modelBuilder.Entity<HetDigitalFile>(entity =>
            {
                entity.HasKey(e => e.DigitalFileId);

                entity.ToTable("HET_DIGITAL_FILE");

                entity.HasIndex(e => e.EquipmentId, "IX_HET_DIGITAL_FILE_EQUIPMENT_ID");

                entity.HasIndex(e => e.MimeTypeId, "IX_HET_DIGITAL_FILE_MIME_TYPE_ID");

                entity.HasIndex(e => e.OwnerId, "IX_HET_DIGITAL_FILE_OWNER_ID");

                entity.HasIndex(e => e.ProjectId, "IX_HET_DIGITAL_FILE_PROJECT_ID");

                entity.HasIndex(e => e.RentalRequestId, "IX_HET_DIGITAL_FILE_RENTAL_REQUEST_ID");

                entity.Property(e => e.DigitalFileId)
                    .HasColumnName("DIGITAL_FILE_ID")
                    .HasDefaultValueSql("nextval('\"HET_DIGITAL_FILE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(2048)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.FileContents).HasColumnName("FILE_CONTENTS");

                entity.Property(e => e.FileName)
                    .HasMaxLength(2048)
                    .HasColumnName("FILE_NAME");

                entity.Property(e => e.MimeTypeId).HasColumnName("MIME_TYPE_ID");

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasColumnName("TYPE");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetDigitalFiles)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK_HET_DIGITAL_FILE_EQUIPMENT_ID");

                entity.HasOne(d => d.MimeType)
                    .WithMany(p => p.HetDigitalFiles)
                    .HasForeignKey(d => d.MimeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_DIGITAL_FILE_MIME_TYPE_ID");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetDigitalFiles)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_HET_DIGITAL_FILE_OWNER_ID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetDigitalFiles)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_HET_DIGITAL_FILE_PROJECT_ID");

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetDigitalFiles)
                    .HasForeignKey(d => d.RentalRequestId)
                    .HasConstraintName("FK_HET_DIGITAL_FILE_RENTAL_REQUEST_ID");
            });

            modelBuilder.Entity<HetDistrict>(entity =>
            {
                entity.HasKey(e => e.DistrictId);

                entity.ToTable("HET_DISTRICT");

                entity.HasIndex(e => e.RegionId, "IX_HET_DISTRICT_REGION_ID");

                entity.Property(e => e.DistrictId)
                    .HasColumnName("DISTRICT_ID")
                    .HasDefaultValueSql("nextval('\"HET_DISTRICT_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.DistrictNumber).HasColumnName("DISTRICT_NUMBER");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.MinistryDistrictId).HasColumnName("MINISTRY_DISTRICT_ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .HasColumnName("NAME");

                entity.Property(e => e.RegionId).HasColumnName("REGION_ID");

                entity.Property(e => e.StartDate).HasColumnName("START_DATE");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.HetDistricts)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_HET_DISTRICT_REGION_ID");
            });

            modelBuilder.Entity<HetDistrictEquipmentType>(entity =>
            {
                entity.HasKey(e => e.DistrictEquipmentTypeId);

                entity.ToTable("HET_DISTRICT_EQUIPMENT_TYPE");

                entity.HasIndex(e => e.DistrictId, "IX_HET_DISTRICT_EQUIPMENT_TYPE_DISTRICT_ID");

                entity.HasIndex(e => e.EquipmentTypeId, "IX_HET_DISTRICT_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID");

                entity.Property(e => e.DistrictEquipmentTypeId)
                    .HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_DISTRICT_EQUIPMENT_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.Deleted).HasColumnName("DELETED");

                entity.Property(e => e.DistrictEquipmentName)
                    .HasMaxLength(255)
                    .HasColumnName("DISTRICT_EQUIPMENT_NAME");

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.Property(e => e.EquipmentTypeId).HasColumnName("EQUIPMENT_TYPE_ID");

                entity.Property(e => e.ServiceAreaId).HasColumnName("SERVICE_AREA_ID");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetDistrictEquipmentTypes)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_HET_DISTRICT_EQUIPMENT_TYPE_DISTRICT_ID");

                entity.HasOne(d => d.EquipmentType)
                    .WithMany(p => p.HetDistrictEquipmentTypes)
                    .HasForeignKey(d => d.EquipmentTypeId)
                    .HasConstraintName("FK_HET_DISTRICT_EQUIPMENT_TYPE_ID");
            });

            modelBuilder.Entity<HetDistrictStatus>(entity =>
            {
                entity.HasKey(e => e.DistrictId);

                entity.ToTable("HET_DISTRICT_STATUS");

                entity.HasIndex(e => e.DistrictId, "IX_HET_DISTRICT_STATUS_DISTRICT_ID");

                entity.Property(e => e.DistrictId)
                    .ValueGeneratedNever()
                    .HasColumnName("DISTRICT_ID");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.CurrentFiscalYear).HasColumnName("CURRENT_FISCAL_YEAR");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.DisplayRolloverMessage).HasColumnName("DISPLAY_ROLLOVER_MESSAGE");

                entity.Property(e => e.DistrictEquipmentTypeCompleteCount).HasColumnName("DISTRICT_EQUIPMENT_TYPE_COMPLETE_COUNT");

                entity.Property(e => e.DistrictEquipmentTypeCount).HasColumnName("DISTRICT_EQUIPMENT_TYPE_COUNT");

                entity.Property(e => e.LocalAreaCompleteCount).HasColumnName("LOCAL_AREA_COMPLETE_COUNT");

                entity.Property(e => e.LocalAreaCount).HasColumnName("LOCAL_AREA_COUNT");

                entity.Property(e => e.NextFiscalYear).HasColumnName("NEXT_FISCAL_YEAR");

                entity.Property(e => e.ProgressPercentage).HasColumnName("PROGRESS_PERCENTAGE");

                entity.Property(e => e.RolloverEndDate).HasColumnName("ROLLOVER_END_DATE");

                entity.Property(e => e.RolloverStartDate).HasColumnName("ROLLOVER_START_DATE");

                entity.HasOne(d => d.District)
                    .WithOne(p => p.HetDistrictStatus)
                    .HasForeignKey<HetDistrictStatus>(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_DISTRICT_STATUS_DISTRICT_ID");
            });

            modelBuilder.Entity<HetEquipment>(entity =>
            {
                entity.HasKey(e => e.EquipmentId);

                entity.ToTable("HET_EQUIPMENT");

                entity.HasIndex(e => e.DistrictEquipmentTypeId, "IX_HET_EQUIPMENT_DISTRICT_EQUIPMENT_TYPE_ID");

                entity.HasIndex(e => e.LocalAreaId, "IX_HET_EQUIPMENT_LOCAL_AREA_ID");

                entity.HasIndex(e => e.OwnerId, "IX_HET_EQUIPMENT_OWNER_ID");

                entity.HasIndex(e => e.EquipmentStatusTypeId, "IX_HET_EQUIPMENT_STATUS_TYPE_ID");

                entity.Property(e => e.EquipmentId)
                    .HasColumnName("EQUIPMENT_ID")
                    .HasDefaultValueSql("nextval('\"HET_EQUIPMENT_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ApprovedDate).HasColumnName("APPROVED_DATE");

                entity.Property(e => e.ArchiveCode)
                    .HasMaxLength(50)
                    .HasColumnName("ARCHIVE_CODE");

                entity.Property(e => e.ArchiveDate).HasColumnName("ARCHIVE_DATE");

                entity.Property(e => e.ArchiveReason)
                    .HasMaxLength(2048)
                    .HasColumnName("ARCHIVE_REASON");

                entity.Property(e => e.BlockNumber).HasColumnName("BLOCK_NUMBER");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.DistrictEquipmentTypeId).HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID");

                entity.Property(e => e.EquipmentCode)
                    .HasMaxLength(25)
                    .HasColumnName("EQUIPMENT_CODE");

                entity.Property(e => e.EquipmentStatusTypeId).HasColumnName("EQUIPMENT_STATUS_TYPE_ID");

                entity.Property(e => e.InformationUpdateNeededReason)
                    .HasMaxLength(2048)
                    .HasColumnName("INFORMATION_UPDATE_NEEDED_REASON");

                entity.Property(e => e.IsInformationUpdateNeeded).HasColumnName("IS_INFORMATION_UPDATE_NEEDED");

                entity.Property(e => e.IsSeniorityOverridden).HasColumnName("IS_SENIORITY_OVERRIDDEN");

                entity.Property(e => e.LastVerifiedDate).HasColumnName("LAST_VERIFIED_DATE");

                entity.Property(e => e.LegalCapacity)
                    .HasMaxLength(150)
                    .HasColumnName("LEGAL_CAPACITY");

                entity.Property(e => e.LicencePlate)
                    .HasMaxLength(20)
                    .HasColumnName("LICENCE_PLATE");

                entity.Property(e => e.LicencedGvw)
                    .HasMaxLength(150)
                    .HasColumnName("LICENCED_GVW");

                entity.Property(e => e.LocalAreaId).HasColumnName("LOCAL_AREA_ID");

                entity.Property(e => e.Make)
                    .HasMaxLength(50)
                    .HasColumnName("MAKE");

                entity.Property(e => e.Model)
                    .HasMaxLength(50)
                    .HasColumnName("MODEL");

                entity.Property(e => e.NumberInBlock).HasColumnName("NUMBER_IN_BLOCK");

                entity.Property(e => e.Operator)
                    .HasMaxLength(255)
                    .HasColumnName("OPERATOR");

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.PayRate).HasColumnName("PAY_RATE");

                entity.Property(e => e.PupLegalCapacity)
                    .HasMaxLength(150)
                    .HasColumnName("PUP_LEGAL_CAPACITY");

                entity.Property(e => e.ReceivedDate).HasColumnName("RECEIVED_DATE");

                entity.Property(e => e.RefuseRate)
                    .HasMaxLength(255)
                    .HasColumnName("REFUSE_RATE");

                entity.Property(e => e.Seniority).HasColumnName("SENIORITY");

                entity.Property(e => e.SeniorityEffectiveDate).HasColumnName("SENIORITY_EFFECTIVE_DATE");

                entity.Property(e => e.SeniorityOverrideReason)
                    .HasMaxLength(2048)
                    .HasColumnName("SENIORITY_OVERRIDE_REASON");

                entity.Property(e => e.SerialNumber)
                    .HasMaxLength(100)
                    .HasColumnName("SERIAL_NUMBER");

                entity.Property(e => e.ServiceHoursLastYear).HasColumnName("SERVICE_HOURS_LAST_YEAR");

                entity.Property(e => e.ServiceHoursThreeYearsAgo).HasColumnName("SERVICE_HOURS_THREE_YEARS_AGO");

                entity.Property(e => e.ServiceHoursTwoYearsAgo).HasColumnName("SERVICE_HOURS_TWO_YEARS_AGO");

                entity.Property(e => e.Size)
                    .HasMaxLength(128)
                    .HasColumnName("SIZE");

                entity.Property(e => e.StatusComment)
                    .HasMaxLength(255)
                    .HasColumnName("STATUS_COMMENT");

                entity.Property(e => e.ToDate).HasColumnName("TO_DATE");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("TYPE");

                entity.Property(e => e.Year)
                    .HasMaxLength(15)
                    .HasColumnName("YEAR");

                entity.Property(e => e.YearsOfService).HasColumnName("YEARS_OF_SERVICE");

                entity.HasOne(d => d.DistrictEquipmentType)
                    .WithMany(p => p.HetEquipments)
                    .HasForeignKey(d => d.DistrictEquipmentTypeId)
                    .HasConstraintName("FK_HET_EQUIPMENT_DISTRICT_EQUIPMENT_TYPE_ID");

                entity.HasOne(d => d.EquipmentStatusType)
                    .WithMany(p => p.HetEquipments)
                    .HasForeignKey(d => d.EquipmentStatusTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_EQUIPMENT_STATUS_TYPE_ID");

                entity.HasOne(d => d.LocalArea)
                    .WithMany(p => p.HetEquipments)
                    .HasForeignKey(d => d.LocalAreaId)
                    .HasConstraintName("FK_HET_EQUIPMENT_LOCAL_AREA_ID");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetEquipments)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_HET_EQUIPMENT_OWNER_ID");
            });

            modelBuilder.Entity<HetEquipmentAttachment>(entity =>
            {
                entity.HasKey(e => e.EquipmentAttachmentId);

                entity.ToTable("HET_EQUIPMENT_ATTACHMENT");

                entity.HasIndex(e => e.EquipmentId, "IX_HET_EQUIPMENT_ATTACHMENT_EQUIPMENT_ID");

                entity.Property(e => e.EquipmentAttachmentId)
                    .HasColumnName("EQUIPMENT_ATTACHMENT_ID")
                    .HasDefaultValueSql("nextval('\"HET_EQUIPMENT_ATTACHMENT_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(2048)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(100)
                    .HasColumnName("TYPE_NAME");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetEquipmentAttachments)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK_HET_EQUIPMENT_ATTACHMENT_EQUIPMENT_ID");
            });

            modelBuilder.Entity<HetEquipmentAttachmentHist>(entity =>
            {
                entity.HasKey(e => e.EquipmentAttachmentHistId)
                    .HasName("HET_EQUIPMENT_ATTACHMENT_HIST_PK");

                entity.ToTable("HET_EQUIPMENT_ATTACHMENT_HIST");

                entity.Property(e => e.EquipmentAttachmentHistId)
                    .HasColumnName("EQUIPMENT_ATTACHMENT_HIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_EQUIPMENT_ATTACHMENT_HIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(2048)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.EquipmentAttachmentId).HasColumnName("EQUIPMENT_ATTACHMENT_ID");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(100)
                    .HasColumnName("TYPE_NAME");
            });

            modelBuilder.Entity<HetEquipmentHist>(entity =>
            {
                entity.HasKey(e => e.EquipmentHistId)
                    .HasName("HET_EQUIPMENT_HIST_PK");

                entity.ToTable("HET_EQUIPMENT_HIST");

                entity.Property(e => e.EquipmentHistId)
                    .HasColumnName("EQUIPMENT_HIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_EQUIPMENT_HIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ApprovedDate).HasColumnName("APPROVED_DATE");

                entity.Property(e => e.ArchiveCode)
                    .HasMaxLength(50)
                    .HasColumnName("ARCHIVE_CODE");

                entity.Property(e => e.ArchiveDate).HasColumnName("ARCHIVE_DATE");

                entity.Property(e => e.ArchiveReason)
                    .HasMaxLength(2048)
                    .HasColumnName("ARCHIVE_REASON");

                entity.Property(e => e.BlockNumber).HasColumnName("BLOCK_NUMBER");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.DistrictEquipmentTypeId).HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID");

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.EquipmentCode)
                    .HasMaxLength(25)
                    .HasColumnName("EQUIPMENT_CODE");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.EquipmentStatusTypeId).HasColumnName("EQUIPMENT_STATUS_TYPE_ID");

                entity.Property(e => e.InformationUpdateNeededReason)
                    .HasMaxLength(2048)
                    .HasColumnName("INFORMATION_UPDATE_NEEDED_REASON");

                entity.Property(e => e.IsInformationUpdateNeeded).HasColumnName("IS_INFORMATION_UPDATE_NEEDED");

                entity.Property(e => e.IsSeniorityOverridden).HasColumnName("IS_SENIORITY_OVERRIDDEN");

                entity.Property(e => e.LastVerifiedDate).HasColumnName("LAST_VERIFIED_DATE");

                entity.Property(e => e.LegalCapacity)
                    .HasMaxLength(150)
                    .HasColumnName("LEGAL_CAPACITY");

                entity.Property(e => e.LicencePlate)
                    .HasMaxLength(20)
                    .HasColumnName("LICENCE_PLATE");

                entity.Property(e => e.LicencedGvw)
                    .HasMaxLength(150)
                    .HasColumnName("LICENCED_GVW");

                entity.Property(e => e.LocalAreaId).HasColumnName("LOCAL_AREA_ID");

                entity.Property(e => e.Make)
                    .HasMaxLength(50)
                    .HasColumnName("MAKE");

                entity.Property(e => e.Model)
                    .HasMaxLength(50)
                    .HasColumnName("MODEL");

                entity.Property(e => e.NumberInBlock).HasColumnName("NUMBER_IN_BLOCK");

                entity.Property(e => e.Operator)
                    .HasMaxLength(255)
                    .HasColumnName("OPERATOR");

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.PayRate).HasColumnName("PAY_RATE");

                entity.Property(e => e.PupLegalCapacity)
                    .HasMaxLength(150)
                    .HasColumnName("PUP_LEGAL_CAPACITY");

                entity.Property(e => e.ReceivedDate).HasColumnName("RECEIVED_DATE");

                entity.Property(e => e.RefuseRate)
                    .HasMaxLength(255)
                    .HasColumnName("REFUSE_RATE");

                entity.Property(e => e.Seniority).HasColumnName("SENIORITY");

                entity.Property(e => e.SeniorityEffectiveDate).HasColumnName("SENIORITY_EFFECTIVE_DATE");

                entity.Property(e => e.SeniorityOverrideReason)
                    .HasMaxLength(2048)
                    .HasColumnName("SENIORITY_OVERRIDE_REASON");

                entity.Property(e => e.SerialNumber)
                    .HasMaxLength(100)
                    .HasColumnName("SERIAL_NUMBER");

                entity.Property(e => e.ServiceHoursLastYear).HasColumnName("SERVICE_HOURS_LAST_YEAR");

                entity.Property(e => e.ServiceHoursThreeYearsAgo).HasColumnName("SERVICE_HOURS_THREE_YEARS_AGO");

                entity.Property(e => e.ServiceHoursTwoYearsAgo).HasColumnName("SERVICE_HOURS_TWO_YEARS_AGO");

                entity.Property(e => e.Size)
                    .HasMaxLength(128)
                    .HasColumnName("SIZE");

                entity.Property(e => e.StatusComment)
                    .HasMaxLength(255)
                    .HasColumnName("STATUS_COMMENT");

                entity.Property(e => e.ToDate).HasColumnName("TO_DATE");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("TYPE");

                entity.Property(e => e.Year)
                    .HasMaxLength(15)
                    .HasColumnName("YEAR");

                entity.Property(e => e.YearsOfService).HasColumnName("YEARS_OF_SERVICE");
            });

            modelBuilder.Entity<HetEquipmentStatusType>(entity =>
            {
                entity.HasKey(e => e.EquipmentStatusTypeId);

                entity.ToTable("HET_EQUIPMENT_STATUS_TYPE");

                entity.HasIndex(e => e.EquipmentStatusTypeCode, "UK_HET_EQUIPMENT_STATUS_TYPE_CODE")
                    .IsUnique();

                entity.Property(e => e.EquipmentStatusTypeId)
                    .HasColumnName("EQUIPMENT_STATUS_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_EQUIPMENT_STATUS_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.EquipmentStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("EQUIPMENT_STATUS_TYPE_CODE");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.ScreenLabel)
                    .HasMaxLength(200)
                    .HasColumnName("SCREEN_LABEL");
            });

            modelBuilder.Entity<HetEquipmentType>(entity =>
            {
                entity.HasKey(e => e.EquipmentTypeId);

                entity.ToTable("HET_EQUIPMENT_TYPE");

                entity.Property(e => e.EquipmentTypeId)
                    .HasColumnName("EQUIPMENT_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_EQUIPMENT_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.BlueBookRateNumber).HasColumnName("BLUE_BOOK_RATE_NUMBER");

                entity.Property(e => e.BlueBookSection).HasColumnName("BLUE_BOOK_SECTION");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.ExtendHours).HasColumnName("EXTEND_HOURS");

                entity.Property(e => e.IsDumpTruck).HasColumnName("IS_DUMP_TRUCK");

                entity.Property(e => e.MaxHoursSub).HasColumnName("MAX_HOURS_SUB");

                entity.Property(e => e.MaximumHours).HasColumnName("MAXIMUM_HOURS");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .HasColumnName("NAME");

                entity.Property(e => e.NumberOfBlocks).HasColumnName("NUMBER_OF_BLOCKS");
            });

            modelBuilder.Entity<HetHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId);

                entity.ToTable("HET_HISTORY");

                entity.HasIndex(e => e.EquipmentId, "IX_HET_HISTORY_EQUIPMENT_ID");

                entity.HasIndex(e => e.OwnerId, "IX_HET_HISTORY_OWNER_ID");

                entity.HasIndex(e => e.ProjectId, "IX_HET_HISTORY_PROJECT_ID");

                entity.HasIndex(e => e.RentalRequestId, "IX_HET_HISTORY_RENTAL_REQUEST_ID");

                entity.Property(e => e.HistoryId)
                    .HasColumnName("HISTORY_ID")
                    .HasDefaultValueSql("nextval('\"HET_HISTORY_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.CreatedDate).HasColumnName("CREATED_DATE");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.HistoryText)
                    .HasMaxLength(2048)
                    .HasColumnName("HISTORY_TEXT");

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetHistories)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK_HET_HISTORY_EQUIPMENT_ID");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetHistories)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_HET_HISTORY_OWNER_ID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetHistories)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_HET_HISTORY_PROJECT_ID");

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetHistories)
                    .HasForeignKey(d => d.RentalRequestId)
                    .HasConstraintName("FK_HET_HISTORY_RENTAL_REQUEST_ID");
            });

            modelBuilder.Entity<HetLocalArea>(entity =>
            {
                entity.HasKey(e => e.LocalAreaId);

                entity.ToTable("HET_LOCAL_AREA");

                entity.HasIndex(e => e.LocalAreaNumber, "HET_LOCAL_AREA_NUMBER_UK")
                    .IsUnique();

                entity.HasIndex(e => e.ServiceAreaId, "IX_HET_LOCAL_AREA_SERVICE_AREA_ID");

                entity.Property(e => e.LocalAreaId)
                    .HasColumnName("LOCAL_AREA_ID")
                    .HasDefaultValueSql("nextval('\"HET_LOCAL_AREA_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.LocalAreaNumber).HasColumnName("LOCAL_AREA_NUMBER");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .HasColumnName("NAME");

                entity.Property(e => e.ServiceAreaId).HasColumnName("SERVICE_AREA_ID");

                entity.Property(e => e.StartDate)
                    .HasColumnName("START_DATE")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.HasOne(d => d.ServiceArea)
                    .WithMany(p => p.HetLocalAreas)
                    .HasForeignKey(d => d.ServiceAreaId)
                    .HasConstraintName("FK_HET_LOCAL_AREA_SERVICE_AREA_ID");
            });

            modelBuilder.Entity<HetLog>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("het_log");

                entity.Property(e => e.Exception).HasColumnName("exception");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.LogEvent)
                    .HasColumnType("jsonb")
                    .HasColumnName("log_event");

                entity.Property(e => e.MachineName).HasColumnName("machine_name");

                entity.Property(e => e.Message).HasColumnName("message");

                entity.Property(e => e.MessageTemplate).HasColumnName("message_template");

                entity.Property(e => e.PropsTest)
                    .HasColumnType("json")
                    .HasColumnName("props_test");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");
            });

            modelBuilder.Entity<HetMimeType>(entity =>
            {
                entity.HasKey(e => e.MimeTypeId);

                entity.ToTable("HET_MIME_TYPE");

                entity.HasIndex(e => e.MimeTypeCode, "UK_HET_MIME_TYPE_CODE")
                    .IsUnique();

                entity.Property(e => e.MimeTypeId)
                    .HasColumnName("MIME_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_MIME_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.MimeTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("MIME_TYPE_CODE");

                entity.Property(e => e.ScreenLabel)
                    .HasMaxLength(200)
                    .HasColumnName("SCREEN_LABEL");
            });

            modelBuilder.Entity<HetNote>(entity =>
            {
                entity.HasKey(e => e.NoteId);

                entity.ToTable("HET_NOTE");

                entity.HasIndex(e => e.EquipmentId, "IX_HET_NOTE_EQUIPMENT_ID");

                entity.HasIndex(e => e.OwnerId, "IX_HET_NOTE_OWNER_ID");

                entity.HasIndex(e => e.ProjectId, "IX_HET_NOTE_PROJECT_ID");

                entity.HasIndex(e => e.RentalRequestId, "IX_HET_NOTE_RENTAL_REQUEST_ID");

                entity.Property(e => e.NoteId)
                    .HasColumnName("NOTE_ID")
                    .HasDefaultValueSql("nextval('\"HET_NOTE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.IsNoLongerRelevant).HasColumnName("IS_NO_LONGER_RELEVANT");

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.Property(e => e.Text)
                    .HasMaxLength(2048)
                    .HasColumnName("TEXT");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetNotes)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK_HET_NOTE_EQUIPMENT_ID");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetNotes)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_HET_NOTE_OWNER_ID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetNotes)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_HET_NOTE_PROJECT_ID");

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetNotes)
                    .HasForeignKey(d => d.RentalRequestId)
                    .HasConstraintName("FK_HET_NOTE_RENTAL_REQUEST_ID");
            });

            modelBuilder.Entity<HetNoteHist>(entity =>
            {
                entity.HasKey(e => e.NoteHistId)
                    .HasName("HET_NOTE_HIST_PK");

                entity.ToTable("HET_NOTE_HIST");

                entity.Property(e => e.NoteHistId)
                    .HasColumnName("NOTE_HIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_NOTE_HIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.IsNoLongerRelevant).HasColumnName("IS_NO_LONGER_RELEVANT");

                entity.Property(e => e.NoteId).HasColumnName("NOTE_ID");

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.Property(e => e.Text)
                    .HasMaxLength(2048)
                    .HasColumnName("TEXT");
            });

            modelBuilder.Entity<HetOwner>(entity =>
            {
                entity.HasKey(e => e.OwnerId);

                entity.ToTable("HET_OWNER");

                entity.HasIndex(e => e.BusinessId, "IX_HET_OWNER_BUSINESS_ID");

                entity.HasIndex(e => e.LocalAreaId, "IX_HET_OWNER_LOCAL_AREA_ID");

                entity.HasIndex(e => e.PrimaryContactId, "IX_HET_OWNER_PRIMARY_CONTACT_ID");

                entity.HasIndex(e => e.OwnerStatusTypeId, "IX_HET_OWNER_STATUS_TYPE_ID");

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OWNER_ID")
                    .HasDefaultValueSql("nextval('\"HET_OWNER_ID_seq\"'::regclass)");

                entity.Property(e => e.Address1)
                    .HasMaxLength(80)
                    .HasColumnName("ADDRESS1");

                entity.Property(e => e.Address2)
                    .HasMaxLength(80)
                    .HasColumnName("ADDRESS2");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ArchiveCode)
                    .HasMaxLength(50)
                    .HasColumnName("ARCHIVE_CODE");

                entity.Property(e => e.ArchiveDate).HasColumnName("ARCHIVE_DATE");

                entity.Property(e => e.ArchiveReason)
                    .HasMaxLength(2048)
                    .HasColumnName("ARCHIVE_REASON");

                entity.Property(e => e.BusinessId).HasColumnName("BUSINESS_ID");

                entity.Property(e => e.CglCompany)
                    .HasMaxLength(255)
                    .HasColumnName("CGL_COMPANY");

                entity.Property(e => e.CglPolicyNumber)
                    .HasMaxLength(50)
                    .HasColumnName("CGL_POLICY_NUMBER");

                entity.Property(e => e.CglendDate).HasColumnName("CGLEND_DATE");

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .HasColumnName("CITY");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.DoingBusinessAs)
                    .HasMaxLength(150)
                    .HasColumnName("DOING_BUSINESS_AS");

                entity.Property(e => e.GivenName)
                    .HasMaxLength(50)
                    .HasColumnName("GIVEN_NAME");

                entity.Property(e => e.IsMaintenanceContractor).HasColumnName("IS_MAINTENANCE_CONTRACTOR");

                entity.Property(e => e.LocalAreaId).HasColumnName("LOCAL_AREA_ID");

                entity.Property(e => e.MeetsResidency).HasColumnName("MEETS_RESIDENCY");

                entity.Property(e => e.OrganizationName)
                    .HasMaxLength(150)
                    .HasColumnName("ORGANIZATION_NAME");

                entity.Property(e => e.OwnerCode)
                    .HasMaxLength(20)
                    .HasColumnName("OWNER_CODE");

                entity.Property(e => e.OwnerStatusTypeId).HasColumnName("OWNER_STATUS_TYPE_ID");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(15)
                    .HasColumnName("POSTAL_CODE");

                entity.Property(e => e.PrimaryContactId).HasColumnName("PRIMARY_CONTACT_ID");

                entity.Property(e => e.Province)
                    .HasMaxLength(50)
                    .HasColumnName("PROVINCE");

                entity.Property(e => e.RegisteredCompanyNumber)
                    .HasMaxLength(150)
                    .HasColumnName("REGISTERED_COMPANY_NUMBER");

                entity.Property(e => e.SharedKey)
                    .HasMaxLength(50)
                    .HasColumnName("SHARED_KEY");

                entity.Property(e => e.StatusComment)
                    .HasMaxLength(255)
                    .HasColumnName("STATUS_COMMENT");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .HasColumnName("SURNAME");

                entity.Property(e => e.WorkSafeBcexpiryDate).HasColumnName("WORK_SAFE_BCEXPIRY_DATE");

                entity.Property(e => e.WorkSafeBcpolicyNumber)
                    .HasMaxLength(50)
                    .HasColumnName("WORK_SAFE_BCPOLICY_NUMBER");

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.HetOwners)
                    .HasForeignKey(d => d.BusinessId)
                    .HasConstraintName("FK_HET_OWNER_BUSINESS_ID");

                entity.HasOne(d => d.LocalArea)
                    .WithMany(p => p.HetOwners)
                    .HasForeignKey(d => d.LocalAreaId)
                    .HasConstraintName("FK_HET_OWNER_LOCAL_AREA_ID");

                entity.HasOne(d => d.OwnerStatusType)
                    .WithMany(p => p.HetOwners)
                    .HasForeignKey(d => d.OwnerStatusTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_OWNER_STATUS_TYPE_ID");

                entity.HasOne(d => d.PrimaryContact)
                    .WithMany(p => p.HetOwners)
                    .HasForeignKey(d => d.PrimaryContactId)
                    .HasConstraintName("FK_HET_OWNER_PRIMARY_CONTACT_ID");
            });

            modelBuilder.Entity<HetOwnerStatusType>(entity =>
            {
                entity.HasKey(e => e.OwnerStatusTypeId);

                entity.ToTable("HET_OWNER_STATUS_TYPE");

                entity.HasIndex(e => e.OwnerStatusTypeCode, "UK_HET_OWNER_STATUS_TYPE_CODE")
                    .IsUnique();

                entity.Property(e => e.OwnerStatusTypeId)
                    .HasColumnName("OWNER_STATUS_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_OWNER_STATUS_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.OwnerStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("OWNER_STATUS_TYPE_CODE");

                entity.Property(e => e.ScreenLabel)
                    .HasMaxLength(200)
                    .HasColumnName("SCREEN_LABEL");
            });

            modelBuilder.Entity<HetPermission>(entity =>
            {
                entity.HasKey(e => e.PermissionId);

                entity.ToTable("HET_PERMISSION");

                entity.HasIndex(e => e.Code, "HET_PRM_CODE_UK")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "HET_PRM_NAME_UK")
                    .IsUnique();

                entity.Property(e => e.PermissionId)
                    .HasColumnName("PERMISSION_ID")
                    .HasDefaultValueSql("nextval('\"HET_PERMISSION_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .HasColumnName("CODE");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(2048)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<HetProject>(entity =>
            {
                entity.HasKey(e => e.ProjectId);

                entity.ToTable("HET_PROJECT");

                entity.HasIndex(e => e.DistrictId, "IX_HET_PROJECT_DISTRICT_ID");

                entity.HasIndex(e => e.PrimaryContactId, "IX_HET_PROJECT_PRIMARY_CONTACT_ID");

                entity.HasIndex(e => e.ProjectStatusTypeId, "IX_HET_PROJECT_STATUS_TYPE_ID");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("PROJECT_ID")
                    .HasDefaultValueSql("nextval('\"HET_PROJECT_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.BusinessFunction)
                    .HasMaxLength(255)
                    .HasColumnName("BUSINESS_FUNCTION");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.CostType)
                    .HasMaxLength(255)
                    .HasColumnName("COST_TYPE");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.Property(e => e.FiscalYear)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("FISCAL_YEAR")
                    .HasDefaultValueSql("'2018/2019'::character varying");

                entity.Property(e => e.Information)
                    .HasMaxLength(2048)
                    .HasColumnName("INFORMATION");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("NAME");

                entity.Property(e => e.PrimaryContactId).HasColumnName("PRIMARY_CONTACT_ID");

                entity.Property(e => e.Product)
                    .HasMaxLength(255)
                    .HasColumnName("PRODUCT");

                entity.Property(e => e.ProjectStatusTypeId).HasColumnName("PROJECT_STATUS_TYPE_ID");

                entity.Property(e => e.ProvincialProjectNumber)
                    .HasMaxLength(150)
                    .HasColumnName("PROVINCIAL_PROJECT_NUMBER");

                entity.Property(e => e.ResponsibilityCentre)
                    .HasMaxLength(255)
                    .HasColumnName("RESPONSIBILITY_CENTRE");

                entity.Property(e => e.ServiceLine)
                    .HasMaxLength(255)
                    .HasColumnName("SERVICE_LINE");

                entity.Property(e => e.Stob)
                    .HasMaxLength(255)
                    .HasColumnName("STOB");

                entity.Property(e => e.WorkActivity)
                    .HasMaxLength(255)
                    .HasColumnName("WORK_ACTIVITY");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetProjects)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_HET_PROJECT_DISTRICT_ID");

                entity.HasOne(d => d.PrimaryContact)
                    .WithMany(p => p.HetProjects)
                    .HasForeignKey(d => d.PrimaryContactId)
                    .HasConstraintName("FK_HET_PROJECT_PRIMARY_CONTACT_ID");

                entity.HasOne(d => d.ProjectStatusType)
                    .WithMany(p => p.HetProjects)
                    .HasForeignKey(d => d.ProjectStatusTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_PROJECT_HET_PROJECT_STATUS_TYPE_ID");
            });

            modelBuilder.Entity<HetProjectStatusType>(entity =>
            {
                entity.HasKey(e => e.ProjectStatusTypeId);

                entity.ToTable("HET_PROJECT_STATUS_TYPE");

                entity.HasIndex(e => e.ProjectStatusTypeCode, "UK_HET_PROJECT_STATUS_TYPE_CODE")
                    .IsUnique();

                entity.Property(e => e.ProjectStatusTypeId)
                    .HasColumnName("PROJECT_STATUS_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_PROJECT_STATUS_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.ProjectStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROJECT_STATUS_TYPE_CODE");

                entity.Property(e => e.ScreenLabel)
                    .HasMaxLength(200)
                    .HasColumnName("SCREEN_LABEL");
            });

            modelBuilder.Entity<HetProvincialRateType>(entity =>
            {
                entity.HasKey(e => e.RateType);

                entity.ToTable("HET_PROVINCIAL_RATE_TYPE");

                entity.Property(e => e.RateType)
                    .HasMaxLength(20)
                    .HasColumnName("RATE_TYPE");

                entity.Property(e => e.Active).HasColumnName("ACTIVE");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.IsInTotalEditable).HasColumnName("IS_IN_TOTAL_EDITABLE");

                entity.Property(e => e.IsIncludedInTotal).HasColumnName("IS_INCLUDED_IN_TOTAL");

                entity.Property(e => e.IsPercentRate).HasColumnName("IS_PERCENT_RATE");

                entity.Property(e => e.IsRateEditable).HasColumnName("IS_RATE_EDITABLE");

                entity.Property(e => e.Overtime).HasColumnName("OVERTIME");

                entity.Property(e => e.PeriodType)
                    .HasMaxLength(20)
                    .HasColumnName("PERIOD_TYPE");

                entity.Property(e => e.Rate).HasColumnName("RATE");
            });

            modelBuilder.Entity<HetRatePeriodType>(entity =>
            {
                entity.HasKey(e => e.RatePeriodTypeId);

                entity.ToTable("HET_RATE_PERIOD_TYPE");

                entity.HasIndex(e => e.RatePeriodTypeCode, "UK_HET_RATE_PERIOD_TYPE_CODE")
                    .IsUnique();

                entity.Property(e => e.RatePeriodTypeId)
                    .HasColumnName("RATE_PERIOD_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_RATE_PERIOD_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.RatePeriodTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("RATE_PERIOD_TYPE_CODE");

                entity.Property(e => e.ScreenLabel)
                    .HasMaxLength(200)
                    .HasColumnName("SCREEN_LABEL");
            });

            modelBuilder.Entity<HetRegion>(entity =>
            {
                entity.HasKey(e => e.RegionId);

                entity.ToTable("HET_REGION");

                entity.Property(e => e.RegionId)
                    .HasColumnName("REGION_ID")
                    .HasDefaultValueSql("nextval('\"HET_REGION_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.MinistryRegionId).HasColumnName("MINISTRY_REGION_ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .HasColumnName("NAME");

                entity.Property(e => e.RegionNumber).HasColumnName("REGION_NUMBER");

                entity.Property(e => e.StartDate).HasColumnName("START_DATE");
            });

            modelBuilder.Entity<HetRentalAgreement>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementId);

                entity.ToTable("HET_RENTAL_AGREEMENT");

                entity.HasIndex(e => e.Number, "HET_RNTAG_NUMBER_UK")
                    .IsUnique();

                entity.HasIndex(e => e.DistrictId, "IX_HET_RENTAL_AGREEMENT_DISTRICT_ID");

                entity.HasIndex(e => e.EquipmentId, "IX_HET_RENTAL_AGREEMENT_EQUIPMENT_ID");

                entity.HasIndex(e => e.RatePeriodTypeId, "IX_HET_RENTAL_AGREEMENT_HET_RATE_PERIOD_TYPE_ID");

                entity.HasIndex(e => e.RentalAgreementStatusTypeId, "IX_HET_RENTAL_AGREEMENT_HET_RENTAL_AGREEMENT_STATUS_TYPE_ID");

                entity.HasIndex(e => e.RentalRequestId, "IX_HET_RENTAL_AGREEMENT_HET_RENTAL_REQUEST_ID");

                entity.HasIndex(e => e.RentalRequestRotationListId, "IX_HET_RENTAL_AGREEMENT_HET_RENTAL_REQUEST_ROTATION_LIST_ID");

                entity.HasIndex(e => e.ProjectId, "IX_HET_RENTAL_AGREEMENT_PROJECT_ID");

                entity.Property(e => e.RentalAgreementId)
                    .HasColumnName("RENTAL_AGREEMENT_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_AGREEMENT_ID_seq\"'::regclass)");

                entity.Property(e => e.AgreementCity)
                    .HasMaxLength(255)
                    .HasColumnName("AGREEMENT_CITY");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DatedOn).HasColumnName("DATED_ON");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.EquipmentRate).HasColumnName("EQUIPMENT_RATE");

                entity.Property(e => e.EstimateHours).HasColumnName("ESTIMATE_HOURS");

                entity.Property(e => e.EstimateStartWork).HasColumnName("ESTIMATE_START_WORK");

                entity.Property(e => e.Note)
                    .HasMaxLength(2048)
                    .HasColumnName("NOTE");

                entity.Property(e => e.Number)
                    .HasMaxLength(30)
                    .HasColumnName("NUMBER");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.RateComment)
                    .HasMaxLength(2048)
                    .HasColumnName("RATE_COMMENT");

                entity.Property(e => e.RatePeriodTypeId).HasColumnName("RATE_PERIOD_TYPE_ID");

                entity.Property(e => e.RentalAgreementStatusTypeId).HasColumnName("RENTAL_AGREEMENT_STATUS_TYPE_ID");

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.Property(e => e.RentalRequestRotationListId).HasColumnName("RENTAL_REQUEST_ROTATION_LIST_ID");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetRentalAgreements)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_DISTRICT_ID");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetRentalAgreements)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_EQUIPMENT_ID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetRentalAgreements)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_PROJECT_ID");

                entity.HasOne(d => d.RatePeriodType)
                    .WithMany(p => p.HetRentalAgreements)
                    .HasForeignKey(d => d.RatePeriodTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_RATE_PERIOD_TYPE_ID");

                entity.HasOne(d => d.RentalAgreementStatusType)
                    .WithMany(p => p.HetRentalAgreements)
                    .HasForeignKey(d => d.RentalAgreementStatusTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_STATUS_TYPE_ID");

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetRentalAgreements)
                    .HasForeignKey(d => d.RentalRequestId)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_RENTAL_REQUEST_ID");

                entity.HasOne(d => d.RentalRequestRotationList)
                    .WithMany(p => p.HetRentalAgreements)
                    .HasForeignKey(d => d.RentalRequestRotationListId)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_RENTAL_REQUEST_ROTATION_LIST_ID");
            });

            modelBuilder.Entity<HetRentalAgreementCondition>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementConditionId);

                entity.ToTable("HET_RENTAL_AGREEMENT_CONDITION");

                entity.HasIndex(e => e.RentalAgreementId, "IX_HET_RENTAL_AGREEMENT_CONDITION_RENTAL_AGREEMENT_ID");

                entity.Property(e => e.RentalAgreementConditionId)
                    .HasColumnName("RENTAL_AGREEMENT_CONDITION_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_AGREEMENT_CONDITION_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(2048)
                    .HasColumnName("COMMENT");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.ConditionName)
                    .HasMaxLength(150)
                    .HasColumnName("CONDITION_NAME");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.HasOne(d => d.RentalAgreement)
                    .WithMany(p => p.HetRentalAgreementConditions)
                    .HasForeignKey(d => d.RentalAgreementId)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_CONDITION_RENTAL_AGREEMENT_ID");
            });

            modelBuilder.Entity<HetRentalAgreementConditionHist>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementConditionHistId)
                    .HasName("HET_RENTAL_AGREEMENT_CONDITION_HIST_PK");

                entity.ToTable("HET_RENTAL_AGREEMENT_CONDITION_HIST");

                entity.Property(e => e.RentalAgreementConditionHistId)
                    .HasColumnName("RENTAL_AGREEMENT_CONDITION_HIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_AGREEMENT_CONDITION_HIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(2048)
                    .HasColumnName("COMMENT");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.ConditionName)
                    .HasMaxLength(150)
                    .HasColumnName("CONDITION_NAME");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.RentalAgreementConditionId).HasColumnName("RENTAL_AGREEMENT_CONDITION_ID");

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");
            });

            modelBuilder.Entity<HetRentalAgreementHist>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementHistId)
                    .HasName("HET_RENTAL_AGREEMENT_HIST_PK");

                entity.ToTable("HET_RENTAL_AGREEMENT_HIST");

                entity.Property(e => e.RentalAgreementHistId)
                    .HasColumnName("RENTAL_AGREEMENT_HIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_AGREEMENT_HIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AgreementCity)
                    .HasMaxLength(255)
                    .HasColumnName("AGREEMENT_CITY");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DatedOn).HasColumnName("DATED_ON");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.EquipmentRate).HasColumnName("EQUIPMENT_RATE");

                entity.Property(e => e.EstimateHours).HasColumnName("ESTIMATE_HOURS");

                entity.Property(e => e.EstimateStartWork).HasColumnName("ESTIMATE_START_WORK");

                entity.Property(e => e.Note)
                    .HasMaxLength(2048)
                    .HasColumnName("NOTE");

                entity.Property(e => e.Number)
                    .HasMaxLength(30)
                    .HasColumnName("NUMBER");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.RateComment)
                    .HasMaxLength(2048)
                    .HasColumnName("RATE_COMMENT");

                entity.Property(e => e.RatePeriodTypeId).HasColumnName("RATE_PERIOD_TYPE_ID");

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.Property(e => e.RentalAgreementStatusTypeId).HasColumnName("RENTAL_AGREEMENT_STATUS_TYPE_ID");
            });

            modelBuilder.Entity<HetRentalAgreementRate>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementRateId);

                entity.ToTable("HET_RENTAL_AGREEMENT_RATE");

                entity.HasIndex(e => e.RentalAgreementId, "IX_HET_RENTAL_AGREEMENT_RATE_RENTAL_AGREEMENT_ID");

                entity.Property(e => e.RentalAgreementRateId)
                    .HasColumnName("RENTAL_AGREEMENT_RATE_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_AGREEMENT_RATE_ID_seq\"'::regclass)");

                entity.Property(e => e.Active)
                    .HasColumnName("ACTIVE")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(2048)
                    .HasColumnName("COMMENT");

                entity.Property(e => e.ComponentName)
                    .HasMaxLength(150)
                    .HasColumnName("COMPONENT_NAME");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.IsIncludedInTotal).HasColumnName("IS_INCLUDED_IN_TOTAL");

                entity.Property(e => e.Overtime)
                    .HasColumnName("OVERTIME")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Rate).HasColumnName("RATE");

                entity.Property(e => e.RatePeriodTypeId).HasColumnName("RATE_PERIOD_TYPE_ID");

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.Property(e => e.Set)
                    .HasColumnName("SET")
                    .HasDefaultValueSql("false");

                entity.HasOne(d => d.RatePeriodType)
                    .WithMany(p => p.HetRentalAgreementRates)
                    .HasForeignKey(d => d.RatePeriodTypeId)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_RATE_PERIOD_TYPE_ID");

                entity.HasOne(d => d.RentalAgreement)
                    .WithMany(p => p.HetRentalAgreementRates)
                    .HasForeignKey(d => d.RentalAgreementId)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_RATE_AGREEMENT_ID");
            });

            modelBuilder.Entity<HetRentalAgreementRateHist>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementRateHistId)
                    .HasName("HET_RENTAL_AGREEMENT_RATE_HIST_PK");

                entity.ToTable("HET_RENTAL_AGREEMENT_RATE_HIST");

                entity.Property(e => e.RentalAgreementRateHistId)
                    .HasColumnName("RENTAL_AGREEMENT_RATE_HIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_AGREEMENT_RATE_HIST_ID_seq\"'::regclass)");

                entity.Property(e => e.Active)
                    .HasColumnName("ACTIVE")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(2048)
                    .HasColumnName("COMMENT");

                entity.Property(e => e.ComponentName)
                    .HasMaxLength(150)
                    .HasColumnName("COMPONENT_NAME");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.IsIncludedInTotal).HasColumnName("IS_INCLUDED_IN_TOTAL");

                entity.Property(e => e.Overtime)
                    .HasColumnName("OVERTIME")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Rate).HasColumnName("RATE");

                entity.Property(e => e.RatePeriodTypeId).HasColumnName("RATE_PERIOD_TYPE_ID");

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.Property(e => e.RentalAgreementRateId).HasColumnName("RENTAL_AGREEMENT_RATE_ID");

                entity.Property(e => e.Set).HasColumnName("SET");
            });

            modelBuilder.Entity<HetRentalAgreementStatusType>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementStatusTypeId);

                entity.ToTable("HET_RENTAL_AGREEMENT_STATUS_TYPE");

                entity.HasIndex(e => e.RentalAgreementStatusTypeCode, "UK_HET_RENTAL_AGREEMENT_STATUS_TYPE_CODE")
                    .IsUnique();

                entity.Property(e => e.RentalAgreementStatusTypeId)
                    .HasColumnName("RENTAL_AGREEMENT_STATUS_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_AGREEMENT_STATUS_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.RentalAgreementStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("RENTAL_AGREEMENT_STATUS_TYPE_CODE");

                entity.Property(e => e.ScreenLabel)
                    .HasMaxLength(200)
                    .HasColumnName("SCREEN_LABEL");
            });

            modelBuilder.Entity<HetRentalRequest>(entity =>
            {
                entity.HasKey(e => e.RentalRequestId);

                entity.ToTable("HET_RENTAL_REQUEST");

                entity.HasIndex(e => e.DistrictEquipmentTypeId, "IX_HET_RENTAL_REQUEST_DISTRICT_EQUIPMENT_TYPE_ID");

                entity.HasIndex(e => e.FirstOnRotationListId, "IX_HET_RENTAL_REQUEST_FIRST_ON_ROTATION_LIST_ID");

                entity.HasIndex(e => e.LocalAreaId, "IX_HET_RENTAL_REQUEST_LOCAL_AREA_ID");

                entity.HasIndex(e => e.ProjectId, "IX_HET_RENTAL_REQUEST_PROJECT_ID");

                entity.HasIndex(e => e.RentalRequestStatusTypeId, "IX_HET_RENTAL_REQUEST_STATUS_TYPE_ID");

                entity.Property(e => e.RentalRequestId)
                    .HasColumnName("RENTAL_REQUEST_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_REQUEST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.DistrictEquipmentTypeId).HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID");

                entity.Property(e => e.EquipmentCount).HasColumnName("EQUIPMENT_COUNT");

                entity.Property(e => e.ExpectedEndDate).HasColumnName("EXPECTED_END_DATE");

                entity.Property(e => e.ExpectedHours).HasColumnName("EXPECTED_HOURS");

                entity.Property(e => e.ExpectedStartDate).HasColumnName("EXPECTED_START_DATE");

                entity.Property(e => e.FirstOnRotationListId).HasColumnName("FIRST_ON_ROTATION_LIST_ID");

                entity.Property(e => e.FiscalYear).HasColumnName("FISCAL_YEAR");

                entity.Property(e => e.LocalAreaId).HasColumnName("LOCAL_AREA_ID");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.RentalRequestStatusTypeId).HasColumnName("RENTAL_REQUEST_STATUS_TYPE_ID");

                entity.HasOne(d => d.DistrictEquipmentType)
                    .WithMany(p => p.HetRentalRequests)
                    .HasForeignKey(d => d.DistrictEquipmentTypeId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_DISTRICT_EQUIPMENT_TYPE_DISTRICT_ID");

                entity.HasOne(d => d.FirstOnRotationList)
                    .WithMany(p => p.HetRentalRequests)
                    .HasForeignKey(d => d.FirstOnRotationListId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_FIRST_ON_ROTATION_LIST_ID");

                entity.HasOne(d => d.LocalArea)
                    .WithMany(p => p.HetRentalRequests)
                    .HasForeignKey(d => d.LocalAreaId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_LOCAL_AREA_ID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetRentalRequests)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_PROJECT_ID");

                entity.HasOne(d => d.RentalRequestStatusType)
                    .WithMany(p => p.HetRentalRequests)
                    .HasForeignKey(d => d.RentalRequestStatusTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_STATUS_TYPE_ID");
            });

            modelBuilder.Entity<HetRentalRequestAttachment>(entity =>
            {
                entity.HasKey(e => e.RentalRequestAttachmentId);

                entity.ToTable("HET_RENTAL_REQUEST_ATTACHMENT");

                entity.HasIndex(e => e.RentalRequestId, "IX_HET_RENTAL_REQUEST_ATTACHMENT_RENTAL_REQUEST_ID");

                entity.Property(e => e.RentalRequestAttachmentId)
                    .HasColumnName("RENTAL_REQUEST_ATTACHMENT_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_REQUEST_ATTACHMENT_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.Attachment)
                    .HasMaxLength(150)
                    .HasColumnName("ATTACHMENT");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetRentalRequestAttachments)
                    .HasForeignKey(d => d.RentalRequestId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_ATTACHMENT_RENTAL_REQUEST_ID");
            });

            modelBuilder.Entity<HetRentalRequestRotationList>(entity =>
            {
                entity.HasKey(e => e.RentalRequestRotationListId);

                entity.ToTable("HET_RENTAL_REQUEST_ROTATION_LIST");

                entity.HasIndex(e => e.EquipmentId, "IX_HET_RENTAL_REQUEST_ROTATION_LIST_EQUIPMENT_ID");

                entity.HasIndex(e => e.RentalAgreementId, "IX_HET_RENTAL_REQUEST_ROTATION_LIST_RENTAL_AGREEMENT_ID");

                entity.HasIndex(e => e.RentalRequestId, "IX_HET_RENTAL_REQUEST_ROTATION_LIST_RENTAL_REQUEST_ID");

                entity.Property(e => e.RentalRequestRotationListId)
                    .HasColumnName("RENTAL_REQUEST_ROTATION_LIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_REQUEST_ROTATION_LIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.AskedDateTime).HasColumnName("ASKED_DATE_TIME");

                entity.Property(e => e.BlockNumber).HasColumnName("BLOCK_NUMBER");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.IsForceHire).HasColumnName("IS_FORCE_HIRE");

                entity.Property(e => e.Note)
                    .HasMaxLength(2048)
                    .HasColumnName("NOTE");

                entity.Property(e => e.OfferRefusalReason)
                    .HasMaxLength(50)
                    .HasColumnName("OFFER_REFUSAL_REASON");

                entity.Property(e => e.OfferResponse).HasColumnName("OFFER_RESPONSE");

                entity.Property(e => e.OfferResponseDatetime).HasColumnName("OFFER_RESPONSE_DATETIME");

                entity.Property(e => e.OfferResponseNote)
                    .HasMaxLength(2048)
                    .HasColumnName("OFFER_RESPONSE_NOTE");

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.Property(e => e.RotationListSortOrder).HasColumnName("ROTATION_LIST_SORT_ORDER");

                entity.Property(e => e.WasAsked).HasColumnName("WAS_ASKED");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetRentalRequestRotationLists)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_ROTATION_LIST_EQUIPMENT_ID");

                entity.HasOne(d => d.RentalAgreement)
                    .WithMany(p => p.HetRentalRequestRotationLists)
                    .HasForeignKey(d => d.RentalAgreementId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_ROTATION_LIST_RENTAL_AGREEMENT_ID");

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetRentalRequestRotationLists)
                    .HasForeignKey(d => d.RentalRequestId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_ROTATION_LIST_RENTAL_REQUEST_ID");
            });

            modelBuilder.Entity<HetRentalRequestRotationListHist>(entity =>
            {
                entity.HasKey(e => e.RentalRequestRotationListHistId)
                    .HasName("HET_RENTAL_REQUEST_ROTATION_LIST_HIST_PK");

                entity.ToTable("HET_RENTAL_REQUEST_ROTATION_LIST_HIST");

                entity.Property(e => e.RentalRequestRotationListHistId)
                    .HasColumnName("RENTAL_REQUEST_ROTATION_LIST_HIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_REQUEST_ROTATION_LIST_HIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.AskedDateTime).HasColumnName("ASKED_DATE_TIME");

                entity.Property(e => e.BlockNumber).HasColumnName("BLOCK_NUMBER");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.IsForceHire).HasColumnName("IS_FORCE_HIRE");

                entity.Property(e => e.Note)
                    .HasMaxLength(2048)
                    .HasColumnName("NOTE");

                entity.Property(e => e.OfferRefusalReason)
                    .HasMaxLength(50)
                    .HasColumnName("OFFER_REFUSAL_REASON");

                entity.Property(e => e.OfferResponse).HasColumnName("OFFER_RESPONSE");

                entity.Property(e => e.OfferResponseDatetime).HasColumnName("OFFER_RESPONSE_DATETIME");

                entity.Property(e => e.OfferResponseNote)
                    .HasMaxLength(2048)
                    .HasColumnName("OFFER_RESPONSE_NOTE");

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.Property(e => e.RentalRequestRotationListId).HasColumnName("RENTAL_REQUEST_ROTATION_LIST_ID");

                entity.Property(e => e.RotationListSortOrder).HasColumnName("ROTATION_LIST_SORT_ORDER");

                entity.Property(e => e.WasAsked).HasColumnName("WAS_ASKED");
            });

            modelBuilder.Entity<HetRentalRequestSeniorityList>(entity =>
            {
                entity.HasKey(e => e.RentalRequestSeniorityListId);

                entity.ToTable("HET_RENTAL_REQUEST_SENIORITY_LIST");

                entity.HasIndex(e => e.EquipmentId, "IX_HET_RENTAL_REQUEST_SENIORITY_LIST_EQUIPMENT_ID");

                entity.HasIndex(e => e.RentalRequestId, "IX_HET_RENTAL_REQUEST_SENIORITY_LIST_RENTAL_REQUEST_ID");

                entity.Property(e => e.RentalRequestSeniorityListId)
                    .HasColumnName("RENTAL_REQUEST_SENIORITY_LIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_REQUEST_SENIORITY_LIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ApprovedDate).HasColumnName("APPROVED_DATE");

                entity.Property(e => e.ArchiveCode)
                    .HasMaxLength(50)
                    .HasColumnName("ARCHIVE_CODE");

                entity.Property(e => e.ArchiveDate).HasColumnName("ARCHIVE_DATE");

                entity.Property(e => e.ArchiveReason)
                    .HasMaxLength(2048)
                    .HasColumnName("ARCHIVE_REASON");

                entity.Property(e => e.BlockNumber).HasColumnName("BLOCK_NUMBER");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.DistrictEquipmentTypeId).HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID");

                entity.Property(e => e.EquipmentCode)
                    .HasMaxLength(25)
                    .HasColumnName("EQUIPMENT_CODE");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.EquipmentStatusTypeId).HasColumnName("EQUIPMENT_STATUS_TYPE_ID");

                entity.Property(e => e.InformationUpdateNeededReason)
                    .HasMaxLength(2048)
                    .HasColumnName("INFORMATION_UPDATE_NEEDED_REASON");

                entity.Property(e => e.IsInformationUpdateNeeded).HasColumnName("IS_INFORMATION_UPDATE_NEEDED");

                entity.Property(e => e.IsSeniorityOverridden).HasColumnName("IS_SENIORITY_OVERRIDDEN");

                entity.Property(e => e.LastCalled).HasColumnName("LAST_CALLED");

                entity.Property(e => e.LastVerifiedDate).HasColumnName("LAST_VERIFIED_DATE");

                entity.Property(e => e.LegalCapacity)
                    .HasMaxLength(150)
                    .HasColumnName("LEGAL_CAPACITY");

                entity.Property(e => e.LicencePlate)
                    .HasMaxLength(20)
                    .HasColumnName("LICENCE_PLATE");

                entity.Property(e => e.LicencedGvw)
                    .HasMaxLength(150)
                    .HasColumnName("LICENCED_GVW");

                entity.Property(e => e.LocalAreaId).HasColumnName("LOCAL_AREA_ID");

                entity.Property(e => e.Make)
                    .HasMaxLength(50)
                    .HasColumnName("MAKE");

                entity.Property(e => e.Model)
                    .HasMaxLength(50)
                    .HasColumnName("MODEL");

                entity.Property(e => e.NumberInBlock).HasColumnName("NUMBER_IN_BLOCK");

                entity.Property(e => e.Operator)
                    .HasMaxLength(255)
                    .HasColumnName("OPERATOR");

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.PayRate).HasColumnName("PAY_RATE");

                entity.Property(e => e.PupLegalCapacity)
                    .HasMaxLength(150)
                    .HasColumnName("PUP_LEGAL_CAPACITY");

                entity.Property(e => e.ReceivedDate).HasColumnName("RECEIVED_DATE");

                entity.Property(e => e.RefuseRate)
                    .HasMaxLength(255)
                    .HasColumnName("REFUSE_RATE");

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.Property(e => e.Seniority).HasColumnName("SENIORITY");

                entity.Property(e => e.SeniorityEffectiveDate).HasColumnName("SENIORITY_EFFECTIVE_DATE");

                entity.Property(e => e.SeniorityOverrideReason)
                    .HasMaxLength(2048)
                    .HasColumnName("SENIORITY_OVERRIDE_REASON");

                entity.Property(e => e.SerialNumber)
                    .HasMaxLength(100)
                    .HasColumnName("SERIAL_NUMBER");

                entity.Property(e => e.ServiceHoursLastYear).HasColumnName("SERVICE_HOURS_LAST_YEAR");

                entity.Property(e => e.ServiceHoursThreeYearsAgo).HasColumnName("SERVICE_HOURS_THREE_YEARS_AGO");

                entity.Property(e => e.ServiceHoursTwoYearsAgo).HasColumnName("SERVICE_HOURS_TWO_YEARS_AGO");

                entity.Property(e => e.Size)
                    .HasMaxLength(128)
                    .HasColumnName("SIZE");

                entity.Property(e => e.StatusComment)
                    .HasMaxLength(255)
                    .HasColumnName("STATUS_COMMENT");

                entity.Property(e => e.ToDate).HasColumnName("TO_DATE");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("TYPE");

                entity.Property(e => e.WorkingNow).HasColumnName("WORKING_NOW");

                entity.Property(e => e.Year)
                    .HasMaxLength(15)
                    .HasColumnName("YEAR");

                entity.Property(e => e.YearsOfService).HasColumnName("YEARS_OF_SERVICE");

                entity.Property(e => e.YtdHours).HasColumnName("YTD_HOURS");

                entity.HasOne(d => d.DistrictEquipmentType)
                    .WithMany(p => p.HetRentalRequestSeniorityLists)
                    .HasForeignKey(d => d.DistrictEquipmentTypeId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_SENIORITY_LIST_DISTRICT_EQUIPMENT_TYPE_ID");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetRentalRequestSeniorityLists)
                    .HasForeignKey(d => d.EquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_SENIORITY_LIST_EQUIPMENT_ID");

                entity.HasOne(d => d.EquipmentStatusType)
                    .WithMany(p => p.HetRentalRequestSeniorityLists)
                    .HasForeignKey(d => d.EquipmentStatusTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_SENIORITY_LIST_STATUS_TYPE_ID");

                entity.HasOne(d => d.LocalArea)
                    .WithMany(p => p.HetRentalRequestSeniorityLists)
                    .HasForeignKey(d => d.LocalAreaId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_SENIORITY_LIST_LOCAL_AREA_ID");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetRentalRequestSeniorityLists)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_SENIORITY_LIST_OWNER_ID");

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetRentalRequestSeniorityLists)
                    .HasForeignKey(d => d.RentalRequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_SENIORITY_LIST_RENTAL_REQUEST_ID");
            });

            modelBuilder.Entity<HetRentalRequestStatusType>(entity =>
            {
                entity.HasKey(e => e.RentalRequestStatusTypeId);

                entity.ToTable("HET_RENTAL_REQUEST_STATUS_TYPE");

                entity.HasIndex(e => e.RentalRequestStatusTypeCode, "UK_HET_RENTAL_REQUEST_STATUS_TYPE_CODE")
                    .IsUnique();

                entity.Property(e => e.RentalRequestStatusTypeId)
                    .HasColumnName("RENTAL_REQUEST_STATUS_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_REQUEST_STATUS_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.RentalRequestStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("RENTAL_REQUEST_STATUS_TYPE_CODE");

                entity.Property(e => e.ScreenLabel)
                    .HasMaxLength(200)
                    .HasColumnName("SCREEN_LABEL");
            });

            modelBuilder.Entity<HetRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("HET_ROLE");

                entity.HasIndex(e => e.Name, "HET_ROLE_NAME_UK")
                    .IsUnique();

                entity.Property(e => e.RoleId)
                    .HasColumnName("ROLE_ID")
                    .HasDefaultValueSql("nextval('\"HET_ROLE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(2048)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<HetRolePermission>(entity =>
            {
                entity.HasKey(e => e.RolePermissionId);

                entity.ToTable("HET_ROLE_PERMISSION");

                entity.HasIndex(e => e.PermissionId, "IX_HET_ROLE_PERMISSION_PERMISSION_ID");

                entity.HasIndex(e => e.RoleId, "IX_HET_ROLE_PERMISSION_ROLE_ID");

                entity.Property(e => e.RolePermissionId)
                    .HasColumnName("ROLE_PERMISSION_ID")
                    .HasDefaultValueSql("nextval('\"HET_ROLE_PERMISSION_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.PermissionId).HasColumnName("PERMISSION_ID");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.HetRolePermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_HET_ROLE_PERMISSION_PERMISSION_ID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.HetRolePermissions)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_HET_ROLE_PERMISSION_ROLE_ID");
            });

            modelBuilder.Entity<HetRolloverProgress>(entity =>
            {
                entity.HasKey(e => e.DistrictId);

                entity.ToTable("HET_ROLLOVER_PROGRESS");

                entity.Property(e => e.DistrictId)
                    .ValueGeneratedNever()
                    .HasColumnName("DISTRICT_ID");

                entity.Property(e => e.ProgressPercentage).HasColumnName("PROGRESS_PERCENTAGE");

                entity.HasOne(d => d.District)
                    .WithOne(p => p.HetRolloverProgress)
                    .HasForeignKey<HetRolloverProgress>(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_ROLLOVER_PROGRESS_DISTRICT_ID");
            });

            modelBuilder.Entity<HetSeniorityAudit>(entity =>
            {
                entity.HasKey(e => e.SeniorityAuditId);

                entity.ToTable("HET_SENIORITY_AUDIT");

                entity.HasIndex(e => e.EquipmentId, "IX_HET_SENIORITY_AUDIT_EQUIPMENT_ID");

                entity.HasIndex(e => e.LocalAreaId, "IX_HET_SENIORITY_AUDIT_LOCAL_AREA_ID");

                entity.HasIndex(e => e.OwnerId, "IX_HET_SENIORITY_AUDIT_OWNER_ID");

                entity.Property(e => e.SeniorityAuditId)
                    .HasColumnName("SENIORITY_AUDIT_ID")
                    .HasDefaultValueSql("nextval('\"HET_SENIORITY_AUDIT_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.BlockNumber).HasColumnName("BLOCK_NUMBER");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.IsSeniorityOverridden).HasColumnName("IS_SENIORITY_OVERRIDDEN");

                entity.Property(e => e.LocalAreaId).HasColumnName("LOCAL_AREA_ID");

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.OwnerOrganizationName)
                    .HasMaxLength(150)
                    .HasColumnName("OWNER_ORGANIZATION_NAME");

                entity.Property(e => e.Seniority).HasColumnName("SENIORITY");

                entity.Property(e => e.SeniorityOverrideReason)
                    .HasMaxLength(2048)
                    .HasColumnName("SENIORITY_OVERRIDE_REASON");

                entity.Property(e => e.ServiceHoursLastYear).HasColumnName("SERVICE_HOURS_LAST_YEAR");

                entity.Property(e => e.ServiceHoursThreeYearsAgo).HasColumnName("SERVICE_HOURS_THREE_YEARS_AGO");

                entity.Property(e => e.ServiceHoursTwoYearsAgo).HasColumnName("SERVICE_HOURS_TWO_YEARS_AGO");

                entity.Property(e => e.StartDate).HasColumnName("START_DATE");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetSeniorityAudits)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK_HET_SENIORITY_AUDIT_EQUIPMENT_ID");

                entity.HasOne(d => d.LocalArea)
                    .WithMany(p => p.HetSeniorityAudits)
                    .HasForeignKey(d => d.LocalAreaId)
                    .HasConstraintName("FK_HET_SENIORITY_AUDIT_LOCAL_AREA_ID");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetSeniorityAudits)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_HET_SENIORITY_AUDIT_OWNER_ID");
            });

            modelBuilder.Entity<HetServiceArea>(entity =>
            {
                entity.HasKey(e => e.ServiceAreaId);

                entity.ToTable("HET_SERVICE_AREA");

                entity.HasIndex(e => e.DistrictId, "IX_HET_SERVICE_AREA_DISTRICT_ID");

                entity.Property(e => e.ServiceAreaId)
                    .HasColumnName("SERVICE_AREA_ID")
                    .HasDefaultValueSql("nextval('\"HET_SERVICE_AREA_ID_seq\"'::regclass)");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.AreaNumber).HasColumnName("AREA_NUMBER");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .HasColumnName("FAX");

                entity.Property(e => e.FiscalEndDate).HasColumnName("FISCAL_END_DATE");

                entity.Property(e => e.FiscalStartDate).HasColumnName("FISCAL_START_DATE");

                entity.Property(e => e.MinistryServiceAreaId).HasColumnName("MINISTRY_SERVICE_AREA_ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .HasColumnName("NAME");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("PHONE");

                entity.Property(e => e.SupportingDocuments)
                    .HasMaxLength(500)
                    .HasColumnName("SUPPORTING_DOCUMENTS");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetServiceAreas)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_HET_SERVICE_AREA_DISTRICT_ID");
            });

            modelBuilder.Entity<HetTimePeriodType>(entity =>
            {
                entity.HasKey(e => e.TimePeriodTypeId);

                entity.ToTable("HET_TIME_PERIOD_TYPE");

                entity.HasIndex(e => e.TimePeriodTypeCode, "UK_HET_TIME_PERIOD_TYPE_CODE")
                    .IsUnique();

                entity.Property(e => e.TimePeriodTypeId)
                    .HasColumnName("TIME_PERIOD_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_TIME_PERIOD_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.ScreenLabel)
                    .HasMaxLength(200)
                    .HasColumnName("SCREEN_LABEL");

                entity.Property(e => e.TimePeriodTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("TIME_PERIOD_TYPE_CODE");
            });

            modelBuilder.Entity<HetTimeRecord>(entity =>
            {
                entity.HasKey(e => e.TimeRecordId);

                entity.ToTable("HET_TIME_RECORD");

                entity.HasIndex(e => e.RentalAgreementId, "IX_HET_TIME_RECORD_RENTAL_AGREEMENT_ID");

                entity.HasIndex(e => e.RentalAgreementRateId, "IX_HET_TIME_RECORD_RENTAL_AGREEMENT_RATE_ID");

                entity.HasIndex(e => e.TimePeriodTypeId, "IX_HET_TIME_RECORD_TIME_PERIOD_TYPE_ID");

                entity.Property(e => e.TimeRecordId)
                    .HasColumnName("TIME_RECORD_ID")
                    .HasDefaultValueSql("nextval('\"HET_TIME_RECORD_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.EnteredDate).HasColumnName("ENTERED_DATE");

                entity.Property(e => e.Hours).HasColumnName("HOURS");

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.Property(e => e.RentalAgreementRateId).HasColumnName("RENTAL_AGREEMENT_RATE_ID");

                entity.Property(e => e.TimePeriodTypeId).HasColumnName("TIME_PERIOD_TYPE_ID");

                entity.Property(e => e.WorkedDate).HasColumnName("WORKED_DATE");

                entity.HasOne(d => d.RentalAgreement)
                    .WithMany(p => p.HetTimeRecords)
                    .HasForeignKey(d => d.RentalAgreementId)
                    .HasConstraintName("FK_HET_TIME_RECORD_RENTAL_AGREEMENT_ID");

                entity.HasOne(d => d.RentalAgreementRate)
                    .WithMany(p => p.HetTimeRecords)
                    .HasForeignKey(d => d.RentalAgreementRateId)
                    .HasConstraintName("FK_HET_TIME_RECORD_RENTAL_AGREEMENT_RATE_ID");

                entity.HasOne(d => d.TimePeriodType)
                    .WithMany(p => p.HetTimeRecords)
                    .HasForeignKey(d => d.TimePeriodTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_TIME_RECORD_TIME_PERIOD_TYPE_ID");
            });

            modelBuilder.Entity<HetTimeRecordHist>(entity =>
            {
                entity.HasKey(e => e.TimeRecordHistId)
                    .HasName("HET_TIME_RECORD_HIST_PK");

                entity.ToTable("HET_TIME_RECORD_HIST");

                entity.Property(e => e.TimeRecordHistId)
                    .HasColumnName("TIME_RECORD_HIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_TIME_RECORD_HIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.EnteredDate).HasColumnName("ENTERED_DATE");

                entity.Property(e => e.Hours).HasColumnName("HOURS");

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.Property(e => e.RentalAgreementRateId).HasColumnName("RENTAL_AGREEMENT_RATE_ID");

                entity.Property(e => e.TimePeriodTypeId).HasColumnName("TIME_PERIOD_TYPE_ID");

                entity.Property(e => e.TimeRecordId).HasColumnName("TIME_RECORD_ID");

                entity.Property(e => e.WorkedDate).HasColumnName("WORKED_DATE");
            });

            modelBuilder.Entity<HetUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("HET_USER");

                entity.HasIndex(e => e.Guid, "HET_USR_GUID_UK")
                    .IsUnique();

                entity.HasIndex(e => e.DistrictId, "IX_HET_USER_DISTRICT_ID");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("nextval('\"HET_USER_ID_seq\"'::regclass)");

                entity.Property(e => e.Active).HasColumnName("ACTIVE");

                entity.Property(e => e.AgreementCity)
                    .HasMaxLength(255)
                    .HasColumnName("AGREEMENT_CITY");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.GivenName)
                    .HasMaxLength(50)
                    .HasColumnName("GIVEN_NAME");

                entity.Property(e => e.Guid)
                    .HasMaxLength(255)
                    .HasColumnName("GUID");

                entity.Property(e => e.Initials)
                    .HasMaxLength(10)
                    .HasColumnName("INITIALS");

                entity.Property(e => e.SmAuthorizationDirectory)
                    .HasMaxLength(255)
                    .HasColumnName("SM_AUTHORIZATION_DIRECTORY");

                entity.Property(e => e.SmUserId)
                    .HasMaxLength(255)
                    .HasColumnName("SM_USER_ID");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .HasColumnName("SURNAME");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetUsers)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_HET_USER_DISTRICT_ID");
            });

            modelBuilder.Entity<HetUserDistrict>(entity =>
            {
                entity.HasKey(e => e.UserDistrictId);

                entity.ToTable("HET_USER_DISTRICT");

                entity.HasIndex(e => e.DistrictId, "IX_HET_USER_DISTRICT_DISTRICT_ID");

                entity.HasIndex(e => e.UserId, "IX_HET_USER_DISTRICT_USER_ID");

                entity.Property(e => e.UserDistrictId)
                    .HasColumnName("USER_DISTRICT_ID")
                    .HasDefaultValueSql("nextval('\"HET_USER_DISTRICT_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.Property(e => e.IsPrimary).HasColumnName("IS_PRIMARY");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetUserDistricts)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_HET_USER_DISTRICT_DISTRICT_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HetUserDistricts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_HET_USER_DISTRICT_USER_ID");
            });

            modelBuilder.Entity<HetUserFavourite>(entity =>
            {
                entity.HasKey(e => e.UserFavouriteId);

                entity.ToTable("HET_USER_FAVOURITE");

                entity.HasIndex(e => e.DistrictId, "IX_HET_USER_FAVOURITE_DISTRICT_ID");

                entity.HasIndex(e => e.UserId, "IX_HET_USER_FAVOURITE_USER_ID");

                entity.Property(e => e.UserFavouriteId)
                    .HasColumnName("USER_FAVOURITE_ID")
                    .HasDefaultValueSql("nextval('\"HET_USER_FAVOURITE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.Property(e => e.IsDefault).HasColumnName("IS_DEFAULT");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .HasColumnName("NAME");

                entity.Property(e => e.Type)
                    .HasMaxLength(150)
                    .HasColumnName("TYPE");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.Property(e => e.Value)
                    .HasMaxLength(2048)
                    .HasColumnName("VALUE");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetUserFavourites)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_HET_USER_FAVOURITE_DISTRICT_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HetUserFavourites)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_HET_USER_FAVOURITE_USER_ID");
            });

            modelBuilder.Entity<HetUserRole>(entity =>
            {
                entity.HasKey(e => e.UserRoleId);

                entity.ToTable("HET_USER_ROLE");

                entity.HasIndex(e => e.RoleId, "IX_HET_USER_ROLE_ROLE_ID");

                entity.HasIndex(e => e.UserId, "IX_HET_USER_ROLE_USER_ID");

                entity.Property(e => e.UserRoleId)
                    .HasColumnName("USER_ROLE_ID")
                    .HasDefaultValueSql("nextval('\"HET_USER_ROLE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasMaxLength(50)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasMaxLength(255)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_CREATE_USER_ID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasMaxLength(63)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID");

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.ExpiryDate).HasColumnName("EXPIRY_DATE");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.HetUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_HET_USER_ROLE_ROLE_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HetUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_HET_USER_ROLE_USER_ID");
            });

            modelBuilder.HasSequence("counter_id_seq");

            modelBuilder.HasSequence("hash_id_seq");

            modelBuilder.HasSequence("HET_BATCH_REPORT_ID_seq");

            modelBuilder.HasSequence("HET_BUSINESS_ID_seq");

            modelBuilder.HasSequence("HET_BUSINESS_USER_ID_seq");

            modelBuilder.HasSequence("HET_BUSINESS_USER_ROLE_ID_seq");

            modelBuilder.HasSequence("HET_CONDITION_TYPE_ID_seq");

            modelBuilder.HasSequence("HET_CONTACT_ID_seq");

            modelBuilder.HasSequence("HET_DIGITAL_FILE_ID_seq");

            modelBuilder.HasSequence("HET_DISTRICT_EQUIPMENT_TYPE_ID_seq");

            modelBuilder.HasSequence("HET_DISTRICT_ID_seq");

            modelBuilder.HasSequence("HET_EQUIPMENT_ATTACHMENT_HIST_ID_seq");

            modelBuilder.HasSequence("HET_EQUIPMENT_ATTACHMENT_ID_seq");

            modelBuilder.HasSequence("HET_EQUIPMENT_HIST_ID_seq");

            modelBuilder.HasSequence("HET_EQUIPMENT_ID_seq");

            modelBuilder.HasSequence("HET_EQUIPMENT_STATUS_TYPE_ID_seq");

            modelBuilder.HasSequence("HET_EQUIPMENT_TYPE_ID_seq");

            modelBuilder.HasSequence("HET_HISTORY_ID_seq");

            modelBuilder.HasSequence("HET_IMPORT_MAP_ID_seq");

            modelBuilder.HasSequence("HET_LOCAL_AREA_ID_seq");

            modelBuilder.HasSequence("HET_LOCAL_AREA_ROTATION_LIST_ID_seq");

            modelBuilder.HasSequence("HET_MIME_TYPE_ID_seq");

            modelBuilder.HasSequence("HET_NOTE_HIST_ID_seq");

            modelBuilder.HasSequence("HET_NOTE_ID_seq");

            modelBuilder.HasSequence("HET_OWNER_ID_seq");

            modelBuilder.HasSequence("HET_OWNER_STATUS_TYPE_ID_seq");

            modelBuilder.HasSequence("HET_PERMISSION_ID_seq");

            modelBuilder.HasSequence("HET_PERSON_ID_seq");

            modelBuilder.HasSequence("HET_PROJECT_ID_seq");

            modelBuilder.HasSequence("HET_PROJECT_STATUS_TYPE_ID_seq");

            modelBuilder.HasSequence("HET_RATE_PERIOD_TYPE_ID_seq");

            modelBuilder.HasSequence("HET_REGION_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_CONDITION_HIST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_CONDITION_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_HIST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_RATE_HIST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_RATE_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_STATUS_TYPE_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_REQUEST_ATTACHMENT_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_REQUEST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_REQUEST_ROTATION_LIST_HIST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_REQUEST_ROTATION_LIST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_REQUEST_SENIORITY_LIST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_REQUEST_STATUS_TYPE_ID_seq");

            modelBuilder.HasSequence("HET_ROLE_ID_seq");

            modelBuilder.HasSequence("HET_ROLE_PERMISSION_ID_seq");

            modelBuilder.HasSequence("HET_SENIORITY_AUDIT_ID_seq");

            modelBuilder.HasSequence("HET_SERVICE_AREA_ID_seq");

            modelBuilder.HasSequence("HET_TIME_PERIOD_TYPE_ID_seq");

            modelBuilder.HasSequence("HET_TIME_RECORD_HIST_ID_seq");

            modelBuilder.HasSequence("HET_TIME_RECORD_ID_seq");

            modelBuilder.HasSequence("HET_USER_DISTRICT_ID_seq");

            modelBuilder.HasSequence("HET_USER_FAVOURITE_ID_seq");

            modelBuilder.HasSequence("HET_USER_ID_seq");

            modelBuilder.HasSequence("HET_USER_ROLE_ID_seq");

            modelBuilder.HasSequence("job_id_seq");

            modelBuilder.HasSequence("jobparameter_id_seq");

            modelBuilder.HasSequence("jobqueue_id_seq");

            modelBuilder.HasSequence("list_id_seq");

            modelBuilder.HasSequence("set_id_seq");

            modelBuilder.HasSequence("state_id_seq");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
