using System;
using HetsCommon;

namespace HetsApi.Helpers
{
    public static class FiscalHelper
    {
        public static int GetCurrentFiscalStartYear()
        {
            DateTime pacificNow = DateUtils.GetPacificNow();
            if (pacificNow.Month == 1 || pacificNow.Month == 2 || pacificNow.Month == 3)
            {
                return pacificNow.AddYears(-1).Year;
            }
            else
            {
                return pacificNow.Year;
            }
        }
    }
}
