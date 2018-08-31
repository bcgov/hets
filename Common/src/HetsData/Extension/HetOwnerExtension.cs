using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetOwner
    {
        [NotMapped]
        public string ReportDate { get; set; }
        
        [NotMapped]
        public string Title { get; set; }

        [NotMapped]
        public int DistrictId { get; set; }

        [NotMapped]
        public int MinistryDistrictId { get; set; }

        [NotMapped]
        public string DistrictName { get; set; }

        [NotMapped]
        public string DistrictAddress { get; set; }

        [NotMapped]
        public string DistrictContact { get; set; }
    }
}
