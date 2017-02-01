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

namespace HETSAPI.Models
{
    public interface IDbAppContextFactory
    {
        IDbAppContext Create();
    }


    public class DbAppContextFactory : IDbAppContextFactory
    {
        DbContextOptions<DbAppContext> _options;

        public DbAppContextFactory(DbContextOptions<DbAppContext> options)
        {
            _options = options;
        }

        public IDbAppContext Create()
        {
            return new DbAppContext(_options);
        }
    }


    public interface IDbAppContext
    {
        DbSet<Attachment> Attachments { get; set; }
        DbSet<City> Cities { get; set; }
        DbSet<Contact> Contacts { get; set; }
        DbSet<ContactAddress> ContactAddresss { get; set; }
        DbSet<ContactPhone> ContactPhones { get; set; }
        DbSet<District> Districts { get; set; }
        DbSet<DumpTruck> DumpTrucks { get; set; }
        DbSet<Equipment> Equipments { get; set; }
        DbSet<EquipmentAttachment> EquipmentAttachments { get; set; }
        DbSet<EquipmentAttachmentType> EquipmentAttachmentTypes { get; set; }
        DbSet<EquipmentType> EquipmentTypes { get; set; }
        DbSet<FavouriteContextType> FavouriteContextTypes { get; set; }
        DbSet<Group> Groups { get; set; }
        DbSet<GroupMembership> GroupMemberships { get; set; }
        DbSet<HireOffer> HireOffers { get; set; }
        DbSet<History> Historys { get; set; }
        DbSet<LocalArea> LocalAreas { get; set; }
        DbSet<Note> Notes { get; set; }
        DbSet<Owner> Owners { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<Region> Regions { get; set; }
        DbSet<RentalAgreement> RentalAgreements { get; set; }
        DbSet<Request> Requests { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<RolePermission> RolePermissions { get; set; }
        DbSet<RotationList> RotationLists { get; set; }
        DbSet<RotationListBlock> RotationListBlocks { get; set; }
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
        /// <summary>
        /// Constructor for Class used for Entity Framework access.
        /// </summary>
        public DbAppContext(DbContextOptions<DbAppContext> options)
                                : base(options)
        { }

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
        public virtual DbSet<ContactAddress> ContactAddresss { get; set; }
        public virtual DbSet<ContactPhone> ContactPhones { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<DumpTruck> DumpTrucks { get; set; }
        public virtual DbSet<Equipment> Equipments { get; set; }
        public virtual DbSet<EquipmentAttachment> EquipmentAttachments { get; set; }
        public virtual DbSet<EquipmentAttachmentType> EquipmentAttachmentTypes { get; set; }
        public virtual DbSet<EquipmentType> EquipmentTypes { get; set; }
        public virtual DbSet<FavouriteContextType> FavouriteContextTypes { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupMembership> GroupMemberships { get; set; }
        public virtual DbSet<HireOffer> HireOffers { get; set; }
        public virtual DbSet<History> Historys { get; set; }
        public virtual DbSet<LocalArea> LocalAreas { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<RentalAgreement> RentalAgreements { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<RotationList> RotationLists { get; set; }
        public virtual DbSet<RotationListBlock> RotationListBlocks { get; set; }
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
    }
}
