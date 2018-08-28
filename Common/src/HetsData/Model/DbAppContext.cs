using Microsoft.EntityFrameworkCore;

namespace HetsData.Model
{
    public partial class DbAppContext : DbContext
    {
        public DbAppContext()
        {
        }

        public DbAppContext(DbContextOptions<DbAppContext> options)
            : base(options)
        {
        }

        private readonly string _connectionString;

        public DbAppContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public virtual DbSet<HetAttachment> HetAttachment { get; set; }
        public virtual DbSet<HetCity> HetCity { get; set; }
        public virtual DbSet<HetConditionType> HetConditionType { get; set; }
        public virtual DbSet<HetContact> HetContact { get; set; }
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
            modelBuilder.Entity<HetAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId);

                entity.ToTable("HET_ATTACHMENT");

                entity.ForNpgsqlHasComment("Uploaded documents related to entity in the application - e.g. piece of Equipment, an Owner, a Project and so on.");

                entity.HasIndex(e => e.EquipmentId);

                entity.HasIndex(e => e.OwnerId);

                entity.HasIndex(e => e.ProjectId);

                entity.HasIndex(e => e.RentalRequestId);

                entity.Property(e => e.AttachmentId)
                    .HasColumnName("ATTACHMENT_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for an Attachment");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("A note about the attachment,  optionally maintained by the user.");

                entity.Property(e => e.EquipmentId)
                    .HasColumnName("EQUIPMENT_ID")
                    .ForNpgsqlHasComment("Link to the Equipment.");

                entity.Property(e => e.FileContents)
                    .HasColumnName("FILE_CONTENTS")
                    .ForNpgsqlHasComment("Binary contents of the file");

                entity.Property(e => e.FileName)
                    .HasColumnName("FILE_NAME")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("Filename as passed by the user uploading the file");

                entity.Property(e => e.MimeType)
                    .HasColumnName("MIME_TYPE")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("Mime-Type for attachment.");

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OWNER_ID")
                    .ForNpgsqlHasComment("Link to the Owner.");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("PROJECT_ID")
                    .ForNpgsqlHasComment("Link to the Project.");

                entity.Property(e => e.RentalRequestId)
                    .HasColumnName("RENTAL_REQUEST_ID")
                    .ForNpgsqlHasComment("Link to the RentalRequest.");

