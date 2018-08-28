using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetRentalRequestAttachment
    {
        [NotMapped]
        public int Id
        {
            get => RentalRequestAttachmentId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                RentalRequestAttachmentId = value;
            }
        }
    }
}
