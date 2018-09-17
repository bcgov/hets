using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HetsData.Model
{
    public partial class DbAppContext : DbContext
    {
        private readonly string _connectionString;

        public DbAppContext()
        {
        }

        public DbAppContext(DbContextOptions<DbAppContext> options)
            : base(options)
        {
        }

        public DbAppContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public virtual DbSet<HetBusiness> HetBusiness { get; set; }
        public virtual DbSet<HetBusinessUser> HetBusinessUser { get; set; }
        public virtual DbSet<HetBusinessUserRole> HetBusinessUserRole { get; set; }
        public virtual DbSet<HetConditionType> HetConditionType { get; set; }
        public virtual DbSet<HetContact> HetContact { get; set; }
        public virtual DbSet<HetDigitalFile> HetDigitalFile { get; set; }
        public virtual DbSet<HetDistrict> HetDistrict { get; set; }
        public virtual DbSet<HetDistrictEquipmentType> HetDistrictEquipmentType { get; set; }
        public virtual DbSet<HetEquipment> HetEquipment { get; set; }
        public virtual DbSet<HetEquipmentAttachment> HetEquipmentAttachment { get; set; }
        public virtual DbSet<HetEquipmentAttachmentHist> HetEquipmentAttachmentHist { get; set; }
        public virtual DbSet<HetEquipmentHist> HetEquipmentHist { get; set; }
        public virtual DbSet<HetEquipmentStatusType> HetEquipmentStatusType { get; set; }
        public virtual DbSet<HetEquipmentType> HetEquipmentType { get; set; }
        public virtual DbSet<HetHistory> HetHistory { get; set; }
        public virtual DbSet<HetImportMap> HetImportMap { get; set; }
        public virtual DbSet<HetLocalArea> HetLocalArea { get; set; }
        public virtual DbSet<HetLocalAreaRotationList> HetLocalAreaRotationList { get; set; }
        public virtual DbSet<HetMimeType> HetMimeType { get; set; }
        public virtual DbSet<HetNote> HetNote { get; set; }
        public virtual DbSet<HetNoteHist> HetNoteHist { get; set; }
        public virtual DbSet<HetOwner> HetOwner { get; set; }
        public virtual DbSet<HetOwnerStatusType> HetOwnerStatusType { get; set; }
        public virtual DbSet<HetPermission> HetPermission { get; set; }
        public virtual DbSet<HetPerson> HetPerson { get; set; }
        public virtual DbSet<HetProject> HetProject { get; set; }
        public virtual DbSet<HetProjectStatusType> HetProjectStatusType { get; set; }
        public virtual DbSet<HetProvincialRateType> HetProvincialRateType { get; set; }
        public virtual DbSet<HetRatePeriodType> HetRatePeriodType { get; set; }
        public virtual DbSet<HetRegion> HetRegion { get; set; }
        public virtual DbSet<HetRentalAgreement> HetRentalAgreement { get; set; }
        public virtual DbSet<HetRentalAgreementCondition> HetRentalAgreementCondition { get; set; }
        public virtual DbSet<HetRentalAgreementConditionHist> HetRentalAgreementConditionHist { get; set; }
        public virtual DbSet<HetRentalAgreementHist> HetRentalAgreementHist { get; set; }
        public virtual DbSet<HetRentalAgreementRate> HetRentalAgreementRate { get; set; }
        public virtual DbSet<HetRentalAgreementRateHist> HetRentalAgreementRateHist { get; set; }
        public virtual DbSet<HetRentalAgreementStatusType> HetRentalAgreementStatusType { get; set; }
        public virtual DbSet<HetRentalRequest> HetRentalRequest { get; set; }
        public virtual DbSet<HetRentalRequestAttachment> HetRentalRequestAttachment { get; set; }
        public virtual DbSet<HetRentalRequestRotationList> HetRentalRequestRotationList { get; set; }
        public virtual DbSet<HetRentalRequestRotationListHist> HetRentalRequestRotationListHist { get; set; }
        public virtual DbSet<HetRentalRequestStatusType> HetRentalRequestStatusType { get; set; }
        public virtual DbSet<HetRole> HetRole { get; set; }
        public virtual DbSet<HetRolePermission> HetRolePermission { get; set; }
        public virtual DbSet<HetSeniorityAudit> HetSeniorityAudit { get; set; }
        public virtual DbSet<HetServiceArea> HetServiceArea { get; set; }
        public virtual DbSet<HetTimePeriodType> HetTimePeriodType { get; set; }
        public virtual DbSet<HetTimeRecord> HetTimeRecord { get; set; }
        public virtual DbSet<HetTimeRecordHist> HetTimeRecordHist { get; set; }
        public virtual DbSet<HetUser> HetUser { get; set; }
        public virtual DbSet<HetUserDistrict> HetUserDistrict { get; set; }
        public virtual DbSet<HetUserFavourite> HetUserFavourite { get; set; }
        public virtual DbSet<HetUserRole> HetUserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HetBusiness>(entity =>
            {
                entity.HasKey(e => e.BusinessId);

                entity.ToTable("HET_BUSINESS");

                entity.HasIndex(e => e.BceidBusinessGuid)
                    .HasName("IX_HET_BUSINESS_BUSINESS_GUID");

                entity.Property(e => e.BusinessId)
                    .HasColumnName("BUSINESS_ID")
                    .HasDefaultValueSql("nextval('\"HET_BUSINESS_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.BceidBusinessGuid)
                    .HasColumnName("BCEID_BUSINESS_GUID")
                    .HasMaxLength(50);

                entity.Property(e => e.BceidBusinessNumber)
                    .HasColumnName("BCEID_BUSINESS_NUMBER")
                    .HasMaxLength(50);

                entity.Property(e => e.BceidDoingBusinessAs)
                    .HasColumnName("BCEID_DOING_BUSINESS_AS")
                    .HasMaxLength(150);

                entity.Property(e => e.BceidLegalName)
                    .HasColumnName("BCEID_LEGAL_NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);                
            });

            modelBuilder.Entity<HetBusinessUser>(entity =>
            {
                entity.HasKey(e => e.BusinessUserId);

                entity.ToTable("HET_BUSINESS_USER");

                entity.HasIndex(e => e.BceidGuid)
                    .HasName("IX_HET_BUSINESS_USER_GUID");

                entity.HasIndex(e => e.BusinessId);

                entity.Property(e => e.BusinessUserId)
                    .HasColumnName("BUSINESS_USER_ID")
                    .HasDefaultValueSql("nextval('\"HET_BUSINESS_USER_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.BceidDisplayName)
                    .HasColumnName("BCEID_DISPLAY_NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.BceidEmail)
                    .HasColumnName("BCEID_EMAIL")
                    .HasMaxLength(150);

                entity.Property(e => e.BceidFirstName)
                    .HasColumnName("BCEID_FIRST_NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.BceidGuid)
                    .HasColumnName("BCEID_GUID")
                    .HasMaxLength(50);

                entity.Property(e => e.BceidLastName)
                    .HasColumnName("BCEID_LAST_NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.BceidTelephone)
                    .HasColumnName("BCEID_TELEPHONE")
                    .HasMaxLength(150);

                entity.Property(e => e.BceidUserId)
                    .HasColumnName("BCEID_USER_ID")
                    .HasMaxLength(150);

                entity.Property(e => e.BusinessId).HasColumnName("BUSINESS_ID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.HetBusinessUser)
                    .HasForeignKey(d => d.BusinessId)
                    .HasConstraintName("FK_HET_BUSINESS_USER_BUSINESS_ID");
            });

            modelBuilder.Entity<HetBusinessUserRole>(entity =>
            {
                entity.HasKey(e => e.BusinessUserRoleId);

                entity.ToTable("HET_BUSINESS_USER_ROLE");

                entity.HasIndex(e => e.BusinessUserId)
                    .HasName("IX_HET_BUSINESS_USER_ROLE_USER_ID");

                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.BusinessUserRoleId)
                    .HasColumnName("BUSINESS_USER_ROLE_ID")
                    .HasDefaultValueSql("nextval('\"HET_BUSINESS_USER_ROLE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.BusinessUserId).HasColumnName("BUSINESS_USER_ID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.ExpiryDate).HasColumnName("EXPIRY_DATE");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.HetBusinessUserRole)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_HET_BUSINESS_USER_ROLE_ROLE_ID");

                entity.HasOne(d => d.BusinessUser)
                    .WithMany(p => p.HetBusinessUserRole)
                    .HasForeignKey(d => d.BusinessUserId)
                    .HasConstraintName("FK_HET_BUSINESS_USER_ROLE_USER_ID");
            });

            modelBuilder.Entity<HetConditionType>(entity =>
            {
                entity.HasKey(e => e.ConditionTypeId);

                entity.ToTable("HET_CONDITION_TYPE");

                entity.HasIndex(e => e.DistrictId);

                entity.Property(e => e.ConditionTypeId)
                    .HasColumnName("CONDITION_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_CONDITION_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.Active).HasColumnName("ACTIVE");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.ConditionTypeCode)
                    .HasColumnName("CONDITION_TYPE_CODE")
                    .HasMaxLength(20);

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048);

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetConditionType)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_HET_CONDITION_TYPE_DISTRICT_ID");
            });

            modelBuilder.Entity<HetContact>(entity =>
            {
                entity.HasKey(e => e.ContactId);

                entity.ToTable("HET_CONTACT");

                entity.HasIndex(e => e.OwnerId);

                entity.HasIndex(e => e.ProjectId);

                entity.Property(e => e.ContactId)
                    .HasColumnName("CONTACT_ID")
                    .HasDefaultValueSql("nextval('\"HET_CONTACT_ID_seq\"'::regclass)");

                entity.Property(e => e.Address1)
                    .HasColumnName("ADDRESS1")
                    .HasMaxLength(80);

                entity.Property(e => e.Address2)
                    .HasColumnName("ADDRESS2")
                    .HasMaxLength(80);

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.City)
                    .HasColumnName("CITY")
                    .HasMaxLength(100);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EmailAddress)
                    .HasColumnName("EMAIL_ADDRESS")
                    .HasMaxLength(255);

                entity.Property(e => e.FaxPhoneNumber)
                    .HasColumnName("FAX_PHONE_NUMBER")
                    .HasMaxLength(20);

                entity.Property(e => e.GivenName)
                    .HasColumnName("GIVEN_NAME")
                    .HasMaxLength(50);

                entity.Property(e => e.MobilePhoneNumber)
                    .HasColumnName("MOBILE_PHONE_NUMBER")
                    .HasMaxLength(20);

                entity.Property(e => e.Notes)
                    .HasColumnName("NOTES")
                    .HasMaxLength(512);

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.PostalCode)
                    .HasColumnName("POSTAL_CODE")
                    .HasMaxLength(15);

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.Province)
                    .HasColumnName("PROVINCE")
                    .HasMaxLength(50);

                entity.Property(e => e.Role)
                    .HasColumnName("ROLE")
                    .HasMaxLength(100);

                entity.Property(e => e.Surname)
                    .HasColumnName("SURNAME")
                    .HasMaxLength(50);

                entity.Property(e => e.WorkPhoneNumber)
                    .HasColumnName("WORK_PHONE_NUMBER")
                    .HasMaxLength(20);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetContact)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_HET_CONTACT_OWNER_ID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetContact)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_HET_CONTACT_PROJECT_ID");
            });

            modelBuilder.Entity<HetDigitalFile>(entity =>
            {
                entity.HasKey(e => e.DigitalFileId);

                entity.ToTable("HET_DIGITAL_FILE");

                entity.HasIndex(e => e.EquipmentId);

                entity.HasIndex(e => e.MimeTypeId);

                entity.HasIndex(e => e.OwnerId);

                entity.HasIndex(e => e.ProjectId);

                entity.HasIndex(e => e.RentalRequestId);

                entity.Property(e => e.DigitalFileId)
                    .HasColumnName("DIGITAL_FILE_ID")
                    .HasDefaultValueSql("nextval('\"HET_DIGITAL_FILE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048);

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.FileContents).HasColumnName("FILE_CONTENTS");

                entity.Property(e => e.FileName)
                    .HasColumnName("FILE_NAME")
                    .HasMaxLength(2048);

                entity.Property(e => e.MimeTypeId).HasColumnName("MIME_TYPE_ID");

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.Property(e => e.Type)
                    .HasColumnName("TYPE")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetDigitalFile)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK_HET_DIGITAL_FILE_EQUIPMENT_ID");

                entity.HasOne(d => d.MimeType)
                    .WithMany(p => p.HetDigitalFile)
                    .HasForeignKey(d => d.MimeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_DIGITAL_FILE_MIME_TYPE_ID");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetDigitalFile)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_HET_DIGITAL_FILE_OWNER_ID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetDigitalFile)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_HET_DIGITAL_FILE_PROJECT_ID");

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetDigitalFile)
                    .HasForeignKey(d => d.RentalRequestId)
                    .HasConstraintName("FK_HET_DIGITAL_FILE_RENTAL_REQUEST_ID");
            });

            modelBuilder.Entity<HetDistrict>(entity =>
            {
                entity.HasKey(e => e.DistrictId);

                entity.ToTable("HET_DISTRICT");

                entity.HasIndex(e => e.RegionId);

                entity.Property(e => e.DistrictId)
                    .HasColumnName("DISTRICT_ID")
                    .HasDefaultValueSql("nextval('\"HET_DISTRICT_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DistrictNumber).HasColumnName("DISTRICT_NUMBER");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.MinistryDistrictId).HasColumnName("MINISTRY_DISTRICT_ID");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.RegionId).HasColumnName("REGION_ID");

                entity.Property(e => e.StartDate).HasColumnName("START_DATE");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.HetDistrict)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_HET_DISTRICT_REGION_ID");
            });

            modelBuilder.Entity<HetDistrictEquipmentType>(entity =>
            {
                entity.HasKey(e => e.DistrictEquipmentTypeId);

                entity.ToTable("HET_DISTRICT_EQUIPMENT_TYPE");

                entity.HasIndex(e => e.DistrictId);

                entity.HasIndex(e => e.EquipmentTypeId);

                entity.Property(e => e.DistrictEquipmentTypeId)
                    .HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_DISTRICT_EQUIPMENT_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DistrictEquipmentName)
                    .HasColumnName("DISTRICT_EQUIPMENT_NAME")
                    .HasMaxLength(255);

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.Property(e => e.EquipmentTypeId).HasColumnName("EQUIPMENT_TYPE_ID");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetDistrictEquipmentType)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_HET_DISTRICT_EQUIPMENT_TYPE_DISTRICT_ID");

                entity.HasOne(d => d.EquipmentType)
                    .WithMany(p => p.HetDistrictEquipmentType)
                    .HasForeignKey(d => d.EquipmentTypeId)
                    .HasConstraintName("FK_HET_DISTRICT_EQUIPMENT_TYPE_ID");
            });

            modelBuilder.Entity<HetEquipment>(entity =>
            {
                entity.HasKey(e => e.EquipmentId);

                entity.ToTable("HET_EQUIPMENT");

                entity.HasIndex(e => e.DistrictEquipmentTypeId);

                entity.HasIndex(e => e.EquipmentStatusTypeId)
                    .HasName("IX_HET_EQUIPMENT_STATUS_TYPE_ID");

                entity.HasIndex(e => e.LocalAreaId);

                entity.HasIndex(e => e.OwnerId);

                entity.Property(e => e.EquipmentId)
                    .HasColumnName("EQUIPMENT_ID")
                    .HasDefaultValueSql("nextval('\"HET_EQUIPMENT_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ApprovedDate).HasColumnName("APPROVED_DATE");

                entity.Property(e => e.ArchiveCode)
                    .HasColumnName("ARCHIVE_CODE")
                    .HasMaxLength(50);

                entity.Property(e => e.ArchiveDate).HasColumnName("ARCHIVE_DATE");

                entity.Property(e => e.ArchiveReason)
                    .HasColumnName("ARCHIVE_REASON")
                    .HasMaxLength(2048);

                entity.Property(e => e.BlockNumber).HasColumnName("BLOCK_NUMBER");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DistrictEquipmentTypeId).HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID");

                entity.Property(e => e.EquipmentCode)
                    .HasColumnName("EQUIPMENT_CODE")
                    .HasMaxLength(25);

                entity.Property(e => e.EquipmentStatusTypeId).HasColumnName("EQUIPMENT_STATUS_TYPE_ID");

                entity.Property(e => e.InformationUpdateNeededReason)
                    .HasColumnName("INFORMATION_UPDATE_NEEDED_REASON")
                    .HasMaxLength(2048);

                entity.Property(e => e.IsInformationUpdateNeeded).HasColumnName("IS_INFORMATION_UPDATE_NEEDED");

                entity.Property(e => e.IsSeniorityOverridden).HasColumnName("IS_SENIORITY_OVERRIDDEN");

                entity.Property(e => e.LastVerifiedDate).HasColumnName("LAST_VERIFIED_DATE");

                entity.Property(e => e.LegalCapacity)
                    .HasColumnName("LEGAL_CAPACITY")
                    .HasMaxLength(150);

                entity.Property(e => e.LicencePlate)
                    .HasColumnName("LICENCE_PLATE")
                    .HasMaxLength(20);

                entity.Property(e => e.LicencedGvw)
                    .HasColumnName("LICENCED_GVW")
                    .HasMaxLength(150);

                entity.Property(e => e.LocalAreaId).HasColumnName("LOCAL_AREA_ID");

                entity.Property(e => e.Make)
                    .HasColumnName("MAKE")
                    .HasMaxLength(50);

                entity.Property(e => e.Model)
                    .HasColumnName("MODEL")
                    .HasMaxLength(50);

                entity.Property(e => e.NumberInBlock).HasColumnName("NUMBER_IN_BLOCK");

                entity.Property(e => e.Operator)
                    .HasColumnName("OPERATOR")
                    .HasMaxLength(255);

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.PayRate).HasColumnName("PAY_RATE");

                entity.Property(e => e.PupLegalCapacity)
                    .HasColumnName("PUP_LEGAL_CAPACITY")
                    .HasMaxLength(150);

                entity.Property(e => e.ReceivedDate).HasColumnName("RECEIVED_DATE");

                entity.Property(e => e.RefuseRate)
                    .HasColumnName("REFUSE_RATE")
                    .HasMaxLength(255);

                entity.Property(e => e.Seniority).HasColumnName("SENIORITY");

                entity.Property(e => e.SeniorityEffectiveDate).HasColumnName("SENIORITY_EFFECTIVE_DATE");

                entity.Property(e => e.SeniorityOverrideReason)
                    .HasColumnName("SENIORITY_OVERRIDE_REASON")
                    .HasMaxLength(2048);

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("SERIAL_NUMBER")
                    .HasMaxLength(100);

                entity.Property(e => e.ServiceHoursLastYear).HasColumnName("SERVICE_HOURS_LAST_YEAR");

                entity.Property(e => e.ServiceHoursThreeYearsAgo).HasColumnName("SERVICE_HOURS_THREE_YEARS_AGO");

                entity.Property(e => e.ServiceHoursTwoYearsAgo).HasColumnName("SERVICE_HOURS_TWO_YEARS_AGO");

                entity.Property(e => e.Size)
                    .HasColumnName("SIZE")
                    .HasMaxLength(128);

                entity.Property(e => e.StatusComment)
                    .HasColumnName("STATUS_COMMENT")
                    .HasMaxLength(255);

                entity.Property(e => e.ToDate).HasColumnName("TO_DATE");

                entity.Property(e => e.Type)
                    .HasColumnName("TYPE")
                    .HasMaxLength(50);

                entity.Property(e => e.Year)
                    .HasColumnName("YEAR")
                    .HasMaxLength(15);

                entity.Property(e => e.YearsOfService).HasColumnName("YEARS_OF_SERVICE");

                entity.HasOne(d => d.DistrictEquipmentType)
                    .WithMany(p => p.HetEquipment)
                    .HasForeignKey(d => d.DistrictEquipmentTypeId)
                    .HasConstraintName("FK_HET_EQUIPMENT_DISTRICT_EQUIPMENT_TYPE_ID");

                entity.HasOne(d => d.EquipmentStatusType)
                    .WithMany(p => p.HetEquipment)
                    .HasForeignKey(d => d.EquipmentStatusTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_EQUIPMENT_STATUS_TYPE_ID");

                entity.HasOne(d => d.LocalArea)
                    .WithMany(p => p.HetEquipment)
                    .HasForeignKey(d => d.LocalAreaId)
                    .HasConstraintName("FK_HET_EQUIPMENT_LOCAL_AREA_ID");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetEquipment)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_HET_EQUIPMENT_OWNER_ID");
            });

            modelBuilder.Entity<HetEquipmentAttachment>(entity =>
            {
                entity.HasKey(e => e.EquipmentAttachmentId);

                entity.ToTable("HET_EQUIPMENT_ATTACHMENT");

                entity.HasIndex(e => e.EquipmentId);

                entity.Property(e => e.EquipmentAttachmentId)
                    .HasColumnName("EQUIPMENT_ATTACHMENT_ID")
                    .HasDefaultValueSql("nextval('\"HET_EQUIPMENT_ATTACHMENT_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048);

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.TypeName)
                    .HasColumnName("TYPE_NAME")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetEquipmentAttachment)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK_HET_EQUIPMENT_ATTACHMENT_EQUIPMENT_ID");
            });

            modelBuilder.Entity<HetEquipmentAttachmentHist>(entity =>
            {
                entity.HasKey(e => e.EquipmentAttachmentHistId);

                entity.ToTable("HET_EQUIPMENT_ATTACHMENT_HIST");

                entity.Property(e => e.EquipmentAttachmentHistId)
                    .HasColumnName("EQUIPMENT_ATTACHMENT_HIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_EQUIPMENT_ATTACHMENT_HIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048);

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.EquipmentAttachmentId).HasColumnName("EQUIPMENT_ATTACHMENT_ID");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.TypeName)
                    .HasColumnName("TYPE_NAME")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<HetEquipmentHist>(entity =>
            {
                entity.HasKey(e => e.EquipmentHistId);

                entity.ToTable("HET_EQUIPMENT_HIST");

                entity.Property(e => e.EquipmentHistId)
                    .HasColumnName("EQUIPMENT_HIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_EQUIPMENT_HIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ApprovedDate).HasColumnName("APPROVED_DATE");

                entity.Property(e => e.ArchiveCode)
                    .HasColumnName("ARCHIVE_CODE")
                    .HasMaxLength(50);

                entity.Property(e => e.ArchiveDate).HasColumnName("ARCHIVE_DATE");

                entity.Property(e => e.ArchiveReason)
                    .HasColumnName("ARCHIVE_REASON")
                    .HasMaxLength(2048);

                entity.Property(e => e.BlockNumber).HasColumnName("BLOCK_NUMBER");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DistrictEquipmentTypeId).HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID");

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.EquipmentCode)
                    .HasColumnName("EQUIPMENT_CODE")
                    .HasMaxLength(25);

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.EquipmentStatusTypeId).HasColumnName("EQUIPMENT_STATUS_TYPE_ID");

                entity.Property(e => e.InformationUpdateNeededReason)
                    .HasColumnName("INFORMATION_UPDATE_NEEDED_REASON")
                    .HasMaxLength(2048);

                entity.Property(e => e.IsInformationUpdateNeeded).HasColumnName("IS_INFORMATION_UPDATE_NEEDED");

                entity.Property(e => e.IsSeniorityOverridden).HasColumnName("IS_SENIORITY_OVERRIDDEN");

                entity.Property(e => e.LastVerifiedDate).HasColumnName("LAST_VERIFIED_DATE");

                entity.Property(e => e.LegalCapacity)
                    .HasColumnName("LEGAL_CAPACITY")
                    .HasMaxLength(150);

                entity.Property(e => e.LicencePlate)
                    .HasColumnName("LICENCE_PLATE")
                    .HasMaxLength(20);

                entity.Property(e => e.LicencedGvw)
                    .HasColumnName("LICENCED_GVW")
                    .HasMaxLength(150);

                entity.Property(e => e.LocalAreaId).HasColumnName("LOCAL_AREA_ID");

                entity.Property(e => e.Make)
                    .HasColumnName("MAKE")
                    .HasMaxLength(50);

                entity.Property(e => e.Model)
                    .HasColumnName("MODEL")
                    .HasMaxLength(50);

                entity.Property(e => e.NumberInBlock).HasColumnName("NUMBER_IN_BLOCK");

                entity.Property(e => e.Operator)
                    .HasColumnName("OPERATOR")
                    .HasMaxLength(255);

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.PayRate).HasColumnName("PAY_RATE");

                entity.Property(e => e.PupLegalCapacity)
                    .HasColumnName("PUP_LEGAL_CAPACITY")
                    .HasMaxLength(150);

                entity.Property(e => e.ReceivedDate).HasColumnName("RECEIVED_DATE");

                entity.Property(e => e.RefuseRate)
                    .HasColumnName("REFUSE_RATE")
                    .HasMaxLength(255);

                entity.Property(e => e.Seniority).HasColumnName("SENIORITY");

                entity.Property(e => e.SeniorityEffectiveDate).HasColumnName("SENIORITY_EFFECTIVE_DATE");

                entity.Property(e => e.SeniorityOverrideReason)
                    .HasColumnName("SENIORITY_OVERRIDE_REASON")
                    .HasMaxLength(2048);

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("SERIAL_NUMBER")
                    .HasMaxLength(100);

                entity.Property(e => e.ServiceHoursLastYear).HasColumnName("SERVICE_HOURS_LAST_YEAR");

                entity.Property(e => e.ServiceHoursThreeYearsAgo).HasColumnName("SERVICE_HOURS_THREE_YEARS_AGO");

                entity.Property(e => e.ServiceHoursTwoYearsAgo).HasColumnName("SERVICE_HOURS_TWO_YEARS_AGO");

                entity.Property(e => e.Size)
                    .HasColumnName("SIZE")
                    .HasMaxLength(128);

                entity.Property(e => e.StatusComment)
                    .HasColumnName("STATUS_COMMENT")
                    .HasMaxLength(255);

                entity.Property(e => e.ToDate).HasColumnName("TO_DATE");

                entity.Property(e => e.Type)
                    .HasColumnName("TYPE")
                    .HasMaxLength(50);

                entity.Property(e => e.Year)
                    .HasColumnName("YEAR")
                    .HasMaxLength(15);

                entity.Property(e => e.YearsOfService).HasColumnName("YEARS_OF_SERVICE");
            });

            modelBuilder.Entity<HetEquipmentStatusType>(entity =>
            {
                entity.HasKey(e => e.EquipmentStatusTypeId);

                entity.ToTable("HET_EQUIPMENT_STATUS_TYPE");

                entity.HasIndex(e => e.EquipmentStatusTypeCode)
                    .HasName("UK_HET_EQUIPMENT_STATUS_TYPE_CODE")
                    .IsUnique();

                entity.Property(e => e.EquipmentStatusTypeId)
                    .HasColumnName("EQUIPMENT_STATUS_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_EQUIPMENT_STATUS_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048);

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.EquipmentStatusTypeCode)
                    .IsRequired()
                    .HasColumnName("EQUIPMENT_STATUS_TYPE_CODE")
                    .HasMaxLength(20);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.ScreenLabel)
                    .HasColumnName("SCREEN_LABEL")
                    .HasMaxLength(200);
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
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.BlueBookRateNumber).HasColumnName("BLUE_BOOK_RATE_NUMBER");

                entity.Property(e => e.BlueBookSection).HasColumnName("BLUE_BOOK_SECTION");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.ExtendHours).HasColumnName("EXTEND_HOURS");

                entity.Property(e => e.IsDumpTruck).HasColumnName("IS_DUMP_TRUCK");

                entity.Property(e => e.MaxHoursSub).HasColumnName("MAX_HOURS_SUB");

                entity.Property(e => e.MaximumHours).HasColumnName("MAXIMUM_HOURS");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.NumberOfBlocks).HasColumnName("NUMBER_OF_BLOCKS");
            });

            modelBuilder.Entity<HetHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId);

                entity.ToTable("HET_HISTORY");

                entity.HasIndex(e => e.EquipmentId);

                entity.HasIndex(e => e.OwnerId);

                entity.HasIndex(e => e.ProjectId);

                entity.HasIndex(e => e.RentalRequestId);

                entity.Property(e => e.HistoryId)
                    .HasColumnName("HISTORY_ID")
                    .HasDefaultValueSql("nextval('\"HET_HISTORY_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.CreatedDate).HasColumnName("CREATED_DATE");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.HistoryText)
                    .HasColumnName("HISTORY_TEXT")
                    .HasMaxLength(2048);

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetHistory)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK_HET_HISTORY_EQUIPMENT_ID");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetHistory)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_HET_HISTORY_OWNER_ID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetHistory)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_HET_HISTORY_PROJECT_ID");

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetHistory)
                    .HasForeignKey(d => d.RentalRequestId)
                    .HasConstraintName("FK_HET_HISTORY_RENTAL_REQUEST_ID");
            });

            modelBuilder.Entity<HetImportMap>(entity =>
            {
                entity.HasKey(e => e.ImportMapId);

                entity.ToTable("HET_IMPORT_MAP");

                entity.Property(e => e.ImportMapId)
                    .HasColumnName("IMPORT_MAP_ID")
                    .HasDefaultValueSql("nextval('\"HET_IMPORT_MAP_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.NewKey).HasColumnName("NEW_KEY");

                entity.Property(e => e.NewTable).HasColumnName("NEW_TABLE");

                entity.Property(e => e.OldKey)
                    .HasColumnName("OLD_KEY")
                    .HasMaxLength(250);

                entity.Property(e => e.OldTable).HasColumnName("OLD_TABLE");
            });

            modelBuilder.Entity<HetLocalArea>(entity =>
            {
                entity.HasKey(e => e.LocalAreaId);

                entity.ToTable("HET_LOCAL_AREA");

                entity.HasIndex(e => e.LocalAreaNumber)
                    .HasName("HET_LOCAL_AREA_NUMBER_UK")
                    .IsUnique();

                entity.HasIndex(e => e.ServiceAreaId);

                entity.Property(e => e.LocalAreaId)
                    .HasColumnName("LOCAL_AREA_ID")
                    .HasDefaultValueSql("nextval('\"HET_LOCAL_AREA_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.LocalAreaNumber).HasColumnName("LOCAL_AREA_NUMBER");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.ServiceAreaId).HasColumnName("SERVICE_AREA_ID");

                entity.Property(e => e.StartDate)
                    .HasColumnName("START_DATE")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.HasOne(d => d.ServiceArea)
                    .WithMany(p => p.HetLocalArea)
                    .HasForeignKey(d => d.ServiceAreaId)
                    .HasConstraintName("FK_HET_LOCAL_AREA_SERVICE_AREA_ID");
            });

            modelBuilder.Entity<HetLocalAreaRotationList>(entity =>
            {
                entity.HasKey(e => e.LocalAreaRotationListId);

                entity.ToTable("HET_LOCAL_AREA_ROTATION_LIST");

                entity.HasIndex(e => e.AskNextBlock1Id);

                entity.HasIndex(e => e.AskNextBlock2Id);

                entity.HasIndex(e => e.AskNextBlockOpenId);

                entity.HasIndex(e => e.DistrictEquipmentTypeId);

                entity.HasIndex(e => e.LocalAreaId);

                entity.Property(e => e.LocalAreaRotationListId)
                    .HasColumnName("LOCAL_AREA_ROTATION_LIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_LOCAL_AREA_ROTATION_LIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AskNextBlock1Id).HasColumnName("ASK_NEXT_BLOCK1_ID");

                entity.Property(e => e.AskNextBlock1Seniority).HasColumnName("ASK_NEXT_BLOCK1_SENIORITY");

                entity.Property(e => e.AskNextBlock2Id).HasColumnName("ASK_NEXT_BLOCK2_ID");

                entity.Property(e => e.AskNextBlock2Seniority).HasColumnName("ASK_NEXT_BLOCK2_SENIORITY");

                entity.Property(e => e.AskNextBlockOpenId).HasColumnName("ASK_NEXT_BLOCK_OPEN_ID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DistrictEquipmentTypeId).HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID");

                entity.Property(e => e.LocalAreaId).HasColumnName("LOCAL_AREA_ID");

                entity.HasOne(d => d.AskNextBlock1)
                    .WithMany(p => p.HetLocalAreaRotationListAskNextBlock1)
                    .HasForeignKey(d => d.AskNextBlock1Id)
                    .HasConstraintName("FK_HET_LOCAL_AREA_ROTATION_LIST_ASK_NEXT_BLOCK1_ID");

                entity.HasOne(d => d.AskNextBlock2)
                    .WithMany(p => p.HetLocalAreaRotationListAskNextBlock2)
                    .HasForeignKey(d => d.AskNextBlock2Id)
                    .HasConstraintName("FK_HET_LOCAL_AREA_ROTATION_LIST_ASK_NEXT_BLOCK2_ID");

                entity.HasOne(d => d.AskNextBlockOpen)
                    .WithMany(p => p.HetLocalAreaRotationListAskNextBlockOpen)
                    .HasForeignKey(d => d.AskNextBlockOpenId)
                    .HasConstraintName("FK_HET_LOCAL_AREA_ROTATION_LIST_ASK_NEXT_BLOCK_OPEN_ID");

                entity.HasOne(d => d.DistrictEquipmentType)
                    .WithMany(p => p.HetLocalAreaRotationList)
                    .HasForeignKey(d => d.DistrictEquipmentTypeId)
                    .HasConstraintName("FK_HET_LOCAL_AREA_ROTATION_LIST_DISTRICT_EQUIPMENT_TYPE_ID");

                entity.HasOne(d => d.LocalArea)
                    .WithMany(p => p.HetLocalAreaRotationList)
                    .HasForeignKey(d => d.LocalAreaId)
                    .HasConstraintName("FK_HET_LOCAL_AREA_ROTATION_LIST_LOCAL_AREA_ID");
            });

            modelBuilder.Entity<HetMimeType>(entity =>
            {
                entity.HasKey(e => e.MimeTypeId);

                entity.ToTable("HET_MIME_TYPE");

                entity.HasIndex(e => e.MimeTypeCode)
                    .HasName("UK_HET_MIME_TYPE_CODE")
                    .IsUnique();

                entity.Property(e => e.MimeTypeId)
                    .HasColumnName("MIME_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_MIME_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048);

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.MimeTypeCode)
                    .IsRequired()
                    .HasColumnName("MIME_TYPE_CODE")
                    .HasMaxLength(20);

                entity.Property(e => e.ScreenLabel)
                    .HasColumnName("SCREEN_LABEL")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<HetNote>(entity =>
            {
                entity.HasKey(e => e.NoteId);

                entity.ToTable("HET_NOTE");

                entity.HasIndex(e => e.EquipmentId);

                entity.HasIndex(e => e.OwnerId);

                entity.HasIndex(e => e.ProjectId);

                entity.HasIndex(e => e.RentalRequestId);

                entity.Property(e => e.NoteId)
                    .HasColumnName("NOTE_ID")
                    .HasDefaultValueSql("nextval('\"HET_NOTE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.IsNoLongerRelevant).HasColumnName("IS_NO_LONGER_RELEVANT");

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.Property(e => e.Text)
                    .HasColumnName("TEXT")
                    .HasMaxLength(2048);

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetNote)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK_HET_NOTE_EQUIPMENT_ID");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetNote)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_HET_NOTE_OWNER_ID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetNote)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_HET_NOTE_PROJECT_ID");

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetNote)
                    .HasForeignKey(d => d.RentalRequestId)
                    .HasConstraintName("FK_HET_NOTE_RENTAL_REQUEST_ID");
            });

            modelBuilder.Entity<HetNoteHist>(entity =>
            {
                entity.HasKey(e => e.NoteHistId);

                entity.ToTable("HET_NOTE_HIST");

                entity.Property(e => e.NoteHistId)
                    .HasColumnName("NOTE_HIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_NOTE_HIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.IsNoLongerRelevant).HasColumnName("IS_NO_LONGER_RELEVANT");

                entity.Property(e => e.NoteId).HasColumnName("NOTE_ID");

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.Property(e => e.Text)
                    .HasColumnName("TEXT")
                    .HasMaxLength(2048);
            });

            modelBuilder.Entity<HetOwner>(entity =>
            {
                entity.HasKey(e => e.OwnerId);

                entity.ToTable("HET_OWNER");

                entity.HasIndex(e => e.BusinessId);

                entity.HasIndex(e => e.LocalAreaId);

                entity.HasIndex(e => e.OwnerStatusTypeId)
                    .HasName("IX_HET_OWNER_STATUS_TYPE_ID");

                entity.HasIndex(e => e.PrimaryContactId);

                entity.HasIndex(e => e.RegisteredCompanyNumber)
                    .HasName("HET_OWN_REGISTERED_COMPANY_NUMBER_UK")
                    .IsUnique();

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OWNER_ID")
                    .HasDefaultValueSql("nextval('\"HET_OWNER_ID_seq\"'::regclass)");

                entity.Property(e => e.Address1)
                    .HasColumnName("ADDRESS1")
                    .HasMaxLength(80);

                entity.Property(e => e.Address2)
                    .HasColumnName("ADDRESS2")
                    .HasMaxLength(80);

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ArchiveCode)
                    .HasColumnName("ARCHIVE_CODE")
                    .HasMaxLength(50);

                entity.Property(e => e.ArchiveDate).HasColumnName("ARCHIVE_DATE");

                entity.Property(e => e.ArchiveReason)
                    .HasColumnName("ARCHIVE_REASON")
                    .HasMaxLength(2048);

                entity.Property(e => e.BusinessId).HasColumnName("BUSINESS_ID");

                entity.Property(e => e.CglPolicyNumber)
                    .HasColumnName("CGL_POLICY_NUMBER")
                    .HasMaxLength(50);

                entity.Property(e => e.CglendDate).HasColumnName("CGLEND_DATE");

                entity.Property(e => e.City)
                    .HasColumnName("CITY")
                    .HasMaxLength(100);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DoingBusinessAs)
                    .HasColumnName("DOING_BUSINESS_AS")
                    .HasMaxLength(150);

                entity.Property(e => e.GivenName)
                    .HasColumnName("GIVEN_NAME")
                    .HasMaxLength(50);

                entity.Property(e => e.IsMaintenanceContractor).HasColumnName("IS_MAINTENANCE_CONTRACTOR");

                entity.Property(e => e.LocalAreaId).HasColumnName("LOCAL_AREA_ID");

                entity.Property(e => e.MeetsResidency).HasColumnName("MEETS_RESIDENCY");

                entity.Property(e => e.OrganizationName)
                    .HasColumnName("ORGANIZATION_NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.OwnerCode)
                    .HasColumnName("OWNER_CODE")
                    .HasMaxLength(20);

                entity.Property(e => e.OwnerStatusTypeId).HasColumnName("OWNER_STATUS_TYPE_ID");

                entity.Property(e => e.PostalCode)
                    .HasColumnName("POSTAL_CODE")
                    .HasMaxLength(15);

                entity.Property(e => e.PrimaryContactId).HasColumnName("PRIMARY_CONTACT_ID");

                entity.Property(e => e.Province)
                    .HasColumnName("PROVINCE")
                    .HasMaxLength(50);

                entity.Property(e => e.RegisteredCompanyNumber)
                    .HasColumnName("REGISTERED_COMPANY_NUMBER")
                    .HasMaxLength(150);

                entity.Property(e => e.SharedKey)
                    .HasColumnName("SHARED_KEY")
                    .HasMaxLength(50);

                entity.Property(e => e.StatusComment)
                    .HasColumnName("STATUS_COMMENT")
                    .HasMaxLength(255);

                entity.Property(e => e.Surname)
                    .HasColumnName("SURNAME")
                    .HasMaxLength(50);

                entity.Property(e => e.WorkSafeBcexpiryDate).HasColumnName("WORK_SAFE_BCEXPIRY_DATE");

                entity.Property(e => e.WorkSafeBcpolicyNumber)
                    .HasColumnName("WORK_SAFE_BCPOLICY_NUMBER")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.HetOwner)
                    .HasForeignKey(d => d.BusinessId)
                    .HasConstraintName("FK_HET_OWNER_BUSINESS_ID");
            });

            modelBuilder.Entity<HetOwnerStatusType>(entity =>
            {
                entity.HasKey(e => e.OwnerStatusTypeId);

                entity.ToTable("HET_OWNER_STATUS_TYPE");

                entity.HasIndex(e => e.OwnerStatusTypeCode)
                    .HasName("UK_HET_OWNER_STATUS_TYPE_CODE")
                    .IsUnique();

                entity.Property(e => e.OwnerStatusTypeId)
                    .HasColumnName("OWNER_STATUS_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_OWNER_STATUS_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048);

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.OwnerStatusTypeCode)
                    .IsRequired()
                    .HasColumnName("OWNER_STATUS_TYPE_CODE")
                    .HasMaxLength(20);

                entity.Property(e => e.ScreenLabel)
                    .HasColumnName("SCREEN_LABEL")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<HetPermission>(entity =>
            {
                entity.HasKey(e => e.PermissionId);

                entity.ToTable("HET_PERMISSION");

                entity.HasIndex(e => e.Code)
                    .HasName("HET_PRM_CODE_UK")
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .HasName("HET_PRM_NAME_UK")
                    .IsUnique();

                entity.Property(e => e.PermissionId)
                    .HasColumnName("PERMISSION_ID")
                    .HasDefaultValueSql("nextval('\"HET_PERMISSION_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.Code)
                    .HasColumnName("CODE")
                    .HasMaxLength(50);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048);

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<HetPerson>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                entity.ToTable("HET_PERSON");

                entity.Property(e => e.PersonId)
                    .HasColumnName("PERSON_ID")
                    .HasDefaultValueSql("nextval('\"HET_PERSON_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.FirstName)
                    .HasColumnName("FIRST_NAME")
                    .HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.MiddleNames)
                    .HasColumnName("MIDDLE_NAMES")
                    .HasMaxLength(200);

                entity.Property(e => e.NameSuffix)
                    .HasColumnName("NAME_SUFFIX")
                    .HasMaxLength(50);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasColumnName("SURNAME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<HetProject>(entity =>
            {
                entity.HasKey(e => e.ProjectId);

                entity.ToTable("HET_PROJECT");

                entity.HasIndex(e => e.DistrictId);

                entity.HasIndex(e => e.PrimaryContactId);

                entity.HasIndex(e => e.ProjectStatusTypeId)
                    .HasName("IX_HET_PROJECT_STATUS_TYPE_ID");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("PROJECT_ID")
                    .HasDefaultValueSql("nextval('\"HET_PROJECT_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.Property(e => e.Information)
                    .HasColumnName("INFORMATION")
                    .HasMaxLength(2048);

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(100);

                entity.Property(e => e.PrimaryContactId).HasColumnName("PRIMARY_CONTACT_ID");

                entity.Property(e => e.ProjectStatusTypeId).HasColumnName("PROJECT_STATUS_TYPE_ID");

                entity.Property(e => e.ProvincialProjectNumber)
                    .HasColumnName("PROVINCIAL_PROJECT_NUMBER")
                    .HasMaxLength(150);

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetProject)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_HET_PROJECT_DISTRICT_ID");

                entity.HasOne(d => d.PrimaryContact)
                    .WithMany(p => p.HetProject)
                    .HasForeignKey(d => d.PrimaryContactId)
                    .HasConstraintName("FK_HET_PROJECT_PRIMARY_CONTACT_ID");

                entity.HasOne(d => d.ProjectStatusType)
                    .WithMany(p => p.HetProject)
                    .HasForeignKey(d => d.ProjectStatusTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_PROJECT_HET_PROJECT_STATUS_TYPE_ID");
            });

            modelBuilder.Entity<HetProjectStatusType>(entity =>
            {
                entity.HasKey(e => e.ProjectStatusTypeId);

                entity.ToTable("HET_PROJECT_STATUS_TYPE");

                entity.HasIndex(e => e.ProjectStatusTypeCode)
                    .HasName("UK_HET_PROJECT_STATUS_TYPE_CODE")
                    .IsUnique();

                entity.Property(e => e.ProjectStatusTypeId)
                    .HasColumnName("PROJECT_STATUS_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_PROJECT_STATUS_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048);

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.ProjectStatusTypeCode)
                    .IsRequired()
                    .HasColumnName("PROJECT_STATUS_TYPE_CODE")
                    .HasMaxLength(20);

                entity.Property(e => e.ScreenLabel)
                    .HasColumnName("SCREEN_LABEL")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<HetProvincialRateType>(entity =>
            {
                entity.HasKey(e => e.RateType);

                entity.ToTable("HET_PROVINCIAL_RATE_TYPE");

                entity.Property(e => e.RateType)
                    .HasColumnName("RATE_TYPE")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.Active).HasColumnName("ACTIVE");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(200);

                entity.Property(e => e.IsInTotalEditable).HasColumnName("IS_IN_TOTAL_EDITABLE");

                entity.Property(e => e.IsIncludedInTotal).HasColumnName("IS_INCLUDED_IN_TOTAL");

                entity.Property(e => e.IsPercentRate).HasColumnName("IS_PERCENT_RATE");

                entity.Property(e => e.IsRateEditable).HasColumnName("IS_RATE_EDITABLE");

                entity.Property(e => e.PeriodType)
                    .HasColumnName("PERIOD_TYPE")
                    .HasMaxLength(20);

                entity.Property(e => e.Rate).HasColumnName("RATE");
            });

            modelBuilder.Entity<HetRatePeriodType>(entity =>
            {
                entity.HasKey(e => e.RatePeriodTypeId);

                entity.ToTable("HET_RATE_PERIOD_TYPE");

                entity.HasIndex(e => e.RatePeriodTypeCode)
                    .HasName("UK_HET_RATE_PERIOD_TYPE_CODE")
                    .IsUnique();

                entity.Property(e => e.RatePeriodTypeId)
                    .HasColumnName("RATE_PERIOD_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_RATE_PERIOD_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048);

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.RatePeriodTypeCode)
                    .IsRequired()
                    .HasColumnName("RATE_PERIOD_TYPE_CODE")
                    .HasMaxLength(20);

                entity.Property(e => e.ScreenLabel)
                    .HasColumnName("SCREEN_LABEL")
                    .HasMaxLength(200);
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
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.MinistryRegionId).HasColumnName("MINISTRY_REGION_ID");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.RegionNumber).HasColumnName("REGION_NUMBER");

                entity.Property(e => e.StartDate).HasColumnName("START_DATE");
            });

            modelBuilder.Entity<HetRentalAgreement>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementId);

                entity.ToTable("HET_RENTAL_AGREEMENT");

                entity.HasIndex(e => e.EquipmentId);

                entity.HasIndex(e => e.Number)
                    .HasName("HET_RNTAG_NUMBER_UK")
                    .IsUnique();

                entity.HasIndex(e => e.ProjectId);

                entity.HasIndex(e => e.RatePeriodTypeId)
                    .HasName("IX_HET_RENTAL_AGREEMENT_HET_RATE_PERIOD_TYPE_ID");

                entity.HasIndex(e => e.RentalAgreementStatusTypeId)
                    .HasName("IX_HET_RENTAL_AGREEMENT_HET_RENTAL_AGREEMENT_STATUS_TYPE_ID");

                entity.Property(e => e.RentalAgreementId)
                    .HasColumnName("RENTAL_AGREEMENT_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_AGREEMENT_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DatedOn).HasColumnName("DATED_ON");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.EquipmentRate).HasColumnName("EQUIPMENT_RATE");

                entity.Property(e => e.EstimateHours).HasColumnName("ESTIMATE_HOURS");

                entity.Property(e => e.EstimateStartWork).HasColumnName("ESTIMATE_START_WORK");

                entity.Property(e => e.Note)
                    .HasColumnName("NOTE")
                    .HasMaxLength(2048);

                entity.Property(e => e.Number)
                    .HasColumnName("NUMBER")
                    .HasMaxLength(30);

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.RateComment)
                    .HasColumnName("RATE_COMMENT")
                    .HasMaxLength(2048);

                entity.Property(e => e.RatePeriodTypeId).HasColumnName("RATE_PERIOD_TYPE_ID");

                entity.Property(e => e.RentalAgreementStatusTypeId).HasColumnName("RENTAL_AGREEMENT_STATUS_TYPE_ID");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetRentalAgreement)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_EQUIPMENT_ID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetRentalAgreement)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_PROJECT_ID");

                entity.HasOne(d => d.RatePeriodType)
                    .WithMany(p => p.HetRentalAgreement)
                    .HasForeignKey(d => d.RatePeriodTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_RATE_PERIOD_TYPE_ID");

                entity.HasOne(d => d.RentalAgreementStatusType)
                    .WithMany(p => p.HetRentalAgreement)
                    .HasForeignKey(d => d.RentalAgreementStatusTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_STATUS_TYPE_ID");
            });

            modelBuilder.Entity<HetRentalAgreementCondition>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementConditionId);

                entity.ToTable("HET_RENTAL_AGREEMENT_CONDITION");

                entity.HasIndex(e => e.RentalAgreementId);

                entity.Property(e => e.RentalAgreementConditionId)
                    .HasColumnName("RENTAL_AGREEMENT_CONDITION_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_AGREEMENT_CONDITION_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.Comment)
                    .HasColumnName("COMMENT")
                    .HasMaxLength(2048);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.ConditionName)
                    .HasColumnName("CONDITION_NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.HasOne(d => d.RentalAgreement)
                    .WithMany(p => p.HetRentalAgreementCondition)
                    .HasForeignKey(d => d.RentalAgreementId)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_CONDITION_RENTAL_AGREEMENT_ID");
            });

            modelBuilder.Entity<HetRentalAgreementConditionHist>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementConditionHistId);

                entity.ToTable("HET_RENTAL_AGREEMENT_CONDITION_HIST");

                entity.Property(e => e.RentalAgreementConditionHistId)
                    .HasColumnName("RENTAL_AGREEMENT_CONDITION_HIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_AGREEMENT_CONDITION_HIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.Comment)
                    .HasColumnName("COMMENT")
                    .HasMaxLength(2048);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.ConditionName)
                    .HasColumnName("CONDITION_NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.RentalAgreementConditionId).HasColumnName("RENTAL_AGREEMENT_CONDITION_ID");

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");
            });

            modelBuilder.Entity<HetRentalAgreementHist>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementHistId);

                entity.ToTable("HET_RENTAL_AGREEMENT_HIST");

                entity.Property(e => e.RentalAgreementHistId)
                    .HasColumnName("RENTAL_AGREEMENT_HIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_AGREEMENT_HIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DatedOn).HasColumnName("DATED_ON");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.EquipmentRate).HasColumnName("EQUIPMENT_RATE");

                entity.Property(e => e.EstimateHours).HasColumnName("ESTIMATE_HOURS");

                entity.Property(e => e.EstimateStartWork).HasColumnName("ESTIMATE_START_WORK");

                entity.Property(e => e.Note)
                    .HasColumnName("NOTE")
                    .HasMaxLength(2048);

                entity.Property(e => e.Number)
                    .HasColumnName("NUMBER")
                    .HasMaxLength(30);

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.RateComment)
                    .HasColumnName("RATE_COMMENT")
                    .HasMaxLength(2048);

                entity.Property(e => e.RatePeriodTypeId).HasColumnName("RATE_PERIOD_TYPE_ID");

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.Property(e => e.RentalAgreementStatusTypeId).HasColumnName("RENTAL_AGREEMENT_STATUS_TYPE_ID");
            });

            modelBuilder.Entity<HetRentalAgreementRate>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementRateId);

                entity.ToTable("HET_RENTAL_AGREEMENT_RATE");

                entity.HasIndex(e => e.RatePeriodTypeId)
                    .HasName("IX_HET_RENTAL_AGREEMENT_RATE_HET_RATE_PERIOD_TYPE_ID");

                entity.HasIndex(e => e.RentalAgreementId);

                entity.Property(e => e.RentalAgreementRateId)
                    .HasColumnName("RENTAL_AGREEMENT_RATE_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_AGREEMENT_RATE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.Comment)
                    .HasColumnName("COMMENT")
                    .HasMaxLength(2048);

                entity.Property(e => e.ComponentName)
                    .HasColumnName("COMPONENT_NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.IsAttachment).HasColumnName("IS_ATTACHMENT");

                entity.Property(e => e.IsIncludedInTotal).HasColumnName("IS_INCLUDED_IN_TOTAL");

                entity.Property(e => e.PercentOfEquipmentRate).HasColumnName("PERCENT_OF_EQUIPMENT_RATE");

                entity.Property(e => e.Rate).HasColumnName("RATE");

                entity.Property(e => e.RatePeriodTypeId).HasColumnName("RATE_PERIOD_TYPE_ID");

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.HasOne(d => d.RatePeriodType)
                    .WithMany(p => p.HetRentalAgreementRate)
                    .HasForeignKey(d => d.RatePeriodTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_RENTAL_RATE_AGREEMENT_PERIOD_TYPE_ID");

                entity.HasOne(d => d.RentalAgreement)
                    .WithMany(p => p.HetRentalAgreementRate)
                    .HasForeignKey(d => d.RentalAgreementId)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_RATE_AGREEMENT_ID");
            });

            modelBuilder.Entity<HetRentalAgreementRateHist>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementRateHistId);

                entity.ToTable("HET_RENTAL_AGREEMENT_RATE_HIST");

                entity.Property(e => e.RentalAgreementRateHistId)
                    .HasColumnName("RENTAL_AGREEMENT_RATE_HIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_AGREEMENT_RATE_HIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.Comment)
                    .HasColumnName("COMMENT")
                    .HasMaxLength(2048);

                entity.Property(e => e.ComponentName)
                    .HasColumnName("COMPONENT_NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.IsAttachment).HasColumnName("IS_ATTACHMENT");

                entity.Property(e => e.IsIncludedInTotal).HasColumnName("IS_INCLUDED_IN_TOTAL");

                entity.Property(e => e.PercentOfEquipmentRate).HasColumnName("PERCENT_OF_EQUIPMENT_RATE");

                entity.Property(e => e.Rate).HasColumnName("RATE");

                entity.Property(e => e.RatePeriodTypeId).HasColumnName("RATE_PERIOD_TYPE_ID");

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.Property(e => e.RentalAgreementRateId).HasColumnName("RENTAL_AGREEMENT_RATE_ID");
            });

            modelBuilder.Entity<HetRentalAgreementStatusType>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementStatusTypeId);

                entity.ToTable("HET_RENTAL_AGREEMENT_STATUS_TYPE");

                entity.HasIndex(e => e.RentalAgreementStatusTypeCode)
                    .HasName("UK_HET_RENTAL_AGREEMENT_STATUS_TYPE_CODE")
                    .IsUnique();

                entity.Property(e => e.RentalAgreementStatusTypeId)
                    .HasColumnName("RENTAL_AGREEMENT_STATUS_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_AGREEMENT_STATUS_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048);

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.RentalAgreementStatusTypeCode)
                    .IsRequired()
                    .HasColumnName("RENTAL_AGREEMENT_STATUS_TYPE_CODE")
                    .HasMaxLength(20);

                entity.Property(e => e.ScreenLabel)
                    .HasColumnName("SCREEN_LABEL")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<HetRentalRequest>(entity =>
            {
                entity.HasKey(e => e.RentalRequestId);

                entity.ToTable("HET_RENTAL_REQUEST");

                entity.HasIndex(e => e.DistrictEquipmentTypeId);

                entity.HasIndex(e => e.FirstOnRotationListId);

                entity.HasIndex(e => e.LocalAreaId);

                entity.HasIndex(e => e.ProjectId);

                entity.HasIndex(e => e.RentalRequestStatusTypeId)
                    .HasName("IX_HET_RENTAL_REQUEST_STATUS_TYPE_ID");

                entity.Property(e => e.RentalRequestId)
                    .HasColumnName("RENTAL_REQUEST_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_REQUEST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DistrictEquipmentTypeId).HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID");

                entity.Property(e => e.EquipmentCount).HasColumnName("EQUIPMENT_COUNT");

                entity.Property(e => e.ExpectedEndDate).HasColumnName("EXPECTED_END_DATE");

                entity.Property(e => e.ExpectedHours).HasColumnName("EXPECTED_HOURS");

                entity.Property(e => e.ExpectedStartDate).HasColumnName("EXPECTED_START_DATE");

                entity.Property(e => e.FirstOnRotationListId).HasColumnName("FIRST_ON_ROTATION_LIST_ID");

                entity.Property(e => e.LocalAreaId).HasColumnName("LOCAL_AREA_ID");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.RentalRequestStatusTypeId).HasColumnName("RENTAL_REQUEST_STATUS_TYPE_ID");

                entity.HasOne(d => d.DistrictEquipmentType)
                    .WithMany(p => p.HetRentalRequest)
                    .HasForeignKey(d => d.DistrictEquipmentTypeId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_DISTRICT_EQUIPMENT_TYPE_DISTRICT_ID");

                entity.HasOne(d => d.FirstOnRotationList)
                    .WithMany(p => p.HetRentalRequest)
                    .HasForeignKey(d => d.FirstOnRotationListId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_FIRST_ON_ROTATION_LIST_ID");

                entity.HasOne(d => d.LocalArea)
                    .WithMany(p => p.HetRentalRequest)
                    .HasForeignKey(d => d.LocalAreaId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_LOCAL_AREA_ID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetRentalRequest)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_PROJECT_ID");

                entity.HasOne(d => d.RentalRequestStatusType)
                    .WithMany(p => p.HetRentalRequest)
                    .HasForeignKey(d => d.RentalRequestStatusTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_STATUS_TYPE_ID");
            });

            modelBuilder.Entity<HetRentalRequestAttachment>(entity =>
            {
                entity.HasKey(e => e.RentalRequestAttachmentId);

                entity.ToTable("HET_RENTAL_REQUEST_ATTACHMENT");

                entity.HasIndex(e => e.RentalRequestId);

                entity.Property(e => e.RentalRequestAttachmentId)
                    .HasColumnName("RENTAL_REQUEST_ATTACHMENT_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_REQUEST_ATTACHMENT_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.Attachment)
                    .HasColumnName("ATTACHMENT")
                    .HasMaxLength(150);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetRentalRequestAttachment)
                    .HasForeignKey(d => d.RentalRequestId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_ATTACHMENT_RENTAL_REQUEST_ID");
            });

            modelBuilder.Entity<HetRentalRequestRotationList>(entity =>
            {
                entity.HasKey(e => e.RentalRequestRotationListId);

                entity.ToTable("HET_RENTAL_REQUEST_ROTATION_LIST");

                entity.HasIndex(e => e.EquipmentId);

                entity.HasIndex(e => e.RentalAgreementId);

                entity.HasIndex(e => e.RentalRequestId);

                entity.Property(e => e.RentalRequestRotationListId)
                    .HasColumnName("RENTAL_REQUEST_ROTATION_LIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_REQUEST_ROTATION_LIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AskedDateTime).HasColumnName("ASKED_DATE_TIME");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.IsForceHire).HasColumnName("IS_FORCE_HIRE");

                entity.Property(e => e.Note)
                    .HasColumnName("NOTE")
                    .HasMaxLength(2048);

                entity.Property(e => e.OfferRefusalReason)
                    .HasColumnName("OFFER_REFUSAL_REASON")
                    .HasMaxLength(50);

                entity.Property(e => e.OfferResponse).HasColumnName("OFFER_RESPONSE");

                entity.Property(e => e.OfferResponseDatetime).HasColumnName("OFFER_RESPONSE_DATETIME");

                entity.Property(e => e.OfferResponseNote)
                    .HasColumnName("OFFER_RESPONSE_NOTE")
                    .HasMaxLength(2048);

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.Property(e => e.RotationListSortOrder).HasColumnName("ROTATION_LIST_SORT_ORDER");

                entity.Property(e => e.WasAsked).HasColumnName("WAS_ASKED");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetRentalRequestRotationList)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_ROTATION_LIST_EQUIPMENT_ID");

                entity.HasOne(d => d.RentalAgreement)
                    .WithMany(p => p.HetRentalRequestRotationList)
                    .HasForeignKey(d => d.RentalAgreementId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_ROTATION_LIST_RENTAL_AGREEMENT_ID");

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetRentalRequestRotationList)
                    .HasForeignKey(d => d.RentalRequestId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_ROTATION_LIST_RENTAL_REQUEST_ID");
            });

            modelBuilder.Entity<HetRentalRequestRotationListHist>(entity =>
            {
                entity.HasKey(e => e.RentalRequestRotationListHistId);

                entity.ToTable("HET_RENTAL_REQUEST_ROTATION_LIST_HIST");

                entity.Property(e => e.RentalRequestRotationListHistId)
                    .HasColumnName("RENTAL_REQUEST_ROTATION_LIST_HIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_REQUEST_ROTATION_LIST_HIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AskedDateTime).HasColumnName("ASKED_DATE_TIME");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.IsForceHire).HasColumnName("IS_FORCE_HIRE");

                entity.Property(e => e.Note)
                    .HasColumnName("NOTE")
                    .HasMaxLength(2048);

                entity.Property(e => e.OfferRefusalReason)
                    .HasColumnName("OFFER_REFUSAL_REASON")
                    .HasMaxLength(50);

                entity.Property(e => e.OfferResponse).HasColumnName("OFFER_RESPONSE");

                entity.Property(e => e.OfferResponseDatetime).HasColumnName("OFFER_RESPONSE_DATETIME");

                entity.Property(e => e.OfferResponseNote)
                    .HasColumnName("OFFER_RESPONSE_NOTE")
                    .HasMaxLength(2048);

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.Property(e => e.RentalRequestRotationListId).HasColumnName("RENTAL_REQUEST_ROTATION_LIST_ID");

                entity.Property(e => e.RotationListSortOrder).HasColumnName("ROTATION_LIST_SORT_ORDER");

                entity.Property(e => e.WasAsked).HasColumnName("WAS_ASKED");
            });

            modelBuilder.Entity<HetRentalRequestStatusType>(entity =>
            {
                entity.HasKey(e => e.RentalRequestStatusTypeId);

                entity.ToTable("HET_RENTAL_REQUEST_STATUS_TYPE");

                entity.HasIndex(e => e.RentalRequestStatusTypeCode)
                    .HasName("UK_HET_RENTAL_REQUEST_STATUS_TYPE_CODE")
                    .IsUnique();

                entity.Property(e => e.RentalRequestStatusTypeId)
                    .HasColumnName("RENTAL_REQUEST_STATUS_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_REQUEST_STATUS_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048);

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.RentalRequestStatusTypeCode)
                    .IsRequired()
                    .HasColumnName("RENTAL_REQUEST_STATUS_TYPE_CODE")
                    .HasMaxLength(20);

                entity.Property(e => e.ScreenLabel)
                    .HasColumnName("SCREEN_LABEL")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<HetRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("HET_ROLE");

                entity.HasIndex(e => e.Name)
                    .HasName("HET_ROLE_NAME_UK")
                    .IsUnique();

                entity.Property(e => e.RoleId)
                    .HasColumnName("ROLE_ID")
                    .HasDefaultValueSql("nextval('\"HET_ROLE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048);

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<HetRolePermission>(entity =>
            {
                entity.HasKey(e => e.RolePermissionId);

                entity.ToTable("HET_ROLE_PERMISSION");

                entity.HasIndex(e => e.PermissionId);

                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RolePermissionId)
                    .HasColumnName("ROLE_PERMISSION_ID")
                    .HasDefaultValueSql("nextval('\"HET_ROLE_PERMISSION_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.PermissionId).HasColumnName("PERMISSION_ID");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.HetRolePermission)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_HET_ROLE_PERMISSION_PERMISSION_ID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.HetRolePermission)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_HET_ROLE_PERMISSION_ROLE_ID");
            });

            modelBuilder.Entity<HetSeniorityAudit>(entity =>
            {
                entity.HasKey(e => e.SeniorityAuditId);

                entity.ToTable("HET_SENIORITY_AUDIT");

                entity.HasIndex(e => e.EquipmentId);

                entity.HasIndex(e => e.LocalAreaId);

                entity.HasIndex(e => e.OwnerId);

                entity.Property(e => e.SeniorityAuditId)
                    .HasColumnName("SENIORITY_AUDIT_ID")
                    .HasDefaultValueSql("nextval('\"HET_SENIORITY_AUDIT_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.BlockNumber).HasColumnName("BLOCK_NUMBER");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.IsSeniorityOverridden).HasColumnName("IS_SENIORITY_OVERRIDDEN");

                entity.Property(e => e.LocalAreaId).HasColumnName("LOCAL_AREA_ID");

                entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");

                entity.Property(e => e.OwnerOrganizationName)
                    .HasColumnName("OWNER_ORGANIZATION_NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.Seniority).HasColumnName("SENIORITY");

                entity.Property(e => e.SeniorityOverrideReason)
                    .HasColumnName("SENIORITY_OVERRIDE_REASON")
                    .HasMaxLength(2048);

                entity.Property(e => e.ServiceHoursLastYear).HasColumnName("SERVICE_HOURS_LAST_YEAR");

                entity.Property(e => e.ServiceHoursThreeYearsAgo).HasColumnName("SERVICE_HOURS_THREE_YEARS_AGO");

                entity.Property(e => e.ServiceHoursTwoYearsAgo).HasColumnName("SERVICE_HOURS_TWO_YEARS_AGO");

                entity.Property(e => e.StartDate).HasColumnName("START_DATE");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetSeniorityAudit)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK_HET_SENIORITY_AUDIT_EQUIPMENT_ID");

                entity.HasOne(d => d.LocalArea)
                    .WithMany(p => p.HetSeniorityAudit)
                    .HasForeignKey(d => d.LocalAreaId)
                    .HasConstraintName("FK_HET_SENIORITY_AUDIT_LOCAL_AREA_ID");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetSeniorityAudit)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_HET_SENIORITY_AUDIT_OWNER_ID");
            });

            modelBuilder.Entity<HetServiceArea>(entity =>
            {
                entity.HasKey(e => e.ServiceAreaId);

                entity.ToTable("HET_SERVICE_AREA");

                entity.HasIndex(e => e.DistrictId);

                entity.Property(e => e.ServiceAreaId)
                    .HasColumnName("SERVICE_AREA_ID")
                    .HasDefaultValueSql("nextval('\"HET_SERVICE_AREA_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AreaNumber).HasColumnName("AREA_NUMBER");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.MinistryServiceAreaId).HasColumnName("MINISTRY_SERVICE_AREA_ID");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.StartDate).HasColumnName("START_DATE");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetServiceArea)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_HET_SERVICE_AREA_DISTRICT_ID");
            });

            modelBuilder.Entity<HetTimePeriodType>(entity =>
            {
                entity.HasKey(e => e.TimePeriodTypeId);

                entity.ToTable("HET_TIME_PERIOD_TYPE");

                entity.HasIndex(e => e.TimePeriodTypeCode)
                    .HasName("UK_HET_TIME_PERIOD_TYPE_CODE")
                    .IsUnique();

                entity.Property(e => e.TimePeriodTypeId)
                    .HasColumnName("TIME_PERIOD_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_TIME_PERIOD_TYPE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048);

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.ScreenLabel)
                    .HasColumnName("SCREEN_LABEL")
                    .HasMaxLength(200);

                entity.Property(e => e.TimePeriodTypeCode)
                    .IsRequired()
                    .HasColumnName("TIME_PERIOD_TYPE_CODE")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<HetTimeRecord>(entity =>
            {
                entity.HasKey(e => e.TimeRecordId);

                entity.ToTable("HET_TIME_RECORD");

                entity.HasIndex(e => e.RentalAgreementId);

                entity.HasIndex(e => e.RentalAgreementRateId);

                entity.HasIndex(e => e.TimePeriodTypeId);

                entity.Property(e => e.TimeRecordId)
                    .HasColumnName("TIME_RECORD_ID")
                    .HasDefaultValueSql("nextval('\"HET_TIME_RECORD_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EnteredDate).HasColumnName("ENTERED_DATE");

                entity.Property(e => e.Hours).HasColumnName("HOURS");

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.Property(e => e.RentalAgreementRateId).HasColumnName("RENTAL_AGREEMENT_RATE_ID");

                entity.Property(e => e.TimePeriodTypeId).HasColumnName("TIME_PERIOD_TYPE_ID");

                entity.Property(e => e.WorkedDate).HasColumnName("WORKED_DATE");

                entity.HasOne(d => d.RentalAgreement)
                    .WithMany(p => p.HetTimeRecord)
                    .HasForeignKey(d => d.RentalAgreementId)
                    .HasConstraintName("FK_HET_TIME_RECORD_RENTAL_AGREEMENT_ID");

                entity.HasOne(d => d.RentalAgreementRate)
                    .WithMany(p => p.HetTimeRecord)
                    .HasForeignKey(d => d.RentalAgreementRateId)
                    .HasConstraintName("FK_HET_TIME_RECORD_RENTAL_AGREEMENT_RATE_ID");

                entity.HasOne(d => d.TimePeriodType)
                    .WithMany(p => p.HetTimeRecord)
                    .HasForeignKey(d => d.TimePeriodTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HET_TIME_RECORD_TIME_PERIOD_TYPE_ID");
            });

            modelBuilder.Entity<HetTimeRecordHist>(entity =>
            {
                entity.HasKey(e => e.TimeRecordHistId);

                entity.ToTable("HET_TIME_RECORD_HIST");

                entity.Property(e => e.TimeRecordHistId)
                    .HasColumnName("TIME_RECORD_HIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_TIME_RECORD_HIST_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

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

                entity.HasIndex(e => e.DistrictId);

                entity.HasIndex(e => e.Guid)
                    .HasName("HET_USR_GUID_UK")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("nextval('\"HET_USER_ID_seq\"'::regclass)");

                entity.Property(e => e.Active).HasColumnName("ACTIVE");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(255);

                entity.Property(e => e.GivenName)
                    .HasColumnName("GIVEN_NAME")
                    .HasMaxLength(50);

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.Initials)
                    .HasColumnName("INITIALS")
                    .HasMaxLength(10);

                entity.Property(e => e.SmAuthorizationDirectory)
                    .HasColumnName("SM_AUTHORIZATION_DIRECTORY")
                    .HasMaxLength(255);

                entity.Property(e => e.SmUserId)
                    .HasColumnName("SM_USER_ID")
                    .HasMaxLength(255);

                entity.Property(e => e.Surname)
                    .HasColumnName("SURNAME")
                    .HasMaxLength(50);

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetUser)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_HET_USER_DISTRICT_ID");
            });

            modelBuilder.Entity<HetUserDistrict>(entity =>
            {
                entity.HasKey(e => e.UserDistrictId);

                entity.ToTable("HET_USER_DISTRICT");

                entity.HasIndex(e => e.DistrictId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserDistrictId)
                    .HasColumnName("USER_DISTRICT_ID")
                    .HasDefaultValueSql("nextval('\"HET_USER_DISTRICT_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.Property(e => e.IsPrimary).HasColumnName("IS_PRIMARY");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetUserDistrict)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_HET_USER_DISTRICT_DISTRICT_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HetUserDistrict)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_HET_USER_DISTRICT_USER_ID");
            });

            modelBuilder.Entity<HetUserFavourite>(entity =>
            {
                entity.HasKey(e => e.UserFavouriteId);

                entity.ToTable("HET_USER_FAVOURITE");

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserFavouriteId)
                    .HasColumnName("USER_FAVOURITE_ID")
                    .HasDefaultValueSql("nextval('\"HET_USER_FAVOURITE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.IsDefault).HasColumnName("IS_DEFAULT");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.Type)
                    .HasColumnName("TYPE")
                    .HasMaxLength(150);

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.Property(e => e.Value)
                    .HasColumnName("VALUE")
                    .HasMaxLength(2048);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HetUserFavourite)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_HET_USER_FAVOURITE_USER_ID");
            });

            modelBuilder.Entity<HetUserRole>(entity =>
            {
                entity.HasKey(e => e.UserRoleId);

                entity.ToTable("HET_USER_ROLE");

                entity.HasIndex(e => e.RoleId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserRoleId)
                    .HasColumnName("USER_ROLE_ID")
                    .HasDefaultValueSql("nextval('\"HET_USER_ROLE_ID_seq\"'::regclass)");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50);

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255);

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255);

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.ExpiryDate).HasColumnName("EXPIRY_DATE");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.HetUserRole)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_HET_USER_ROLE_ROLE_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HetUserRole)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_HET_USER_ROLE_USER_ID");
            });

            modelBuilder.HasSequence("HET_BUSINESS_ID_seq");

            modelBuilder.HasSequence("HET_BUSINESS_USER_ID_seq");

            modelBuilder.HasSequence("HET_BUSINESS_USER_ROLE_ID_seq");

            modelBuilder.HasSequence("HET_CITY_ID_seq");

            modelBuilder.HasSequence("HET_CONDITION_TYPE_ID_seq");

            modelBuilder.HasSequence("HET_CONTACT_ID_seq");

            modelBuilder.HasSequence("HET_DIGITAL_FILE_ID_seq");

            modelBuilder.HasSequence("HET_DISTRICT_EQUIPMENT_TYPE_ID_seq");

            modelBuilder.HasSequence("HET_DISTRICT_ID_seq");

            modelBuilder.HasSequence("HET_EQUIPMENT_ATTACHMENT_HIST_ID_seq");

            modelBuilder.HasSequence("HET_EQUIPMENT_ATTACHMENT_ID_seq");

            modelBuilder.HasSequence("HET_EQUIPMENT_HIST_ID_seq");

            modelBuilder.HasSequence("HET_EQUIPMENT_ID_seq");

            modelBuilder.HasSequence("HET_EQUIPMENT_STATUS_TYPE_ID_seq").StartsAt(92);

            modelBuilder.HasSequence("HET_EQUIPMENT_TYPE_ID_seq");

            modelBuilder.HasSequence("HET_HISTORY_ID_seq");

            modelBuilder.HasSequence("HET_IMPORT_MAP_ID_seq");

            modelBuilder.HasSequence("HET_LOCAL_AREA_ID_seq");

            modelBuilder.HasSequence("HET_LOCAL_AREA_ROTATION_LIST_ID_seq");

            modelBuilder.HasSequence("HET_MIME_TYPE_ID_seq").StartsAt(92);

            modelBuilder.HasSequence("HET_NOTE_HIST_ID_seq");

            modelBuilder.HasSequence("HET_NOTE_ID_seq");

            modelBuilder.HasSequence("HET_OWNER_ID_seq");

            modelBuilder.HasSequence("HET_OWNER_STATUS_TYPE_ID_seq").StartsAt(92);

            modelBuilder.HasSequence("HET_PERMISSION_ID_seq");

            modelBuilder.HasSequence("HET_PROJECT_ID_seq");

            modelBuilder.HasSequence("HET_PROJECT_STATUS_TYPE_ID_seq").StartsAt(92);

            modelBuilder.HasSequence("HET_RATE_PERIOD_TYPE_ID_seq").StartsAt(92);

            modelBuilder.HasSequence("HET_REGION_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_CONDITION_HIST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_CONDITION_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_HIST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_RATE_HIST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_RATE_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_REQUEST_ATTACHMENT_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_REQUEST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_REQUEST_ROTATION_LIST_HIST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_REQUEST_ROTATION_LIST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_REQUEST_STATUS_TYPE_ID_seq").StartsAt(92);

            modelBuilder.HasSequence("HET_ROLE_ID_seq");

            modelBuilder.HasSequence("HET_ROLE_PERMISSION_ID_seq");

            modelBuilder.HasSequence("HET_SENIORITY_AUDIT_ID_seq");

            modelBuilder.HasSequence("HET_SERVICE_AREA_ID_seq");

            modelBuilder.HasSequence("HET_TIME_RECORD_HIST_ID_seq");

            modelBuilder.HasSequence("HET_TIME_RECORD_ID_seq");

            modelBuilder.HasSequence("HET_USER_DISTRICT_ID_seq");

            modelBuilder.HasSequence("HET_USER_FAVOURITE_ID_seq");

            modelBuilder.HasSequence("HET_USER_ID_seq");

            modelBuilder.HasSequence("HET_USER_ROLE_ID_seq");
        }
    }
}

