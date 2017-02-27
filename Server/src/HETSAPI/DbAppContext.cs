/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace HETSAPI.Models
{
    public interface IDbAppContextFactory
    {
        IDbAppContext Create();
    }


    public class DbAppContextFactory : IDbAppContextFactory
    {
        DbContextOptions<DbAppContext> _options;
        IHttpContextAccessor _httpContextAccessor;

        public DbAppContextFactory(IHttpContextAccessor httpContextAccessor, DbContextOptions<DbAppContext> options)
        {
            _options = options;
            _httpContextAccessor = httpContextAccessor;
        }

        public IDbAppContext Create()
        {
            return new DbAppContext(_httpContextAccessor, _options);
        }
    }


    public interface IDbAppContext
    {
        DbSet<Attachment> Attachments { get; set; }
        DbSet<City> Cities { get; set; }
        DbSet<Contact> Contacts { get; set; }        
        DbSet<District> Districts { get; set; }
        DbSet<DumpTruck> DumpTrucks { get; set; }
        DbSet<Equipment> Equipments { get; set; }
        DbSet<EquipmentAttachment> EquipmentAttachments { get; set; }        
        DbSet<EquipmentType> EquipmentTypes { get; set; }
        DbSet<EquipmentTypeNextRental> EquipmentTypeNextRentals { get; set; }
        DbSet<Group> Groups { get; set; }
        DbSet<GroupMembership> GroupMemberships { get; set; }        
        DbSet<History> Historys { get; set; }
        DbSet<LocalArea> LocalAreas { get; set; }
        DbSet<Note> Notes { get; set; }
        DbSet<Owner> Owners { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<Region> Regions { get; set; }
        DbSet<RentalAgreement> RentalAgreements { get; set; }
        DbSet<RentalAgreementCondition> RentalAgreementConditions { get; set; }
        DbSet<RentalAgreementRate> RentalAgreementRates { get; set; }
        DbSet<RentalRequest> RentalRequests { get; set; }
        DbSet<RentalRequestRotationList> RentalRequestRotationLists { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<RolePermission> RolePermissions { get; set; }        
        DbSet<SeniorityAudit> SeniorityAudits { get; set; }
        DbSet<ServiceArea> ServiceAreas { get; set; }
        DbSet<TimeRecord> TimeRecords { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserFavourite> UserFavourites { get; set; }
        DbSet<UserRole> UserRoles { get; set; }


        /// <summary>
        /// Starts a new transaction.
        /// </summary>
        /// <returns>
        /// A Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction that represents
        /// the started transaction.
        /// </returns>
        IDbContextTransaction BeginTransaction();

        int SaveChanges();
    }

    public class DbAppContext : DbContext, IDbAppContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor for Class used for Entity Framework access.
        /// </summary>
        public DbAppContext(IHttpContextAccessor httpContextAccessor, DbContextOptions<DbAppContext> options)
                                : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Override for OnModelCreating - used to change the database naming convention.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // add our naming convention extension
            modelBuilder.UpperCaseUnderscoreSingularConvention();
        }

        // Add methods here to get and set items in the model.
        // For example:

        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }        
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<DumpTruck> DumpTrucks { get; set; }
        public virtual DbSet<Equipment> Equipments { get; set; }
        public virtual DbSet<EquipmentAttachment> EquipmentAttachments { get; set; }        
        public virtual DbSet<EquipmentType> EquipmentTypes { get; set; }
        public virtual DbSet<EquipmentTypeNextRental> EquipmentTypeNextRentals { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupMembership> GroupMemberships { get; set; }        
        public virtual DbSet<History> Historys { get; set; }
        public virtual DbSet<LocalArea> LocalAreas { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<RentalAgreement> RentalAgreements { get; set; }
        public virtual DbSet<RentalAgreementCondition> RentalAgreementConditions { get; set; }
        public virtual DbSet<RentalAgreementRate> RentalAgreementRates { get; set; }
        public virtual DbSet<RentalRequest> RentalRequests { get; set; }
        public virtual DbSet<RentalRequestRotationList> RentalRequestRotationLists { get; set; }        
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }                
        public virtual DbSet<SeniorityAudit> SeniorityAudits { get; set; }
        public virtual DbSet<ServiceArea> ServiceAreas { get; set; }
        public virtual DbSet<TimeRecord> TimeRecords { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserFavourite> UserFavourites { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }


        /// <summary>
        /// Starts a new transaction.
        /// </summary>
        /// <returns>
        /// A Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction that represents
        /// the started transaction.
        /// </returns>
        public virtual IDbContextTransaction BeginTransaction()
        {
            bool existingTransaction = true;
            IDbContextTransaction transaction = this.Database.CurrentTransaction;
            if (transaction == null)
            {
                existingTransaction = false;
                transaction = this.Database.BeginTransaction();
            }
            return new DbContextTransactionWrapper(transaction, existingTransaction);
        }

        /// <summary>
        /// Returns the current web user
        /// </summary>
        protected ClaimsPrincipal HttpContextUser
        {
            get { return _httpContextAccessor.HttpContext.User; }
        }

        /// <summary>
        /// Returns the current user ID 
        /// </summary>
        /// <returns></returns>
        protected string GetCurrentUserId()
        {
            string result = null;

            try
            {
                result = HttpContextUser.FindFirst(ClaimTypes.Name).Value;
            }
            catch (Exception e)
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Override for Save Changes to implement the audit log
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            // update the audit fields for this item.
            string smUserId = null;
            if (_httpContextAccessor != null)
                smUserId = GetCurrentUserId();

            var modifiedEntries = ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            DateTime currentTime = DateTime.UtcNow;

            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity.GetType().InheritsOrImplements(typeof(AuditableEntity)))
                {
                    var theObject = (AuditableEntity)entry.Entity;
                    theObject.LastUpdateUserid = smUserId;
                    theObject.LastUpdateTimestamp = currentTime;

                    if (entry.State == EntityState.Added)
                    {
                        theObject.CreateUserid = smUserId;
                        theObject.CreateTimestamp = currentTime;
                    }
                }
            }

            return base.SaveChanges();
        }
    }
}
