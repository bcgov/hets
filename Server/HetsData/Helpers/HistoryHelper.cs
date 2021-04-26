using System;

namespace HetsData.Helpers
{
    #region History Models

    public class History
    {
        public int? Id { get; set; }
        public string HistoryText { get; set; }
        public string LastUpdateUserid { get; set; }
        public DateTime? LastUpdateTimestamp { get; set; }
        public int? AffectedEntityId { get; set; }
    }

    #endregion    
}
