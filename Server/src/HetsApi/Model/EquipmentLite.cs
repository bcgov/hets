using System;

namespace HetsApi.Model
{
    public sealed class EquipmentLite
    {
        public int Id { get; set; }

        public string EquipmentType { get; set; }

        public string OwnerName { get; set; }

        public int? OwnerId { get; set; }

        public bool IsHired { get; set; }

        public string SeniorityString { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Size { get; set; }

        public string EquipmentCode { get; set; }

        public int AttachmentCount { get; set; }

        public DateTime? LastVerifiedDate { get; set; }
        
        public int SenioritySortOrder { get; set; }
    }
}
