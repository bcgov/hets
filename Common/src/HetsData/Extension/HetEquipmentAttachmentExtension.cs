using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetEquipmentAttachment
    {
        [NotMapped]
        public int Id
        {
            get => EquipmentAttachmentId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                EquipmentAttachmentId = value;
            }
        }
    }
}
