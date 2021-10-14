using System;

namespace HetsApi.Helpers
{
    public static class FiscalHelper
    {
        public static int GetCurrentFiscalStartYear()
        {
            if (DateTime.UtcNow.Month == 1 || DateTime.UtcNow.Month == 2 || DateTime.UtcNow.Month == 3)
            {
                return DateTime.UtcNow.AddYears(-1).Year;
            }
            else
            {
                return DateTime.UtcNow.Year;
            }
        }
    }
}
