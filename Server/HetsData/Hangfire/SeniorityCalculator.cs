using Hangfire;
using HetsData.Helpers;
using HetsData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace HetsData.Hangfire
{
    public class SeniorityCalculator
    {
        private DbAppContext _dbContext;
        private string _jobId;
        private ILogger<SeniorityCalculator> _logger;

        public SeniorityCalculator(DbAppContext dbContext, ILogger<SeniorityCalculator> logger)
        {
            _dbContext = dbContext;
            _jobId = Guid.NewGuid().ToString();
            _logger = logger;
        }
        /// <summary>
        /// Recalculates seniority with the new sorting rule (sorting by equipment code) for the district equipment types that have the same seniority and received date
        /// </summary>
        /// <param name="context"></param>
        /// <param name="seniorityScoringRules"></param>
        /// <param name="connectionString"></param>
        [SkipSameJob]
        [AutomaticRetry(Attempts = 0)]
        public void RecalculateSeniorityList(string seniorityScoringRules, Action<string> logInfoAction, Action<string, Exception> logErrorAction)
        {
            // get equipment status
            int? equipmentStatusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _dbContext);
            if (equipmentStatusId == null)
            {
                throw new ArgumentException("Status Code not found");
            }

            WriteLog("Recalculation Started", logInfoAction);

            var equipments = _dbContext.HetEquipments
                .Where(x => x.EquipmentStatusTypeId == equipmentStatusId)
                .GroupBy(x => new { x.LocalAreaId, x.DistrictEquipmentTypeId, x.Seniority, x.ReceivedDate })
                .Where(x => x.Count() > 1)
                .Select(x => new { x.Key.LocalAreaId, x.Key.DistrictEquipmentTypeId })
                .Distinct()
                .ToList();

            var count = 0;
            foreach (var equipment in equipments)
            {
                EquipmentHelper.RecalculateSeniority(equipment.LocalAreaId, equipment.DistrictEquipmentTypeId, _dbContext, seniorityScoringRules, logErrorAction);
                WriteLog($"Processed {count} / {equipments.Count}", logInfoAction);
            }

            _dbContext.SaveChanges();

            WriteLog("Recalculation Finished", logInfoAction);
        }

        private void WriteLog(string message, Action<string> logInfoAction)
        {
            logInfoAction($"Seniority Calculator[{_jobId}] {message}");
        }
    }
}
