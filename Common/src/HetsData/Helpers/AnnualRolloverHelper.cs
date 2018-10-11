using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HetsData.Model;

namespace HetsData.Helpers
{
    /// <summary>
    /// Annual Rollover List Helper
    /// </summary>
    public static class AnnualRolloverHelper
    {
        #region Get a District Status record

        /// <summary>
        /// Get a District Status record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static HetDistrictStatus GetRecord(int id, DbAppContext context)
        {
            HetDistrictStatus status = context.HetDistrictStatus.AsNoTracking()
                .Include(a => a.District)
                .FirstOrDefault(a => a.DistrictId == id);

            // if there isn't a status - we'll add one now
            if (status == null)
            {
                // *******************************************************************************
                // determine current fiscal year - check for existing rotation lists this year
                // *******************************************************************************
                DateTime fiscalStart;

                if (DateTime.UtcNow.Month == 1 || DateTime.UtcNow.Month == 2 || DateTime.UtcNow.Month == 3)
                {
                    fiscalStart = new DateTime(DateTime.UtcNow.AddYears(-1).Year, 4, 1);
                }
                else
                {
                    fiscalStart = new DateTime(DateTime.UtcNow.Year, 4, 1);
                }

                status = new HetDistrictStatus
                {
                    DistrictId = id,
                    CurrentFiscalYear = fiscalStart.Year,
                    NextFiscalYear = fiscalStart.Year + 1,
                    DisplayRolloverMessage = false,
                    ProgressPercentage = 100
                };

                context.HetDistrictStatus.Add(status);
                context.SaveChanges();

                // get updated record
                status = context.HetDistrictStatus.AsNoTracking()
                    .Include(a => a.District)
                    .First(a => a.DistrictId == id);
            }

            return status;
        }

        #endregion

        /*
        /// <summary>
        /// Hangfire job to do the Annual Rollover tasks
        /// </summary>
        /// <param name="context"></param>
        /// <param name="connectionString"></param>
        /// <param name="configuration"></param>
        public static void AnnualRolloverJob(PerformContext context, string connectionString, IConfiguration configuration)
        {
            try
            {
                // open a connection to the database
                DbContextOptionsBuilder<DbAppContext> options = new DbContextOptionsBuilder<DbAppContext>();
                options.UseNpgsql(connectionString);
                DbAppContext dbContext = new DbAppContext(null, options.Options);

                // get processing rules
                SeniorityScoringRules scoringRules = new SeniorityScoringRules(configuration);

                // update progress bar
                IProgressBar progress = context.WriteProgressBar();
                context.WriteLine("Starting Annual Rollover Job");

                progress.SetValue(0);

                // get all equipment types
                List<EquipmentType> equipmentTypes = dbContext.EquipmentTypes.ToList();

                // The annual rollover will process all local areas in turn
                List<LocalArea> localAreas = dbContext.LocalAreas.ToList();

                foreach (LocalArea localArea in localAreas.WithProgress(progress))
                {
                    if (localArea.Name != null)
                    {
                        context.WriteLine("Local Area: " + localArea.Name);
                    }
                    else
                    {
                        context.WriteLine("Local Area ID: " + localArea.Id);
                    }

                    foreach (EquipmentType equipmentType in equipmentTypes)
                    {
                        // it this a dump truck? 
                        bool isDumpTruck = equipmentType.IsDumpTruck;

                        // get rules for scoring and seniority block
                        int seniorityScoring = isDumpTruck ? scoringRules.GetEquipmentScore("DumpTruck") : scoringRules.GetEquipmentScore();
                        int blockSize = isDumpTruck ? scoringRules.GetBlockSize("DumpTruck") : scoringRules.GetBlockSize();
                        int totalBlocks = isDumpTruck ? scoringRules.GetTotalBlocks("DumpTruck") : scoringRules.GetTotalBlocks();

                        using (DbAppContext etContext = new DbAppContext(null, options.Options))
                        {
                            List<Equipment> data = etContext.Equipments
                                .Include(x => x.LocalArea)
                                .Include(x => x.DistrictEquipmentType.EquipmentType)
                                .Where(x => x.Status == Equipment.StatusApproved &&
                                            x.LocalArea.Id == localArea.Id &&
                                            x.DistrictEquipmentType.EquipmentType.Id == equipmentType.Id)
                                .Select(x => x)
                                .ToList();

                            foreach (Equipment equipment in data)
                            {
                                // rollover the year
                                equipment.ServiceHoursThreeYearsAgo = equipment.ServiceHoursTwoYearsAgo;
                                equipment.ServiceHoursTwoYearsAgo = equipment.ServiceHoursLastYear;
                                equipment.ServiceHoursLastYear = equipment.GetYtdServiceHours(dbContext);
                                equipment.CalculateYearsOfService(DateTime.UtcNow);

                                // blank out the override reason
                                equipment.SeniorityOverrideReason = "";

                                // update the seniority score
                                equipment.CalculateSeniority(seniorityScoring);

                                etContext.Equipments.Update(equipment);
                                etContext.SaveChanges();
                                etContext.Entry(equipment).State = EntityState.Detached;
                            }
                        }

                        // now update the rotation list
                        using (DbAppContext abContext = new DbAppContext(null, options.Options))
                        {
                            int localAreaId = localArea.Id;
                            int equipmentTypeId = equipmentType.Id;

                            AssignBlocks(abContext, localAreaId, equipmentTypeId, blockSize, totalBlocks);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        */
    }
}
