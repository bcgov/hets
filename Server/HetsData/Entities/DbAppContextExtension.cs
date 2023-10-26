using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using HetsCommon;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace HetsData.Entities
{
    public partial class DbAppContext
    {
        public string SmUserId { get; set; } = null;
        public string DirectoryName { get; set; } = null;
        public string SmUserGuid { get; set; } = null;
        public string SmBusinessGuid { get; set; } = null;

        #region Method for Batch / Bulk Saving of Records

        /// <summary>
        /// For importing data only (mass inserts)
        /// </summary>
        /// <returns></returns>
        public int SaveChangesForImport()
        {
            // update the audit fields for this item.
            IEnumerable<EntityEntry> modifiedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || 
                            e.State == EntityState.Modified);

            int records = 0;

            foreach (EntityEntry entry in modifiedEntries)
            {
                records++;

                if (AuditableEntity(entry.Entity))
                {
                    if (entry.State == EntityState.Added)
                    {
                        SetAuditProperty(entry.Entity, "ConcurrencyControlNumber", 1);
                    }
                    else
                    {
                        int controlNumber = (int)GetAuditProperty(entry.Entity, "ConcurrencyControlNumber");
                        controlNumber = controlNumber + 1;
                        SetAuditProperty(entry.Entity, "ConcurrencyControlNumber", controlNumber);
                    }
                }                
            }

            _logger.LogInformation($"Saving Import Data. Total Entries: {records}");

            int result;

            try
            {
                result = base.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError($"SaveChangesForImport exception: {e.ToString()}");

                // e.InnerException.Message	"20180: Concurrency Failure 5"	string
                if (e.InnerException != null &&
                    e.InnerException.Message.StartsWith("20180"))
                {
                    // concurrency error
                    throw new HetsDbConcurrencyException("This record has been updated by another user.");
                }

                throw;
            }

            return result;
        }

        #endregion

        #region Override SaveChanges Method (include audit data)

        private static bool AuditableEntity(object objectToCheck)
        {
            Type type = objectToCheck.GetType();
            return type.GetProperty("AppCreateUserDirectory") != null;
        }

        private static object GetAuditProperty(object obj, string property)
        {
            PropertyInfo prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);

            if (prop != null && prop.CanRead)
            {
                return prop.GetValue(obj);
            }

            return null;
        }

        private static void SetAuditProperty(object obj, string property, object value)
        {
            PropertyInfo prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);

            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(obj, value, null);
            }
        }



        public void UpdateDateTimeProperties(object entity)
        {
            UpdateDateTimePropertiesRecursively(entity, "Entity");
        }

        private void UpdateDateTimePropertiesRecursively(object obj, string objName)
        {
            if (obj == null)
                return;

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object propertyValue = property.GetValue(obj);
                string propertyName = $"{objName}.{property.Name}";

                if (propertyValue is DateTime)
                {
                    DateTime tempDateTime = (DateTime)propertyValue;

                    if (tempDateTime.Kind != DateTimeKind.Utc)
                    {
                        tempDateTime = DateTime.SpecifyKind(tempDateTime, DateTimeKind.Utc); // Update the Kind to Utc
                        property.SetValue(obj, tempDateTime); // Set the updated value back to the object
                        //Console.WriteLine($"DateTime property found: {propertyName} - {tempDateTime}");
                    }
                }

                if (propertyValue is IEnumerable enumerable && propertyValue.GetType() != typeof(string))
                {
                    int index = 0;
                    foreach (var item in enumerable)
                    {
                        string indexedPropertyName = $"{propertyName}[{index}]";
                        UpdateDateTimePropertiesRecursively(item, indexedPropertyName);
                        index++;
                    }
                }
            }
        }

        /// <summary>
        /// Override for Save Changes to implement the audit log
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {

            // get all of the modified records
            List<EntityEntry> modifiedEntries = ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added ||
                                e.State == EntityState.Modified)
                    .ToList();

            // manage the audit columns and the concurrency column
            DateTime currentTime = DateTime.UtcNow;

            List<HetSeniorityAudit> seniorityAudits = new List<HetSeniorityAudit>();

            foreach (EntityEntry entry in modifiedEntries)
            {
                if (AuditableEntity(entry.Entity))
                {
                    SetAuditProperty(entry.Entity, "AppLastUpdateUserid", SmUserId);
                    SetAuditProperty(entry.Entity, "AppLastUpdateUserDirectory", DirectoryName);
                    SetAuditProperty(entry.Entity, "AppLastUpdateUserGuid", SmUserGuid);
                    SetAuditProperty(entry.Entity, "AppLastUpdateTimestamp", currentTime);

                    if (entry.State == EntityState.Added)
                    {
                        SetAuditProperty(entry.Entity, "AppCreateUserid", SmUserId);
                        SetAuditProperty(entry.Entity, "AppCreateUserDirectory", DirectoryName);
                        SetAuditProperty(entry.Entity, "AppCreateUserGuid", SmUserGuid);
                        SetAuditProperty(entry.Entity, "AppCreateTimestamp", currentTime);
                        SetAuditProperty(entry.Entity, "ConcurrencyControlNumber", 1);
                    }
                    else
                    {
                        int controlNumber = (int)GetAuditProperty(entry.Entity, "ConcurrencyControlNumber");
                        controlNumber = controlNumber + 1;
                        SetAuditProperty(entry.Entity, "ConcurrencyControlNumber", controlNumber);
                    }
                }

                

                if (entry.Entity is HetEquipment)
                {
                    DoEquipmentAudit(seniorityAudits, entry, SmUserId);
                }

                UpdateDateTimeProperties(entry.Entity);
            }

            // *************************************************
            // attempt to save updates
            // *************************************************
            int result;

            try
            {
                result = base.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError($"SaveChanges exception: {e.ToString()}");

                // e.InnerException.Message	"20180: Concurrency Failure 5"	string
                if (e.InnerException != null &&
                    e.InnerException.Message.StartsWith("20180"))
                {
                    // concurrency error
                    throw new HetsDbConcurrencyException("This record has been updated by another user.");
                }

                throw;
            }

            // *************************************************
            // manage seniority audit records
            // *************************************************
            if (seniorityAudits.Count > 0)
            {
                foreach (HetSeniorityAudit seniorityAudit in seniorityAudits)
                {
                    HetSeniorityAudits.Add(seniorityAudit);
                }
            }

            base.SaveChanges();

            return result;
        }

        #endregion

        #region Equipment Audit

        private void DoEquipmentAudit(List<HetSeniorityAudit> audits, EntityEntry entry, string smUserId)
        {
            HetEquipment changed = (HetEquipment)entry.Entity;

            int tempChangedId = changed.EquipmentId;

            // if this is an "empty" record - exit
            if (tempChangedId <= 0)
            {
                return;
            }

            HetEquipment original = HetEquipments.AsNoTracking()
                .Include(x => x.LocalArea)
                .Include(x => x.Owner)
                .FirstOrDefault(a => a.EquipmentId == tempChangedId);

            // record doesn't exist
            if (original == null)
            {
                return;
            }

            // compare the old and new
            if (changed.IsSeniorityAuditRequired(original))
            {
                DateTime currentTime = DateTime.UtcNow;

                // create the audit entry.
                HetSeniorityAudit seniorityAudit = new()
                {
                    BlockNumber = original.BlockNumber,
                    EndDate = currentTime
                };

                int tempLocalAreaId = original.LocalArea.LocalAreaId;
                int tempOwnerId = original.Owner.OwnerId;

                changed.SeniorityEffectiveDate = currentTime;
                seniorityAudit.AppCreateTimestamp = currentTime;
                seniorityAudit.AppLastUpdateTimestamp = currentTime;
                seniorityAudit.AppCreateUserid = smUserId;
                seniorityAudit.AppLastUpdateUserid = smUserId;
                seniorityAudit.EquipmentId = tempChangedId;
                seniorityAudit.LocalAreaId = tempLocalAreaId;
                seniorityAudit.OwnerId = tempOwnerId;

                if (seniorityAudit.Owner != null)
                {
                    seniorityAudit.OwnerOrganizationName = seniorityAudit.Owner.OrganizationName;
                }

                if (original.SeniorityEffectiveDate is DateTime seniorityEffectiveDateUtc)
                {
                    seniorityAudit.StartDate = DateUtils.AsUTC(seniorityEffectiveDateUtc);
                }

                seniorityAudit.Seniority = original.Seniority;
                seniorityAudit.ServiceHoursLastYear = original.ServiceHoursLastYear;
                seniorityAudit.ServiceHoursTwoYearsAgo = original.ServiceHoursTwoYearsAgo;
                seniorityAudit.ServiceHoursThreeYearsAgo = original.ServiceHoursThreeYearsAgo;

                audits.Add(seniorityAudit);
            }
        }

        #endregion        
    }
}
