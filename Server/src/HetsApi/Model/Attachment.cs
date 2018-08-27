using System;
using HetsData.Model;

namespace HetsApi.Model
{    
    public sealed class Attachment : HetAttachment
    {
        public int Id
        {
            get => AttachmentId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                AttachmentId = value;
            }
        }
                
        public int? FileSize { get; set; }

        public string LastUpdateUserid
        {
            get => AppLastUpdateUserid;
            set => AppLastUpdateUserid = value ?? throw new ArgumentNullException(nameof(value));
        }

        public DateTime? LastUpdateTimestamp
        {
            get => AppLastUpdateTimestamp;
            set
            {
                if (value != null)
                {
                    AppLastUpdateTimestamp = (DateTime)value;
                }                
            }
        }
    }
}
