using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HETSAPI
{
    // various helper functions for Hangfire
    public static class HangfireTools
    {
        /// <summary>
        ///  On startup Hangfire will have the servers from the previous run.  This routine will clear those out.
        /// </summary>
        public static void ClearHangfire()
        {            
            using (var connection = JobStorage.Current.GetConnection())
            {
                // remove any servers older than 5 seconds.  
                connection.RemoveTimedOutServers(TimeSpan.FromSeconds(5));
            }

        }

    }
}
