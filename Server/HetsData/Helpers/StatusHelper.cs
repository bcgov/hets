using System.Linq;
using HetsData.Entities;
using Microsoft.EntityFrameworkCore;

namespace HetsData.Helpers
{
    public static class StatusHelper
    {
        #region Retrieve Status Id using a Code value

        public static int? GetStatusId(string status, string statusType, DbAppContext context)
        {
            switch (statusType.ToLower())
            {
                case "ownerstatus":
                    return GetOwnerStatusId(status, context);

                case "equipmentstatus":
                    return GetEquipmentStatusId(status, context);

                case "projectstatus":
                    return GetProjectStatusId(status, context);

                case "rentalrequeststatus":
                    return GetRentalRequestStatusId(status, context);

                case "rentalagreementstatus":
                    return GetRentalAgreementStatusId(status, context);

                default:
                    return null;
            }
        }

        public static int? GetOwnerStatusId(string status, DbAppContext context)
        {
            HetOwnerStatusType ownerStatus = context.HetOwnerStatusTypes.AsNoTracking()
                .FirstOrDefault(x => x.OwnerStatusTypeCode.ToLower().Equals(status.ToLower()));

            return ownerStatus?.OwnerStatusTypeId;
        }

        public static int? GetEquipmentStatusId(string status, DbAppContext context)
        {
            HetEquipmentStatusType equipmentStatus = context.HetEquipmentStatusTypes.AsNoTracking()
                .FirstOrDefault(x => x.EquipmentStatusTypeCode.ToLower().Equals(status.ToLower()));

            return equipmentStatus?.EquipmentStatusTypeId;
        }

        public static int? GetProjectStatusId(string status, DbAppContext context)
        {
            HetProjectStatusType projectStatus = context.HetProjectStatusTypes.AsNoTracking()
                .FirstOrDefault(x => x.ProjectStatusTypeCode.ToLower().Equals(status.ToLower()));

            return projectStatus?.ProjectStatusTypeId;
        }

        public static int? GetRentalRequestStatusId(string status, DbAppContext context)
        {
            HetRentalRequestStatusType requestStatus = context.HetRentalRequestStatusTypes.AsNoTracking()
                .FirstOrDefault(x => x.RentalRequestStatusTypeCode.ToLower().Equals(status.ToLower()));

            return requestStatus?.RentalRequestStatusTypeId;
        }

        public static int? GetRentalAgreementStatusId(string status, DbAppContext context)
        {
            HetRentalAgreementStatusType agreementStatus = context.HetRentalAgreementStatusTypes.AsNoTracking()
                .FirstOrDefault(x => x.RentalAgreementStatusTypeCode.ToLower().Equals(status.ToLower()));

            return agreementStatus?.RentalAgreementStatusTypeId;
        }

        #endregion

        #region Get Mime Type Id using a Mime Type value

        public static int? GetMimeTypeId(string mimeTypeCode, DbAppContext context)
        {
            HetMimeType mimeType = context.HetMimeTypes.AsNoTracking()
                .FirstOrDefault(x => x.MimeTypeCode.ToLower().Equals(mimeTypeCode.ToLower()));

            return mimeType?.MimeTypeId;
        }

        #endregion

        #region Get Time Period Type Id using a Time Period Type value

        public static int? GetTimePeriodId(string timePeriodCode, DbAppContext context)
        {
            HetTimePeriodType timePeriod = context.HetTimePeriodTypes.AsNoTracking()
                .FirstOrDefault(x => x.TimePeriodTypeCode.ToLower().Equals(timePeriodCode.ToLower()));

            return timePeriod?.TimePeriodTypeId;
        }

        #endregion

        #region Get Rate Period Type Id using a Rate Period Type value

        public static int? GetRatePeriodId(string ratePeriodCode, DbAppContext context)
        {
            HetRatePeriodType timePeriod = context.HetRatePeriodTypes.AsNoTracking()
                .FirstOrDefault(x => x.RatePeriodTypeCode.ToLower().Equals(ratePeriodCode.ToLower()));

            return timePeriod?.RatePeriodTypeId;
        }

        #endregion

        #region Get Role Id using a Name

        public static int? GetRoleId(string roleName, DbAppContext context)
        {
            HetRole role = context.HetRoles.AsNoTracking()
                .FirstOrDefault(x => x.Name.ToLower().Contains(roleName.ToLower()));

            return role?.RoleId;
        }

        #endregion
    }
}
