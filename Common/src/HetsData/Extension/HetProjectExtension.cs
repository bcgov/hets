using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetProject
    {
        [NotMapped]
        public int Id
        {
            get => ProjectId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                ProjectId = value;
            }
        }
    }
}
