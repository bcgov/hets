using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Entities
{    
    public partial class HetDigitalFile
    {
        [NotMapped]
        public int? FileSize { get; set; }

        [NotMapped]
        public string LastUpdateUserid
        {
            get => AppLastUpdateUserid;
            set => AppLastUpdateUserid = value ?? throw new ArgumentNullException(nameof(value));
        }

        [NotMapped]
        public string UserName { get; set; }

        [NotMapped]
        public DateTime? LastUpdateTimestamp
        {
            get => AppLastUpdateTimestamp;
            set
            {
                if (value != null)
                {
                    AppLastUpdateTimestamp = DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
                }
            }
        }
    }
}
