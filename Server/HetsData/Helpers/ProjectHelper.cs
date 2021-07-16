using System;
using System.Collections.Generic;
using System.Linq;
using HetsData.Model;
using Microsoft.EntityFrameworkCore;

namespace HetsData.Helpers
{
    #region Project Models

    public class ProjectLite
    {
        public int Id { get; set; }
        public HetDistrict District { get; set; }
        public string Name { get; set; }
        public HetContact PrimaryContact { get; set; }        
        public int? Hires { get; set; }
        public int? Requests { get; set; }
        public string Status { get; set; }
        public string ProvincialProjectNumber { get; set; }
        public string FiscalYear { get; set; }
    }

    public class ProjectLiteList
    {
        public int Id { get; set; }
        public string Name { get; set; }        
    }

    public class ProjectAgreementSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> AgreementIds { get; set; }
    }

    public class ProjectRentalAgreementClone
    {
        public int ProjectId { get; set; }
        public int AgreementToCloneId { get; set; }
        public int RentalAgreementId { get; set; }
    }

    #endregion

    public static class ProjectHelper
    {
        #region Convert full project record to a "Lite" version

        /// <summary>
        /// Convert to Project Lite Model
        /// </summary>
        /// <param name="project"></param>
        public static ProjectLite ToLiteModel(HetProject project)
        {
            ProjectLite projectLite = new ProjectLite();

            if (project != null)
            {
                projectLite.Id = project.ProjectId;
                projectLite.Status = project.ProjectStatusType?.Description;
                projectLite.Name = project.Name;
                projectLite.PrimaryContact = project.PrimaryContact;
                projectLite.District = project.District;                
                projectLite.Requests = project.HetRentalRequest?.Count;
                projectLite.Hires = project.HetRentalAgreement?.Count;
                projectLite.ProvincialProjectNumber = project.ProvincialProjectNumber;
                projectLite.FiscalYear = project.FiscalYear;
            }            

            return projectLite;
        }

        #endregion

        #region Get Project History

        public static List<History> GetHistoryRecords(int id, int? offset, int? limit, DbAppContext context)
        {
            HetProject project = context.HetProject.AsNoTracking()
                .Include(x => x.HetHistory)
                .First(a => a.ProjectId == id);

            List<HetHistory> data = project.HetHistory
                .OrderByDescending(y => y.AppLastUpdateTimestamp)
                .ToList();

            if (offset == null)
            {
                offset = 0;
            }

            if (limit == null)
            {
                limit = data.Count - offset;
            }

            List<History> result = new List<History>();

            for (int i = (int)offset; i < data.Count && i < offset + limit; i++)
            {
                History temp = new History();

                if (data[i] != null)
                {
                    temp.HistoryText = data[i].HistoryText;
                    temp.Id = data[i].HistoryId;
                    temp.LastUpdateTimestamp = data[i].AppLastUpdateTimestamp;
                    temp.LastUpdateUserid = data[i].AppLastUpdateUserid;
                    temp.AffectedEntityId = data[i].ProjectId;
                }

                result.Add(temp);
            }

            return result;
        }

        #endregion
    }
}
