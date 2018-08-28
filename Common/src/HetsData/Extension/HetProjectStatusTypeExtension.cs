using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetProjectStatusType
    {
        [NotMapped]
        public int Id
        {
            get => ProjectStatusTypeId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                ProjectStatusTypeId = value;
            }
        }
    }
}
