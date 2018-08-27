using System;
using HetsData.Model;

namespace HetsApi.Model
{
    public sealed class EquipmentAttachment : HetEquipmentAttachment
    {
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
