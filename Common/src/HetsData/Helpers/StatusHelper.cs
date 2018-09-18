using System;
using System.Linq;
using HetsData.Model;
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
            HetOwnerStatusType ownerStatus = context.HetOwnerStatusType.AsNoTracking()
                .FirstOrDefault(x => x.OwnerStatusTypeCode.Equals(status, StringComparison.CurrentCultureIgnoreCase));

            return ownerStatus?.OwnerStatusTypeId;
        }

        public static int? GetEquipmentStatusId(string status, DbAppContext context)
        {
            HetEquipmentStatusType equipmentStatus = context.HetEquipmentStatusType.AsNoTracking()
                .FirstOrDefault(x => x.EquipmentStatusTypeCode.Equals(status, StringComparison.CurrentCultureIgnoreCase));

            return equipmentStatus?.EquipmentStatusTypeId;
        }

        public static int? GetProjectStatusId(string status, DbAppContext context)
        {
            HetProjectStatusType projectStatus = context.HetProjectStatusType.AsNoTracking()
                .FirstOrDefault(x => x.ProjectStatusTypeCode.Equals(status, StringComparison.CurrentCultureIgnoreCase));

            return projectStatus?.ProjectStatusTypeId;
        }

        public static int? GetRentalRequestStatusId(string status, DbAppContext context)
        {
            HetRentalRequestStatusType requestStatus = context.HetRentalRequestStatusType.AsNoTracking()
                .FirstOrDefault(x => x.RentalRequestStatusTypeCode.Equals(status, StringComparison.CurrentCultureIgnoreCase));

            return requestStatus?.RentalRequestStatusTypeId;
        }

        public static int? GetRentalAgreementStatusId(string status, DbAppContext context)
        {
            HetRentalAgreementStatusType agreementStatus = context.HetRentalAgreementStatusType.AsNoTracking()
                .FirstOrDefault(x => x.RentalAgreementStatusTypeCode.Equals(status, StringComparison.CurrentCultureIgnoreCase));

            return agreementStatus?.RentalAgreementStatusTypeId;
        }

        #endregion

        #region Get Mime Type Id using a Mime Type value

        public static int? GetMimeTypeId(string mimeTypeCode, DbAppContext context)
        {
            HetMimeType mimeType = context.HetMimeType.AsNoTracking()
                .FirstOrDefault(x => x.MimeTypeCode.Equals(mimeTypeCode, StringComparison.CurrentCultureIgnoreCase));

            return mimeType?.MimeTypeId;
        }

        #endregion

        #region Get Time Period Type Id using a Time Period Type value

        public static int? GetTimePeriodId(string timePeriodCode, DbAppContext context)
        {
            HetTimePeriodType timePeriod = context.HetTimePeriodType.AsNoTracking()
                .FirstOrDefault(x => x.TimePeriodTypeCode.Equals(timePeriodCode, StringComparison.CurrentCultureIgnoreCase));

            return timePeriod?.TimePeriodTypeId;
        }

        #endregion

        #region Get Rate Period Type Id using a Rate Period Type value

        public static int? GetRatePeriodId(string ratePeriodCode, DbAppContext context)
        {
            HetRatePeriodType timePeriod = context.HetRatePeriodType.AsNoTracking()
                .FirstOrDefault(x => x.RatePeriodTypeCode.Equals(ratePeriodCode, StringComparison.CurrentCultureIgnoreCase));

            return timePeriod?.RatePeriodTypeId;
        }

        #endregion

        #region Get Role Id using a Name

        public static int? GetRoleId(string roleName, DbAppContext context)
        {
            HetRole role = context.HetRole.AsNoTracking()
                .FirstOrDefault(x => x.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));

            return role?.RoleId;
        }

        #endregion
    }
}
