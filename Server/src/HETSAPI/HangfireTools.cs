using Hangfire;
using System;
using Hangfire.Storage;

namespace HETSAPI
{
    /// <summary>
    /// Hangfire helper functions
    /// </summary>
    public static class HangfireTools
    {
        /// <summary>
        ///  On startup Hangfire will have the servers from the previous run.  This routine will clear those out.
        /// </summary>
        public static void ClearHangfire()
        {            
            using (IStorageConnection connection = JobStorage.Current.GetConnection())
            {
                // remove any servers older than 5 seconds.  
                connection.RemoveTimedOutServers(TimeSpan.FromSeconds(5));
            }
        }
    }
}
