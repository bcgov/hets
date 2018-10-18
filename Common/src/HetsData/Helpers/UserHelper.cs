using System;
using System.Collections.Generic;
using System.Linq;
using HetsData.Model;
using Microsoft.EntityFrameworkCore;

namespace HetsData.Helpers
{
    #region User Models

    public class LogoffModel
    {
        public string LogoffUrl { get; set; }
    }

    #endregion

    public static class UserHelper
    {
        #region Get all User records (plus associated records)

        /// <summary>
        /// Get all User records
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<HetUser> GetRecords(DbAppContext context)
        {
            List<HetUser> users = context.HetUser.AsNoTracking()
                .Include(x => x.District)
                .Include(x => x.HetUserDistrict)
                .Include(x => x.HetUserRole)
                    .ThenInclude(y => y.Role)
                        .ThenInclude(z => z.HetRolePermission)
                            .ThenInclude(z => z.Permission)
                .ToList();

            foreach (HetUser user in users)            
            {
                // remove inactive roles
                user.HetUserRole = user.HetUserRole
                    .Where(x => x.Role != null &&
                                x.EffectiveDate <= DateTime.UtcNow &&
                                (x.ExpiryDate == null || x.ExpiryDate > DateTime.UtcNow))
                    .ToList();
            }

            return users;
        }

        #endregion

        #region Get a User record (plus associated records)

        /// <summary>
        /// Get a User record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static HetUser GetRecord(int id, DbAppContext context)
        {
            HetUser user = context.HetUser.AsNoTracking()
                .Include(x => x.District)
                .Include(x => x.HetUserDistrict)
                .Include(x => x.HetUserRole)
                    .ThenInclude(y => y.Role)
                        .ThenInclude(z => z.HetRolePermission)
                            .ThenInclude(z => z.Permission)
                .FirstOrDefault(x => x.UserId == id);

            if (user != null)
            {
                // remove inactive roles
                user.HetUserRole = user.HetUserRole
                    .Where(x => x.Role != null &&
                                x.EffectiveDate <= DateTime.UtcNow &&
                                (x.ExpiryDate == null || x.ExpiryDate > DateTime.UtcNow))
                    .ToList();
                
            }

            return user;
        }

        #endregion
    }
}
