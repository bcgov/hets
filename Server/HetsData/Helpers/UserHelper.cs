using System;
using System.Collections.Generic;
using System.Linq;
using HetsData.Entities;
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
        public static string GetUserName(string smUserId, DbAppContext context)
        {
            var user = context.HetUsers.AsNoTracking().FirstOrDefault(x => x.SmUserId.ToUpper() == smUserId);

            if (user == null)
                return smUserId;

            return $"{user.Surname}, {user.GivenName}";
        }
    }
}
