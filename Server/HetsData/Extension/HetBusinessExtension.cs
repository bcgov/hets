using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetBusiness
    {
        // attach the linked owner after verification
        [NotMapped]
        public HetOwner LinkedOwner { get; set; }
    }
}