                entity.Property(e => e.Type)
                    .HasColumnName("TYPE")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("Type of attachment");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetAttachment)
                    .HasForeignKey(d => d.EquipmentId);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetAttachment)
                    .HasForeignKey(d => d.OwnerId);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetAttachment)
                    .HasForeignKey(d => d.ProjectId);

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetAttachment)
                    .HasForeignKey(d => d.RentalRequestId);
            });

            modelBuilder.Entity<HetCity>(entity =>
            {
                entity.HasKey(e => e.CityId);

                entity.ToTable("HET_CITY");

                entity.ForNpgsqlHasComment("A list of cities in BC. Authoritative source to be determined.");

                entity.Property(e => e.CityId)
                    .HasColumnName("CITY_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a City");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The name of the City");
            });

            modelBuilder.Entity<HetConditionType>(entity =>
            {
                entity.HasKey(e => e.ConditionTypeId);

                entity.ToTable("HET_CONDITION_TYPE");

                entity.ForNpgsqlHasComment("The standard conditions used in creating a new rental agreement.");

                entity.HasIndex(e => e.DistrictId);

                entity.Property(e => e.ConditionTypeId)
                    .HasColumnName("CONDITION_TYPE_ID")
                    .ForNpgsqlHasComment("A unique id for the ConditionType record (required).");

                entity.Property(e => e.Active)
                    .HasColumnName("ACTIVE")
                    .ForNpgsqlHasComment("A flag indicating if this ConditionType should be used on new rental agreements.");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.ConditionTypeCode)
                    .HasColumnName("CONDITION_TYPE_CODE")
                    .HasMaxLength(20)
                    .ForNpgsqlHasComment("A code value for a ConditionType (required).");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("A description of the ConditionType as used in the rental agreement.");

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetConditionType)
                    .HasForeignKey(d => d.DistrictId);
            });

            modelBuilder.Entity<HetContact>(entity =>
            {
                entity.HasKey(e => e.ContactId);

                entity.ToTable("HET_CONTACT");

                entity.ForNpgsqlHasComment("A person and their related contact information linked to one or more entities in the system. For examples, there are contacts for Owners, Projects.");

                entity.HasIndex(e => e.OwnerId);

                entity.HasIndex(e => e.ProjectId);

                entity.Property(e => e.ContactId)
                    .HasColumnName("CONTACT_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a Contact");

                entity.Property(e => e.Address1)
                    .HasColumnName("ADDRESS1")
                    .HasMaxLength(80)
                    .ForNpgsqlHasComment("Address 1 line of the address.");

                entity.Property(e => e.Address2)
                    .HasColumnName("ADDRESS2")
                    .HasMaxLength(80)
                    .ForNpgsqlHasComment("Address 2 line of the address.");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.City)
                    .HasColumnName("CITY")
                    .HasMaxLength(100)
                    .ForNpgsqlHasComment("The City of the address.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.EmailAddress)
                    .HasColumnName("EMAIL_ADDRESS")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The email address for the contact.");

                entity.Property(e => e.FaxPhoneNumber)
                    .HasColumnName("FAX_PHONE_NUMBER")
                    .HasMaxLength(20)
                    .ForNpgsqlHasComment("The fax phone number for the contact.");

                entity.Property(e => e.GivenName)
                    .HasColumnName("GIVEN_NAME")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The given name of the contact.");

                entity.Property(e => e.MobilePhoneNumber)
                    .HasColumnName("MOBILE_PHONE_NUMBER")
                    .HasMaxLength(20)
                    .ForNpgsqlHasComment("The mobile phone number for the contact.");

                entity.Property(e => e.Notes)
                    .HasColumnName("NOTES")
                    .HasMaxLength(512)
                    .ForNpgsqlHasComment("A note about the contact maintained by the users.");

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OWNER_ID")
                    .ForNpgsqlHasComment("Link to the Owner.");

                entity.Property(e => e.PostalCode)
                    .HasColumnName("POSTAL_CODE")
                    .HasMaxLength(15)
                    .ForNpgsqlHasComment("The postal code of the address.");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("PROJECT_ID")
                    .ForNpgsqlHasComment("Link to the Project.");

                entity.Property(e => e.Province)
                    .HasColumnName("PROVINCE")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The Province of the address.");

                entity.Property(e => e.Role)
                    .HasColumnName("ROLE")
                    .HasMaxLength(100)
                    .ForNpgsqlHasComment("The role of the contact. UI controlled as to whether it is free form or selected from an enumerated list - for initial implementation, the field is freeform.");

                entity.Property(e => e.Surname)
                    .HasColumnName("SURNAME")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The surname of the contact.");

                entity.Property(e => e.WorkPhoneNumber)
                    .HasColumnName("WORK_PHONE_NUMBER")
                    .HasMaxLength(20)
                    .ForNpgsqlHasComment("The work phone number for the contact.");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetContact)
                    .HasForeignKey(d => d.OwnerId);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetContact)
                    .HasForeignKey(d => d.ProjectId);
            });

            modelBuilder.Entity<HetDistrict>(entity =>
            {
                entity.HasKey(e => e.DistrictId);

                entity.ToTable("HET_DISTRICT");

                entity.ForNpgsqlHasComment("The Ministry of Transportion and Infrastructure DISTRICT");

                entity.HasIndex(e => e.RegionId);

                entity.Property(e => e.DistrictId)
                    .HasColumnName("DISTRICT_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a District");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.DistrictNumber)
                    .HasColumnName("DISTRICT_NUMBER")
                    .ForNpgsqlHasComment("A number that uniquely defines a Ministry District.");

                entity.Property(e => e.EndDate)
                    .HasColumnName("END_DATE")
                    .ForNpgsqlHasComment("The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM");

                entity.Property(e => e.MinistryDistrictId)
                    .HasColumnName("MINISTRY_DISTRICT_ID")
                    .ForNpgsqlHasComment("A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The Name of a Ministry District.");

                entity.Property(e => e.RegionId)
                    .HasColumnName("REGION_ID")
                    .ForNpgsqlHasComment("The region in which the District is found.");

                entity.Property(e => e.StartDate)
                    .HasColumnName("START_DATE")
                    .ForNpgsqlHasComment("The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.HetDistrict)
                    .HasForeignKey(d => d.RegionId);
            });

            modelBuilder.Entity<HetDistrictEquipmentType>(entity =>
            {
                entity.HasKey(e => e.DistrictEquipmentTypeId);

                entity.ToTable("HET_DISTRICT_EQUIPMENT_TYPE");

                entity.ForNpgsqlHasComment("An Equipment Type defined at the District level. Links to a provincial Equipment Type for the name of the equipment but supports the District HETS Clerk setting a local name for the Equipment Type. Within a given District, the same provincial Equipment Type might be reused multiple times in, for example, separate sizes (small, medium, large). This enables local areas with large number of the same Equipment Type to have multiple lists.");

                entity.HasIndex(e => e.DistrictId);

                entity.HasIndex(e => e.EquipmentTypeId);

                entity.Property(e => e.DistrictEquipmentTypeId)
                    .HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for an EquipmentType");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.DistrictEquipmentName)
                    .HasColumnName("DISTRICT_EQUIPMENT_NAME")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The name of this equipment type used at the District Level. This could be just the equipmentName if this is the only EquipmentType defined in this District, or could be a name that separates out multiple EquipmentTypes used within a District to, for instance, separate out the same EquipmentName by size.");

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.Property(e => e.EquipmentTypeId).HasColumnName("EQUIPMENT_TYPE_ID");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetDistrictEquipmentType)
                    .HasForeignKey(d => d.DistrictId);

                entity.HasOne(d => d.EquipmentType)
                    .WithMany(p => p.HetDistrictEquipmentType)
                    .HasForeignKey(d => d.EquipmentTypeId)
                    .HasConstraintName("FK_HET_DISTRICT_EQUIPMENT_TYPE_HET_EQUIPMENT_TYPE_EQUIPMENT_TYP");
            });

            modelBuilder.Entity<HetEquipment>(entity =>
            {
                entity.HasKey(e => e.EquipmentId);

                entity.ToTable("HET_EQUIPMENT");

                entity.ForNpgsqlHasComment("A piece of equipment in the HETS system. Each piece of equipment is of a specific equipment type, owned by an Owner, and is within a Local Area.");

                entity.HasIndex(e => e.DistrictEquipmentTypeId);

                entity.HasIndex(e => e.LocalAreaId);

                entity.HasIndex(e => e.OwnerId);

                entity.Property(e => e.EquipmentId)
                    .HasColumnName("EQUIPMENT_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a Equipment");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ApprovedDate)
                    .HasColumnName("APPROVED_DATE")
                    .ForNpgsqlHasComment("The date the piece of equipment was first approved in HETS. Part of the seniority calculation for a piece of equipment is based on this date.");

                entity.Property(e => e.ArchiveCode)
                    .HasColumnName("ARCHIVE_CODE")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("Archive code (Y/N) indicating if a piece of equipment has been archived.");

                entity.Property(e => e.ArchiveDate)
                    .HasColumnName("ARCHIVE_DATE")
                    .ForNpgsqlHasComment("The date on which a user most recenly marked this piece of equipment as archived.");

                entity.Property(e => e.ArchiveReason)
                    .HasColumnName("ARCHIVE_REASON")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("An optional comment about why this piece of equipment has been archived.");

                entity.Property(e => e.BlockNumber)
                    .HasColumnName("BLOCK_NUMBER")
                    .ForNpgsqlHasComment("The current block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined y the business  - 1, 2 or Open");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.DistrictEquipmentTypeId)
                    .HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for a Equipment Type");

                entity.Property(e => e.EquipmentCode)
                    .HasColumnName("EQUIPMENT_CODE")
                    .HasMaxLength(25)
                    .ForNpgsqlHasComment("A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated at record creation time based on the unique Owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083.");

                entity.Property(e => e.InformationUpdateNeededReason)
                    .HasColumnName("INFORMATION_UPDATE_NEEDED_REASON")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("A note about why the needed information&#x2F;status update that is needed about the equipment.");

                entity.Property(e => e.IsInformationUpdateNeeded)
                    .HasColumnName("IS_INFORMATION_UPDATE_NEEDED")
                    .ForNpgsqlHasComment("Set true if a need to update the information&#x2F;status of the equipment is needed. Used during the processing of a request when an update is noted, but the Clerk does not have time to make the update.");

                entity.Property(e => e.IsSeniorityOverridden)
                    .HasColumnName("IS_SENIORITY_OVERRIDDEN")
                    .ForNpgsqlHasComment("True if the Seniority for the piece of equipment was manually overridden. Set if a user has gone in and explicitly updated the seniority base information. Indicates that underlying numbers were manually overridden.");

                entity.Property(e => e.LastVerifiedDate)
                    .HasColumnName("LAST_VERIFIED_DATE")
                    .ForNpgsqlHasComment("The date the equipment was last verified by the HETS Clerk as being still in service in the Local Area and available for the HETS Programme.");

                entity.Property(e => e.LegalCapacity)
                    .HasColumnName("LEGAL_CAPACITY")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The legal capacity of the dump truck.");

                entity.Property(e => e.LicencePlate)
                    .HasColumnName("LICENCE_PLATE")
                    .HasMaxLength(20)
                    .ForNpgsqlHasComment("The licence plate (if any) of the piece of equipment, as entered by the HETS Clerk.");

                entity.Property(e => e.LicencedGvw)
                    .HasColumnName("LICENCED_GVW")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The Gross Vehicle Weight for which the vehicle is licensed. GVW includes the vehicle&#39;s chassis, body, engine, engine fluids, fuel, accessories, driver, passengers and cargo but excluding that of any trailers.");

                entity.Property(e => e.LocalAreaId)
                    .HasColumnName("LOCAL_AREA_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for a Local Area");

                entity.Property(e => e.Make)
                    .HasColumnName("MAKE")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The make of the piece of equipment, as provided by the Equipment Owner.");

                entity.Property(e => e.Model)
                    .HasColumnName("MODEL")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The model of the piece of equipment, as provided by the Equipment Owner.");

                entity.Property(e => e.NumberInBlock)
                    .HasColumnName("NUMBER_IN_BLOCK")
                    .ForNpgsqlHasComment("The number in the block of the piece of equipment so that it can be displayed to the user where it will be useful. This saves the user from having to figure out in their head the order when the list is displayed in Rotation Order.");

                entity.Property(e => e.Operator)
                    .HasColumnName("OPERATOR")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?");

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OWNER_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for an Owner");

                entity.Property(e => e.PayRate)
                    .HasColumnName("PAY_RATE")
                    .ForNpgsqlHasComment("TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?");

                entity.Property(e => e.PupLegalCapacity)
                    .HasColumnName("PUP_LEGAL_CAPACITY")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The pup legal capacity.");

                entity.Property(e => e.ReceivedDate)
                    .HasColumnName("RECEIVED_DATE")
                    .ForNpgsqlHasComment("The date the piece of equipment was first received and recorded in HETS.");

                entity.Property(e => e.RefuseRate)
                    .HasColumnName("REFUSE_RATE")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?");

                entity.Property(e => e.Seniority)
                    .HasColumnName("SENIORITY")
                    .ForNpgsqlHasComment("The current seniority calculation result for this piece of equipment. The calculation is based on the &quot;numYears&quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo).");

                entity.Property(e => e.SeniorityEffectiveDate)
                    .HasColumnName("SENIORITY_EFFECTIVE_DATE")
                    .ForNpgsqlHasComment("The time the seniority data in the record went into effect. Used to populate the SeniorityAudit table when the seniority data is next updated.");

                entity.Property(e => e.SeniorityOverrideReason)
                    .HasColumnName("SENIORITY_OVERRIDE_REASON")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("A text reason for why the piece of equipments underlying data was overridden to change their seniority number.");

                entity.Property(e => e.SerialNumber)
                    .HasColumnName("SERIAL_NUMBER")
                    .HasMaxLength(100)
                    .ForNpgsqlHasComment("The serial number of the piece of equipment as provided by the Equipment Owner. Used to detect and reconcile pieces of equipment moved between Local Areas. Duplicate serial numbers are flagged in the system but permitted. The duplicates are flagged in the UI until the HETS Clerks reconcile the differences - either correcting the serial number or archiving a piece of equipment moved to a new local area.");

                entity.Property(e => e.ServiceHoursLastYear)
                    .HasColumnName("SERVICE_HOURS_LAST_YEAR")
                    .ForNpgsqlHasComment("Number of hours of service by this piece of equipment in the previous fiscal year");

                entity.Property(e => e.ServiceHoursThreeYearsAgo)
                    .HasColumnName("SERVICE_HOURS_THREE_YEARS_AGO")
                    .ForNpgsqlHasComment("Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015");

                entity.Property(e => e.ServiceHoursTwoYearsAgo)
                    .HasColumnName("SERVICE_HOURS_TWO_YEARS_AGO")
                    .ForNpgsqlHasComment("Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016");

                entity.Property(e => e.Size)
                    .HasColumnName("SIZE")
                    .HasMaxLength(128)
                    .ForNpgsqlHasComment("The size of the piece of equipment, as provided by the Equipment Owner.");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The current status of the equipment in a UI-controlled string. Initial values are Pending, Approved and Archived, but other values may be added.");

                entity.Property(e => e.StatusComment)
                    .HasColumnName("STATUS_COMMENT")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("A comment field to capture information specific to the change of status.");

                entity.Property(e => e.ToDate)
                    .HasColumnName("TO_DATE")
                    .ForNpgsqlHasComment("TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?");

                entity.Property(e => e.Type)
                    .HasColumnName("TYPE")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("A user entered type field. Typically used by the business to identify unique characteristics regarding the piece of equipment.");

                entity.Property(e => e.Year)
                    .HasColumnName("YEAR")
                    .HasMaxLength(15)
                    .ForNpgsqlHasComment("The model year of the piece of equipment, as provided by the Equipment Owner.");

                entity.Property(e => e.YearsOfService)
                    .HasColumnName("YEARS_OF_SERVICE")
                    .ForNpgsqlHasComment("The number of years of active service of this piece of equipment at the time seniority is calculated - April 1 of the current FY.");

                entity.HasOne(d => d.DistrictEquipmentType)
                    .WithMany(p => p.HetEquipment)
                    .HasForeignKey(d => d.DistrictEquipmentTypeId)
                    .HasConstraintName("FK_HET_EQUIPMENT_HET_DISTRICT_EQUIPMENT_TYPE_DISTRICT_EQUIPMENT");

                entity.HasOne(d => d.LocalArea)
                    .WithMany(p => p.HetEquipment)
                    .HasForeignKey(d => d.LocalAreaId);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetEquipment)
                    .HasForeignKey(d => d.OwnerId);
            });

            modelBuilder.Entity<HetEquipmentAttachment>(entity =>
            {
                entity.HasKey(e => e.EquipmentAttachmentId);

                entity.ToTable("HET_EQUIPMENT_ATTACHMENT");

                entity.ForNpgsqlHasComment("An Equipment Attachment associated with a piece of Equipment.");

                entity.HasIndex(e => e.EquipmentId);

                entity.Property(e => e.EquipmentAttachmentId)
                    .HasColumnName("EQUIPMENT_ATTACHMENT_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for an EquipmentAttachment");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("A description of the equipment attachment if the Equipment Attachment Type Name is insufficient.");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.TypeName)
                    .HasColumnName("TYPE_NAME")
                    .HasMaxLength(100)
                    .ForNpgsqlHasComment("The name of the attachment type");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetEquipmentAttachment)
                    .HasForeignKey(d => d.EquipmentId);
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

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1");

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

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1");

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

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasMaxLength(50);

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

                entity.ForNpgsqlHasComment("A valid status that a given equipment be in.");

                entity.Property(e => e.EquipmentStatusTypeId)
                    .HasColumnName("EQUIPMENT_STATUS_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_EQUIPMENT_STATUS_TYPE_ID_seq\"'::regclass)")
                    .ForNpgsqlHasComment("A system-generated unique identifier for an Equipment Status Type.");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .IsRequired()
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .IsRequired()
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("Free format text explaining the meaning of the given status type.");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("DISPLAY_ORDER")
                    .ForNpgsqlHasComment("A number indicating the order in which this status type should show when displayed on a picklist.");

                entity.Property(e => e.EquipmentStatusTypeCode)
                    .IsRequired()
                    .HasColumnName("EQUIPMENT_STATUS_TYPE_CODE")
                    .HasMaxLength(20)
                    .ForNpgsqlHasComment("A code value for an Equipment Status Type.");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true")
                    .ForNpgsqlHasComment("A True/False value indicating whether this status type is active and can therefore be used for classifying an equipment.");

                entity.Property(e => e.ScreenLabel)
                    .HasColumnName("SCREEN_LABEL")
                    .HasMaxLength(200)
                    .ForNpgsqlHasComment("Free format text to be used for displaying this status type on a screen or report.");
            });

            modelBuilder.Entity<HetEquipmentType>(entity =>
            {
                entity.HasKey(e => e.EquipmentTypeId);

                entity.ToTable("HET_EQUIPMENT_TYPE");

                entity.ForNpgsqlHasComment("A provincial-wide Equipment Type, the related Blue Book Chapter Section and related usage attributes.");

                entity.Property(e => e.EquipmentTypeId)
                    .HasColumnName("EQUIPMENT_TYPE_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for an EquipmentName");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.BlueBookRateNumber)
                    .HasColumnName("BLUE_BOOK_RATE_NUMBER")
                    .ForNpgsqlHasComment("The rate number in the Blue Book that is related to equipment types of this name.");

                entity.Property(e => e.BlueBookSection)
                    .HasColumnName("BLUE_BOOK_SECTION")
                    .ForNpgsqlHasComment("The section of the Blue Book that is related to equipment types of this name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.ExtendHours)
                    .HasColumnName("EXTEND_HOURS")
                    .ForNpgsqlHasComment("The number of extended hours per year that equipment types of this name&#x2F;Blue Book section can work.");

                entity.Property(e => e.IsDumpTruck)
                    .HasColumnName("IS_DUMP_TRUCK")
                    .ForNpgsqlHasComment("True if the Equipment Type is a Dump Truck. Equipment of this type will have a related Dump Truck record containing dump truck-related attributes.");

                entity.Property(e => e.MaxHoursSub)
                    .HasColumnName("MAX_HOURS_SUB")
                    .ForNpgsqlHasComment("The number of substitute hours per year that equipment types of this name&#x2F;Blue Book section can work.");

                entity.Property(e => e.MaximumHours)
                    .HasColumnName("MAXIMUM_HOURS")
                    .ForNpgsqlHasComment("The maximum number of hours per year that equipment types of this name&#x2F;Blue Book section can work in a year");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The generic name of an equipment type - e.g. Dump Truck, Excavator and so on.");

                entity.Property(e => e.NumberOfBlocks)
                    .HasColumnName("NUMBER_OF_BLOCKS")
                    .ForNpgsqlHasComment("The number of blocks defined for the equipment of this name and Blue Book section. In general Dump Truck-class equipment types have 3 blocks, while non-Dump Truck equipment types have 2 blocks.");
            });

            modelBuilder.Entity<HetHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId);

                entity.ToTable("HET_HISTORY");

                entity.ForNpgsqlHasComment("A log entry created by the system based on a triggering event and related to an entity in the application - e.g. piece of Equipment, an Owner, a Project and so on.");

                entity.HasIndex(e => e.EquipmentId);

                entity.HasIndex(e => e.OwnerId);

                entity.HasIndex(e => e.ProjectId);

                entity.HasIndex(e => e.RentalRequestId);

                entity.Property(e => e.HistoryId)
                    .HasColumnName("HISTORY_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a History");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("CREATED_DATE")
                    .ForNpgsqlHasComment("Date the record is created.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.EquipmentId)
                    .HasColumnName("EQUIPMENT_ID")
                    .ForNpgsqlHasComment("Link to the Equipment.");

                entity.Property(e => e.HistoryText)
                    .HasColumnName("HISTORY_TEXT")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("The text of the history entry tracked against the related entity.");

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OWNER_ID")
                    .ForNpgsqlHasComment("Link to the Owner.");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("PROJECT_ID")
                    .ForNpgsqlHasComment("Link to the Project.");

                entity.Property(e => e.RentalRequestId)
                    .HasColumnName("RENTAL_REQUEST_ID")
                    .ForNpgsqlHasComment("Link to the RentalRequest.");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetHistory)
                    .HasForeignKey(d => d.EquipmentId);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetHistory)
                    .HasForeignKey(d => d.OwnerId);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetHistory)
                    .HasForeignKey(d => d.ProjectId);

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetHistory)
                    .HasForeignKey(d => d.RentalRequestId);
            });

            modelBuilder.Entity<HetImportMap>(entity =>
            {
                entity.HasKey(e => e.ImportMapId);

                entity.ToTable("HET_IMPORT_MAP");

                entity.Property(e => e.ImportMapId)
                    .HasColumnName("IMPORT_MAP_ID")
                    .ForNpgsqlHasComment("A system generated unique identifier for the ImportMap");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.NewKey)
                    .HasColumnName("NEW_KEY")
                    .ForNpgsqlHasComment("New primary key for record");

                entity.Property(e => e.NewTable)
                    .HasColumnName("NEW_TABLE")
                    .ForNpgsqlHasComment("Table name in new system.");

                entity.Property(e => e.OldKey)
                    .HasColumnName("OLD_KEY")
                    .HasMaxLength(250)
                    .ForNpgsqlHasComment("Old primary key for record");

                entity.Property(e => e.OldTable)
                    .HasColumnName("OLD_TABLE")
                    .ForNpgsqlHasComment("Table name in old system");
            });

            modelBuilder.Entity<HetLocalArea>(entity =>
            {
                entity.HasKey(e => e.LocalAreaId);

                entity.ToTable("HET_LOCAL_AREA");

                entity.ForNpgsqlHasComment("A HETS-application defined area that is within a Service Area.");

                entity.HasIndex(e => e.ServiceAreaId);

                entity.Property(e => e.LocalAreaId)
                    .HasColumnName("LOCAL_AREA_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a LocalArea");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.EndDate)
                    .HasColumnName("END_DATE")
                    .ForNpgsqlHasComment("The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM");

                entity.Property(e => e.LocalAreaNumber)
                    .HasColumnName("LOCAL_AREA_NUMBER")
                    .ForNpgsqlHasComment("A system-generated, visible to the user number for the Local Area");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The full name of the Local Area");

                entity.Property(e => e.ServiceAreaId)
                    .HasColumnName("SERVICE_AREA_ID")
                    .ForNpgsqlHasComment("The Service Area in which the Local Area is found.");

                entity.Property(e => e.StartDate)
                    .HasColumnName("START_DATE")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM");

                entity.HasOne(d => d.ServiceArea)
                    .WithMany(p => p.HetLocalArea)
                    .HasForeignKey(d => d.ServiceAreaId);
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

                entity.Property(e => e.LocalAreaRotationListId).HasColumnName("LOCAL_AREA_ROTATION_LIST_ID");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.AskNextBlock1Id)
                    .HasColumnName("ASK_NEXT_BLOCK1_ID")
                    .ForNpgsqlHasComment("The id of the next piece of Block 1 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 1.");

                entity.Property(e => e.AskNextBlock1Seniority)
                    .HasColumnName("ASK_NEXT_BLOCK1_SENIORITY")
                    .ForNpgsqlHasComment("The seniority score of the piece of equipment that is the next to be asked in Block 1.");

                entity.Property(e => e.AskNextBlock2Id)
                    .HasColumnName("ASK_NEXT_BLOCK2_ID")
                    .ForNpgsqlHasComment("The id of the next piece of Block 2 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 2.");

                entity.Property(e => e.AskNextBlock2Seniority)
                    .HasColumnName("ASK_NEXT_BLOCK2_SENIORITY")
                    .ForNpgsqlHasComment("The seniority score of the piece of equipment that is the next to be asked in Block 1.");

                entity.Property(e => e.AskNextBlockOpenId)
                    .HasColumnName("ASK_NEXT_BLOCK_OPEN_ID")
                    .ForNpgsqlHasComment("The id of the next piece of Block Open Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block Open.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.DistrictEquipmentTypeId)
                    .HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for an Equipment Type");

                entity.Property(e => e.LocalAreaId).HasColumnName("LOCAL_AREA_ID");

                entity.HasOne(d => d.AskNextBlock1)
                    .WithMany(p => p.HetLocalAreaRotationListAskNextBlock1)
                    .HasForeignKey(d => d.AskNextBlock1Id)
                    .HasConstraintName("FK_HET_LOCAL_AREA_ROTATION_LIST_HET_EQUIPMENT_ASK_NEXT_BLOCK1_I");

                entity.HasOne(d => d.AskNextBlock2)
                    .WithMany(p => p.HetLocalAreaRotationListAskNextBlock2)
                    .HasForeignKey(d => d.AskNextBlock2Id)
                    .HasConstraintName("FK_HET_LOCAL_AREA_ROTATION_LIST_HET_EQUIPMENT_ASK_NEXT_BLOCK2_I");

                entity.HasOne(d => d.AskNextBlockOpen)
                    .WithMany(p => p.HetLocalAreaRotationListAskNextBlockOpen)
                    .HasForeignKey(d => d.AskNextBlockOpenId)
                    .HasConstraintName("FK_HET_LOCAL_AREA_ROTATION_LIST_HET_EQUIPMENT_ASK_NEXT_BLOCK_OP");

                entity.HasOne(d => d.DistrictEquipmentType)
                    .WithMany(p => p.HetLocalAreaRotationList)
                    .HasForeignKey(d => d.DistrictEquipmentTypeId)
                    .HasConstraintName("FK_HET_LOCAL_AREA_ROTATION_LIST_HET_DISTRICT_EQUIPMENT_TYPE_DIS");

                entity.HasOne(d => d.LocalArea)
                    .WithMany(p => p.HetLocalAreaRotationList)
                    .HasForeignKey(d => d.LocalAreaId);
            });

            modelBuilder.Entity<HetMimeType>(entity =>
            {
                entity.HasKey(e => e.MimeTypeId);

                entity.ToTable("HET_MIME_TYPE");

                entity.ForNpgsqlHasComment("The MIME type that may be used to qualify a document.");

                entity.Property(e => e.MimeTypeId)
                    .HasColumnName("MIME_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_MIME_TYPE_ID_seq\"'::regclass)")
                    .ForNpgsqlHasComment("A system-generated unique identifier for an MIME Type.");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .IsRequired()
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .IsRequired()
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("Free format text explaining the meaning of the given MIME type.");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("DISPLAY_ORDER")
                    .ForNpgsqlHasComment("A number indicating the order in which this status type should show when displayed on a picklist.");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true")
                    .ForNpgsqlHasComment("A True/False value indicating whether this status type is active and can therefore be used for classifying a rate.");

                entity.Property(e => e.MimeTypeCode)
                    .IsRequired()
                    .HasColumnName("MIME_TYPE_CODE")
                    .HasMaxLength(20)
                    .ForNpgsqlHasComment("A code value for an MIME Type.");

                entity.Property(e => e.ScreenLabel)
                    .HasColumnName("SCREEN_LABEL")
                    .HasMaxLength(200)
                    .ForNpgsqlHasComment("Free format text to be used for displaying this status type on a screen or report.");
            });

            modelBuilder.Entity<HetNote>(entity =>
            {
                entity.HasKey(e => e.NoteId);

                entity.ToTable("HET_NOTE");

                entity.ForNpgsqlHasComment("Text entered about an entity in the application - e.g. piece of Equipment, an Owner, a Project and so on.");

                entity.HasIndex(e => e.EquipmentId);

                entity.HasIndex(e => e.OwnerId);

                entity.HasIndex(e => e.ProjectId);

                entity.HasIndex(e => e.RentalRequestId);

                entity.Property(e => e.NoteId)
                    .HasColumnName("NOTE_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a Note");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.EquipmentId)
                    .HasColumnName("EQUIPMENT_ID")
                    .ForNpgsqlHasComment("Link to the Equipment.");

                entity.Property(e => e.IsNoLongerRelevant)
                    .HasColumnName("IS_NO_LONGER_RELEVANT")
                    .ForNpgsqlHasComment("A user set flag that the note is no longer relevant. Allows the note to be retained for historical reasons,  but identified to other users as no longer applicable.");

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OWNER_ID")
                    .ForNpgsqlHasComment("Link to the Owner.");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("PROJECT_ID")
                    .ForNpgsqlHasComment("Link to the Project.");

                entity.Property(e => e.RentalRequestId)
                    .HasColumnName("RENTAL_REQUEST_ID")
                    .ForNpgsqlHasComment("Link to the RentalRequest.");

                entity.Property(e => e.Text)
                    .HasColumnName("TEXT")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("Notes entered by users about instance of entities - e.g. School Buses and School Bus Owners");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetNote)
                    .HasForeignKey(d => d.EquipmentId);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetNote)
                    .HasForeignKey(d => d.OwnerId);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetNote)
                    .HasForeignKey(d => d.ProjectId);

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetNote)
                    .HasForeignKey(d => d.RentalRequestId);
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

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1");

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

                entity.ForNpgsqlHasComment("The person or company to which a piece of construction equipment belongs.");

                entity.HasIndex(e => e.LocalAreaId);

                entity.HasIndex(e => e.PrimaryContactId);

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OWNER_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a Owner");

                entity.Property(e => e.Address1)
                    .HasColumnName("ADDRESS1")
                    .HasMaxLength(80)
                    .ForNpgsqlHasComment("Address 1 line of the address.");

                entity.Property(e => e.Address2)
                    .HasColumnName("ADDRESS2")
                    .HasMaxLength(80)
                    .ForNpgsqlHasComment("Address 2 line of the address.");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ArchiveCode)
                    .HasColumnName("ARCHIVE_CODE")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("TO BE REVIEWED WITH THE BUSINESS - IS THIS NEEDED -A coded reason for why an owner record has been moved to Archived.");

                entity.Property(e => e.ArchiveDate)
                    .HasColumnName("ARCHIVE_DATE")
                    .ForNpgsqlHasComment("The date the Owner record was changed to Archived and removed from active use in the system.");

                entity.Property(e => e.ArchiveReason)
                    .HasColumnName("ARCHIVE_REASON")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("A text note about why the owner record has been changed to Archived.");

                entity.Property(e => e.CglPolicyNumber)
                    .HasColumnName("CGL_POLICY_NUMBER")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The owner&amp;#39;s Commercial General Liability Policy Number");

                entity.Property(e => e.CglendDate)
                    .HasColumnName("CGLEND_DATE")
                    .ForNpgsqlHasComment("The end date of the owner&#39;s Commercial General Liability insurance coverage. Coverage is only needed prior to an owner&#39;s piece of equipment starting a rental period (not when in the HETS program but not hired). The details of the coverage can be entered into a Note, or more often - attached as a scanned&#x2F;faxed document.");

                entity.Property(e => e.City)
                    .HasColumnName("CITY")
                    .HasMaxLength(100)
                    .ForNpgsqlHasComment("The City of the address.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.DoingBusinessAs)
                    .HasColumnName("DOING_BUSINESS_AS")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("An official (per BC Registries) alternate name for an Owner organization under which it does business. The application does not verify the name against any registry&#x2F;lookup.");

                entity.Property(e => e.GivenName)
                    .HasColumnName("GIVEN_NAME")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The given name of the contact.");

                entity.Property(e => e.IsMaintenanceContractor)
                    .HasColumnName("IS_MAINTENANCE_CONTRACTOR")
                    .ForNpgsqlHasComment("True if the owner is contracted by MOTI to handle Maintenance activities in the area - e.g. provided services in address unscheduled issues on the roads in the area.");

                entity.Property(e => e.LocalAreaId).HasColumnName("LOCAL_AREA_ID");

                entity.Property(e => e.MeetsResidency)
                    .HasColumnName("MEETS_RESIDENCY")
                    .ForNpgsqlHasComment("True to indicate that the owner of the business has confirmed to the HETS Clerk that they meet the residency requirements of the HETS programme. See the published information about the MOTI HETS programme for information on the owner residency requirements.");

                entity.Property(e => e.OrganizationName)
                    .HasColumnName("ORGANIZATION_NAME")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The name of the organization of the Owner. May simply be the First Name, Last Name of the Owner if the Owner is a sole proprietorship, or the name of a company.");

                entity.Property(e => e.OwnerCode)
                    .HasColumnName("OWNER_CODE")
                    .HasMaxLength(20)
                    .ForNpgsqlHasComment("A unique prefix in the system that is used to generate the human-friendly IDs of the equipment. E.g. An owner Edwards might have a prefix &quot;EDW&quot; and their equipment numbered sequentially with that prefix - e.g. EDW-0082.");

                entity.Property(e => e.PostalCode)
                    .HasColumnName("POSTAL_CODE")
                    .HasMaxLength(15)
                    .ForNpgsqlHasComment("The postal code of the address.");

                entity.Property(e => e.PrimaryContactId)
                    .HasColumnName("PRIMARY_CONTACT_ID")
                    .ForNpgsqlHasComment("Link to the designated Primary Contact.");

                entity.Property(e => e.Province)
                    .HasColumnName("PROVINCE")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The Province of the address.");

                entity.Property(e => e.RegisteredCompanyNumber)
                    .HasColumnName("REGISTERED_COMPANY_NUMBER")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The BC Registries number under which the business is registered.  The application does not verify the number against any registry&#x2F;lookup.");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The status of the owner record in the system. Current set of values are &quot;Pending&quot;, &quot;Approved&quot; and &quot;Archived&quot;. Pending is used when an owner self-registers and a HETS Clerk has not reviewed and Approved the record. Archived is when the owner is no longer part of the HETS programme. &quot;Approved&quot; is used in all other cases.");

                entity.Property(e => e.StatusComment)
                    .HasColumnName("STATUS_COMMENT")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("A comment field to capture information specific to the change of status.");

                entity.Property(e => e.Surname)
                    .HasColumnName("SURNAME")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The surname of the contact.");

                entity.Property(e => e.WorkSafeBcexpiryDate)
                    .HasColumnName("WORK_SAFE_BCEXPIRY_DATE")
                    .ForNpgsqlHasComment("The expiration of the owner&#39;s current WorkSafeBC (aka WCB) permit.");

                entity.Property(e => e.WorkSafeBcpolicyNumber)
                    .HasColumnName("WORK_SAFE_BCPOLICY_NUMBER")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The Owner&#39;s WorkSafeBC (aka WCB) Insurance Policy Number.");

                entity.HasOne(d => d.LocalArea)
                    .WithMany(p => p.HetOwner)
                    .HasForeignKey(d => d.LocalAreaId);

                entity.HasOne(d => d.PrimaryContact)
                    .WithMany(p => p.HetOwner)
                    .HasForeignKey(d => d.PrimaryContactId);
            });

            modelBuilder.Entity<HetOwnerStatusType>(entity =>
            {
                entity.HasKey(e => e.OwnerStatusTypeId);

                entity.ToTable("HET_OWNER_STATUS_TYPE");

                entity.ForNpgsqlHasComment("A valid status that a given OWNER may be in.");

                entity.Property(e => e.OwnerStatusTypeId)
                    .HasColumnName("OWNER_STATUS_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_OWNER_STATUS_TYPE_ID_seq\"'::regclass)")
                    .ForNpgsqlHasComment("A system-generated unique identifier for an OWNER Status Type.");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .IsRequired()
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .IsRequired()
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("Free format text explaining the meaning of the given status type.");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("DISPLAY_ORDER")
                    .ForNpgsqlHasComment("A number indicating the order in which this status type should show when displayed on a picklist.");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true")
                    .ForNpgsqlHasComment("A True/False value indicating whether this status type is active and can therefore be used for classifying an OWNER.");

                entity.Property(e => e.OwnerStatusTypeCode)
                    .IsRequired()
                    .HasColumnName("OWNER_STATUS_TYPE_CODE")
                    .HasMaxLength(20)
                    .ForNpgsqlHasComment("A code value for an OWNER Status Type.");

                entity.Property(e => e.ScreenLabel)
                    .HasColumnName("SCREEN_LABEL")
                    .HasMaxLength(200)
                    .ForNpgsqlHasComment("Free format text to be used for displaying this status type on a screen or report.");
            });

            modelBuilder.Entity<HetPermission>(entity =>
            {
                entity.HasKey(e => e.PermissionId);

                entity.ToTable("HET_PERMISSION");

                entity.ForNpgsqlHasComment("A named element of authorization defined in the code that triggers some behavior in the application. For example, a permission might allow users to see data or to have access to functionality not accessible to users without that permission. Permissions are created as needed to the application code and are added to the permissions table by data migrations executed at the time the software that uses the permission is deployed.");

                entity.Property(e => e.PermissionId)
                    .HasColumnName("PERMISSION_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a Permission");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.Code)
                    .HasColumnName("CODE")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The name of the permission referenced in the software of the application.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("A description of the purpose of the permission and exposed to the user selecting the permissions to be included in a Role.");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The &#39;user friendly&#39; name of the permission exposed to the user selecting the permissions to be included in a Role.");
            });

            modelBuilder.Entity<HetPerson>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                entity.ToTable("HET_PERSON");

                entity.ForNpgsqlHasComment("A human being of interest to the HET business.");

                entity.Property(e => e.PersonId)
                    .HasColumnName("PERSON_ID")
                    .HasDefaultValueSql("nextval('\"HET_PERSON_ID_seq\"'::regclass)")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a PERSON.");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .IsRequired()
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .IsRequired()
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.FirstName)
                    .HasColumnName("FIRST_NAME")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The first name of a PERSON.");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true")
                    .ForNpgsqlHasComment("A True/False value indicating whether this PERSON is active and can therefore be referenced in new data records.");

                entity.Property(e => e.MiddleNames)
                    .HasColumnName("MIDDLE_NAMES")
                    .HasMaxLength(200)
                    .ForNpgsqlHasComment("The names that may be inserted between a PERSONs FIRST NAME and SURNAME, which together with these, make up the full name for that PERSON.");

                entity.Property(e => e.NameSuffix)
                    .HasColumnName("NAME_SUFFIX")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("A suffix added to the name, such as Jr or Sr.");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasColumnName("SURNAME")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The last name or family name of a PERSON.");
            });

            modelBuilder.Entity<HetProject>(entity =>
            {
                entity.HasKey(e => e.ProjectId);

                entity.ToTable("HET_PROJECT");

                entity.ForNpgsqlHasComment("A Provincial Project that my from time to time request equipment under the HETS programme from a Service Area.");

                entity.HasIndex(e => e.DistrictId);

                entity.HasIndex(e => e.PrimaryContactId);

                entity.Property(e => e.ProjectId)
                    .HasColumnName("PROJECT_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a Project");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.DistrictId)
                    .HasColumnName("DISTRICT_ID")
                    .ForNpgsqlHasComment("The District associated with this Project record.");

                entity.Property(e => e.Information)
                    .HasColumnName("INFORMATION")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("Information about the Project needed by the HETS Clerks. Used for capturing varying (project by project) metadata needed to process requests related to the project.");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(100)
                    .ForNpgsqlHasComment("A descriptive name for the Project, useful to the HETS Clerk and Project Manager.");

                entity.Property(e => e.PrimaryContactId)
                    .HasColumnName("PRIMARY_CONTACT_ID")
                    .ForNpgsqlHasComment("Link to the designated Primary Contact for the Project - usually the Project Manager requesting to hire equipment.");

                entity.Property(e => e.ProvincialProjectNumber)
                    .HasColumnName("PROVINCIAL_PROJECT_NUMBER")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("TO BE REVIEWED WITH THE BUSINESS - The Provincial charge code for the equipment hiring related to this project. This will be the same across multiple service areas that provide equipment for the same Project.");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The status of the project to determine if it is listed when creating new requests");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetProject)
                    .HasForeignKey(d => d.DistrictId);

                entity.HasOne(d => d.PrimaryContact)
                    .WithMany(p => p.HetProject)
                    .HasForeignKey(d => d.PrimaryContactId);
            });

            modelBuilder.Entity<HetProjectStatusType>(entity =>
            {
                entity.HasKey(e => e.ProjectStatusTypeId);

                entity.ToTable("HET_PROJECT_STATUS_TYPE");

                entity.ForNpgsqlHasComment("A valid status that a given PROJECT may be in.");

                entity.Property(e => e.ProjectStatusTypeId)
                    .HasColumnName("PROJECT_STATUS_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_PROJECT_STATUS_TYPE_ID_seq\"'::regclass)")
                    .ForNpgsqlHasComment("A system-generated unique identifier for an PROJECT Status Type.");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .IsRequired()
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .IsRequired()
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("Free format text explaining the meaning of the given status type.");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("DISPLAY_ORDER")
                    .ForNpgsqlHasComment("A number indicating the order in which this status type should show when displayed on a picklist.");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true")
                    .ForNpgsqlHasComment("A True/False value indicating whether this status type is active and can therefore be used for classifying an PROJECT.");

                entity.Property(e => e.ProjectStatusTypeCode)
                    .IsRequired()
                    .HasColumnName("PROJECT_STATUS_TYPE_CODE")
                    .HasMaxLength(20)
                    .ForNpgsqlHasComment("A code value for an PROJECT Status Type.");

                entity.Property(e => e.ScreenLabel)
                    .HasColumnName("SCREEN_LABEL")
                    .HasMaxLength(200)
                    .ForNpgsqlHasComment("Free format text to be used for displaying this status type on a screen or report.");
            });

            modelBuilder.Entity<HetProvincialRateType>(entity =>
            {
                entity.HasKey(e => e.RateType);

                entity.ToTable("HET_PROVINCIAL_RATE_TYPE");

                entity.ForNpgsqlHasComment("The standard rates used in creating a new rental agreement.");

                entity.Property(e => e.RateType)
                    .HasColumnName("RATE_TYPE")
                    .HasMaxLength(20)
                    .ValueGeneratedNever()
                    .ForNpgsqlHasComment("A unique code value for a ProvincialRateType.");

                entity.Property(e => e.Active)
                    .HasColumnName("ACTIVE")
                    .ForNpgsqlHasComment("A flag indicating if this ProvincialRateType should be used on new rental agreements.");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(200)
                    .ForNpgsqlHasComment("A description of the ProvincialRateType as used in the rental agreement.");

                entity.Property(e => e.IsInTotalEditable)
                    .HasColumnName("IS_IN_TOTAL_EDITABLE")
                    .ForNpgsqlHasComment("Indicates if the user can override the default Included In Total value.");

                entity.Property(e => e.IsIncludedInTotal)
                    .HasColumnName("IS_INCLUDED_IN_TOTAL")
                    .ForNpgsqlHasComment("Indicates if this rate is added to the total in the rental agreement.");

                entity.Property(e => e.IsPercentRate)
                    .HasColumnName("IS_PERCENT_RATE")
                    .ForNpgsqlHasComment("Indicates this ProvincialRateType is calculated as a percentage of the base rate.");

                entity.Property(e => e.IsRateEditable)
                    .HasColumnName("IS_RATE_EDITABLE")
                    .ForNpgsqlHasComment("Indicates if a user can modify the rate for this ProvincialRateType.");

                entity.Property(e => e.PeriodType)
                    .HasColumnName("PERIOD_TYPE")
                    .HasMaxLength(20)
                    .ForNpgsqlHasComment("The period this ProvincialRateTYpe is billed at (either Hourly or Daily).");

                entity.Property(e => e.Rate)
                    .HasColumnName("RATE")
                    .ForNpgsqlHasComment("Rate in dollars for this ProvincialRateType.");
            });

            modelBuilder.Entity<HetRatePeriodType>(entity =>
            {
                entity.HasKey(e => e.RatePeriodTypeId);

                entity.ToTable("HET_RATE_PERIOD_TYPE");

                entity.ForNpgsqlHasComment("A valid status that a given RENTAL_REQUEST may be in.");

                entity.Property(e => e.RatePeriodTypeId)
                    .HasColumnName("RATE_PERIOD_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_RATE_PERIOD_TYPE_ID_seq\"'::regclass)")
                    .ForNpgsqlHasComment("A system-generated unique identifier for an RENTAL_REQUEST Status Type.");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .IsRequired()
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .IsRequired()
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("Free format text explaining the meaning of the given status type.");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("DISPLAY_ORDER")
                    .ForNpgsqlHasComment("A number indicating the order in which this status type should show when displayed on a picklist.");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true")
                    .ForNpgsqlHasComment("A True/False value indicating whether this status type is active and can therefore be used for classifying an RENTAL_REQUEST.");

                entity.Property(e => e.RatePeriodTypeCode)
                    .IsRequired()
                    .HasColumnName("RATE_PERIOD_TYPE_CODE")
                    .HasMaxLength(20)
                    .ForNpgsqlHasComment("A code value for an RENTAL_REQUEST Status Type.");

                entity.Property(e => e.ScreenLabel)
                    .HasColumnName("SCREEN_LABEL")
                    .HasMaxLength(200)
                    .ForNpgsqlHasComment("Free format text to be used for displaying this status type on a screen or report.");
            });

            modelBuilder.Entity<HetRegion>(entity =>
            {
                entity.HasKey(e => e.RegionId);

                entity.ToTable("HET_REGION");

                entity.ForNpgsqlHasComment("The Ministry of Transportion and Infrastructure REGION.");

                entity.Property(e => e.RegionId)
                    .HasColumnName("REGION_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a Region");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.EndDate)
                    .HasColumnName("END_DATE")
                    .ForNpgsqlHasComment("The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM");

                entity.Property(e => e.MinistryRegionId)
                    .HasColumnName("MINISTRY_REGION_ID")
                    .ForNpgsqlHasComment("A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The name of a Minsitry Region.");

                entity.Property(e => e.RegionNumber)
                    .HasColumnName("REGION_NUMBER")
                    .ForNpgsqlHasComment("A code that uniquely defines a Region.");

                entity.Property(e => e.StartDate)
                    .HasColumnName("START_DATE")
                    .ForNpgsqlHasComment("The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM");
            });

            modelBuilder.Entity<HetRentalAgreement>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementId);

                entity.ToTable("HET_RENTAL_AGREEMENT");

                entity.ForNpgsqlHasComment("Information about the hiring of a specific piece of equipment to satisfy part or all of a request from a project. TABLE DEFINITION IN PROGRESS - MORE COLUMNS TO BE ADDED");

                entity.HasIndex(e => e.EquipmentId);

                entity.HasIndex(e => e.ProjectId);

                entity.Property(e => e.RentalAgreementId)
                    .HasColumnName("RENTAL_AGREEMENT_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a RentalAgreement");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DatedOn)
                    .HasColumnName("DATED_ON")
                    .ForNpgsqlHasComment("The dated on date to put on the Rental Agreement.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.EquipmentId)
                    .HasColumnName("EQUIPMENT_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for an Equipment");

                entity.Property(e => e.EquipmentRate)
                    .HasColumnName("EQUIPMENT_RATE")
                    .ForNpgsqlHasComment("The dollar rate for the piece of equipment itself for this Rental Agreement. Other rates associated with the Rental Agreement are in the Rental Agreement Rate table.");

                entity.Property(e => e.EstimateHours)
                    .HasColumnName("ESTIMATE_HOURS")
                    .ForNpgsqlHasComment("The estimated number of hours of work to be put onto the Rental Agreement.");

                entity.Property(e => e.EstimateStartWork)
                    .HasColumnName("ESTIMATE_START_WORK")
                    .ForNpgsqlHasComment("The estimated start date of the work to be placed on the rental agreement.");

                entity.Property(e => e.Note)
                    .HasColumnName("NOTE")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("An optional note to be placed onto the Rental Agreement.");

                entity.Property(e => e.Number)
                    .HasColumnName("NUMBER")
                    .HasMaxLength(30)
                    .ForNpgsqlHasComment("A system-generated unique rental agreement number in a format defined by the business as suitable for the business and client to see and use.");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("PROJECT_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for a Project");

                entity.Property(e => e.RateComment)
                    .HasColumnName("RATE_COMMENT")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("A comment about the rate for the piece of equipment.");

                entity.Property(e => e.RatePeriod)
                    .HasColumnName("RATE_PERIOD")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The period of the rental rate. The vast majority will be hourly, but the rate could apply across a different period, e.g. daily.");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The current status of the Rental Agreement, such as Active or Complete");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetRentalAgreement)
                    .HasForeignKey(d => d.EquipmentId);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetRentalAgreement)
                    .HasForeignKey(d => d.ProjectId);
            });

            modelBuilder.Entity<HetRentalAgreementCondition>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementConditionId);

                entity.ToTable("HET_RENTAL_AGREEMENT_CONDITION");

                entity.ForNpgsqlHasComment("A condition about the rental agreement to be displayed on the Rental Agreement.");

                entity.HasIndex(e => e.RentalAgreementId);

                entity.Property(e => e.RentalAgreementConditionId)
                    .HasColumnName("RENTAL_AGREEMENT_CONDITION_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_AGREEMENT_CONDITIO_RENTAL_AGREEMENT_CONDITION_ID_seq\"'::regclass)")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a RentalAgreementCondition");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.Comment)
                    .HasColumnName("COMMENT")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("A comment about the condition to be applied to the Rental Agreement.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.ConditionName)
                    .HasColumnName("CONDITION_NAME")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The name of the condition to be placed onto the Rental Agreement.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.RentalAgreementId)
                    .HasColumnName("RENTAL_AGREEMENT_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for a Rental Agreement");

                entity.HasOne(d => d.RentalAgreement)
                    .WithMany(p => p.HetRentalAgreementCondition)
                    .HasForeignKey(d => d.RentalAgreementId)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_CONDITION_HET_RENTAL_AGREEMENT_RENTAL_A");
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

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.ConditionName)
                    .HasColumnName("CONDITION_NAME")
                    .HasMaxLength(150);

                entity.Property(e => e.DbCreateTimestamp).HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp).HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

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

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1");

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

                entity.Property(e => e.RatePeriod)
                    .HasColumnName("RATE_PERIOD")
                    .HasMaxLength(50);

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<HetRentalAgreementRate>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementRateId);

                entity.ToTable("HET_RENTAL_AGREEMENT_RATE");

                entity.ForNpgsqlHasComment("The rate associated with an element of a rental agreement.");

                entity.HasIndex(e => e.RentalAgreementId);

                entity.Property(e => e.RentalAgreementRateId)
                    .HasColumnName("RENTAL_AGREEMENT_RATE_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a RentalAgreementRate");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.Comment)
                    .HasColumnName("COMMENT")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("A comment about the rental of this component of the Rental Agreement.");

                entity.Property(e => e.ComponentName)
                    .HasColumnName("COMPONENT_NAME")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("Name of the component for the Rental Agreement for which the attached rates apply.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.IsAttachment)
                    .HasColumnName("IS_ATTACHMENT")
                    .ForNpgsqlHasComment("True if this rate is for an attachment to the piece of equipment.");

                entity.Property(e => e.IsIncludedInTotal)
                    .HasColumnName("IS_INCLUDED_IN_TOTAL")
                    .ForNpgsqlHasComment("Indicates if this rate is added to the total in the rental agreement.");

                entity.Property(e => e.PercentOfEquipmentRate)
                    .HasColumnName("PERCENT_OF_EQUIPMENT_RATE")
                    .ForNpgsqlHasComment("For other than the actual piece of equipment, the percent of the equipment rate to use for this component of the rental agreement.");

                entity.Property(e => e.Rate)
                    .HasColumnName("RATE")
                    .ForNpgsqlHasComment("The dollar rate associated with this component of the rental agreement.");

                entity.Property(e => e.RatePeriod)
                    .HasColumnName("RATE_PERIOD")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The period of the rental rate. The vast majority will be hourly, but the rate could apply across a different period, e.g. daily.");

                entity.Property(e => e.RentalAgreementId)
                    .HasColumnName("RENTAL_AGREEMENT_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for a Rental Agreement");

                entity.HasOne(d => d.RentalAgreement)
                    .WithMany(p => p.HetRentalAgreementRate)
                    .HasForeignKey(d => d.RentalAgreementId)
                    .HasConstraintName("FK_HET_RENTAL_AGREEMENT_RATE_HET_RENTAL_AGREEMENT_RENTAL_AGREEM");
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

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.DbCreateTimestamp).HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.DbLastUpdateTimestamp).HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63);

                entity.Property(e => e.EffectiveDate).HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.EndDate).HasColumnName("END_DATE");

                entity.Property(e => e.IsAttachment).HasColumnName("IS_ATTACHMENT");

                entity.Property(e => e.IsIncludedInTotal).HasColumnName("IS_INCLUDED_IN_TOTAL");

                entity.Property(e => e.PercentOfEquipmentRate).HasColumnName("PERCENT_OF_EQUIPMENT_RATE");

                entity.Property(e => e.Rate).HasColumnName("RATE");

                entity.Property(e => e.RatePeriod)
                    .HasColumnName("RATE_PERIOD")
                    .HasMaxLength(50);

                entity.Property(e => e.RentalAgreementId).HasColumnName("RENTAL_AGREEMENT_ID");

                entity.Property(e => e.RentalAgreementRateId).HasColumnName("RENTAL_AGREEMENT_RATE_ID");
            });

            modelBuilder.Entity<HetRentalAgreementStatusType>(entity =>
            {
                entity.HasKey(e => e.RentalAgreementStatusTypeId);

                entity.ToTable("HET_RENTAL_AGREEMENT_STATUS_TYPE");

                entity.ForNpgsqlHasComment("A valid status that a given RENTAL_AGREEMENT may be in.");

                entity.Property(e => e.RentalAgreementStatusTypeId)
                    .HasColumnName("RENTAL_AGREEMENT_STATUS_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_AGREEMENT_STATUS_TYPE_ID_seq\"'::regclass)")
                    .ForNpgsqlHasComment("A system-generated unique identifier for an RENTAL_AGREEMENT Status Type.");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .IsRequired()
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .IsRequired()
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("Free format text explaining the meaning of the given status type.");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("DISPLAY_ORDER")
                    .ForNpgsqlHasComment("A number indicating the order in which this status type should show when displayed on a picklist.");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true")
                    .ForNpgsqlHasComment("A True/False value indicating whether this status type is active and can therefore be used for classifying an RENTAL_AGREEMENT.");

                entity.Property(e => e.RentalAgreementStatusTypeCode)
                    .IsRequired()
                    .HasColumnName("RENTAL_AGREEMENT_STATUS_TYPE_CODE")
                    .HasMaxLength(20)
                    .ForNpgsqlHasComment("A code value for an RENTAL_AGREEMENT Status Type.");

                entity.Property(e => e.ScreenLabel)
                    .HasColumnName("SCREEN_LABEL")
                    .HasMaxLength(200)
                    .ForNpgsqlHasComment("Free format text to be used for displaying this status type on a screen or report.");
            });

            modelBuilder.Entity<HetRentalRequest>(entity =>
            {
                entity.HasKey(e => e.RentalRequestId);

                entity.ToTable("HET_RENTAL_REQUEST");

                entity.ForNpgsqlHasComment("A request from a Project for one or more of a type of equipment from a specific Local Area.");

                entity.HasIndex(e => e.DistrictEquipmentTypeId);

                entity.HasIndex(e => e.FirstOnRotationListId);

                entity.HasIndex(e => e.LocalAreaId);

                entity.HasIndex(e => e.ProjectId);

                entity.Property(e => e.RentalRequestId)
                    .HasColumnName("RENTAL_REQUEST_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a Request");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.DistrictEquipmentTypeId)
                    .HasColumnName("DISTRICT_EQUIPMENT_TYPE_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for an Equipment Type");

                entity.Property(e => e.EquipmentCount)
                    .HasColumnName("EQUIPMENT_COUNT")
                    .ForNpgsqlHasComment("The number of pieces of the equipment type wanted for hire as part of this request.");

                entity.Property(e => e.ExpectedEndDate)
                    .HasColumnName("EXPECTED_END_DATE")
                    .ForNpgsqlHasComment("The expected end date of each piece of equipment hired against this request, as provided by the Project Manager making the request.");

                entity.Property(e => e.ExpectedHours)
                    .HasColumnName("EXPECTED_HOURS")
                    .ForNpgsqlHasComment("The expected number of rental hours for each piece equipment hired against this request, as provided by the Project Manager making the request.");

                entity.Property(e => e.ExpectedStartDate)
                    .HasColumnName("EXPECTED_START_DATE")
                    .ForNpgsqlHasComment("The expected start date of each piece of equipment hired against this request, as provided by the Project Manager making the request.");

                entity.Property(e => e.FirstOnRotationListId)
                    .HasColumnName("FIRST_ON_ROTATION_LIST_ID")
                    .ForNpgsqlHasComment("The first piece of equipment on the rotation list at the time of the creation of the request.");

                entity.Property(e => e.LocalAreaId)
                    .HasColumnName("LOCAL_AREA_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for a Local Area");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The status of the Rental Request - whether it in progress, completed or was cancelled.");

                entity.HasOne(d => d.DistrictEquipmentType)
                    .WithMany(p => p.HetRentalRequest)
                    .HasForeignKey(d => d.DistrictEquipmentTypeId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_HET_DISTRICT_EQUIPMENT_TYPE_DISTRICT_EQUI");

                entity.HasOne(d => d.FirstOnRotationList)
                    .WithMany(p => p.HetRentalRequest)
                    .HasForeignKey(d => d.FirstOnRotationListId);

                entity.HasOne(d => d.LocalArea)
                    .WithMany(p => p.HetRentalRequest)
                    .HasForeignKey(d => d.LocalAreaId);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.HetRentalRequest)
                    .HasForeignKey(d => d.ProjectId);
            });

            modelBuilder.Entity<HetRentalRequestAttachment>(entity =>
            {
                entity.HasKey(e => e.RentalRequestAttachmentId);

                entity.ToTable("HET_RENTAL_REQUEST_ATTACHMENT");

                entity.ForNpgsqlHasComment("Attachments that are required as part of the Rental Requests");

                entity.HasIndex(e => e.RentalRequestId);

                entity.Property(e => e.RentalRequestAttachmentId)
                    .HasColumnName("RENTAL_REQUEST_ATTACHMENT_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a RentalRequestAttachment");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.Attachment)
                    .HasColumnName("ATTACHMENT")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The name&#x2F;type attachment needed as part of the fulfillment of the request");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.RentalRequestId)
                    .HasColumnName("RENTAL_REQUEST_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for a Rental Request");

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetRentalRequestAttachment)
                    .HasForeignKey(d => d.RentalRequestId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_ATTACHMENT_HET_RENTAL_REQUEST_RENTAL_REQU");
            });

            modelBuilder.Entity<HetRentalRequestRotationList>(entity =>
            {
                entity.HasKey(e => e.RentalRequestRotationListId);

                entity.ToTable("HET_RENTAL_REQUEST_ROTATION_LIST");

                entity.ForNpgsqlHasComment("An eligible piece of equipment for a request and a tracking of the hire offer and response process related to a request for that piece of equipment. Includes a link from the equipment to a Rental Agreement if the equipment was hired to satisfy a part of the request.");

                entity.HasIndex(e => e.EquipmentId);

                entity.HasIndex(e => e.RentalAgreementId);

                entity.HasIndex(e => e.RentalRequestId);

                entity.Property(e => e.RentalRequestRotationListId)
                    .HasColumnName("RENTAL_REQUEST_ROTATION_LIST_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_REQUEST_ROTATION_L_RENTAL_REQUEST_ROTATION_LIST__seq\"'::regclass)")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a RequestRotationList");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.AskedDateTime)
                    .HasColumnName("ASKED_DATE_TIME")
                    .ForNpgsqlHasComment("The Date-Time the HETS clerk contacted the equipment owner and asked to hire the piece of equipment.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.EquipmentId).HasColumnName("EQUIPMENT_ID");

                entity.Property(e => e.IsForceHire)
                    .HasColumnName("IS_FORCE_HIRE")
                    .ForNpgsqlHasComment("True if the HETS Clerk designated the hire of this equipment as being a Forced Hire. A Force Hire implies that the usual seniority rules for hiring are bypassed because of special circumstances related to the hire - e.g. a the hire requires an attachment only one piece of equipment has.");

                entity.Property(e => e.Note)
                    .HasColumnName("NOTE")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("An optional general note about the offer.");

                entity.Property(e => e.OfferRefusalReason)
                    .HasColumnName("OFFER_REFUSAL_REASON")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The reason why the user refused the offer based on a selection of values from the UI.");

                entity.Property(e => e.OfferResponse)
                    .HasColumnName("OFFER_RESPONSE")
                    .ForNpgsqlHasComment("The response to the offer to hire. Null prior to receiving a response; a string after with the response - likely just Yes or No");

                entity.Property(e => e.OfferResponseDatetime)
                    .HasColumnName("OFFER_RESPONSE_DATETIME")
                    .ForNpgsqlHasComment("The date and time the final response to the offer was established.");

                entity.Property(e => e.OfferResponseNote)
                    .HasColumnName("OFFER_RESPONSE_NOTE")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("A note entered about the response to the offer from the equipment owner about the offer. Usually used when the offer is a &quot;No&quot; or &quot;Force Hire&quot;.");

                entity.Property(e => e.RentalAgreementId)
                    .HasColumnName("RENTAL_AGREEMENT_ID")
                    .ForNpgsqlHasComment("The rental agreement (if any) created for an accepted hire offer.");

                entity.Property(e => e.RentalRequestId).HasColumnName("RENTAL_REQUEST_ID");

                entity.Property(e => e.RotationListSortOrder)
                    .HasColumnName("ROTATION_LIST_SORT_ORDER")
                    .ForNpgsqlHasComment("The sort order of the piece of equipment on the rotation list at the time the request was created. This is the order the equipment will be offered the available work.");

                entity.Property(e => e.WasAsked)
                    .HasColumnName("WAS_ASKED")
                    .ForNpgsqlHasComment("True if the HETS Clerk contacted the equipment owner and asked to hire the piece of equipment.");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetRentalRequestRotationList)
                    .HasForeignKey(d => d.EquipmentId);

                entity.HasOne(d => d.RentalAgreement)
                    .WithMany(p => p.HetRentalRequestRotationList)
                    .HasForeignKey(d => d.RentalAgreementId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_RENTAL_AGREEMENT_RENTAL");

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.HetRentalRequestRotationList)
                    .HasForeignKey(d => d.RentalRequestId)
                    .HasConstraintName("FK_HET_RENTAL_REQUEST_ROTATION_LIST_HET_RENTAL_REQUEST_RENTAL_R");
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

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1");

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

                entity.ForNpgsqlHasComment("A valid status that a given RENTAL_REQUEST may be in.");

                entity.Property(e => e.RentalRequestStatusTypeId)
                    .HasColumnName("RENTAL_REQUEST_STATUS_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_RENTAL_REQUEST_STATUS_TYPE_ID_seq\"'::regclass)")
                    .ForNpgsqlHasComment("A system-generated unique identifier for an RENTAL_REQUEST Status Type.");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .IsRequired()
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .IsRequired()
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("Free format text explaining the meaning of the given status type.");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("DISPLAY_ORDER")
                    .ForNpgsqlHasComment("A number indicating the order in which this status type should show when displayed on a picklist.");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true")
                    .ForNpgsqlHasComment("A True/False value indicating whether this status type is active and can therefore be used for classifying an RENTAL_REQUEST.");

                entity.Property(e => e.RentalRequestStatusTypeCode)
                    .IsRequired()
                    .HasColumnName("RENTAL_REQUEST_STATUS_TYPE_CODE")
                    .HasMaxLength(20)
                    .ForNpgsqlHasComment("A code value for an RENTAL_REQUEST Status Type.");

                entity.Property(e => e.ScreenLabel)
                    .HasColumnName("SCREEN_LABEL")
                    .HasMaxLength(200)
                    .ForNpgsqlHasComment("Free format text to be used for displaying this status type on a screen or report.");
            });

            modelBuilder.Entity<HetRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("HET_ROLE");

                entity.ForNpgsqlHasComment("A HETS application-managed Role that has a selected list of permissions and can be assigned to Users. A role coresponds to the authorization level provided a user based on the work for which they are responsible.");

                entity.HasIndex(e => e.Name)
                    .HasName("HET_ROLE_NAME_UK")
                    .IsUnique();

                entity.Property(e => e.RoleId)
                    .HasColumnName("ROLE_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a Role");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("A description of the role as set by the user creating&#x2F;updating the role.");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The name of the Role, as established by the user creating the role.");
            });

            modelBuilder.Entity<HetRolePermission>(entity =>
            {
                entity.HasKey(e => e.RolePermissionId);

                entity.ToTable("HET_ROLE_PERMISSION");

                entity.ForNpgsqlHasComment("A permission that is part of a Role - a component of the authorization provided by the Role to the user to which the Role is assigned.");

                entity.HasIndex(e => e.PermissionId);

                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RolePermissionId)
                    .HasColumnName("ROLE_PERMISSION_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a RolePermission");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.PermissionId)
                    .HasColumnName("PERMISSION_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for a Permission");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.HetRolePermission)
                    .HasForeignKey(d => d.PermissionId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.HetRolePermission)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<HetSeniorityAudit>(entity =>
            {
                entity.HasKey(e => e.SeniorityAuditId);

                entity.ToTable("HET_SENIORITY_AUDIT");

                entity.ForNpgsqlHasComment("The history of all changes to the seniority of a piece of equipment. The current seniority information (underlying data elements and the calculation result) is in the equipment record. Every time that information changes, the old values are copied to here, with a start date, end date range. In the normal case, an annual update triggers the old values being copied here and the new values put into the equipment record. If a user manually changes the values, the existing values are copied into a record added here.");

                entity.HasIndex(e => e.EquipmentId);

                entity.HasIndex(e => e.LocalAreaId);

                entity.HasIndex(e => e.OwnerId);

                entity.Property(e => e.SeniorityAuditId)
                    .HasColumnName("SENIORITY_AUDIT_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a SeniorityAudit");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.BlockNumber)
                    .HasColumnName("BLOCK_NUMBER")
                    .ForNpgsqlHasComment("The block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined by the business - 1, 2 or Open");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.EndDate)
                    .HasColumnName("END_DATE")
                    .ForNpgsqlHasComment("The effective date at which the Seniority data in this record ceased to be in effect.");

                entity.Property(e => e.EquipmentId)
                    .HasColumnName("EQUIPMENT_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for an Equipment");

                entity.Property(e => e.IsSeniorityOverridden)
                    .HasColumnName("IS_SENIORITY_OVERRIDDEN")
                    .ForNpgsqlHasComment("True if the Seniority for the piece of equipment was manually overridden. Set if a user has gone in and explicitly updated the seniority base information. Indicates that underlying numbers were manually overridden.");

                entity.Property(e => e.LocalAreaId)
                    .HasColumnName("LOCAL_AREA_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for a Local Area");

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OWNER_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for an Owner");

                entity.Property(e => e.OwnerOrganizationName)
                    .HasColumnName("OWNER_ORGANIZATION_NAME")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The name of the organization of the owner from the Owner Record, captured at the time this record was created.");

                entity.Property(e => e.Seniority)
                    .HasColumnName("SENIORITY")
                    .ForNpgsqlHasComment("The seniority calculation result for this piece of equipment. The calculation is based on the &quot;numYears&quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo).");

                entity.Property(e => e.SeniorityOverrideReason)
                    .HasColumnName("SENIORITY_OVERRIDE_REASON")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("A text reason for why the piece of equipments underlying data was overridden to change their seniority number.");

                entity.Property(e => e.ServiceHoursLastYear)
                    .HasColumnName("SERVICE_HOURS_LAST_YEAR")
                    .ForNpgsqlHasComment("Number of hours of service by this piece of equipment in the previous fiscal year");

                entity.Property(e => e.ServiceHoursThreeYearsAgo)
                    .HasColumnName("SERVICE_HOURS_THREE_YEARS_AGO")
                    .ForNpgsqlHasComment("Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015");

                entity.Property(e => e.ServiceHoursTwoYearsAgo)
                    .HasColumnName("SERVICE_HOURS_TWO_YEARS_AGO")
                    .ForNpgsqlHasComment("Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016");

                entity.Property(e => e.StartDate)
                    .HasColumnName("START_DATE")
                    .ForNpgsqlHasComment("The effective date that the Seniority data in this record went into effect.");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.HetSeniorityAudit)
                    .HasForeignKey(d => d.EquipmentId);

                entity.HasOne(d => d.LocalArea)
                    .WithMany(p => p.HetSeniorityAudit)
                    .HasForeignKey(d => d.LocalAreaId);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.HetSeniorityAudit)
                    .HasForeignKey(d => d.OwnerId);
            });

            modelBuilder.Entity<HetServiceArea>(entity =>
            {
                entity.HasKey(e => e.ServiceAreaId);

                entity.ToTable("HET_SERVICE_AREA");

                entity.ForNpgsqlHasComment("The Ministry of Transportation and Infrastructure SERVICE AREA.");

                entity.HasIndex(e => e.DistrictId);

                entity.Property(e => e.ServiceAreaId)
                    .HasColumnName("SERVICE_AREA_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a ServiceArea");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.AreaNumber)
                    .HasColumnName("AREA_NUMBER")
                    .ForNpgsqlHasComment("A number that uniquely defines a Ministry Service Area.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.DistrictId)
                    .HasColumnName("DISTRICT_ID")
                    .ForNpgsqlHasComment("The district in which the Service Area is found.");

                entity.Property(e => e.EndDate)
                    .HasColumnName("END_DATE")
                    .ForNpgsqlHasComment("The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM");

                entity.Property(e => e.MinistryServiceAreaId)
                    .HasColumnName("MINISTRY_SERVICE_AREA_ID")
                    .ForNpgsqlHasComment("A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The Name of a Ministry Service Area.");

                entity.Property(e => e.StartDate)
                    .HasColumnName("START_DATE")
                    .ForNpgsqlHasComment("The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetServiceArea)
                    .HasForeignKey(d => d.DistrictId);
            });

            modelBuilder.Entity<HetTimePeriodType>(entity =>
            {
                entity.HasKey(e => e.TimePeriodTypeId);

                entity.ToTable("HET_TIME_PERIOD_TYPE");

                entity.ForNpgsqlHasComment("A period of TIME that may be used to provide a boundary of applicability of some data. The vast majority will be hourly, but the TIME could apply across a different period, e.g. daily.");

                entity.Property(e => e.TimePeriodTypeId)
                    .HasColumnName("TIME_PERIOD_TYPE_ID")
                    .HasDefaultValueSql("nextval('\"HET_TIME_PERIOD_TYPE_ID_seq\"'::regclass)")
                    .ForNpgsqlHasComment("A system-generated unique identifier for an TIME_PERIOD Type.");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .IsRequired()
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("now()")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .IsRequired()
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .HasDefaultValueSql("\"current_user\"()")
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("Free format text explaining the meaning of the given TIME PERIOD type.");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("DISPLAY_ORDER")
                    .ForNpgsqlHasComment("A number indicating the order in which this time period type should show when displayed on a picklist.");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("true")
                    .ForNpgsqlHasComment("A True/False value indicating whether this time period type is active and can therefore be used for classifying a record.");

                entity.Property(e => e.ScreenLabel)
                    .HasColumnName("SCREEN_LABEL")
                    .HasMaxLength(200)
                    .ForNpgsqlHasComment("Free format text to be used for displaying this time period type on a screen or report.");

                entity.Property(e => e.TimePeriodTypeCode)
                    .IsRequired()
                    .HasColumnName("TIME_PERIOD_TYPE_CODE")
                    .HasMaxLength(20)
                    .ForNpgsqlHasComment("A code value for an TIME_PERIOD Type.");
            });

            modelBuilder.Entity<HetTimeRecord>(entity =>
            {
                entity.HasKey(e => e.TimeRecordId);

                entity.ToTable("HET_TIME_RECORD");

                entity.ForNpgsqlHasComment("A record of time worked for a piece of equipment hired for a specific project within a Local Area.");

                entity.HasIndex(e => e.RentalAgreementId);

                entity.HasIndex(e => e.RentalAgreementRateId);

                entity.Property(e => e.TimeRecordId)
                    .HasColumnName("TIME_RECORD_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a TimeRecord");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.EnteredDate)
                    .HasColumnName("ENTERED_DATE")
                    .ForNpgsqlHasComment("The date-time the time record information was entered.");

                entity.Property(e => e.Hours)
                    .HasColumnName("HOURS")
                    .ForNpgsqlHasComment("The number of hours worked by the equipment.");

                entity.Property(e => e.RentalAgreementId)
                    .HasColumnName("RENTAL_AGREEMENT_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for a Rental Agreement");

                entity.Property(e => e.RentalAgreementRateId)
                    .HasColumnName("RENTAL_AGREEMENT_RATE_ID")
                    .ForNpgsqlHasComment("The Rental Agreement Rate component to which this Rental Agreement applies. If null, this time applies to the equipment itself.");

                entity.Property(e => e.TimePeriod)
                    .HasColumnName("TIME_PERIOD")
                    .HasMaxLength(20)
                    .ForNpgsqlHasComment("The time period of the entry - either day or week. HETS Clerk have the option of entering time records on a day-by-day or week-by-week basis.");

                entity.Property(e => e.WorkedDate)
                    .HasColumnName("WORKED_DATE")
                    .ForNpgsqlHasComment("The date of the time record entry - the day of the entry if it is a daily entry, or a date in the week in which the work occurred if tracked weekly.");

                entity.HasOne(d => d.RentalAgreement)
                    .WithMany(p => p.HetTimeRecord)
                    .HasForeignKey(d => d.RentalAgreementId);

                entity.HasOne(d => d.RentalAgreementRate)
                    .WithMany(p => p.HetTimeRecord)
                    .HasForeignKey(d => d.RentalAgreementRateId)
                    .HasConstraintName("FK_HET_TIME_RECORD_HET_RENTAL_AGREEMENT_RATE_RENTAL_AGREEMENT_R");
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

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("1");

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

                entity.Property(e => e.TimePeriod)
                    .HasColumnName("TIME_PERIOD")
                    .HasMaxLength(20);

                entity.Property(e => e.TimeRecordId).HasColumnName("TIME_RECORD_ID");

                entity.Property(e => e.WorkedDate).HasColumnName("WORKED_DATE");
            });

            modelBuilder.Entity<HetUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("HET_USER");

                entity.ForNpgsqlHasComment("An identified user in the HETS Application that has a defined authorization level.");

                entity.HasIndex(e => e.DistrictId);

                entity.HasIndex(e => e.Guid)
                    .HasName("HET_USR_GUID_UK")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a User");

                entity.Property(e => e.Active)
                    .HasColumnName("ACTIVE")
                    .ForNpgsqlHasComment("A flag indicating the User is active in the system. Set false to remove access to the system for the user.");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.DistrictId)
                    .HasColumnName("DISTRICT_ID")
                    .ForNpgsqlHasComment("The District that the User belongs to");

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The email address of the user in the system.");

                entity.Property(e => e.GivenName)
                    .HasColumnName("GIVEN_NAME")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("Given name of the user.");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The GUID unique to the user as provided by the authentication system. In this case, authentication is done by Siteminder and the GUID uniquely identifies the user within the user directories managed by Siteminder - e.g. IDIR and BCeID. The GUID is equivalent to the IDIR Id, but is guaranteed unique to a person, while the IDIR ID is not - IDIR IDs can be recycled.");

                entity.Property(e => e.Initials)
                    .HasColumnName("INITIALS")
                    .HasMaxLength(10)
                    .ForNpgsqlHasComment("Initials of the user, to be presented where screen space is at a premium.");

                entity.Property(e => e.SmAuthorizationDirectory)
                    .HasColumnName("SM_AUTHORIZATION_DIRECTORY")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user directory service used by Siteminder to authenticate the user - usually IDIR or BCeID.");

                entity.Property(e => e.SmUserId)
                    .HasColumnName("SM_USER_ID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("Security Manager User ID");

                entity.Property(e => e.Surname)
                    .HasColumnName("SURNAME")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("Surname of the user.");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetUser)
                    .HasForeignKey(d => d.DistrictId);
            });

            modelBuilder.Entity<HetUserDistrict>(entity =>
            {
                entity.HasKey(e => e.UserDistrictId);

                entity.ToTable("HET_USER_DISTRICT");

                entity.ForNpgsqlHasComment("Manages users who work in multiple districts.");

                entity.HasIndex(e => e.DistrictId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserDistrictId)
                    .HasColumnName("USER_DISTRICT_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a UserDistrict.");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.DistrictId).HasColumnName("DISTRICT_ID");

                entity.Property(e => e.IsPrimary)
                    .HasColumnName("IS_PRIMARY")
                    .ForNpgsqlHasComment("A flag indicating if this is the Primary District for this user.");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.HetUserDistrict)
                    .HasForeignKey(d => d.DistrictId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HetUserDistrict)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<HetUserFavourite>(entity =>
            {
                entity.HasKey(e => e.UserFavouriteId);

                entity.ToTable("HET_USER_FAVOURITE");

                entity.ForNpgsqlHasComment("User specific settings for a specific location in the UI. The location and saved settings are internally defined by the UI.");

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserFavouriteId)
                    .HasColumnName("USER_FAVOURITE_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a UserFavourite");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.IsDefault)
                    .HasColumnName("IS_DEFAULT")
                    .ForNpgsqlHasComment("True if this Favourite is the default for this Context Type. On first access to a context in a session the default favourite for the context it is invoked. If there is no default favourite,  a system-wide default is invoked. On return to the context within a session,  the last parameters used are reapplied.");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The user-defined name for the recorded settings. Allows the user to save different groups of settings and access each one easily when needed.");

                entity.Property(e => e.Type)
                    .HasColumnName("TYPE")
                    .HasMaxLength(150)
                    .ForNpgsqlHasComment("The type of Favourite");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .ForNpgsqlHasComment("The User who has this Favourite");

                entity.Property(e => e.Value)
                    .HasColumnName("VALUE")
                    .HasMaxLength(2048)
                    .ForNpgsqlHasComment("The settings saved by the user. In general,  a UI defined chunk of json that stores the settings in place when the user created the favourite.");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HetUserFavourite)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<HetUserRole>(entity =>
            {
                entity.HasKey(e => e.UserRoleId);

                entity.ToTable("HET_USER_ROLE");

                entity.ForNpgsqlHasComment("A join table that provides allows each user to have any number of Roles in the system.  At login time the user is given the sum of the permissions of the roles assigned to that user.");

                entity.HasIndex(e => e.RoleId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserRoleId)
                    .HasColumnName("USER_ROLE_ID")
                    .ForNpgsqlHasComment("A system-generated unique identifier for a UserRole");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created the record.");

                entity.Property(e => e.AppCreateUserDirectory)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_CREATE_USERID is defined.");

                entity.Property(e => e.AppCreateUserGuid)
                    .HasColumnName("APP_CREATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who performed the action that created the record.");

                entity.Property(e => e.AppCreateUserid)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time of the application action that created or last updated the record.");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasMaxLength(50)
                    .ForNpgsqlHasComment("The directory in which APP_LAST_UPDATE_USERID is defined.");

                entity.Property(e => e.AppLastUpdateUserGuid)
                    .HasColumnName("APP_LAST_UPDATE_USER_GUID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The Globally Unique Identifier of the application user who most recently updated the record.");

                entity.Property(e => e.AppLastUpdateUserid)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasMaxLength(255)
                    .ForNpgsqlHasComment("The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .ForNpgsqlHasComment("Used to manage concurrency for the application.");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created.");

                entity.Property(e => e.DbCreateUserId)
                    .HasColumnName("DB_CREATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created the record.");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("'0001-01-01 00:00:00'::timestamp without time zone")
                    .ForNpgsqlHasComment("The date and time the record was created or last updated.");

                entity.Property(e => e.DbLastUpdateUserId)
                    .HasColumnName("DB_LAST_UPDATE_USER_ID")
                    .HasMaxLength(63)
                    .ForNpgsqlHasComment("The user or proxy account that created or last updated the record.");

                entity.Property(e => e.EffectiveDate)
                    .HasColumnName("EFFECTIVE_DATE")
                    .ForNpgsqlHasComment("The date on which the user was given the related role.");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnName("EXPIRY_DATE")
                    .ForNpgsqlHasComment("The date on which a role previously assigned to a user was removed from that user.");

                entity.Property(e => e.RoleId)
                    .HasColumnName("ROLE_ID")
                    .ForNpgsqlHasComment("A foreign key reference to the system-generated unique identifier for a Role");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.HetUserRole)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HetUserRole)
                    .HasForeignKey(d => d.UserId);
            });
            
            modelBuilder.HasSequence("HET_EQUIPMENT_ATTACHMENT_HIST_ID_seq");

            modelBuilder.HasSequence("HET_EQUIPMENT_COPY_EQUIPMENT_ID_seq");

            modelBuilder.HasSequence("HET_EQUIPMENT_COPY_HIST_EQUIPMENT_ID_seq");

            modelBuilder.HasSequence("HET_EQUIPMENT_HIST_ID_seq");

            modelBuilder.HasSequence("HET_EQUIPMENT_STATUS_TYPE_ID_seq").StartsAt(92);

            modelBuilder.HasSequence("HET_MIME_TYPE_ID_seq").StartsAt(92);

            modelBuilder.HasSequence("HET_NOTE_HIST_ID_seq");

            modelBuilder.HasSequence("HET_OWNER_STATUS_TYPE_ID_seq").StartsAt(92);

            modelBuilder.HasSequence("HET_PERSON_ID_seq").StartsAt(92);

            modelBuilder.HasSequence("HET_PROJECT_STATUS_TYPE_ID_seq").StartsAt(92);

            modelBuilder.HasSequence("HET_RATE_PERIOD_TYPE_ID_seq").StartsAt(92);

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_CONDITIO_RENTAL_AGREEMENT_CONDITION_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_CONDITION_HIST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_HIST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_RATE_HIST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_AGREEMENT_STATUS_TYPE_ID_seq").StartsAt(92);

            modelBuilder.HasSequence("HET_RENTAL_REQUEST_ROTATION_L_RENTAL_REQUEST_ROTATION_LIST__seq");

            modelBuilder.HasSequence("HET_RENTAL_REQUEST_ROTATION_LIST_HIST_ID_seq");

            modelBuilder.HasSequence("HET_RENTAL_REQUEST_STATUS_TYPE_ID_seq").StartsAt(92);

            modelBuilder.HasSequence("HET_TIME_PERIOD_TYPE_ID_seq").StartsAt(92);

            modelBuilder.HasSequence("HET_TIME_RECORD_HIST_ID_seq");
        }
    }
}
