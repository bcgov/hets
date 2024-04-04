using System;

namespace HetsData.Helpers
{
    #region History Models

    public class History
    {
        public int? Id { get; set; }
        public string HistoryText { get; set; }
        public string LastUpdateUserid { get; set; }

        private DateTime? _lastUpdateTimestamp;
        public DateTime? LastUpdateTimestamp {
            get => _lastUpdateTimestamp is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _lastUpdateTimestamp = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public int? AffectedEntityId { get; set; }
    }

    #endregion    
}
